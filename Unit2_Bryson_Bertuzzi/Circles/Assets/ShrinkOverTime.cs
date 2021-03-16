using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{
    float Scale;
    // Start is called before the first frame update
    void Start()
    {
        Scale = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        Scale -= Time.deltaTime;
        if (Scale < 0)
        {
            Destroy(gameObject);
        }
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
