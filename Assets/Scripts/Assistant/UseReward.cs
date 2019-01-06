using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The script is used to implement the voice commands for the rewards obtained by solving the riddles.
public class UseReward : MonoBehaviour {
    public int navigationhelpCounter = 0;
    public int XrayhelpCounter = 0;
    AudioSource[] audioData;
    GameObject[] riddles;
    GameObject enemy;
    ShowPath path;
    GameObject end;
    GameObject begin;
    private RiddleController _riddle_ctrl;
    Riddle riddle;
    GameObject[] TwoSidedWall;
    AssistantAgent Assistant;
    MaterialCOntroller _materialController;
    // Use this for initialization
    void Start () {
        audioData = GetComponents<AudioSource>();
        riddles = GameObject.FindGameObjectsWithTag("riddle");
        path = GetComponent<ShowPath>();
        enemy = GameObject.Find("Enemy");
        end = GameObject.Find("End");
        begin = GameObject.Find("Begin");
        _riddle_ctrl = GetComponent<RiddleController>();
        Assistant = GetComponent<AssistantAgent>();
        TwoSidedWall = GameObject.FindGameObjectsWithTag("insidewall");


    }
    public void Use(string playerQuery)
    {
        if (playerQuery.ToLower().Contains("navigate")) // Navigation help 
        {
            if (navigationhelpCounter > 0)
            {
                //  StartCoroutine(LowerThemeVolume(audioData[2]));
                audioData[1].Play();
                path.enabled = true;
                navigationhelpCounter = navigationhelpCounter - 1;
            }
            else
            {
                //StartCoroutine(LowerThemeVolume(audioData[6]));
                audioData[2].Play();
            }
        }
        if (playerQuery.ToLower().Contains("x. ray"))  // See through walls or Xray walls help
        {
            if (XrayhelpCounter > 0)
                foreach (GameObject gos in TwoSidedWall)
                {
                    _materialController = gos.GetComponent<MaterialCOntroller>();
                    _materialController.Activate = true;
                    XrayhelpCounter = XrayhelpCounter - 1;
                }
            else Assistant.setResultFieldText("No X-Ray vision help available");
        }
        if (playerQuery.ToLower().Contains("tell me more") || playerQuery.ToLower().Contains("help"))
        {
            
            riddle = _riddle_ctrl.closestRiddle;
            string tip = riddle.hint;
            //  Assistant.ResultsField.text = tip;
            Assistant.setResultFieldText(tip);

        }
        if (playerQuery.ToLower().Contains("distance"))
        {
            string distance_to_goal = Vector3.Distance(transform.position, end.transform.position).ToString();
            string distance_to_begin = Vector3.Distance(transform.position, begin.transform.position).ToString() ;
            string distance_text = "Distance to goal : " + distance_to_goal + "       Distance Covered : " + distance_to_begin;
            //   Assistant.ResultsField.text = distance_text;
            Assistant.setResultFieldText(distance_text);
        }



    }
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator LowerThemeVolume(AudioSource audio)
     {
         AudioClip clip = audio.clip;
         float time = clip.length;
         if (audioData[7])
         {
            audioData[7].volume = 0.027f;
            yield return new WaitForSeconds(time);
             audioData[7].volume = 0.123f;
         }
     }
 }

