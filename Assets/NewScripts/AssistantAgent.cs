using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AssistantAgent : MonoBehaviour {
    public string _username_STT;
    public string _password_STT;
    public string _url_STT;

    [Tooltip("Text field to display the results of streaming.")]
    public Text ResultsField;

    void Start()
    {
        SpeechToTextController _STT_ctrl = new SpeechToTextController(_username_STT, _password_STT, _url_STT);
        _STT_ctrl.Start(SttResultHandler);
    }

    private void SttResultHandler(string result)
    {
        ResultsField.text = result;
        
    }

    private void applyRules(string text) {
        if (alt.transcript.Contains("Bob")) // needs to be final or ECHO happens
        {
            audioData[0].Play();
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("rock and roll")) // needs to be final or ECHO happens
        {
            audioData[1].Play();
            agent.enabled = false;
            anim.SetInteger("Dance", 1);
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("navigate ")) // needs to be final or ECHO happens
        {
            if (navigation_help_counter > 0)
            {
                StartCoroutine(LowerThemeVolume(audioData[2]));
                audioData[2].Play();
                path.enabled = true;
                navigation_help_counter = navigation_help_counter - 1;
            }
            else
            {
                StartCoroutine(LowerThemeVolume(audioData[6]));
                audioData[6].Play();
            }
        }
        if (alt.transcript.Contains("steps ") || alt.transcript.Contains("footsteps ")) // needs to be final or ECHO happens
        {
            string word = "steps";
            ClosestRiddle = riddleFinder.FindRiddle(riddles, transform);
            GameObject question1 = ClosestRiddle.transform.GetChild(0).gameObject;
            textmesh = ClosestRiddle.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
            string answer = textmesh.text;



            if (answer.Equals(word))
            {

                navigation_help_counter = navigation_help_counter + 1;
                TextMeshPro textmesh1 = ClosestRiddle.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
                textmesh1.fontSize = 17;
                textmesh1.font = BangersSDF;
                textmesh1.fontSharedMaterial = BangersSDFMaterial;
                textmesh1.text = "You have Unlocked a Navigation hint!";
                if (ClosestRiddle.activeSelf)
                {
                    StartCoroutine(DeactivateRiddle(ClosestRiddle));
                }
                //   ClosestRiddle.transform.GetChild(0).gameObject.SetActive(false);
                //   ClosestRiddle.transform.GetChild(2).gameObject.Destroy();
            }
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("television ")) // needs to be final or ECHO happens
        {
            string word = "television";
            ClosestRiddle = riddleFinder.FindRiddle(riddles, transform);
            GameObject question = ClosestRiddle.transform.GetChild(0).gameObject;
            textmesh = ClosestRiddle.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
            string answer = textmesh.text;
            GameObject portal = GameObject.Find("Portal");


            if (answer.Equals(word))
            {


                TextMeshPro textmesh1 = ClosestRiddle.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
                textmesh1.fontSize = 16;
                textmesh1.font = BangersSDF;
                textmesh1.fontSharedMaterial = BangersSDFMaterial;
                textmesh1.text = "You have Unlocked a Portal!";
                //   textmesh1.fontSize = 17;
                //  textmesh1.font = BangersSDF;
                //   textmesh1.fontSharedMaterial = BangersSDFMaterial;
                foreach (GameObject gos in portals)
                {
                    gos.SetActive(true);
                    //     
                }
                if (ClosestRiddle.activeSelf)
                {
                    StartCoroutine(DeactivateRiddle(ClosestRiddle));
                }
                //   ClosestRiddle.transform.GetChild(0).gameObject.SetActive(false);
                //   ClosestRiddle.transform.GetChild(2).gameObject.Destroy();
            }
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("tips ") || alt.transcript.Contains("dips ")) // needs to be final or ECHO happens
        {
            ClosestRiddle = riddleFinder.FindRiddle(riddles, transform);
            TempAudioSpurce = ClosestRiddle.GetComponent<AudioSource>();
            StartCoroutine(LowerThemeVolume(TempAudioSpurce));
            TempAudioSpurce.Play();
        }
        if (alt.transcript.Contains("distance")) // needs to be final or ECHO happens
        {
            float distance_goal = Vector3.Distance(end.transform.position, transform.position);
            float distance_Covered = Vector3.Distance(begin.transform.position, transform.position);
            ClosestRiddle = riddleFinder.FindRiddle(riddles, transform);
            textmesh = ClosestRiddle.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
            textmesh.text = "Distance to Goal    " + distance_goal.ToString() + "              Distance covered   " + distance_Covered.ToString();
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("enemy")) // needs to be final or ECHO happens
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            ClosestRiddle = riddleFinder.FindRiddle(riddles, transform);
            textmesh = ClosestRiddle.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
            textmesh.text = "DIstance to Enemy : " + distance.ToString();
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("here ")) // needs to be final or ECHO happens
        {

            path.enabled = false;
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("bored ")) // needs to be final or ECHO happens
        {
            audioData[3].Play();
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("sure")) // needs to be final or ECHO happens
        {
            audioData[4].Play();
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("tell ")) // needs to be final or ECHO happens
        {
            audioData[5].Play();
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("xray")) // needs to be final or ECHO happens
        {

            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("stop ")) // needs to be final or ECHO happens

        {
            foreach (AudioSource audio in audioData)
            {
                if (audio.isPlaying)
                {
                    audio.Stop();
                    agent.enabled = true;
                    anim.SetInteger("Dance", 0);
                }
            }

            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
        if (alt.transcript.Contains("finish")) // needs to be final or ECHO happens
        {
            Active = false;
            this.enabled = false;
            //  _testString = "<speak version=\"1.0\"><express-as type=\"GoodNews\">I love the color of the sky too!</express-as></speak>";
            //   Runnable.Run(Examples());

        }
    }

    
}