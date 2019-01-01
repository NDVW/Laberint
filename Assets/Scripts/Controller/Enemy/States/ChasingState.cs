using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : FSMState<EnemyController>
{
    AnimatorStateInfo info;
    bool clipInfo;

    private static ChasingState instance = null;
    public static ChasingState Instance()
    {
        {
            if (instance == null)
                instance = new ChasingState();

            return instance;
        }
    }

    private ChasingState() { }

    public override void Enter(EnemyController entity)
    {
        entity.SetSpeed(5);
        entity.ChangeAnimation("Run", 1);
    }

    public override void Execute(EnemyController entity)
    {
        GameObject nearestSmell = FindSmellPoints.FindSmell(
            entity.GetPosition(), "playerScent", 30, 
            entity.GetCurrentDestination());
        if(nearestSmell == null)
        {
            entity.FiniteStateMachine.ChangeState(MovingState.Instance());
        }
        else
        {
            entity.SetPoint(nearestSmell.transform.position);
        }
    }

    public override void Exit(EnemyController entity)
    {
        entity.GotoNextPoint();
    }

}
