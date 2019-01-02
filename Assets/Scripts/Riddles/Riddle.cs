using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Riddle class for the riddles. 
public class Riddle : MonoBehaviour {		
	public string riddleText;
	public string answer;
    public string rewardText;
	public bool solved = false;
    GameObject Assistant;
    UseReward referenceScript;
    public Color color = Color.white;
    public TMP_FontAsset BangersSDF;
    public Material BangersSDFMaterial;
    GameObject[] portals;
    Vector3 start;
    Vector3 end;
    bool timetoMove = false;
    private float lerptime = 4;
    private float currentLerptime = 0;
    private float MoveWallDistance = 3f;
    private  GameObject wall;


    void Start() {
		ResetRiddleText();
        if (this.name == "portal")
        {
            portals = GameObject.FindGameObjectsWithTag("portal");
            foreach (GameObject gos in portals)
            {
                gos.SetActive(false);
            }
        }
        if (this.name == "navigation" || this.name == "xray")
        {
            Assistant = GameObject.Find("Assistant");
            referenceScript = Assistant.GetComponent<UseReward>();
        }
        if(this.name == "door")
        {
           // this.wall = GameObject.Find("riddleWall");
            this.start = transform.position;
            this.end = transform.position + Vector3.up * this.MoveWallDistance;
        }

    }

    void Update() {
		ResetRiddleText();
        if (timetoMove)
        {   
            MoveWall();
            if (this.transform.position == end)
            {
                timetoMove = false;
            }
        }
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

        StartCoroutine(ToggleSolvedRiddle());
        string riddleType = this.name;
        generateReward(riddleType);
        
	}

	public void OnWrongAnswer() {
		SetRiddleText("InCorrect!");
	}

	public void SetRiddleText(string text) {
		TextMeshPro _TextMesh = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        
        _TextMesh.text = text;

        _TextMesh.fontSize = 10;
        _TextMesh.font = BangersSDF;
      /// if (this.name == "door")
     //  {
    //   _TextMesh.color = color;
     //  }
        _TextMesh.fontSharedMaterial = BangersSDFMaterial;
    }
    public void generateReward(string riddleType)
    {
        switch (riddleType)
        {
            case "portal":
                foreach (GameObject gos in portals)
                         {
                              gos.SetActive(true);
                                                       
                         }
                    break;
            case "navigation":
                referenceScript.navigationhelpCounter = referenceScript.navigationhelpCounter + 1;
                break;
            case "xray":
                referenceScript.XrayhelpCounter = referenceScript.XrayhelpCounter + 1;
                break;
            case "door":
                timetoMove = true;  
             //   StartCoroutine(WallremoveBonus());
             
                break;
        }
    }
    IEnumerator ToggleSolvedRiddle()
    {
        SetRiddleText(rewardText);
        yield return new WaitForSeconds(10);
        SetRiddleText("Solved");
    }
    IEnumerator WallremoveBonus()
    {  if (wall.transform.position != end)
        {
            Debug.Log("here");
            MoveWall();
            yield return new WaitForSeconds(0);
        }
    }
    void MoveWall()
    {
        this.currentLerptime += Time.deltaTime;
        if (this.currentLerptime >= this.lerptime)
        {
            this.currentLerptime = this.lerptime;
        }
        float perc = this.currentLerptime / this.lerptime;
        this.transform.position = Vector3.Lerp(start, end, perc);
    }
}
