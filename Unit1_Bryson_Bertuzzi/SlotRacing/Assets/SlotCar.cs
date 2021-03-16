using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCar : MonoBehaviour
{
    public Catmul spline;
    //  Use this for initialization
    public float speed = 0.02f;
    public float maxrotationspeed = 5.0f; // radians
    public float angle;
    public float crashLimit = 25.0f;
    public int nextwaypoint = 1;
    public float closeEnough = 0.1f;
    bool offTrack = false;
    void Start()
    {
        Catmul spline = UnityEngine.Object.FindObjectOfType<Catmul>();

        Vector3 pos = spline.newPoints[0];
        gameObject.transform.position = pos;
        Vector3 next = spline.newPoints[1];
        Vector3 direction = next - pos;

        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, direction, maxrotationspeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    // Update is called once per frame
    void Update()
    {
        if (offTrack == false)
        {
            AdjustSpeed();
        }

        // try to move towards the next point
        // if close enough, select the subsequent point to get to.
        // we also need to know if we've gone to fast for the track.  For now, lets not worry about that, and just
        // animate along it.
        Vector3 pos = gameObject.transform.position;
        Vector3 next = spline.newPoints[nextwaypoint];
        Vector3 direction = next - pos;
        angle = Vector3.Angle(direction, gameObject.transform.forward);
        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, direction, maxrotationspeed * Time.deltaTime, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);

        if (Vector3.Distance(gameObject.transform.position, spline.newPoints[nextwaypoint]) < speed * Time.deltaTime)
        {
            nextwaypoint = (nextwaypoint + 1) % spline.newPoints.Count;
        }

        gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        CheckForCrash();
    }

    void AdjustSpeed()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            speed += 2.0f;
            if (speed > 30.0f)
            {
                speed = 30.0f;
            }
        }
        else if (d < 0f)
        {
            speed -= 2.0f;
            if (speed < 0.0f)
            {
                speed = 0.0f;
            }
        }
    }

    void CheckForCrash()
    {
        if (speed * Time.deltaTime * angle > crashLimit)
        {
            Debug.Log(speed * angle);
            Debug.Log("Crashed");
            speed = 0.0f;
        }
        else if (speed * Time.deltaTime * angle > crashLimit * .7f)
        {
            Debug.Log(speed * angle);
            Debug.Log("Skidding");
        }
    }
}