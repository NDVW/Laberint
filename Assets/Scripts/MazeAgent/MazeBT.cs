using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnitySteer.Behaviors;

using FluentBehaviourTree;

public class MazeBT : MonoBehaviour {

    private IBehaviourTreeNode tree;
	private float enemyKillPlayerDistance = 5f;
	private float helpTimeInterval = 2f;
    private float playerLostTime = 10f;
	private float playerAdvancedTime = 0.5f;
	private float timepassed = 0f;
	private Transform EndGate;  // Save EndGate Position
	private Transform player1; // Save Player Info
	private Transform enemy1; // Save Enemy Info
	private float MoveWallDistance = 5f; //wall will be moved up. Distance of the displacment
	private float currentWallLerptime = 0;  // time to control wall movement
	private float wallLerptime = 5; // time to control wall movement
	private bool isWallCoroutineStarted = false;

    // Use this for initialization
    void Start () {
		Debug.Log("MazeBT start");
		//Debug.Break();
        // Code here, e.g. GetComponent of GO
        // 
		//var init 
		
		//this.player1 = PlayerManager.instance.player.transform;
		GameObject eg = GameObject.Find("End");
		GameObject pl = GameObject.Find("Player");
		GameObject en = GameObject.Find("Enemy");
		this.EndGate = eg.transform;
		this.player1 = pl.transform;
		this.enemy1 = en.transform;
		// Building the tree
        var builder = new BehaviourTreeBuilder();

        this.tree = builder
			.Selector("")
				.Sequence("KillPlayerEndGame")
					.Condition("EnemyReachedPlayer", t => EnemyIsWithPlayer())             
					.Do("KillThePlayer", t => KillPlayer())
					.Do("EndOfGame", t => EndGame())

				.End()
				.Sequence("AdaptDifficulty")
					.Condition("HelpTime", t => IsTimeToHelp())
					.Sequence("HelpPlayer")
						//.Condition("PlayerProgressSlow", t => PlayerIsLost())
						.Condition("EnemyCloserToPlayer", t => EnemyIsCloserToPlayer())             
						.Do("RemoveWallForPlayer", t => RemoveWallForPlayer())
					.End()
					.Sequence("HelpEnemy")
						//.Condition("PlayerProgressFast", t => PlayerReachingGoalFast())
						.Do("RemoveWallForEnemy", t => RemoveWallForEnemy())
					.End()
				.End()
            .End()
            .Build();
        }
	// Update is called once per frame
	void Update () {
		Debug.Log("MazeBT update");
		//Debug.Break();
		var delta = new TimeData(Time.deltaTime);
		this.timepassed += delta.deltaTime;
        this.tree.Tick(new TimeData(Time.deltaTime));
		
	}



	/////////    CONDITIONS  ////////
	bool EnemyIsWithPlayer(){
		

		if (EnemyPlayerDistance() <= this.enemyKillPlayerDistance){
			Debug.Log("MazeBT Enemy is with player");
			Debug.Break();
			return true;
		}
		Debug.Log("MazeBT Enemy is not with player");
		//Debug.Break();
		return false;
	}

	bool IsTimeToHelp(){
		if (this.timepassed >= this.helpTimeInterval){
			this.timepassed = 0;
			Debug.Log("MazeBT it is time to help");
			Debug.Break();
			return true; 
		}
		Debug.Log("MazeBT it is NOT time to help");
		//Debug.Break();
		return false; 
	}

	bool PlayerIsLost() {
		// TBD
		return true; 
		return false; 
	}
	bool PlayerReachingGoalFast(){
		// TBD
		return true;
		return false;
	}

	bool EnemyIsCloserToPlayer(){
		if (EnemyPlayerDistance() <= GoalPlayerDistance()){
			Debug.Log("MazeBT Enemy is closer Enemy   Goal");
			Debug.Log(EnemyPlayerDistance());
			Debug.Log(GoalPlayerDistance());
			Debug.Break();
			return true; 
		}
		Debug.Log("MazeBT Enemy is NOT closer Enemy   Goal");
		Debug.Log(EnemyPlayerDistance());
		Debug.Log(GoalPlayerDistance());
		Debug.Break();
		return false; 

	}

	// BehaviourTreeStatus EnemyIsWithPlayer(){
		
	// 	if (EnemyPlayerDistance() <= this.enemyKillPlayerDistance){
	// 		return BehaviourTreeStatus.Success;
	// 	}
	// 	return BehaviourTreeStatus.Failure;
	// }
	// BehaviourTreeStatus IsTimeToHelp(){
	// 	if (this.timepassed >= this.helpTimeInterval){
	// 		this.timepassed = 0;
	// 		return BehaviourTreeStatus.Success;
	// 	}
	// 	return BehaviourTreeStatus.Failure;
	// }

	// BehaviourTreeStatus EnemyIsCloserToPlayer(){
	// 	if (EnemyPlayerDistance() <= GoalPlayerDistance()){
	// 		return BehaviourTreeStatus.Success;
	// 	}
	// 	return BehaviourTreeStatus.Failure;

	// }

	// BehaviourTreeStatus PlayerIsLost() {
	// 	// TBD
	// 	return BehaviourTreeStatus.Success;
	// 	return BehaviourTreeStatus.Failure;
	// }

	// BehaviourTreeStatus PlayerReachingGoalFast(){
	// 	// TBD
	// 	return BehaviourTreeStatus.Success;
	// 	return BehaviourTreeStatus.Failure;
	// }

	/////////    ACTIONS  ////////

	BehaviourTreeStatus KillPlayer(){
		// Kill player Code  TBD
		Debug.Log("MazeBT Kill the player");
		Debug.Break();
		return BehaviourTreeStatus.Success;
	}
	BehaviourTreeStatus EndGame(){
		// End Game Code  TBD
		Debug.Log("Maze BT End Game");
		Debug.Break();
		return BehaviourTreeStatus.Success; 
	}
	BehaviourTreeStatus RemoveWallForPlayer(){
		Debug.Log("Maze BT Removing Wall");
		Debug.Break();
		RaycastHit hit;
		string wallName;
		if (Physics.Raycast(this.player1.position, (this.EndGate.position - this.player1.position).normalized, out hit))
        {
			Debug.DrawRay(this.player1.position,  (this.EndGate.position - this.player1.position).normalized * hit.distance, Color.red);
            Debug.Log("Did Hit");
            Debug.Log(hit.collider.name);
            wallName = hit.collider.name;
            GameObject wallToOpen = GameObject.Find(wallName);
            Debug.Log("First Line is " + wallToOpen);
            var start = wallToOpen.transform.position; //start position of the wall
            var end = wallToOpen.transform.position + Vector3.up * this.MoveWallDistance;  //End position of the wall

            //distance_player_wall = Vector3.Distance(target.position, wall.transform.position);  //I think this was not used

            if (!this.isWallCoroutineStarted)
            {
                StartCoroutine(RemoveWall(start, end, wallToOpen));
            }
           
        }
        // Debug.Log(h.collider.name);
		return BehaviourTreeStatus.Success;

	}

	BehaviourTreeStatus RemoveWallForEnemy(){
		RaycastHit hit;
		string wallName;
		if (Physics.Raycast(this.enemy1.position, (this.player1.position - this.enemy1.position).normalized, out hit))
        {
			Debug.DrawRay(this.enemy1.position,  (this.player1.position - this.enemy1.position).normalized * hit.distance, Color.red);
            Debug.Log("Did Hit");
            Debug.Log(hit.collider.name);

            wallName = hit.collider.name;
            GameObject wallToOpen = GameObject.Find(wallName);
            Debug.Log("First Line is " + wallToOpen);

            var start = wallToOpen.transform.position; //start position of the wall
            var end = wallToOpen.transform.position + Vector3.up * this.MoveWallDistance;  //End position of the wall

            if (!this.isWallCoroutineStarted)
            {
                StartCoroutine(RemoveWall(start, end, wallToOpen));
            }
           
        }
        // Debug.Log(h.collider.name);
		return BehaviourTreeStatus.Success;
		return BehaviourTreeStatus.Success;

	}

	float EnemyPlayerDistance(){
		float distance_enemy_palyer = Vector3.Distance(this.player1.position, this.enemy1.position);
		return distance_enemy_palyer;

	}
	float GoalPlayerDistance(){
		Vector3 distance_endgate11 = this.EndGate.position;
		Vector3 distance_endgate12 = this.player1.position;
		float distance_endgate = Vector3.Distance(this.player1.position, this.EndGate.position);
		return distance_endgate;
	}

	IEnumerator RemoveWall(Vector3 start, Vector3 end, GameObject wall)
    {
            this.isWallCoroutineStarted = true;
            
            this.currentWallLerptime += Time.deltaTime;
            if (this.currentWallLerptime >= this.wallLerptime)
            {
                this.currentWallLerptime = this.wallLerptime;
            }
            float perc = this.currentWallLerptime / this.wallLerptime;
            wall.transform.position = Vector3.Lerp(start, end, perc);
            yield return null;
           if (wall.transform.position == end)
           {
              this.isWallCoroutineStarted = false;
           }

    }

}
