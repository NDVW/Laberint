using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AssistantAgent : MonoBehaviour
{
    // STT Config
    public string usernameSTT = "630db3e4-9c69-4aa1-bb7c-6f69410be997";
    public string passwordSTT = "ZPjG3vEeSTz6";
    public string urlSTT = "https://stream.watsonplatform.net/speech-to-text/api";

    // Chatbot Config
    public string usernameTone = "088bd540-a4a9-412e-a681-3aa558b30c9d";
    public string passwordTone = "XQJDRlN5U4h4";
    public string urlTone = "https://gateway.watsonplatform.net/tone-analyzer/api";
    public string urlChat = "159.203.118.21:8000";

    // Results display
    public Text resultsField;
    
    // Internal variables
    private RiddleController _riddleCtrl;
    private SpeechToTextController _sttCtrl;
    private UseReward _useReward;
    private ChatBotController _chatCtrl;

    void Start()
    {
        _sttCtrl = new SpeechToTextController(usernameSTT, passwordSTT, urlSTT);
        _sttCtrl.Start(OnSTTResult);

        _chatCtrl = new ChatBotController(usernameTone, passwordTone, urlTone, urlChat);
        _chatCtrl.Start(OnChatbotReply);
        
        _riddleCtrl = GetComponent<RiddleController>();
        _useReward = GetComponent<UseReward>();
    }

    private void OnSTTResult(string result)
    {                   
        Debug.Log("STT Result" + result);
        _chatCtrl.sendMessage(result);
        
        if (_riddleCtrl.closestRiddle && !_riddleCtrl.closestRiddle.Solved) _riddleCtrl.SolveRiddles(result);
        else _useReward.Use(result);
    }

    private void OnChatbotReply(string reply) {
        resultsField.text = reply;
    }
}