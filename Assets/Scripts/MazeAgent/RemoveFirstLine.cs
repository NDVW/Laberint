﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFirstLine : MonoBehaviour
{

    public float lookRadius;
    Transform target;
    private Vector3 wallDir;
    GameObject wall;
    GameObject prev;
    string planeName;
    GameObject LightWall;
    private Vector3 start;
    private Vector3 end;
    private float AngleThreshold;
    private float Move_distance = 5f;
    private float lerptime = 2;
    private float currentLerptime = 0;
    float distance_player_wall;
    bool isCoroutineStarted = false;
    RaycastHit hit;
    FIndClosestWall find = new FIndClosestWall();
    // Use this for initialization
    bool foundFirstWall = false;
    private GameObject EndGate;
    string wallName;
    void Start()
    {
        var find = new FIndClosestWall();
        target = PlayerManager.instance.player.transform;
        EndGate = GameObject.Find("End");

    }

    // Update is called once per frame
    void Update()
    {   if (!foundFirstWall) { 
            float distance_endgate = Vector3.Distance(target.position, EndGate.transform.position);
            if (Physics.Raycast(target.position, (EndGate.transform.position - target.position).normalized, out hit))
            {
                // Debug.Log(hit.collider.name);
                planeName = hit.collider.name;
                LightWall = GameObject.Find(planeName);
                wall = LightWall.transform.parent.gameObject;
                Debug.Log("First Line is " + wall);

                foundFirstWall = true;
                start = wall.transform.position;
                end = wall.transform.position + Vector3.up * Move_distance;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentLerptime += Time.deltaTime;
                    if (currentLerptime >= lerptime)
                    {
                        currentLerptime = lerptime;
                    }
                    float perc = currentLerptime / lerptime;
                    wall.transform.position = Vector3.Lerp(start, end, perc);
                    if (wall.transform.position == end)
                    {
                        foundFirstWall = false;
                    }
              //      distance_player_wall = Vector3.Distance(target.position, wall.transform.position);

                }
            }
                //    if (!isCoroutineStarted)
                //    {
                //         StartCoroutine(Remove(start, end, wall, 0, 5));
                //       }
                // wallDir = (wall.transform.position - target.position);
                //  float angle = Vector3.Dot(target.forward, wallDir);
                //   if (angle > 0.4)
                //   {
                //       if (distance_player_wall < 10)

                //   {
                //  currentLerptime += Time.deltaTime; 
                //  if (currentLerptime >= lerptime)
                ////          {
                //             currentLerptime = lerptime;
                //    float perc = currentLerptime / lerptime;
                //   wall.transform.position = Vector3.Lerp(start, end, perc);
                //   }
                // }
            }





        }


    
   IEnumerator Remove(Vector3 start, Vector3 end, GameObject wall, float currentLerptime, float lerptime)
    {
            isCoroutineStarted = true;
            
            currentLerptime += Time.deltaTime;
            if (currentLerptime >= lerptime)
            {
                currentLerptime = lerptime;
            }
            float perc = currentLerptime / lerptime;
            wall.transform.position = Vector3.Lerp(start, end, perc);
            yield return null;
           if (wall.transform.position == end)
           {
              isCoroutineStarted = false;
           }

    }
}