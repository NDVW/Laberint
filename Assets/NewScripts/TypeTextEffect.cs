using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TypeTextEffect : MonoBehaviour {

    // Use this for initialization
    private TextMeshPro textmesh;
    int counter = 0;
    void Start()
    {   
        StartCoroutine(type());
    }
	IEnumerator type () {

        textmesh = GetComponent<TextMeshPro>();
        int totalVisibleCharacter = textmesh.textInfo.characterCount;
    //    int counter = 0;
        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacter + 1);
            textmesh.maxVisibleCharacters = visibleCount;
           if(visibleCount == totalVisibleCharacter)
            {
                break;        //       yield return new WaitForSeconds(1.0f);
            }
            counter += 1;
            yield return new WaitForSeconds(0.04f);
        }
	}
	
	// Update is called once per frame

}
