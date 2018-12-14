﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleController : MonoBehaviour {

    // Use this for initialization
  //  GameObject gos;
    AnimatorStateInfo info;
    Animator anim;
    GameObject riddle;
    float distance;
    Renderer rend;
    bool clipInfo;
    Material defaultMaterial;
    Material HologramMaterial;
    public Material[] UserMaterials;

    Material material;
    GameObject[] riddles;
    GameObject currentRiddle;
    Material restoreMaterial;
    FindClosestRiddle riddleFinder = new FindClosestRiddle();
    void Start () {
    //    riddle = GameObject.Find("Riddle2");
       
        
        riddles = GameObject.FindGameObjectsWithTag("riddle");
        rend = riddles[1].GetComponent<Renderer>();
        defaultMaterial = rend.material;
        HologramMaterial = defaultMaterial;
        // rend.sharedMaterial = material[0];
    }
	
	// Update is called once per frame
	void Update () {
        currentRiddle = riddleFinder.FindRiddle(riddles, transform);
        anim = currentRiddle.GetComponent<Animator>();
        rend = currentRiddle.GetComponent<Renderer>();
        HologramMaterial = rend.material;
        if(defaultMaterial != HologramMaterial)
        {
            restoreMaterial = defaultMaterial;
        }
        defaultMaterial = HologramMaterial;
        distance = Vector3.Distance(currentRiddle.transform.position, transform.position);
        if (distance < 3)
        {
            anim.SetInteger("moveback", 0);
            rend.sharedMaterial = UserMaterials[0];
            //   riddle.SetActive(true);
            anim.SetInteger("appear", 1);
            info = anim.GetCurrentAnimatorStateInfo(0);
            
            clipInfo = info.IsName("end");
            if (clipInfo == true)
            {
               
                currentRiddle.transform.GetChild(0).gameObject.SetActive(true);
            }

            
        }
        if (clipInfo == true && distance > 3)
        {
            anim.SetInteger("moveback", 1);
            anim.SetInteger("appear", 0);
       //     rend.sharedMaterial = restoreMaterial;
            currentRiddle.transform.GetChild(0).gameObject.SetActive(false);
        }
 
    }
}
