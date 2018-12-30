using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : FSMState<EnemyController>
{
    private static AttackingState instance = null;
    public static AttackingState Instance()
    {
        {
            if (instance == null)
                instance = new AttackingState();

            return instance;
        }
    }
    public override void Enter(EnemyController entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute(EnemyController entity)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit(EnemyController entity)
    {
        throw new System.NotImplementedException();
    }

}

