using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : BehaviorTree
{
    public GameObject[] waypoints;
    public int index;
    public float Speed = 5.0f;
    public float TurnSpeed = 5.0f;
    public float Accuracy = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        Selector TreeRoot = new Selector();
        Sequence Patrol = new Sequence();
        MoveTo MoveToWP = new MoveTo();

        //Wait Pause = new Wait();

        SelectNextGameObject PickNextWP = new SelectNextGameObject();
        PlayerChangeLane changeLane = new PlayerChangeLane();

        SetValue("Waypoints", waypoints);
        SetValue("Waypoint", waypoints[0]);
        SetValue("Index", 0);
        //SetValue("TimeToPause", 0.0f);
        SetValue("Speed", Speed);
        SetValue("TurnSpeed", TurnSpeed);
        SetValue("Accuracy", Accuracy);
        SetValue("Direction", 1);
        SetValue("TurnRequested", Turning.STRAIGHT);

        MoveToWP.TargetName = "Waypoint";
        // Pause.TimeToWaitKey = "TimeToPause";
        PickNextWP.ArrayKey = "Waypoints";
        PickNextWP.GameObjectKey = "Waypoint";
        PickNextWP.IndexKey = "Index";
        PickNextWP.DirectionKey = "Direction";


        Patrol.children.Add(MoveToWP);
        //Patrol.children.Add(Pause);
        Patrol.children.Add(PickNextWP);
        Patrol.children.Add(changeLane);
        TreeRoot.children.Add(Patrol);

        Patrol.tree = this;
        TreeRoot.tree = this;
        MoveToWP.tree = this;
        //Pause.tree = this;
        PickNextWP.tree = this;
        changeLane.tree = this;
        root = TreeRoot;
    }

    // Update is called once per frame
    public override void Update()
    {
        //SetValue("Speed", Speed);
        SetValue("TurnSpeed", TurnSpeed);
        SetValue("Accuracy", Accuracy);
        base.Update();

    }
}
