using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleController {
    public float maxRiddleDistance = 10.0f;
    GameObject[] riddles = GameObject.FindGameObjectsWithTag("riddle");

    public string SolveRiddles(Transform _player_transform, string TextQuery) {        
        string response = "";        
        GameObject _ClosestRiddle = FindClosestRiddle(_player_transform);        
        return _ClosestRiddle.solve(TextQuery);        
    }
    
    public GameObject FindClosestRiddle(Transform _player_transform)
    {
        GameObject closest = null;        
        Vector3 position = _player_transform.position;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject riddle in riddles)
        {   
            float riddleDistance = (riddle.transform.position - position).sqrMagnitude;
            if (riddleDistance < shortestDistance && riddleDistance < maxRiddleDistance && riddle.Solved == false)
            {
                closest = riddle;
            }
        }
        return closest;
    }
}
