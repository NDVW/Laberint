using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TypeTextEffect : MonoBehaviour {
    private bool isCoroutineStarted;
    AudioSource[] sounds;
    AudioSource typingSoundEffect;
    GameObject Assistant;
    int visibleCount;
    // Use this for initialization
    private TextMeshPro textmesh;
    int counter = 0;
    void Start()
    {
        Assistant = GameObject.Find("Assistant");

        sounds = Assistant.GetComponents<AudioSource>();
        typingSoundEffect = sounds[3];
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
        textmesh.maxVisibleCharacters = 0;
        if (typingSoundEffect != null)
        {
            if (typingSoundEffect.isPlaying)
            {
                typingSoundEffect.Stop();
            }
        }
    }
	IEnumerator type () {
        typingSoundEffect.Play();
        isCoroutineStarted = true;
        textmesh = GetComponent<TextMeshPro>();

        int totalVisibleCharacter = textmesh.textInfo.characterCount;
    //    int counter = 0;
        while (true)
        {
            visibleCount = counter % (totalVisibleCharacter + 1);
            textmesh.maxVisibleCharacters = visibleCount;
           if(visibleCount == totalVisibleCharacter)
            {
                break;        //       yield return new WaitForSeconds(1.0f);
            }
            counter += 1;
            yield return new WaitForSeconds(0.04f);
        }
        typingSoundEffect.Stop();

    }
	
}
