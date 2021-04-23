using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : Task
{
    public int lane = 0;

    public override NodeResult Execute()
    {
        lane = Random.Range(1, 4);
        if (lane == 1)
        {
            tree.GetComponent<Movement>().SetValue("TurnRequested", Turning.LEFT);
        }
        else if (lane == 2)
        {
            tree.GetComponent<Movement>().SetValue("TurnRequested", Turning.STRAIGHT);
        }
        else if (lane == 3)
        {
            tree.GetComponent<Movement>().SetValue("TurnRequested", Turning.RIGHT);
        }

        return NodeResult.SUCCESS;
    }
}