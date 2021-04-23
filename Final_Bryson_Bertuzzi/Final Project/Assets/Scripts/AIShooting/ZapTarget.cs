using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapTarget : Task
{
    LineRenderer line;
    public GameObject otherBoid;

    public override NodeResult Execute()
    {
        otherBoid = (GameObject)tree.GetValue("Hit");

        if (otherBoid != null)
        {
            GameObject go = tree.gameObject;
            line = go.GetComponent<LineRenderer>();

            line.positionCount = 2;
            line.SetPosition(0, go.transform.position);
            line.SetPosition(1, otherBoid.transform.position);
        }

        return NodeResult.SUCCESS;
    }
}