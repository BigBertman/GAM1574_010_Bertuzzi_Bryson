using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAI : BehaviorTree
{
    public GameObject[] waypoints;

    // Start is called before the first frame update
    void Start()
    {
        Sequence dumbAI = new Sequence();

        Wait Pause = new Wait();
        AITurn turn = new AITurn();


        SetValue("TimeToPause", 3.0f);

        Pause.TimeToWaitKey = "TimeToPause";

        dumbAI.children.Add(Pause);
        dumbAI.children.Add(turn);

        dumbAI.tree = this;
        Pause.tree = this;
        turn.tree = this;
        root = dumbAI;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
