using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.ToneAnalyzer.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using IBM.Watson.DeveloperCloud.Connection;

public class JsonChatData
{
	public string[] context;
	public string emotion;
}

public class ChatBotController
{    
    public delegate void ResultHandler(string result);

    private ResultHandler _replyHandler;
    private ToneAnalyzer _toneAnalyzer;
    private string _urlChat;
    private string[] _chatbotAllowedTones = {"anger", "fear", "joy", "sadness"};
    private string _chatbotDefaultTone = "neutral";
    private string _versionTone = "2018-12-12";

    public ChatBotController(string usernameTone, string passwordTone, string urlTone, string urlChat) {
        _urlChat = urlChat;
        _toneAnalyzer = new ToneAnalyzer(new Credentials(usernameTone, passwordTone, urlTone)) {
            VersionDate = _versionTone
        };        
    }

    public void Start(ResultHandler handler) {
        _replyHandler = handler;
    }

    public void sendMessage(string message) {
        _toneAnalyzer.GetToneAnalyze(OnGetToneAnalyze, OnToneFail, message);
    }

    private void OnGetToneAnalyze(ToneAnalysis resp, Dictionary<string, object> customData)
    {
        Debug.Log("On tone analyze");
        Debug.Log(resp);
        Debug.Log(customData["json"].ToString());
        // anger, fear, joy, and sadness (emotional tones); analytical, confident, and tentative
        //     {"document_tone":{"tones":[{"score":1.0,"tone_id":"joy","tone_name":"Joy"},{"score":0.97759,"tone_id":"confident","tone_name":"Confident"}]}}
    }

    void sendMessage(string message, string emotion)
    {        
        JsonChatData data = new JsonChatData();
        data.context =  new string[1];
        data.context[0] = message;
		data.emotion = emotion; // 'neutral', 'anger', 'joy', 'fear', 'sadness'   
        Runnable.Run(PostRequest(_urlChat, JsonUtility.ToJson(data)));
    }

    private IEnumerator PostRequest(string url, string json)
    {
        UnityWebRequest uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError) OnChatFail(uwr);
        else _replyHandler(uwr.downloadHandler.text);        
    }

    private void OnChatFail(UnityWebRequest uwr) {
        Debug.Log("Error While Sending message to ChatBot: " + uwr.error);
    }

    private void OnToneFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Debug.Log("Error analyzing tone" + error.ToString());
    }
}
