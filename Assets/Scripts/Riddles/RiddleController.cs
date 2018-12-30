using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RiddleController : MonoBehaviour {    
    AnimatorStateInfo info;
    Animator anim;
    float distance;
    Renderer rend;
    bool clipInfo;
    Material defaultMaterial;
    Material HologramMaterial;
    public Material[] UserMaterials;
    Material material;
    Material restoreMaterial;
    private Vector3 wallDir;
    public float maxRiddleDistance = 10.0f;

    void Start () {
        GameObject[] riddles = GameObject.FindGameObjectsWithTag("riddle");
        rend = riddles[1].GetComponent<Renderer>();
        defaultMaterial = rend.material;
        HologramMaterial = defaultMaterial;        
    }
	
	void Update () {        
        GameObject riddle = FindClosestRiddle(transform, GameObject.FindGameObjectsWithTag("riddle"));
        
        if (riddle != null) {            
            anim = riddle.GetComponent<Animator>();
            rend = riddle.GetComponent<Renderer>();
            HologramMaterial = rend.material;
            
            if(defaultMaterial != HologramMaterial)
            {
                restoreMaterial = defaultMaterial;
            }
            defaultMaterial = HologramMaterial;
            distance = Vector3.Distance(riddle.transform.position, transform.position);
            wallDir = (riddle.transform.position - transform.position);
            float angle = Vector3.Dot(transform.forward, wallDir);
            
            if (angle > 0.8)
            {
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

                        riddle.transform.GetChild(0).gameObject.SetActive(true);
                    }


                }
                if (clipInfo == true && distance > 3)
                {
                    anim.SetInteger("moveback", 1);
                    anim.SetInteger("appear", 0);
                    //     rend.sharedMaterial = restoreMaterial;
                    riddle.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    public GameObject FindClosestRiddle(Transform _player_transform, GameObject[] riddles)
    {
        float riddleDistance;
        float closestRiddleDistance = Mathf.Infinity;
        GameObject closestRiddle = null;
        
        foreach (GameObject riddle in riddles) {
            riddleDistance = (riddle.transform.position - _player_transform.position).sqrMagnitude;
            
            if (riddleDistance < closestRiddleDistance) // && riddle.Solved == false
            {
                closestRiddleDistance = riddleDistance;
                closestRiddle = riddle;
            }
        }
        return closestRiddle;
    }

    public string SolveRiddles(Transform _player_transform, string TextQuery) {        
        GameObject closestRiddle = FindClosestRiddle(_player_transform, GameObject.FindGameObjectsWithTag("riddle"));
        return TextQuery; // _ClosestRiddle.solve(TextQuery);
    }
}
