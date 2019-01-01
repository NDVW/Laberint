using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindSmellPoints {
    /// Finds the smell closes to the enemy. Smell has to be between the radius.
    public static GameObject FindSmell(Vector3 position, string tag, float radius,
    Vector3 currentDestination)
    {
        
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            if(currentDestination != go.transform.position)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if ((curDistance < distance) && (curDistance <= radius))
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    public static int PointsInRange(Vector3 position, string tag, float radius)
    {
        int res = 0;
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance <= radius)
            {
                res++;
            }
        }
        return res;
    }
}
