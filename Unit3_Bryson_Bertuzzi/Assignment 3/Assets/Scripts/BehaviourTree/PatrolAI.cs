using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : BehaviorTree
{

    //public TankSpawner spline;

    public GameObject[] waypoints;
    public int index;
    public float Speed;
    public float TurnSpeed;
    public float Accuracy;
    //construct the actual tree
    void Start()
    {

        // create nodes
        Selector TreeRoot = new Selector();
        Sequence Patrol = new Sequence();
        MoveTo MoveToWP = new MoveTo();
        Wait Pause = new Wait();

        // TODO Create a spin instance
        //Spin Spinner = new Spin();


        SelectNextGameObject PickNextWP = new SelectNextGameObject();
        // create blackboard keys and initialize them with values
        // NOTE - SHOULD BE USING CONSTANTS
        TurnSpeed = 2.0f;
        Speed = 5.0f;
        Accuracy = 1.5f;
        SetValue("Waypoints", waypoints);
        SetValue("CurrentWaypoint", waypoints[0]);
        SetValue("WaypointIndex", 0);
        SetValue("TimeToPause", 0.0f);
        SetValue("Speed", Speed);
        SetValue("TurnSpeed", TurnSpeed);
        SetValue("Accuracy", Accuracy);
        //SetValue("SpinSpeed", 180.0f);
        // set node parameters - connect them to the blackboard
        MoveToWP.TargetName = "CurrentWaypoint";
        Pause.TimeToWaitKey = "TimeToPause";
        PickNextWP.ArrayKey = "Waypoints";
        PickNextWP.GameObjectKey = "CurrentWaypoint";
        PickNextWP.IndexKey = "WaypointIndex";
        //Spinner.SpeedKey = "SpinSpeed";
        // connect nodes
        Patrol.children.Add(MoveToWP);
        Patrol.children.Add(Pause);

        // TODO add the spin to the children of partol (twice)
        //Patrol.children.Add(Spinner);

        Patrol.children.Add(PickNextWP);
        TreeRoot.children.Add(Patrol);
        Patrol.tree = this;
        TreeRoot.tree = this;
        MoveToWP.tree = this;
        Pause.tree = this;

        // TODO Set the tree for spin object as well
        //Spinner.tree = this;

        PickNextWP.tree = this;
        root = TreeRoot;

    }

    // we don't need an update - the base class will deal with that.
    // but, since we have one, we can use it to set some of the moveto parameters on the fly
    // which lets us tweak them in the inspector
    public override void Update()
    {
        SetValue("Speed", Speed);
        SetValue("TurnSpeed", TurnSpeed);
        SetValue("Accuracy", Accuracy);
        base.Update();
    }

    //public void setter(GameObject[] pb)
    //{
    //    waypoints = pb;
    //}


}
