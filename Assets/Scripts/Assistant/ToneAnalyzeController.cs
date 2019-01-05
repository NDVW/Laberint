using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToneAnalyzeController : MonoBehaviour {
	public string _username_STT;
    public string _password_STT;
    public string _url_STT;

	private ToneAnalyzer _toneAnalyze = _toneAnalyze;

	public SpeechToTextController(string _username_STT, string _password_STT, string _url_STT) {
		Credentials credentials_STT = new Credentials(_username_STT, _password_STT, _url_STT);
		_toneAnalyze = new ToneAnalyzer(credentials_STT);		
	}
	
	private void AnalyzeTone()
	{
		if (!_toneAnalyzer.GetToneAnalyze(OnGetToneAnalyze, OnFail, _stringToTestTone))
			Log.Debug("ExampleToneAnalyzer.GetToneAnalyze()", "Failed to analyze!");
	}

	private void OnGetToneAnalyze(ToneAnalyzerResponse resp, Dictionary<string, object> customData)
	{
		Log.Debug("ExampleToneAnalyzer.OnGetToneAnalyze()", "Tone Analyzer - Analyze Response: {0}", customData["json"].ToString());
	}

	private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
	{
		Log.Error("ExampleToneAnalyzer.OnFail()", "Error received: {0}", error.ToString());
	}
}
