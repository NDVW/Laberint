using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    private string[] _helpWords = {"tell me more", "help"};

    public TextMeshProUGUI ResultsField;
    GameObject panel;
    HelperTextTyping chatText;

    void Start()
    {
        _sttCtrl = new SpeechToTextController(usernameSTT, passwordSTT, urlSTT);
        _sttCtrl.Start(OnSTTResult);

        _chatCtrl = new ChatBotController(usernameTone, passwordTone, urlTone, urlChat);
        _chatCtrl.Start(OnChatbotReply);
        
        _riddleCtrl = GetComponent<RiddleController>();

        panel = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        ResultsField = panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        chatText = panel.transform.GetChild(0).gameObject.GetComponent<HelperTextTyping>();
        _useReward = GetComponent<UseReward>();
    }

    private void OnSTTResult(string result)
    {                   
        Debug.Log("STT Result " + result);

        if (_riddleCtrl.closestRiddle && ArrayContains(result, _helpWords))
        {               
            Debug.Log("Launching Hint");
            SetResultFieldText(_riddleCtrl.closestRiddle.hint);
        } 
        else if (_riddleCtrl.closestRiddle && !_riddleCtrl.closestRiddle.Solved)
        {
            Debug.Log("Solving Riddles");
            _riddleCtrl.SolveRiddles(result);
        } 
        else if (ArrayContains(result, _useReward.keyWords))
        {
            Debug.Log("Using Reward");
            _useReward.Use(result);
        }     
        else
        {               
            Debug.Log("Sending to chat");
            _chatCtrl.SendMessage(result);
        }        
    }

    private bool ArrayContains(string result, string[] array) 
    {
        bool contains = false;
        
        foreach (var word in array)
        {
            if (result.ToLower().Contains(word)) contains = true;
        }

        return contains;
    }

    private void OnChatbotReply(string reply) 
    {        
        SetResultFieldText("..." + reply);
    }

    public void SetResultFieldText(string text)
    {
        ResultsField.text = text;
        chatText.enabled = true;
    }
}