using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAI : BehaviorTree
{
    // Start is called before the first frame update
    void Start()
    {
        Spin turn = new Spin();
        SetValue("TurretSpeed", 45.0f);
        turn.SpeedKey = "TurretSpeed";
        turn.tree = this;
        root = turn;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
