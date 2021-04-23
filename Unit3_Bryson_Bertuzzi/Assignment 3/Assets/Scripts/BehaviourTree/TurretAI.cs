using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : BehaviorTree
{
    // Start is called before the first frame update

    FindTargetTask find;

    void Start()
    {
        Selector TreeRoot = new Selector();
        Sequence Fire = new Sequence();

        find = new FindTargetTask();
        //Spin turn = new Spin();
        Wait reload = new Wait();

       // SetValue("TurretSpeed", 45.0f);
        SetValue("TimeToPause", 0.25f);

       // turn.SpeedKey = "TurretSpeed";
        reload.TimeToWaitKey = "TimeToPause";
        //find.muzzle = transform;

        Fire.children.Add(find);
        Fire.children.Add(reload);
        //Fire.children.Add(turn);


       // TreeRoot.children.Add(par);

        TreeRoot.children.Add(Fire);
        //TreeRoot.children.Add(turn);

        TreeRoot.tree = this;

        //turn.tree = this;
        find.tree = this;
        reload.tree = this;
        Fire.tree = this;
        //par.tree = this;

        root = TreeRoot;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
