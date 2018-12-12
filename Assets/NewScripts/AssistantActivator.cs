using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

public class AssistantActivator : MonoBehaviour {

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    ExampleStreaming _Assistant;
    //AssistantAgent _Assistant;
    GameObject _Canvas;
    GameObject _EventSystem;
    // Use this for initialization
    void Start()
    {
       _Assistant = GetComponent<ExampleStreaming>();
      //  _Assistant = GetComponent<AssistantAgent>();
        _Canvas = GameObject.Find("Canvas");
        _EventSystem = GameObject.Find("EventSystem");
        //  _Assistant.SetActive(false);
        _Canvas.SetActive(false);
        _EventSystem.SetActive(false);

        actions.Add("Helper", Activate);
        actions.Add("deactivate", Deactivate);
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

    }
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Activate()
    {
        _Assistant.enabled = true;
        _Canvas.SetActive(true);
        _EventSystem.SetActive(true);

    }
    private void Deactivate()
    {
        _Assistant.enabled = false;
        _Canvas.SetActive(false);
        _EventSystem.SetActive(false);
    }
}
