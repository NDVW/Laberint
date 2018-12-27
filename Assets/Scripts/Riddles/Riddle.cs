using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riddle : MonoBehaviour {	
	public bool Solved = false;
	public string Answer;
	public string SolutionText;	

	public string Solve(string proposedAnswer) {	
		if (Answer.includes(proposedAnswer)) 
		{
			TextMeshPro textmesh1 = ClosestRiddle.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();			
			textmesh1.fontSize = 17;
			textmesh1.font = BangersSDF;
			textmesh1.fontSharedMaterial = BangersSDFMaterial;
			if (ClosestRiddle.activeSelf)
			{
				StartCoroutine(DeactivateRiddle(ClosestRiddle));
			}
			ClosestRiddle.transform.GetChild(0).gameObject.SetActive(false);
			ClosestRiddle.transform.GetChild(2).gameObject.Destroy();
			return SolutionText
		}
		
		return "";
	}

	// Use this for initialization
	void Start () {
		Solved = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
