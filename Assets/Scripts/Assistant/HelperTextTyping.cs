using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HelperTextTyping : MonoBehaviour
{
    private bool isCoroutineStarted;
    AudioSource[] sounds;
    AudioSource typingSoundEffect;
    GameObject Assistant;
    int visibleCount ;
    // Use this for initialization
    private TextMeshProUGUI textmesh;
    int counter = 0;
    int totalVisibleCharacter;
    void Start()
    {
        Assistant = GameObject.Find("Assistant");

        sounds = Assistant.GetComponents<AudioSource>();
        typingSoundEffect = sounds[4];
        textmesh = this.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (!isCoroutineStarted)
        {
            StartCoroutine(type());
        }
    }
    void OnDisable()
    {
        isCoroutineStarted = false;
        counter = 0;
        visibleCount = 0;
      //  this.textmesh.maxVisibleCharacters = 0;

    }
    IEnumerator type()
    {
        typingSoundEffect.Play();
        isCoroutineStarted = true;


        totalVisibleCharacter = this.textmesh.textInfo.characterCount;
        while (true)
        {
            visibleCount = counter % (totalVisibleCharacter + 1);
            textmesh.maxVisibleCharacters = visibleCount;
            if (visibleCount == totalVisibleCharacter)
            {
                break;        //       yield return new WaitForSeconds(1.0f);
            }
            counter += 1;
            yield return new WaitForSeconds(0.04f);
        }
        typingSoundEffect.Stop();
        this.enabled = false;


        //    int counter = 0;


    }

}
