using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Services.Assistant.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using IBM.Watson.DeveloperCloud.Connection;

public class IbmController 
{
	// WatsonAssistant (WA)	
	private Assistant _WatsonAssistant;	
	private string _workspaceId = "2dc0c463-af37-421c-86b1-3913f135a5be";
	private string _waVersionDate = "2018-09-20";

	// SpeechToText	(STT)
	public delegate void SttResultHandler(string recognizedText);

	private SttResultHandler _sttResultHandler;	
	private SpeechToText _SpeechToText;			
	private int _recordingRoutine = 0;
	private string _microphoneID = null;
	private AudioClip _recording = null;
	private int _recordingBufferSize = 1;
	private int _recordingHZ = 22050;
	
	public IbmController() 
	{		
		Credentials credentials_STT = new Credentials(
			"630db3e4-9c69-4aa1-bb7c-6f69410be997", 
			"ZPjG3vEeSTz6", 
			"https://stream.watsonplatform.net/speech-to-text/api"
		);
		Credentials credentials_WA = new Credentials(
			"apikey",
			"Gp6mQLlhpuu-oyE5jG9kXb21YMc3FOAYD9YAiRXTATz6", 
			"https://gateway-fra.watsonplatform.net/assistant/api"
		);
		_SpeechToText = new SpeechToText(credentials_STT);
		_WatsonAssistant = new Assistant(credentials_WA) {
			VersionDate = _waVersionDate
		};
	}

	public void MessageWa(string textInput)
	{
		MessageRequest messageRequest = new MessageRequest()
		{
			Input = new Dictionary<string, object>()
			{
				{ "text", textInput }
			}
		};

		if (!_WatsonAssistant.Message(OnMessage, OnWaFail, _workspaceId, messageRequest)) 
		{
			Debug.Log("Failed to message assistant!");
		}			
	}

	private void OnMessage(object resp, Dictionary<string, object> customData)
	{
		Debug.Log("Assistant: Message Response: " + customData["json"].ToString());
	}

	public void StartStt(SttResultHandler handler)
	{
		_sttResultHandler = handler;		
		_SpeechToText.DetectSilence = true;
		_SpeechToText.EnableWordConfidence = true;
		_SpeechToText.EnableTimestamps = true;
		_SpeechToText.SilenceThreshold = 0.01f;
		_SpeechToText.MaxAlternatives = 0;
		_SpeechToText.EnableInterimResults = true;
		_SpeechToText.OnError = OnSttError;
		_SpeechToText.InactivityTimeout = -1;
		_SpeechToText.ProfanityFilter = false;
		_SpeechToText.SmartFormatting = true;
		_SpeechToText.SpeakerLabels = false;
		_SpeechToText.WordAlternativesThreshold = null;		
		_SpeechToText.StartListening(OnRecognize, OnRecognizeSpeaker, null);
		
		if (_recordingRoutine == 0)
		{
			UnityObjectUtil.StartDestroyQueue();
			_recordingRoutine = Runnable.Run(RecordingHandler());
		}
	}

	private void StopRecording()
	{
		if (_recordingRoutine != 0)
		{
			Microphone.End(_microphoneID);
			Runnable.Stop(_recordingRoutine);
			_recordingRoutine = 0;
		}
	}

	private IEnumerator RecordingHandler()
	{
		_recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
		yield return null;

		if (_recording == null)
		{
			StopRecording();
			yield break;
		}

		bool bFirstBlock = true;
		int midPoint = _recording.samples / 2;
		float[] samples = null;

		while (_recordingRoutine != 0 && _recording != null)
		{
			int writePos = Microphone.GetPosition(_microphoneID);
			if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
			{
				Debug.Log("<color=red>Fatal error:</color>" + "Microphone disconnected.");
				StopRecording();
				yield break;
			}

			if ((bFirstBlock && writePos >= midPoint) || (!bFirstBlock && writePos < midPoint))
			{
				// front block is recorded, make a RecordClip and pass it onto our callback.
				samples = new float[midPoint];
				_recording.GetData(samples, bFirstBlock ? 0 : midPoint);

				AudioData record = new AudioData();
				record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
				record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
				record.Clip.SetData(samples, 0);
                _SpeechToText.OnListen(record);
				bFirstBlock = !bFirstBlock;
			}
			else
			{
				// calculate the number of samples remaining until we ready for a block of audio, 
				// and wait that amount of time it will take to record.
				int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
				float timeRemaining = (float)remaining / (float)_recordingHZ;
				yield return new WaitForSeconds(timeRemaining);
			}

		}
		yield break;
	}

	private void OnRecognize(SpeechRecognitionEvent result, Dictionary<string, object> customData)
	{
		if (result != null && result.results.Length > 0)
		{			
			foreach (var res in result.results)
			{
				foreach (var alt in res.alternatives) {					
					if (res.final) _sttResultHandler(alt.transcript);
				}
			}			
		}
	}

	private void OnRecognizeSpeaker(SpeakerRecognitionEvent result,Dictionary<string, object> customData)
	{
	}

	private void OnWaFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Debug.Log("Watson Assistant Fail:" + error.ToString());
	}

	private void OnSttError(string error)
	{
		_SpeechToText.StopListening();
		Debug.Log("<color=red>Fatal error:</color>" + "STT ERROR" + error.ToString());
	}
}