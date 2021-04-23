using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BehaviorTree
{
    public GameObject hitResult;
    public string mask;

    // Start is called before the first frame update
    void Start()
    {
        Sequence ShootAI = new Sequence();
        Wait Pause = new Wait();
        DetectTarget detect = new DetectTarget();
        ZapTarget zap = new ZapTarget();
        DamageBoid damage = new DamageBoid();

        SetValue("TimeToPause", 1.0f);
        SetValue("Hit", hitResult);
        SetValue("Mask", mask);
        Pause.TimeToWaitKey = "TimeToPause";

        ShootAI.children.Add(Pause);
        ShootAI.children.Add(detect);
        ShootAI.children.Add(zap);
        ShootAI.children.Add(damage);

        ShootAI.tree = this;
        Pause.tree = this;
        detect.tree = this;
        zap.tree = this;
        damage.tree = this;
        root = ShootAI;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
