using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Node // can only put tasks under here - anything else is not permitted
{
    NodeResult childResult;
    List<Node> StillRunning = null;
    int numberrunning;
    public override NodeResult Execute()
    {
        for (int i = 0; i < StillRunning.Count; i++)
        {
            currentChild = i;
            if (StillRunning[i] != null)
            {
                tree.PushNodeWithOutReset(StillRunning[i]);
                NodeResult result = tree.RunStack();
                if (result == NodeResult.SUCCESS) // will have cleared the top off the tree
                {
                    StillRunning[i] = null;
                    numberrunning--;
                }
                else if (result == NodeResult.FAILURE)
                {
                    Reset();
                    return NodeResult.FAILURE;
                }
                else
                {   // if it's made it here, it's either still working, or tried to put something else on the tree.  Since we only have taks under a parallel, we know it's not that.
                    // so wipe him off the tree to make space fot the next child
                    tree.PopTop();
                }
            }
        }
        if (numberrunning <= 0)
        {
            Reset();
            return NodeResult.SUCCESS;
        }
        return NodeResult.RUNNING;

    }

    public override void Reset()
    {
        if (StillRunning  == null)
        {
            StillRunning = new List<Node>();
        }
        StillRunning.Clear();
        numberrunning = 0;
        foreach (Node child in children)
        {
            child.Reset();
            StillRunning.Add(child);
            numberrunning++;
        }
        base.Reset();
    }


    public override bool SetChildResult(NodeResult result)
    {
        if (result == NodeResult.SUCCESS || result == NodeResult.FAILURE)
        {
            StillRunning[currentChild].Reset();
            StillRunning[currentChild] = null;
            numberrunning--;
        }
        childResult = result;
        return false;
    }
}
