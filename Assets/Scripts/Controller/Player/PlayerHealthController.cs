using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerHealthController : MonoBehaviour {

    public PostProcessingProfile chasedBehaviour;
    public PostProcessingProfile nearBehaviour;
    public PostProcessingProfile defaultProfile;
    public int chasingDistance;
    public int nearDistance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool chased = false;
        GameObject[] gos = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject go in gos)
        {
            Vector3 enemyPosition = go.transform.position;
            float dist = Vector3.Distance(enemyPosition, transform.position);
            if (dist < nearDistance)
            {
                Camera.main.GetComponent<PostProcessingBehaviour>().profile = nearBehaviour;
                chased = true;
                break;
            }
            if (dist < chasingDistance)
            {
                Camera.main.GetComponent<PostProcessingBehaviour>().profile = chasedBehaviour;
                chased = true;
                break;
            }
        }
        if (!chased)
        { 
            Camera.main.GetComponent<PostProcessingBehaviour>().profile = defaultProfile;
        }


    }
}
