using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Riddle class for the riddles. 
public class Riddle : MonoBehaviour {		
	public string riddleText;
	public string answer;
    public string rewardText;
    public string hint;
    public bool solved = false;
    public string riddleType;

    private UseReward referenceScript;    
    private TMP_FontAsset defaultFont;
    private Material DefaultFontMaterial;
    private float defaultFontSize;
    private GameObject[] portals;
    private Vector3 start;
    private TypeTextEffect tt;
    private Vector3 end;
    private bool timetoMove = false;    
    private GameObject Assistant;
    private float lerptime = 4;
    private float currentLerptime = 0;
    private float MoveWallDistance = 3f;
    private  GameObject wall;
    private string Riddle_question;
    private TextMeshPro _TextMesh;    
    
    void Start() {
        riddleType = this.name;
        _TextMesh = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        defaultFont = _TextMesh.font;
        DefaultFontMaterial = _TextMesh.fontSharedMaterial;
        Riddle_question = _TextMesh.text;
        defaultFontSize = _TextMesh.fontSize;        

        // if (riddleType == "portal")
        // {
        //     portals = GameObject.FindGameObjectsWithTag("portal");
        //     foreach (GameObject gos in portals)
        //     {
        //         gos.SetActive(false);
        //     }
        // }

        if (riddleType == "navigation" || riddleType == "xray")
        {
            Assistant = GameObject.Find("Assistant");
            referenceScript = Assistant.GetComponent<UseReward>();
        }

        if(riddleType == "door")
        {           
            this.start = transform.position;
            this.end = transform.position + Vector3.up * this.MoveWallDistance;
        }
    }

    void Update() {		
        if (timetoMove)
        {   
            MoveWall();
            if (this.transform.position == end)
            {
                timetoMove = false;
            }
        }
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
                StartCoroutine(OnWrongAnswer());
                
            }
        }
    }

	public void OnSolved() {
        StartCoroutine(ToggleSolvedRiddle());        
        generateReward(riddleType);        
	}

    IEnumerator OnWrongAnswer() {
		SetRiddleText("InCorrect!  Please try Again", 10);
        yield return new WaitForSeconds(2);
        SetRiddleText(Riddle_question, defaultFontSize);        
    }

	public void SetRiddleText(string text, float fontSize) {        
        _TextMesh.text = text;
        _TextMesh.fontSize = fontSize;
        _TextMesh.font = this.defaultFont;
        _TextMesh.fontSharedMaterial = this.DefaultFontMaterial;
    }

    public void generateReward(string riddleType)
    {
        switch (riddleType)
        {
            case "portal":
                activatePortals();
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

    public void  activatePortals() 
    {
        GameObject portal = GameObject.FindGameObjectsWithTag("portal")[0];
        portal.transform.GetChild(0).gameObject.SetActive(true);
        portal.transform.GetChild(3).gameObject.SetActive(true);
    }

    IEnumerator ToggleSolvedRiddle()
    {
        SetRiddleText(rewardText, 10);
        yield return new WaitForSeconds(10);
        SetRiddleText("Solved", 10);
    }

    IEnumerator WallremoveBonus()
    {  
        if (wall.transform.position != end)
        {
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
