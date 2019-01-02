using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBonus : MonoBehaviour
{
    public int navigationhelpCounter = 0;
    public int XrayhelpCounter = 0;
    AudioSource[] audioData;
    GameObject[] riddles;
    GameObject enemy;
    ShowPath path;
    GameObject end;
    GameObject TwoSidedWall;
    MaterialCOntroller _materialController;
    // Use this for initialization
    void Start()
    {
        audioData = GetComponents<AudioSource>();
        riddles = GameObject.FindGameObjectsWithTag("riddle");
        path = GetComponent<ShowPath>();
        enemy = GameObject.Find("Enemy");
        end = GameObject.Find("End");
        TwoSidedWall = GameObject.Find("TwoSided");
        _materialController = TwoSidedWall.GetComponent<MaterialCOntroller>();
    }
    public void Use(string playerQuery)
    {
        switch (playerQuery.ToLower())
        {
            case "navigate":
                if (navigationhelpCounter > 0)
                {
                    StartCoroutine(LowerThemeVolume(audioData[2]));
                    audioData[2].Play();
                    path.enabled = true;
                    navigationhelpCounter = navigationhelpCounter - 1;
                }
                else
                {
                    StartCoroutine(LowerThemeVolume(audioData[6]));
                    audioData[6].Play();
                }

                break;
            case "xray":
                if (XrayhelpCounter > 0)
                    _materialController.Activate = true;
                break;

        }

    }
    // Update is called once per frame
    void Update()
    {

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

