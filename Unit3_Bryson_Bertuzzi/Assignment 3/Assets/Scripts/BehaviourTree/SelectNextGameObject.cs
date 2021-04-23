using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNextGameObject : Node {
    public string ArrayKey;
    public string IndexKey;
    public string GameObjectKey;
    // Use this for initialization

    public override NodeResult Execute()
    {
        int index = (int)tree.GetValue(IndexKey);
        GameObject[] goa = (GameObject[])(tree.GetValue(ArrayKey));
        index++;
        if (index >= goa.Length)
        {
            index = 0;
        }
        tree.SetValue(IndexKey, index);
        tree.SetValue(GameObjectKey, goa[index]);
        return NodeResult.SUCCESS;
    }

    public override void Reset()
    {
        GameObject[] goa = (GameObject[])(tree.GetValue(ArrayKey));
        tree.SetValue(GameObjectKey, goa[0]);
        base.Reset();
    }
}
