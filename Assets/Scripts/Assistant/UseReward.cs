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
    GameObject[] TwoSidedWall;
    MaterialCOntroller _materialController;
    // Use this for initialization
    void Start () {
        audioData = GetComponents<AudioSource>();
        riddles = GameObject.FindGameObjectsWithTag("riddle");
        path = GetComponent<ShowPath>();
        enemy = GameObject.Find("Enemy");
        end = GameObject.Find("End");
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
        if (playerQuery.ToLower().Contains("vision"))  // See through walls or Xray walls help
        {
            if (XrayhelpCounter > 0)
                foreach (GameObject gos in TwoSidedWall)
                {
                    _materialController = gos.GetComponent<MaterialCOntroller>();
                    _materialController.Activate = true;
                }
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

