using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindSmellPoints {
    /// Finds the strongest smell closes to the enemy. Smell has to be between the radius.
    public static GameObject FindSmell(Vector3 position, string tag, float radius,
    Vector3 currentDestination)
    {

        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> scentsInRange = new List<GameObject>();
        foreach(GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance <= radius)
            {
                scentsInRange.Add(go);
            }
        }


        GameObject closest = null;
        float strongest = 0;
        foreach (GameObject sc in scentsInRange)
        {
            float strength = sc.GetComponent<ScentController>().intensity;
            if (strength > strongest)
            {
                closest = sc;
                strongest = strength;
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
