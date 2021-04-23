using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : Task
{
    public string maskString;
    public override NodeResult Execute()
    {

        GameObject go = tree.gameObject;
        Vector3 fwd = go.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        maskString = (string)tree.GetValue("Mask");
        LayerMask mask = LayerMask.GetMask(maskString);
        mask = ~mask;

        if (Physics.Raycast(go.transform.position, fwd, out hit, Mathf.Infinity, mask) && hit.transform.tag == "boid")
        {
            Debug.DrawRay(go.transform.position, go.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            tree.SetValue("Hit", hit.transform.gameObject);
        }

        return NodeResult.SUCCESS;
    }
}