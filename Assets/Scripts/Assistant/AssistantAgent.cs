using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AssistantAgent : MonoBehaviour {
    public string _username_STT;
    public string _password_STT;
    public string _url_STT;
    public Text ResultsField;
    private GameObject[] riddles;
    GameObject ClosestRiddle;
    float Distance;
    Riddle _riddle;
    private RiddleController _riddle_ctrl;
    private SpeechToTextController _STT_ctrl;
    private UseReward _useReward;
    void Start()
    {
        riddles = GameObject.FindGameObjectsWithTag("riddle");
        _STT_ctrl = new SpeechToTextController(_username_STT, _password_STT, _url_STT);
        _riddle_ctrl = GetComponent<RiddleController>();
        _useReward = GetComponent<UseReward>();
        _STT_ctrl.Start(OnSTTResult);
    }
    void Update()
    {
        ClosestRiddle = _riddle_ctrl.FindClosestRiddle(transform, riddles);
        _riddle = ClosestRiddle.GetComponent<Riddle>();
        Distance = Vector3.Distance(transform.position, ClosestRiddle.transform.position);
    }
    private void OnSTTResult(string result)
    {
        if (Distance < 2)
     {
          _riddle_ctrl.SolveRiddles(result);
            ResultsField.text = result;
       }
        else if (Distance > 3  || _riddle.Solved == true)
            _useReward.Use(result);
             ResultsField.text = result;




    }
}