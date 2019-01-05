using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AssistantAgent : MonoBehaviour
{
    public string _username_STT;
    public string _password_STT;
    public string _url_STT;
    public Text ResultsField;

    private RiddleController _riddle_ctrl;
    private SpeechToTextController _STT_ctrl;
    private UseReward _useReward;

    void Start()
    {
        _STT_ctrl = new SpeechToTextController(_username_STT, _password_STT, _url_STT);
        
        _riddle_ctrl = GetComponent<RiddleController>();
        _useReward = GetComponent<UseReward>();
        _STT_ctrl.Start(OnSTTResult);
    }

    private void OnSTTResult(string result)
    {
        ResultsField.text = result;
        if (_riddle_ctrl.closestRiddle && !_riddle_ctrl.closestRiddle.Solved) _riddle_ctrl.SolveRiddles(result);
        else _useReward.Use(result);
    }
}