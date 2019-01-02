using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RiddleController : MonoBehaviour {    
    public float maxRiddleDistance = 10.0f;
    public int _RiddleDistanceThreshold = 3;
    public Material material;
    Renderer rend;
    private GameObject[] riddles;

    void Start () {
        riddles = GameObject.FindGameObjectsWithTag("riddle");
        
    }
	
	void Update () {  
        GameObject closestRiddle = FindClosestRiddle(transform, riddles);
        if (closestRiddle != null) AnimateRiddle(closestRiddle, transform);
    }

    public GameObject FindClosestRiddle(Transform _player_transform, GameObject[] riddles)
    {
        float riddleDistance;
        float closestRiddleDistance = Mathf.Infinity;
        GameObject closestRiddle = null;
        
        foreach (GameObject riddle in riddles) {
            riddleDistance = (riddle.transform.position - _player_transform.position).sqrMagnitude;
            
            if (riddleDistance < closestRiddleDistance)
            {
                closestRiddleDistance = riddleDistance;
                closestRiddle = riddle;
            }
        }
        return closestRiddle;
    }
 // Aniamte the riddles when the player is close to them.

    void AnimateRiddle (GameObject riddle, Transform _playerTransform) {
        Animator anim = riddle.GetComponent<Animator>();
        rend = riddle.GetComponent<Renderer>();
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0); 
        float distance = Vector3.Distance(riddle.transform.position, _playerTransform.position);
        Vector3 wallDir = (riddle.transform.position - _playerTransform.position);        
    
        if (distance < _RiddleDistanceThreshold)
        {
            anim.SetInteger("moveback", 0);
            anim.SetInteger("appear", 1);
            info = anim.GetCurrentAnimatorStateInfo(0);
            if (riddle.name != "door")
            {
                rend.sharedMaterial = material;
            }
            if (info.IsName("end") == true) DisplayRiddleText(riddle, true);
        }
        if (info.IsName("end") == true && distance > _RiddleDistanceThreshold)
        {
            anim.SetInteger("moveback", 1);
            anim.SetInteger("appear", 0);                
            DisplayRiddleText(riddle, false);
        }
	}

    void DisplayRiddleText(GameObject riddle, bool active) {
        riddle.transform.GetChild(0).gameObject.SetActive(active);
    }
    // Solve riddles by matching the player voice input to the riddle answers. If Correct, make the rewards available for the player.
    public void SolveRiddles(string PlayerQuery) {
        GameObject closestRiddle = FindClosestRiddle(transform, riddles);
        
        if (closestRiddle != null) {
            Riddle riddle = closestRiddle.GetComponent<Riddle>();

            if (PlayerQuery.ToLower().Contains(riddle.answer.ToLower())) {
                riddle.Solved = true;
            } else {
                riddle.Solved = false;
            }
        }
    }
}
