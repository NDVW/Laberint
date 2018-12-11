using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Controls the Enemy AI */

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 10f;  // Detection range for player

    Transform target;   // Reference to the player
    NavMeshAgent agent; // Reference to the NavMeshAgent
    Animator anim;
    GameObject PlayerSmell;
    private float moveSpeed = 2f; 
    FIndPlayerSmell findplayer = new FIndPlayerSmell();
    //  CharacterCombat combat;

    // Use this for initialization
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 1);
        // combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        // Distance to the target
       PlayerSmell = findplayer.FIndPlayer("PlayerSmell");
        if (PlayerSmell != null)
        {
            float distance = Vector3.Distance(PlayerSmell.transform.position, transform.position);
            if (distance <= lookRadius)
            {
                if (PlayerSmell.activeSelf)
                {
                    FaceTarget();
                    agent.SetDestination(PlayerSmell.transform.position);
  

                    anim.SetInteger("Run", 1);
                     
                }
                else anim.SetInteger("Run", 0);
            }
        }
        else anim.SetInteger("Run", 0);
    }
       
    void GoTonext()
    {

    }

    // Rotate to face the target
    void FaceTarget()
    {
        //   Vector3 direction = (PlayerSmell.transform.position - transform.position).normalized;
        Vector3 direction = (PlayerSmell.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Show the lookRadius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}