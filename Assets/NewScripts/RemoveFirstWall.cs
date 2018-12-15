using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFirstWall : MonoBehaviour {
    GameObject target;
    //Transform target;
    GameObject EndGate;
    GameObject wall;
    string wallName;
    Vector3 start;
    Vector3 end;
    string planeName;
    GameObject Light;
    GameObject light;
    RaycastHit hit;
    public Material material;
    GameObject LightWall;
    private float Move_distance = 5f;
    private float lerptime = 2;
    private float currentLerptime = 0;
    float distance_player_wall;
    bool isCoroutineStarted = false;
    Renderer rend;
    // Use this for initialization
    void Start()
    {
       /// target = PlayerManager.instance.player.transform;
        EndGate = GameObject.Find("End");
     //   Debug.Log(target);
        target = GameObject.Find("Player");
        if (Physics.Raycast(transform.position , transform.TransformDirection(Vector3.forward).normalized, out hit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log(hit);
            planeName = hit.collider.name;
            Light = GameObject.Find(planeName);
            LightWall = Light.transform.parent.gameObject;
            wall = LightWall.transform.parent.gameObject;
            int children = LightWall.transform.childCount;
            Debug.Log("First Line is " + LightWall);
            Debug.Log(children);
            for (int i = 0; i < children; ++i) {
                light = LightWall.transform.GetChild(i).gameObject;
                rend = light.GetComponent<Renderer>();
         //       rend.sharedMaterial = material;
                    }
    
          //  start = wall.transform.position;
          //  end = wall.transform.position + Vector3.up * Move_distance;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
