using UnityEngine;
using System.Collections;

public class MovingState : FSMState<EnemyController>
{
    AnimatorStateInfo info;
    bool clipInfo;

    public override void Enter(EnemyController entity)
    {

    }

    public override void Execute(EnemyController entity)
    {
        info = entity.GetAnimatorStateInfo(0);
        clipInfo = info.IsName("Twist");
        Debug.Log(clipInfo);

        if (clipInfo == false)
        {
            if (!entity.agent.pathPending && entity.agent.remainingDistance < 0.5f)
                entity.GotoNextPoint();
        }
    }

    public override void Exit(EnemyController entity)
    {
       
    }

}
