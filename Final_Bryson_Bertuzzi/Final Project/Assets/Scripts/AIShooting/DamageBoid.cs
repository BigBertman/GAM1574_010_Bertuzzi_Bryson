using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoid : Task
{
    public GameObject otherBoid;

    public override NodeResult Execute()
    {
        otherBoid = (GameObject)tree.GetValue("Hit");

        if (otherBoid != null)
        {
            otherBoid.GetComponent<Boid>().speed *= 1.75f;
            otherBoid.GetComponent<Boid>().turnspeed *= 0.5f;
        }

        return NodeResult.SUCCESS;
    }
}
