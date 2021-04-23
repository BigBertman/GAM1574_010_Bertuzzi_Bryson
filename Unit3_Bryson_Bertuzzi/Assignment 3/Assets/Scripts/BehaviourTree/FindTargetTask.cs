using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetTask : Task
{
    public Transform muzzle;

    LineRenderer line;

    public int ShootCount = 0;


    public override NodeResult Execute()
    {
        GameObject Go = tree.gameObject;

        if (Go != null)
        {
            muzzle = Go.transform.Find("Turret/Cylinder (1)/MuzzlePoint");
        }

        line = Go.GetComponent<LineRenderer>();

        //line.positionCount = 0;

        RaycastHit hit;
        Vector3 fwd = muzzle.TransformDirection(Vector3.forward);

        if (Physics.Raycast(muzzle.position, fwd, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Target"))
            {
                line.positionCount = 2;
                line.SetPosition(0, muzzle.position);
                line.SetPosition(1, hit.transform.position);

                //Debug.DrawRay(muzzle.position, muzzle.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");

                ShootCount++;

                if (ShootCount >= 2)
                {
                    TargetSpawner.Destroy(hit.transform.gameObject);
                }
                return NodeResult.SUCCESS;
            }
        }
        else
        {
            //Debug.DrawRay(muzzle.position, fwd * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
        return NodeResult.RUNNING;
    }

    public override void Reset()
    {
        line = tree.gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        base.Reset();
    }
}

