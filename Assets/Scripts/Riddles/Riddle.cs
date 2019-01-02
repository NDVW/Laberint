using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Riddle : MonoBehaviour {		
	public string riddleText;
	public string answer;
	public bool solved = false;	

	void Start() {
		ResetRiddleText();
	}

	void Update() {
		ResetRiddleText();
	}

	void ResetRiddleText() {
		// SetRiddleText(riddleText);
	}

	public bool Solved
    {
        get { return solved; }
        set
        {
            if (value == true && solved == false)
            {	
				// Logic for a riddle being solved
				solved = true;
				OnSolved();
            }
            else if (value == false && solved == false)
            {
				// Logic for wrong answer
				OnWrongAnswer();
            }
        }
    }

	public void OnSolved() {
		SetRiddleText("Correct");
	}

	public void OnWrongAnswer() {
		SetRiddleText("InCorrect!");
	}

	public void SetRiddleText(string text) {
		TextMeshPro _TextMesh = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
		_TextMesh.text = text;
	}
}
