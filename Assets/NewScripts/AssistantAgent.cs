using UnityEngine;
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
    private string _iamApikey_STT = "SG0cH8kMQN4SIhJsCnrNUJX0dY5M4xYKvb6lGkXACoNL";
    private string _IamAccessToken_STT = "eyJraWQiOiIyMDE3MTAzMC0wMDowMDowMCIsImFsZyI6IlJTMjU2In0.eyJpYW1faWQiOiJpYW0tU2VydmljZUlkLTA0N2MyZTM1LTAyOTEtNDU5MC1iMTBmLWY5MjE1OWU1YjgyZCIsImlkIjoiaWFtLVNlcnZpY2VJZC0wNDdjMmUzNS0wMjkxLTQ1OTAtYjEwZi1mOTIxNTllNWI4MmQiLCJyZWFsbWlkIjoiaWFtIiwiaWRlbnRpZmllciI6IlNlcnZpY2VJZC0wNDdjMmUzNS0wMjkxLTQ1OTAtYjEwZi1mOTIxNTllNWI4MmQiLCJzdWIiOiJTZXJ2aWNlSWQtMDQ3YzJlMzUtMDI5MS00NTkwLWIxMGYtZjkyMTU5ZTViODJkIiwic3ViX3R5cGUiOiJTZXJ2aWNlSWQiLCJ1bmlxdWVfaW5zdGFuY2VfY3JucyI6WyJjcm46djE6Ymx1ZW1peDpwdWJsaWM6c3BlZWNoLXRvLXRleHQ6YXUtc3lkOmEvNDk3OWI2MzkwMjljNDg3ZGJkZjhlYmQ3NjE5NGZjMjU6YTZiNGFiNTMtYjI4Yi00MTdmLWJkYmItMmI3YTEyNWIwMTE1OjoiXSwiYWNjb3VudCI6eyJ2YWxpZCI6dHJ1ZSwiYnNzIjoiNDk3OWI2MzkwMjljNDg3ZGJkZjhlYmQ3NjE5NGZjMjUifSwiaWF0IjoxNTQ0NDg1MDcyLCJleHAiOjE1NDQ0ODg2NzIsImlzcyI6Imh0dHBzOi8vaWFtLmJsdWVtaXgubmV0L2lkZW50aXR5IiwiZ3JhbnRfdHlwZSI6InVybjppYm06cGFyYW1zOm9hdXRoOmdyYW50LXR5cGU6YXBpa2V5Iiwic2NvcGUiOiJpYm0gb3BlbmlkIiwiY2xpZW50X2lkIjoiZGVmYXVsdCIsImFjciI6MSwiYW1yIjpbInB3ZCJdfQ.SiYK5mmThsUq0R_-SeF-Tt1k9ngPmyDd3sL6Dj0X_EoLzGl0DqlPuxAQjK61bIFl7r3F5vPjVA8DGillynjC5SFr3tjFuKtbzuge_y2TEgofC4XbV6bxCqF9OEjTR4wIG0Rmmn1D6BXfZaSbRr6D7YYyYZrazMVN0KDuVEHEPNrDKvGu9Iu6t6tjKqXVCmezwqVGB7m6VvLrVyGLIvpzVTIfZDHTwSo4Joaz0vnojVo2vbelJ3-Pitwmd8uNKdSq-ImAhZOq4EP0KAw4EjMsFrQJK7kRfTc7xqD_ou-GgSMaJbZVyk-6i4l-I6f99TQckUk6pfrIq5jCo20TQBCfyg";
    private string _url_STT = "https://gateway-syd.watsonplatform.net/speech-to-text/api";


    public Text ResultsField;

    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToText _speechToText;

    // TEXT TO SPEECH - BURNER CREDENTIALS FOR PUBLIC DEMO I WILL DELETE AFTER RECORDING
    private string _iamApikey_TTS = "Ty666WwnanxNp7JV5hBjpb89gJdNoKbad4-n2DfZwhSw";// "J_AtYm18caYvM4QFhnPsTBFT7tBOGCPh7ywEgefPp3-C";
    private string _IamAccessToken_TTS = "eyJraWQiOiIyMDE3MTAzMC0wMDowMDowMCIsImFsZyI6IlJTMjU2In0.eyJpYW1faWQiOiJpYW0tU2VydmljZUlkLTI1OTdjM2E2LWViNWMtNDBhMS04NjFlLTk4OTE3MTc2OTJmNCIsImlkIjoiaWFtLVNlcnZpY2VJZC0yNTk3YzNhNi1lYjVjLTQwYTEtODYxZS05ODkxNzE3NjkyZjQiLCJyZWFsbWlkIjoiaWFtIiwiaWRlbnRpZmllciI6IlNlcnZpY2VJZC0yNTk3YzNhNi1lYjVjLTQwYTEtODYxZS05ODkxNzE3NjkyZjQiLCJzdWIiOiJTZXJ2aWNlSWQtMjU5N2MzYTYtZWI1Yy00MGExLTg2MWUtOTg5MTcxNzY5MmY0Iiwic3ViX3R5cGUiOiJTZXJ2aWNlSWQiLCJ1bmlxdWVfaW5zdGFuY2VfY3JucyI6WyJjcm46djE6Ymx1ZW1peDpwdWJsaWM6dGV4dC10by1zcGVlY2g6ZXUtZGU6YS8xYWZiNzJhMGQ0MTk0ZTliYjY2Nzg0ZDA4MzhmNTU5YjplN2Y3ZmNiYi0wNzMzLTQxMWUtOTE1Yi1lNTJjZmYzZjEyZDA6OiJdLCJhY2NvdW50Ijp7InZhbGlkIjp0cnVlLCJic3MiOiIxYWZiNzJhMGQ0MTk0ZTliYjY2Nzg0ZDA4MzhmNTU5YiJ9LCJpYXQiOjE1NDQ1NTY1NTksImV4cCI6MTU0NDU2MDE1OSwiaXNzIjoiaHR0cHM6Ly9pYW0uYmx1ZW1peC5uZXQvaWRlbnRpdHkiLCJncmFudF90eXBlIjoidXJuOmlibTpwYXJhbXM6b2F1dGg6Z3JhbnQtdHlwZTphcGlrZXkiLCJzY29wZSI6ImlibSBvcGVuaWQiLCJjbGllbnRfaWQiOiJkZWZhdWx0IiwiYWNyIjoxLCJhbXIiOlsicHdkIl19.RuNCS2LByZCMpF5FHN2-opdItnm7HWTuGI6N3VpkbfIJilx-ru4DP-Cc8wMEMw_snUs_-eADgmo-j2ICcwNyMIeaODsG9S8bPneanBPpmucTLQEk5eiHgRr9KhSEHkzWe1xQWeye9k-eoHHjnW49wpbAKjxohFoP_KNf7CL_zvLAa87LjlOjgrNf1-nodXZK5W264j1BtUYF3c7R4eb3nLNVHJKdhrYII06QXf3-WMJH6ATyHadAhiUpHhW7rGJ6vVLnnahdmrr7uIK13vAEv-zjAZ5b4WNlgr-WJxL8V1V9cXJUCUtAhZjQ4Kks0IAqsmgoF9GamdjQi6dR_PcuRQ";
    private string _url_TTS = "https://stream-fra.watsonplatform.net/text-to-speech/api";

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