using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject boid;
    public float Speed = 5.0f;
    // Start is called before the first frame update
    void Update()
    {
        //Updates player's speed
        if (Input.GetKeyDown(KeyCode.W))
        {
            Speed += 1.0f;
            boid.GetComponent<Movement>().SetValue("Speed", Speed);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Speed -= 1.0f;
            boid.GetComponent<Movement>().SetValue("Speed", Speed);
        }

        //Checks which way player wants to turn
        if (Input.GetKeyDown(KeyCode.A))
        {
            boid.GetComponent<Movement>().SetValue("TurnRequested", Turning.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            boid.GetComponent<Movement>().SetValue("TurnRequested", Turning.RIGHT);
        }
    }
}
