using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonChatData
{
	public string[] context;
	public string emotion;
}

public class ChatBot {    
    // Tone analyze credentials
    public string _username_Tone = "088bd540-a4a9-412e-a681-3aa558b30c9d";
    public string _password_Tone = "XQJDRlN5U4h4";
    public string _url_Tone = "https://gateway.watsonplatform.net/tone-analyzer/api";
    
    // Chatbot url
    public string _chatUrl = "http://bd4f22ea.ngrok.io/cakechat_api/v1/actions/get_response";

    public delegate void ResultHandler(string result);
    private ResultHandler _reply_handler;
    private Array _chatbotAllowedTones = ['anger', 'fear', 'joy', 'sadness'];
    private string _chatbotDefaultTone = 'neutral';

    public ChatBot(ResultHandler handler) {
        _toneAnalyze = new ToneAnalyzer(new Credentials(_username_Tone, _password_Tone, _url_Tone));
        _reply_handler = handler;
    }

    public string sendMessage(string message, ResultHandler handler) {
        _toneAnalyzer.GetToneAnalyze(OnGetToneAnalyze, OnToneFail, message)
    }

    private void OnGetToneAnalyze(ToneAnalyzerResponse resp, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleToneAnalyzer.OnGetToneAnalyze()", "Tone Analyzer - Analyze Response: {0}", resp);
        Log.Debug("ExampleToneAnalyzer.OnGetToneAnalyze()", "Tone Analyzer - Analyze Data: {0}", customData["json"].ToString());
        // anger, fear, joy, and sadness (emotional tones); analytical, confident, and tentative
    }

    void sendMessage(string message, string emotion)
    {        
        JsonChatData data = new JsonChatData();
        data.context =  new string[1];
        data.context[0] = message;
		data.emotion = emotion; // 'neutral', 'anger', 'joy', 'fear', 'sadness'   
        StartCoroutine(PostRequest(_chatUrl, JsonUtility.ToJson(data)));
    }

    IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) OnChatFail(uwr);
        else _reply_handler(uwr.downloadHandler.text);        
    }

    private void OnChatFail(UnityWebRequest uwr) {
        Log.Error("Error While Sending message to ChatBot: " + uwr.error);
    }

    private void OnToneFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("Error analyzing tone", "Error received: {0}", error.ToString());
    }
}
