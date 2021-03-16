using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCar : MonoBehaviour
{
    public Catmul spline;
    public Catmul[] Spline;
    // Use this for initialization
    public float speed = 0.5f;
    public float maxrotationspeed = 0.4f; // radians
    public float angle;
    public float crashLimit = 5.0f;
    public int nextwaypoint = 1;
    public float closeEnough = 0.1f;

    public int currentdirection = 1;
    public int newdirection = 1;

    public int track = 1;

    void Start()
    {
        Vector3 pos = spline.getPoint(0, track);
        gameObject.transform.position = pos;
        Vector3 next = spline.getPoint(1, track);
        Vector3 direction = next - pos;

        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, direction, maxrotationspeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        BoidSpawner bs = GetComponent<BoidSpawner>();
        bs.SpawnBoids(10);
    }

    // Update is called once per frame
    void Update()
    {

        // try to move towards the next point
        // if close enough, select the subsequent point to get to.
        // we also need to know if we've gone to fast for the track.  For now, lets not worry about that, and just
        // animate along it.
        Vector3 pos = gameObject.transform.position;
        Vector3 next = spline.getPoint(nextwaypoint, track);
        Vector3 direction = next - pos;
        angle = Vector3.Angle(direction, gameObject.transform.forward);
        Vector3 newDir = Vector3.RotateTowards(gameObject.transform.forward, direction, maxrotationspeed, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDir);
        float distanceToMove = speed * Time.deltaTime;
        gameObject.transform.Translate(Vector3.forward * distanceToMove);

        if (Vector3.Distance(gameObject.transform.position, spline.getPoint(nextwaypoint, track)) < distanceToMove)
        {

            // Are we at a control point?
            for (int i = 0; i < spline.points.Length; i++)
            {
                if (Vector3.Distance(spline.newPoints[nextwaypoint], spline.points[i].position) < 0.1f)
                {
                    Debug.Log("Arrived at control point " + i + " on spline " + spline.SplineNumber + " nextwp: " + nextwaypoint);
                    // is spline.points[i] an intersection with another spline?
                    foreach (Catmul s in Spline)
                    {
                        if (s == spline)
                        {
                            continue;
                        }
                        // are we not on s if we make it here
                        for (int j = 0; j < s.points.Length; j++)
                        {
                            if (Vector3.Distance(spline.points[i].position, s.points[j].position) < 0.1f)
                            {
                                Vector3 AV = spline.newPoints[i + 1] - spline.newPoints[i];
                                Vector3 BV = spline.newPoints[j + 1] - spline.newPoints[j];
                                float AB = Vector3.SignedAngle(AV, BV, Vector3.up);

                                // We were traveling in the +ve A direction, and turning left
                                {
                                    if (track == 0 && currentdirection == 1 && AB > 0)
                                    {
                                        currentdirection = 1;
                                    }
                                    if (track == 0 && currentdirection == 1 && AB <= 0)
                                    {
                                        currentdirection = -1;
                                    }
                                }

                                // We were traveling in the + ve A direction, and turning right
                                {
                                    if (track > 1 && currentdirection == 1 && AB > 0)
                                    {
                                        newdirection = 1;
                                    }
                                    if (track > 1 && currentdirection == 1 && AB <= 0)
                                    {
                                        newdirection = -1;
                                    }
                                }

                                // We were traveling in the -ve A direction and turning left
                                {
                                    if (track == 0 && currentdirection == -1 && AB > 0)
                                    {
                                        newdirection = 1;
                                    }
                                    if (track == 0 && currentdirection == -1 && AB <= 0)
                                    {
                                        newdirection = -1;
                                    }
                                }

                                // We were traveling in the -ve A direction and turning right
                                {
                                    if (track > 1 && currentdirection == -1 && AB > 0)
                                    {
                                        newdirection = 1;
                                    }
                                    if (track > 1 && currentdirection == -1 && AB <= 0)
                                    {
                                        newdirection = -1;
                                    }
                                }
                                currentdirection = newdirection;

                                Debug.Log("And we are at an intersection");
                                spline = s;
                                // We also need to switch nextwaypoint to match the position on the new track.


                                if (currentdirection == 1)
                                {
                                    nextwaypoint = ((j - 1 + s.points.Length) % s.points.Length) * (int)s.amountOfPoints + 1;
                                }
                                if (currentdirection == -1)
                                {
                                    nextwaypoint = ((j - 1 + s.points.Length) % s.points.Length) * (int)s.amountOfPoints - 1;
                                }
                                break;
                            }
                        }

                        if (s == spline) // did i move to the new spline
                        {
                            track = Random.Range(0, 3);

                            break;
                        }
                    }

                }

                //are we at intersection if so change lanes else dont
            }

            nextwaypoint += currentdirection;
            if (nextwaypoint < 0)
            {
                nextwaypoint = spline.newPoints.Count - 1;
            }
            if (nextwaypoint == spline.newPoints.Count)
            {
                nextwaypoint = 0;
                AdjustSpeed();
            }
        }
    }

    void AdjustSpeed()
    {
        speed *= 1.1f;
        if (speed > 20.0f)
        {
            speed = 20.0f;
        }
    }
}
