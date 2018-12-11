using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ShowPath : MonoBehaviour {
  //  private NavMeshAgent agent;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    private Color c = Color.white;
    GameObject checkpoint;
    FindFloorTrack findtrack = new FindFloorTrack();
    GameObject track;
    GameObject[] points;
    Renderer rend;
    Vector3 surfacePoint1;
    Vector3 surfacePoint2;
    float len;
    Collider collider1;
    Collider collider2;
    float distance_player_checkpoint_first;

    // Use this for initialization
    void Start () {
    path = new NavMeshPath();
    // agent = GetComponent<NavMeshAgent>();
    collider1 = GetComponent<Collider>();
    checkpoint = GameObject.Find("checkpoint");
   // points = GameObject.FindGameObjectsWithTag("floorarrow");
    points = GameObject.FindGameObjectsWithTag("floortrack");
        //  foreach (GameObject go in points)
        //   {
        //        go.SetActive(false);
        //    }
     NavMesh.CalculatePath(transform.position, checkpoint.transform.position, NavMesh.AllAreas, path);
     len = path.corners.Length;
     distance_player_checkpoint_first = Vector3.Distance(transform.position, checkpoint.transform.position);
   
     Debug.Log(path.corners.Length);
    }

    // Update is called once per frames
    void Update()
    {
        float distance_player_checkpoint = Vector3.Distance(transform.position, checkpoint.transform.position);
        if(distance_player_checkpoint < 3)
        {
            this.enabled = false;
        }
        NavMesh.CalculatePath(transform.position, checkpoint.transform.position, NavMesh.AllAreas, path);
        Debug.Log(path.corners.Length);
        if (path.corners.Length <= len )//&& distance_player_checkpoint <= distance_player_checkpoint_first)
       {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                track = findtrack.FindTrack(points, path.corners[i]);
                rend = track.GetComponent<Renderer>();
                collider2 = track.GetComponent<Collider>();
                float distance_track_point = Vector3.Distance(track.transform.position, path.corners[i]);
                if (distance_track_point < 7)
                {
                    surfacePoint2 = collider2.ClosestPointOnBounds(transform.position);
                    surfacePoint1 = collider1.ClosestPointOnBounds(track.transform.position);
                    float distance = Vector3.Distance(surfacePoint1, surfacePoint2); 
                    float distance_transform= Vector3.Distance(track.transform.position, transform.position);
                    if (distance < 1)
                    {
                        rend.enabled = true;
                        //  track.SetActive(true);
                    }
                   // if (distance_transform >6) { rend.enabled = false; }
                }  //   Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
          }
        }
    }
}


