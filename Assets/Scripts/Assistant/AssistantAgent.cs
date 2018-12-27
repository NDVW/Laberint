using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AssistantAgent : MonoBehaviour {
    public string _username_STT;
    public string _password_STT;
    public string _url_STT;
    public Text ResultsField;

    private RiddleController _riddle_ctrl;
    private SpeechToTextController _STT_ctrl;

    void Start()
    {
        _STT_ctrl = new SpeechToTextController(_username_STT, _password_STT, _url_STT);        
        _riddle_ctrl = new RiddleController();
        _STT_ctrl.Start(OnSTTResult);
    }

    private void OnSTTResult(string result)
    {
        string riddleSolution = _riddle_ctrl.SolveRiddles(transform, result);
        ResultsField.text = riddleSolution;
    }
}