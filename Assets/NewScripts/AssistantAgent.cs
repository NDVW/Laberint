﻿using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Connection;

public class AssistantAgent : MonoBehaviour {

    // Use this for initialization
    private string _iamApikey_STT = "mJy-MqY8OZUXoMmpugXd-spyXhconbKcb2kypoOnTNHm";
    private string _IamAccessToken_STT = "eyJraWQiOiIyMDE3MTAzMC0wMDowMDowMCIsImFsZyI6IlJTMjU2In0.eyJpYW1faWQiOiJpYW0tU2VydmljZUlkLTg4Mjk5NTAyLWJiNDctNGUxYy05ODM0LTg3ZGNkMzVlMDkyNyIsImlkIjoiaWFtLVNlcnZpY2VJZC04ODI5OTUwMi1iYjQ3LTRlMWMtOTgzNC04N2RjZDM1ZTA5MjciLCJyZWFsbWlkIjoiaWFtIiwiaWRlbnRpZmllciI6IlNlcnZpY2VJZC04ODI5OTUwMi1iYjQ3LTRlMWMtOTgzNC04N2RjZDM1ZTA5MjciLCJzdWIiOiJTZXJ2aWNlSWQtODgyOTk1MDItYmI0Ny00ZTFjLTk4MzQtODdkY2QzNWUwOTI3Iiwic3ViX3R5cGUiOiJTZXJ2aWNlSWQiLCJ1bmlxdWVfaW5zdGFuY2VfY3JucyI6WyJjcm46djE6Ymx1ZW1peDpwdWJsaWM6c3BlZWNoLXRvLXRleHQ6YXUtc3lkOmEvY2YyZGEzZGY2OGE2NGM5NzllNDc3ODg1ZTU1MmE2OTc6NDc1ZmNmYTktZWNjMC00NTM4LWExNjMtODZiNmMxYjE5NmVlOjoiXSwiYWNjb3VudCI6eyJ2YWxpZCI6dHJ1ZSwiYnNzIjoiY2YyZGEzZGY2OGE2NGM5NzllNDc3ODg1ZTU1MmE2OTcifSwiaWF0IjoxNTQ1NjI4NDI2LCJleHAiOjE1NDU2MzIwMjYsImlzcyI6Imh0dHBzOi8vaWFtLmJsdWVtaXgubmV0L2lkZW50aXR5IiwiZ3JhbnRfdHlwZSI6InVybjppYm06cGFyYW1zOm9hdXRoOmdyYW50LXR5cGU6YXBpa2V5Iiwic2NvcGUiOiJpYm0gb3BlbmlkIiwiY2xpZW50X2lkIjoiZGVmYXVsdCIsImFjciI6MSwiYW1yIjpbInB3ZCJdfQ.MhL5pEfSc4U7ZD7-XaZlSxv0_6YawpB5K9CaRu5m11Gvu1tuDfx4YMIOYEZuJ5KMqJq7edDwsYe8c8932PEU1A_BJyH6yrYzoaUy_uSM8W6u_P52NM8YDHcEjgnm8SRApXk6kumtW9xNmkzBktJ5Wygk7ePxAmJ_GSi0QYcTHQ1srfoUjv8wL2JIkCRd084bsjk8JGaX0aibG6XchXieIfRA3LxvtQNV2Zgz0FLWY82d5ohgmaWheqha495Wx7D8jsDBbj3jKjixuxNwSmtTl1iwY72FvYJ43rEvKIJHWTdF563Pi_8HS-vX_yB23kowUPW-35-qZs5NOtnICjtIBw";
    private string _url_STT = "https://gateway-syd.watsonplatform.net/speech-to-text/api";


    public Text ResultsField;

    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToText _speechToText;

    // TEXT TO SPEECH - BURNER CREDENTIALS FOR PUBLIC DEMO I WILL DELETE AFTER RECORDING
    private string _iamApikey_TTS = "uW5Utq19DD1_x3fgcWL2CDjM7oyIVPrTZ6cREhFIwVKz";// "J_AtYm18caYvM4QFhnPsTBFT7tBOGCPh7ywEgefPp3-C";
    private string _IamAccessToken_TTS = "eyJraWQiOiIyMDE3MTAzMC0wMDowMDowMCIsImFsZyI6IlJTMjU2In0.eyJpYW1faWQiOiJpYW0tU2VydmljZUlkLTFkY2UwYjUzLTIwMWUtNDhiOC1hZjlhLTlhNzNkOWRlMjFmNCIsImlkIjoiaWFtLVNlcnZpY2VJZC0xZGNlMGI1My0yMDFlLTQ4YjgtYWY5YS05YTczZDlkZTIxZjQiLCJyZWFsbWlkIjoiaWFtIiwiaWRlbnRpZmllciI6IlNlcnZpY2VJZC0xZGNlMGI1My0yMDFlLTQ4YjgtYWY5YS05YTczZDlkZTIxZjQiLCJzdWIiOiJTZXJ2aWNlSWQtMWRjZTBiNTMtMjAxZS00OGI4LWFmOWEtOWE3M2Q5ZGUyMWY0Iiwic3ViX3R5cGUiOiJTZXJ2aWNlSWQiLCJ1bmlxdWVfaW5zdGFuY2VfY3JucyI6WyJjcm46djE6Ymx1ZW1peDpwdWJsaWM6dGV4dC10by1zcGVlY2g6YXUtc3lkOmEvY2YyZGEzZGY2OGE2NGM5NzllNDc3ODg1ZTU1MmE2OTc6ZDM4YWZjODctMmIzOS00Yjg1LTk5OTctZWVlNzNiYjFjNjdlOjoiXSwiYWNjb3VudCI6eyJ2YWxpZCI6dHJ1ZSwiYnNzIjoiY2YyZGEzZGY2OGE2NGM5NzllNDc3ODg1ZTU1MmE2OTcifSwiaWF0IjoxNTQ1NjI4NDQ0LCJleHAiOjE1NDU2MzIwNDQsImlzcyI6Imh0dHBzOi8vaWFtLmJsdWVtaXgubmV0L2lkZW50aXR5IiwiZ3JhbnRfdHlwZSI6InVybjppYm06cGFyYW1zOm9hdXRoOmdyYW50LXR5cGU6YXBpa2V5Iiwic2NvcGUiOiJpYm0gb3BlbmlkIiwiY2xpZW50X2lkIjoiZGVmYXVsdCIsImFjciI6MSwiYW1yIjpbInB3ZCJdfQ.RP7qPvpgqIZ5NVJDzpsKhauDQJ5u6sLVnhMGiDu_epWExypPINS9yJkdgiuFT3wJ0eq0xzJ1M-NLMUlFud0-dWmf6SCS7cat1S42-YZKfYVUiKGDKEywfWVcjtjAwW93_Sl_rMHWQTAjwPpIlvx4FENExqEuT4EGes_0qWRhrwx-LevE58Znc92g9Ukez9OAxR1vXHwySQ0_n0sjpvUGZuQ2Vw1zG60f-AnG9tlFQF5CH2UC4lR3G3V8Z9XIJeWs2302lgaA3AAZF-t0NsMGgh198AJxDlDIE_T-2HI6cEibUNlZhGCDhjAMMyrzGMYoDGUUEyenZsn8r1eJrME6vw";
    private string _url_TTS = "wss://gateway-syd.watsonplatform.net/text-to-speech/api";

    private TextToSpeech _textToSpeech;

    //string _testString = "<speak version=\"1.0\"><say-as interpret-as=\"letters\">I'm sorry</say-as>. <prosody pitch=\"150Hz\">This is Text to Speech!</prosody><express-as type=\"GoodNews\">I'm sorry. This is Text to Speech!</express-as></speak>";

    /// TEST STRINGS OK

    // Pitch Shifting
    //string _testString = "<speak version=\"1.0\"><prosody pitch=\"150Hz\">This is Text to Speech!</prosody></speak>";
    //string _testString = "<speak version=\"1.0\"><prosody pitch=\"250Hz\">This is Text to Speech!</prosody></speak>";
    //string _testString = "<speak version=\"1.0\"><prosody pitch=\"350Hz\">This is Text to Speech!</prosody></speak>";
    //string _testString = "<speak version=\"1.0\"><prosody pitch=\"350Hz\">hi</prosody></speak>";

    // Good news and sorrow and uncertainty - ref https://console.bluemix.net/docs/services/text-to-speech/SSML-expressive.html#expressive
    // <express-as type="GoodNews">This is Text to Speech!</express-as>
    string _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">Hello! Good News! Text to Speech is Working!</express-as></speak>";
    //string _testString = "<speak version=\"1.0\"><express-as type=\"Apology\">I am terribly sorry for the quality of service you have received.</express-as></speak>";
    //string _testString = "<speak version=\"1.0\"><express-as type=\"Uncertainty\">Can you please explain it again? I am not sure I understand.</express-as></speak>";

    //string _testString = "<speak version=\"1.0\"><prosody pitch=\\\"350Hz\\\"><express-as type=\"Uncertainty\">Can you please explain it again? I am confused and I'm not sure I understand.</express-as></prosody></speak>";


    string _createdCustomizationId;
    CustomVoiceUpdate _customVoiceUpdate;
    string _customizationName = "unity-example-customization";
    string _customizationLanguage = "en-US";
    string _customizationDescription = "A text to speech voice customization created within Unity.";
    string _testWord = "Watson";

    private bool _synthesizeTested = false;
    private bool _getVoicesTested = false;
    private bool _getVoiceTested = false;
    private bool _getPronuciationTested = false;
    private bool _getCustomizationsTested = false;
    private bool _createCustomizationTested = false;
    private bool _deleteCustomizationTested = false;
    private bool _getCustomizationTested = false;
    private bool _updateCustomizationTested = false;
    private bool _getCustomizationWordsTested = false;
    private bool _addCustomizationWordsTested = false;
    private bool _deleteCustomizationWordTested = false;
    private bool _getCustomizationWordTested = false;



    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
        //  //  Create credential and instantiate service
        //   Credentials credentials_STT = new Credentials(_username_STT, _password_STT, _url_STT);
        //   Credentials credentials_TTS = new Credentials(_username_TTS, _password_TTS, _url_TTS);

        //  _speechToText = new SpeechToText(credentials_STT);
        //  _textToSpeech = new TextToSpeech(credentials_TTS);



    }
    private IEnumerator CreateService()
    {
        //  Create credential and instantiate service
        Credentials credentials_STT = null;
        Credentials credentials_TTS = null;

        TokenOptions tokenOptions_TTS = new TokenOptions()
        {
            IamApiKey = _iamApikey_TTS,
           IamAccessToken = _IamAccessToken_TTS

        };

        credentials_TTS = new Credentials(tokenOptions_TTS, _url_TTS);

        //  Wait for tokendata
        while (!credentials_TTS.HasIamTokenData())
            yield return null;


        //  Authenticate using iamApikey
        TokenOptions tokenOptions_STT = new TokenOptions()
            {
                IamApiKey = _iamApikey_STT,
                IamAccessToken = _IamAccessToken_STT

            };

            credentials_STT = new Credentials(tokenOptions_STT, _url_STT);

            //  Wait for tokendata
            while (!credentials_STT.HasIamTokenData())
                yield return null;
        
      

       _speechToText = new SpeechToText(credentials_STT);
       _textToSpeech = new TextToSpeech(credentials_TTS);

        _speechToText.StreamMultipart = true;

        Active = true;
        StartRecording();
      Runnable.Run(Examples());
    }

    public bool Active
    {
        get { return _speechToText.IsListening; }
        set
        {
            if (value && !_speechToText.IsListening)
            {
                _speechToText.DetectSilence = true;
                _speechToText.EnableWordConfidence = true;
                _speechToText.EnableTimestamps = true;
                _speechToText.SilenceThreshold = 0.01f;
                _speechToText.MaxAlternatives = 0;
                _speechToText.EnableInterimResults = true;
                _speechToText.OnError = OnError;
                _speechToText.InactivityTimeout = -1;
                _speechToText.ProfanityFilter = false;
                _speechToText.SmartFormatting = true;
                _speechToText.SpeakerLabels = false;
                _speechToText.WordAlternativesThreshold = null;
                _speechToText.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && _speechToText.IsListening)
            {
                _speechToText.StopListening();
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

                _speechToText.OnListen(record);

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

    private void OnRecognize(SpeechRecognitionEvent result, Dictionary<string, object> customData )
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence);
                    Log.Debug("ExampleStreaming.OnRecognize()", text);
                    ResultsField.text = text;

                    if (alt.transcript.Contains("blue") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {  
                        _testString = "<;;I love the color of the sky too!</express-as></speak>";
                       Runnable.Run(Examples());

                    }
                    if (alt.transcript.Contains("yellow") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        _testString = "<speak version=\"1.0\"><prosody pitch=\\\"350Hz\\\"><express-as type=\"GoodNews\">Oh Yes! The color of daisies and bananas!</express-as></prosody></speak>";
                        Runnable.Run(Examples());
                    }  // Cannot ECHO the trigger condition (or be ready for loop

                    if (alt.transcript.Contains("happy") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        _testString = "<speak version=\"1.0\"><prosody pitch=\\\"250Hz\\\"><express-as type=\"GoodNews\">That's so nice! You bring me great joy as well!</express-as></prosody></speak>";
                        Runnable.Run(Examples());
                    }  // Cannot ECHO the trigger condition (or be ready for loop

                    if (alt.transcript.Contains("mistake") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        _testString = "<speak version=\"1.0\"><prosody pitch=\\\"50Hz\\\"><express-as type=\"Apology\">I am so sorry.  I'll try to do better next time.</express-as></prosody></speak>";
                        Runnable.Run(Examples());
                    }  // Cannot ECHO the trigger condition (or be ready for loop

                    if (alt.transcript.Contains("quantum") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        _testString = "<speak version=\"1.0\"><prosody pitch=\\\"200Hz\\\"><express-as type=\"Uncertainty\">I dont really know how to answer that.</express-as></prosody></speak>";
                        Runnable.Run(Examples());
                    }  // Cannot ECHO the trigger condition (or be ready for loop

                    if (alt.transcript.Contains("goodbye") && ResultsField.text.Contains("Final")) // needs to be final or ECHO happens
                    {
                        _testString = "<speak version=\"1.0\"><prosody pitch=\\\"200Hz\\\"><express-as type=\"GoodNews\">Bye bye! And thank you!</express-as></prosody></speak>";
                        Runnable.Run(Examples());
                    }  // Cannot ECHO the trigger condition (or be ready for loop



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
                        foreach (var alternative in wordAlternative.alternatives)
                            Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
                    }
                }
            }
        }
    }

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result , Dictionary<string, object> customData )
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognize()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }


    // TTS CODE
    private IEnumerator Examples()
    {
        //  Synthesize
        
        _textToSpeech.Voice = VoiceType.en_US_Lisa;
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting synthesize.");
        _textToSpeech.ToSpeech(HandleToSpeechCallback, OnFail, _testString, true);
        while (!_synthesizeTested)
            yield return null;

        //	Get Voices
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get voices.");
        _textToSpeech.GetVoices(OnGetVoices, OnFail);
        while (!_getVoicesTested)
            yield return null;

        //	Get Voice
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get voice {0}.", VoiceType.en_US_Allison);
        _textToSpeech.GetVoice(OnGetVoice, OnFail, VoiceType.en_US_Allison);
        while (!_getVoiceTested)
            yield return null;

        //	Get Pronunciation
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get pronunciation of {0}", _testWord);
        _textToSpeech.GetPronunciation(OnGetPronunciation, OnFail, _testWord, VoiceType.en_US_Allison);
        while (!_getPronuciationTested)
            yield return null;

        //  Get Customizations
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get a list of customizations");
        //		_textToSpeech.GetCustomizations(OnGetCustomizations, OnFail);
        //		while (!_getCustomizationsTested)
        //			yield return null;

        //  Create Customization
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to create a customization");
        //		_textToSpeech.CreateCustomization(OnCreateCustomization, OnFail, _customizationName, _customizationLanguage, _customizationDescription);
        //		while (!_createCustomizationTested)
        //			yield return null;

        //  Get Customization
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get a customization");
        //		if (!_textToSpeech.GetCustomization(OnGetCustomization, OnFail, _createdCustomizationId))
        //			Log.Debug("ExampleTextToSpeech.Examples()", "Failed to get custom voice model!");
        //		while (!_getCustomizationTested)
        //			yield return null;

        //  Update Customization
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to update a customization");
        //		Word[] wordsToUpdateCustomization =
        //		{
        //			new Word()
        //			{
        //				word = "hello",
        //				translation = "hullo"
        //			},
        //			new Word()
        //			{
        //				word = "goodbye",
        //				translation = "gbye"
        //			},
        //			new Word()
        //			{
        //				word = "hi",
        //				translation = "ohioooo"
        //			}
        //		};

        //		_customVoiceUpdate = new CustomVoiceUpdate()
        //		{
        //			words = wordsToUpdateCustomization,
        //			description = "My updated description",
        //			name = "My updated name"
        //		};

        if (!_textToSpeech.UpdateCustomization(OnUpdateCustomization, OnFail, _createdCustomizationId, _customVoiceUpdate))
            Log.Debug("ExampleTextToSpeech.Examples()", "Failed to update customization!");
        while (!_updateCustomizationTested)
            yield return null;

        //  Get Customization Words
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get a customization's words");
        //		if (!_textToSpeech.GetCustomizationWords(OnGetCustomizationWords, OnFail, _createdCustomizationId))
        //			Log.Debug("ExampleTextToSpeech.GetCustomizationWords()", "Failed to get {0} words!", _createdCustomizationId);
        //		while (!_getCustomizationWordsTested)
        //			yield return null;

        //  Add Customization Words
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to add words to a customization");
        //		Word[] wordArrayToAddToCustomization =
        //		{
        //			new Word()
        //			{
        //				word = "bananna",
        //				translation = "arange"
        //			},
        //			new Word()
        //			{
        //				word = "orange",
        //				translation = "gbye"
        //			},
        //			new Word()
        //			{
        //				word = "tomato",
        //				translation = "tomahto"
        //			}
        //		};

        //		Words wordsToAddToCustomization = new Words()
        //		{
        //			words = wordArrayToAddToCustomization
        //		};

        //		if (!_textToSpeech.AddCustomizationWords(OnAddCustomizationWords, OnFail, _createdCustomizationId, wordsToAddToCustomization))
        //			Log.Debug("ExampleTextToSpeech.AddCustomizationWords()", "Failed to add words to {0}!", _createdCustomizationId);
        //		while (!_addCustomizationWordsTested)
        //			yield return null;

        //  Get Customization Word
        //		Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to get the translation of a custom voice model's word.");
        //		string customIdentifierWord = wordsToUpdateCustomization[0].word;
        //		if (!_textToSpeech.GetCustomizationWord(OnGetCustomizationWord, OnFail, _createdCustomizationId, customIdentifierWord))
        //			Log.Debug("ExampleTextToSpeech.GetCustomizationWord()", "Failed to get the translation of {0} from {1}!", customIdentifierWord, _createdCustomizationId);
        //		while (!_getCustomizationWordTested)
        //			yield return null;

        //  Delete Customization Word
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to delete customization word from custom voice model.");
        string wordToDelete = "goodbye";
        if (!_textToSpeech.DeleteCustomizationWord(OnDeleteCustomizationWord, OnFail, _createdCustomizationId, wordToDelete))
            Log.Debug("ExampleTextToSpeech.DeleteCustomizationWord()", "Failed to delete {0} from {1}!", wordToDelete, _createdCustomizationId);
        while (!_deleteCustomizationWordTested)
            yield return null;

        //  Delete Customization
        Log.Debug("ExampleTextToSpeech.Examples()", "Attempting to delete a customization");
        if (!_textToSpeech.DeleteCustomization(OnDeleteCustomization, OnFail, _createdCustomizationId))
            Log.Debug("ExampleTextToSpeech.DeleteCustomization()", "Failed to delete custom voice model!");
        while (!_deleteCustomizationTested)
            yield return null;

        Log.Debug("ExampleTextToSpeech.Examples()", "Text to Speech examples complete.");
    }






    void HandleToSpeechCallback(AudioClip clip, Dictionary<string, object> customData = null)
    {
        PlayClip(clip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            GameObject audioObject = new GameObject("AudioObject");
            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            Destroy(audioObject, clip.length);

            _synthesizeTested = true;
        }
    }

    private void OnGetVoices(Voices voices, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnGetVoices()", "Text to Speech - Get voices response: {0}", customData["json"].ToString());
        _getVoicesTested = true;
    }

    private void OnGetVoice(Voice voice, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnGetVoice()", "Text to Speech - Get voice  response: {0}", customData["json"].ToString());
        _getVoiceTested = true;
    }

    private void OnGetPronunciation(Pronunciation pronunciation, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnGetPronunciation()", "Text to Speech - Get pronunciation response: {0}", customData["json"].ToString());
        _getPronuciationTested = true;
    }

    //	private void OnGetCustomizations(Customizations customizations, Dictionary<string, object> customData = null)
    //	{
    //		Log.Debug("ExampleTextToSpeech.OnGetCustomizations()", "Text to Speech - Get customizations response: {0}", customData["json"].ToString());
    //		_getCustomizationsTested = true;
    //	}

    //	private void OnCreateCustomization(CustomizationID customizationID, Dictionary<string, object> customData = null)
    //	{
    //		Log.Debug("ExampleTextToSpeech.OnCreateCustomization()", "Text to Speech - Create customization response: {0}", customData["json"].ToString());
    //		_createdCustomizationId = customizationID.customization_id;
    //		_createCustomizationTested = true;
    //	}

    private void OnDeleteCustomization(bool success, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnDeleteCustomization()", "Text to Speech - Delete customization response: {0}", customData["json"].ToString());
        _createdCustomizationId = null;
        _deleteCustomizationTested = true;
    }

    //	private void OnGetCustomization(Customization customization, Dictionary<string, object> customData = null)
    //	{
    //		Log.Debug("ExampleTextToSpeech.OnGetCustomization()", "Text to Speech - Get customization response: {0}", customData["json"].ToString());
    //		_getCustomizationTested = true;
    //	}

    private void OnUpdateCustomization(bool success, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnUpdateCustomization()", "Text to Speech - Update customization response: {0}", customData["json"].ToString());
        _updateCustomizationTested = true;
    }

    //	private void OnGetCustomizationWords(Words words, Dictionary<string, object> customData = null)
    //	{
    //		Log.Debug("ExampleTextToSpeech.OnGetCustomizationWords()", "Text to Speech - Get customization words response: {0}", customData["json"].ToString());
    //		_getCustomizationWordsTested = true;
    //	}

    private void OnAddCustomizationWords(bool success, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnAddCustomizationWords()", "Text to Speech - Add customization words response: {0}", customData["json"].ToString());
        _addCustomizationWordsTested = true;
    }

    private void OnDeleteCustomizationWord(bool success, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnDeleteCustomizationWord()", "Text to Speech - Delete customization word response: {0}", customData["json"].ToString());
        _deleteCustomizationWordTested = true;
    }

    private void OnGetCustomizationWord(Translation translation, Dictionary<string, object> customData = null)
    {
        Log.Debug("ExampleTextToSpeech.OnGetCustomizationWord()", "Text to Speech - Get customization word response: {0}", customData["json"].ToString());
        _getCustomizationWordTested = true;
    }

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
    }



}