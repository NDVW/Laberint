﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AssistantAgent : MonoBehaviour
{
    // STT Config
    public string usernameSTT;
    public string passwordSTT;
    public string urlSTT;

    // Chatbot Config
    public string usernameTone;
    public string passwordTone;
    public string urlTone;
    public string urlChat;

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
        Debug.Log("STT Result " + result);
        resultsField.text = result;
        _chatCtrl.SendMessage(result);
        
        if (_riddleCtrl.closestRiddle && !_riddleCtrl.closestRiddle.Solved) _riddleCtrl.SolveRiddles(result);
        else _useReward.Use(result);
    }

    private void OnChatbotReply(string reply) {        
        resultsField.text = "..." + reply;
    }
}