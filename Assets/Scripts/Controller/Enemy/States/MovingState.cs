using UnityEngine;
using System.Collections;

public class MovingState : FSMState<EnemyController>
{
    private static MovingState instance = null;
    public static MovingState Instance()
    {
        {
            if (instance == null)
                instance = new MovingState();

            return instance;
        }
    }

    private MovingState() { }
    AnimatorStateInfo info;
    bool clipInfo;

    public override void Enter(EnemyController entity)
    {
        entity.SetSpeed(1);
        entity.ChangeAnimation("Walk", 1);
        entity.ChangeAnimation("Run", 0);
    }

    public override void Execute(EnemyController entity)
    {
        info = entity.GetAnimatorStateInfo(0);
        clipInfo = info.IsName("Twist");

        if (clipInfo == false)
        {
            if (!entity.agent.pathPending && entity.agent.remainingDistance < 0.5f)
                entity.GotoNextPoint();
        }
        // Detect player's scent
        GameObject nearestSmell = FindSmellPoints.FindSmell(
        entity.GetPosition(), "playerScent", 30, 
            entity.GetCurrentDestination());
        if (nearestSmell != null)
        {
            entity.FiniteStateMachine.ChangeState(ChasingState.Instance());
        }
    }

    public override void Exit(EnemyController entity)
    {
       
    }

}
