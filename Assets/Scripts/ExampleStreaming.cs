/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;

public class ExampleStreaming : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
    [SerializeField]
    private string _serviceUrl;
    [Tooltip("Text field to display the results of streaming.")]
    public Text ResultsField;
    [Header("CF Authentication")]
    [Tooltip("The authentication username.")]
    [SerializeField]
    private string _username;
    [Tooltip("The authentication password.")]
    [SerializeField]
    private string _password;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;
    [Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
    [SerializeField]
    private string _iamUrl;

    [Header("Parameters")]
    // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
   [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
   [SerializeField]
    private string _recognizeModel;
    #endregion

 //   public Text ResultsField;
    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;
    AudioSource[] audioData;
   // GameObject[] points;
    ShowPath path; 
    private SpeechToText _service;
    Animator anim;
    GameObject enemy;
    private NavMeshAgent agent;
    private string _IamAccessToken = "eyJraWQiOiIyMDE3MTAzMC0wMDowMDowMCIsImFsZyI6IlJTMjU2In0.eyJpYW1faWQiOiJpYW0tU2VydmljZUlkLTA0N2MyZTM1LTAyOTEtNDU5MC1iMTBmLWY5MjE1OWU1YjgyZCIsImlkIjoiaWFtLVNlcnZpY2VJZC0wNDdjMmUzNS0wMjkxLTQ1OTAtYjEwZi1mOTIxNTllNWI4MmQiLCJyZWFsbWlkIjoiaWFtIiwiaWRlbnRpZmllciI6IlNlcnZpY2VJZC0wNDdjMmUzNS0wMjkxLTQ1OTAtYjEwZi1mOTIxNTllNWI4MmQiLCJzdWIiOiJTZXJ2aWNlSWQtMDQ3YzJlMzUtMDI5MS00NTkwLWIxMGYtZjkyMTU5ZTViODJkIiwic3ViX3R5cGUiOiJTZXJ2aWNlSWQiLCJ1bmlxdWVfaW5zdGFuY2VfY3JucyI6WyJjcm46djE6Ymx1ZW1peDpwdWJsaWM6c3BlZWNoLXRvLXRleHQ6YXUtc3lkOmEvNDk3OWI2MzkwMjljNDg3ZGJkZjhlYmQ3NjE5NGZjMjU6YTZiNGFiNTMtYjI4Yi00MTdmLWJkYmItMmI3YTEyNWIwMTE1OjoiXSwiYWNjb3VudCI6eyJ2YWxpZCI6dHJ1ZSwiYnNzIjoiNDk3OWI2MzkwMjljNDg3ZGJkZjhlYmQ3NjE5NGZjMjUifSwiaWF0IjoxNTQ0NTQ5NDY5LCJleHAiOjE1NDQ1NTMwNjksImlzcyI6Imh0dHBzOi8vaWFtLmJsdWVtaXgubmV0L2lkZW50aXR5IiwiZ3JhbnRfdHlwZSI6InVybjppYm06cGFyYW1zOm9hdXRoOmdyYW50LXR5cGU6YXBpa2V5Iiwic2NvcGUiOiJpYm0gb3BlbmlkIiwiY2xpZW50X2lkIjoiZGVmYXVsdCIsImFjciI6MSwiYW1yIjpbInB3ZCJdfQ.HUxMI8LmbSXVyxBJ9EnMptJIM2u5tx6TnbfN17pNxEo9csu-55Yp__0riPSsZe2xRD7Vk65lJBbYfbBRV-H4L09d8x0yFe_zhV7W_O4Fo-0k-nYO-un9IJsK4cofynQWCEKRkPMVoOJ-aU-QsyOT9Hzuz75WJcNYMT-rhV9Nlmb-jz68DpqVw4aGxos6UKyNyy0L3mzb0t_PhoJRnhpjtlvUXjwleDg1Q2QL57ptNlhJz0j_oHcJI_Sjri86lHZu1nP4YMg_vrHt11q1OFMqk7Pshj1v2PK--n4O96rV76B66O5zDnUvrhwDeDOAKYzRnNBMHMujECiyDBqgieZG7g";

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
        audioData = GetComponents<AudioSource>();
        path = GetComponent<ShowPath>();
        enemy = GameObject.Find("Enemy");
        anim = enemy.GetComponent<Animator>();
        agent = enemy.GetComponent<NavMeshAgent>();
        //     points = GameObject.FindGameObjectsWithTag("floortrack");
        //      foreach (GameObject go in points)
        //       {
        //        go.SetActive(false);
        //      }
    }

    private IEnumerator CreateService()
    {
        //  Create credential and instantiate service
        Credentials credentials = null;
        if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
        {
            //  Authenticate using username and password
            credentials = new Credentials(_username, _password, _serviceUrl);
        }
        else if (!string.IsNullOrEmpty(_iamApikey))
        {
            //  Authenticate using iamApikey
            TokenOptions tokenOptions = new TokenOptions()
            {
                IamApiKey = _iamApikey,
                IamAccessToken = _IamAccessToken

            };

            credentials = new Credentials(tokenOptions, _serviceUrl);

            //  Wait for tokendata
            while (!credentials.HasIamTokenData())
                yield return null;
        }
        else
        {
            throw new WatsonException("Please provide either username and password or IAM apikey to authenticate the service.");
        }

        _service = new SpeechToText(credentials);
        _service.StreamMultipart = true;

        Active = true;
        StartRecording();
    }

    public bool Active
    {
        get { return _service.IsListening; }
        set
        {
            if (value && !_service.IsListening)
            {
                _service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                _service.DetectSilence = true;
                _service.EnableWordConfidence = true;
                _service.EnableTimestamps = true;
                _service.SilenceThreshold = 0.01f;
                _service.MaxAlternatives = 0;
                _service.EnableInterimResults = true;
                _service.OnError = OnError;
                _service.InactivityTimeout = -1;
                _service.ProfanityFilter = false;
                _service.SmartFormatting = true;
                _service.SpeakerLabels = false;
                _service.WordAlternativesThreshold = null;
                _service.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _service.IsListening)
            {
                _service.StopListening();
            }
        }
    }

    private void StartRecording()
    {
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

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
    }

    private IEnumerator RecordingHandler()
    {
        Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
        _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
        yield return null;      // let _recordingRoutine get set..

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
                Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

                StopRecording();
                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
              || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
				record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                record.Clip.SetData(samples, 0);

                _service.OnListen(record);

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
                foreach (var alt in res.alternatives)
                {
                    string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    Log.Debug("ExampleStreaming.OnRecognize()", text);
                  //  audioData.Play();
                    //  Log.Debug("ExampleStreaming.OnRecognize()", "mama");
                    ResultsField.text = text;

                      if (alt.transcript.Contains("Bob") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                       {
                        audioData[0].Play();
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                      if (alt.transcript.Contains("rock and roll") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {    
                        audioData[1].Play();
                        agent.enabled = false;
                        anim.SetInteger("Dance", 1);
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("navigation ") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        audioData[2].Play();
                        path.enabled = true;
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("here ") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        
                        path.enabled = false;
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("bored ") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        audioData[3].Play();
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("sure") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        audioData[4].Play();
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("tell ") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        audioData[5].Play();
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }

                    if (alt.transcript.Contains("stop ") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    
                    { foreach (AudioSource audio in audioData)
                        {  
                            if (audio.isPlaying)
                            {
                                audio.Stop();
                                agent.enabled = true;
                                anim.SetInteger("Dance", 0);
                            }
                        }
                        
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("finish") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        Active = false;
                        this.enabled = false;
                        //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
                        //   Runnable.Run(Examples());

                    }
                }

                if (res.keywords_result != null && res.keywords_result.keyword != null)
                {
                    foreach (var keyword in res.keywords_result.keyword)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
                    }
                }

                if (res.word_alternatives != null)
                {
                    foreach (var wordAlternative in res.word_alternatives)
                    {
                        Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
                        foreach(var alternative in wordAlternative.alternatives)
                            Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result, Dictionary<string, object> customData)
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognize()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }
}
