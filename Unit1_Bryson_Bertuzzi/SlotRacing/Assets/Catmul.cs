using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Catmul : MonoBehaviour
{
    public GameObject waypointPrefab;
    public List<Vector3> leftPoints = new List<Vector3>();
    public List<Vector3> rightPoints = new List<Vector3>();
    public List<Vector3> midPoints = new List<Vector3>();

    //Use the transforms of GameObjects in 3d space as your points or define array with desired points
    public Transform[] points;

    //Store points on the Catmull curve so we can visualize them
    public List<Vector3> newPoints = new List<Vector3>();

    //How many points you want on the curve
    public float amountOfPoints = 20.0f;

    //set from 0-1
    public float alpha = 0.5f;

    /////////////////////////////
    private void Awake()
    {
        newPoints.Clear();

        points = new Transform[20];

        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(-40.0f, 40.0f);
            float z = Random.Range(-40.0f, 40.0f);
            Vector3 pos = new Vector3(x, 0.0f, z);
            GameObject wp = GameObject.Instantiate(waypointPrefab, pos, Quaternion.identity);
            midPoints.Add(pos);
            points[i] = wp.transform;
        }

        // Generate the middle points
        for (int i = 0; i < points.Length; i++)
        {
            CatmulRom(i);
        }

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 dir = points[(i + 1) % points.Length].position - points[i].position;
            // Vector3.up
            Vector3 left = Vector3.Cross(dir, Vector3.up).normalized * 0.5f;
            Vector3 leftPoint = points[i].position + left;
            Vector3 rightPoint = points[i].position - left;
            leftPoints.Add(leftPoint);
            rightPoints.Add(rightPoint);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeLanes(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ChangeLanes(1);
        }
        else if (Input.GetMouseButtonDown(2))
        {
            ChangeLanes(2);
        }
    }

    int getIndex(int index)
    {
        return index % points.Length;
    }

    void CatmulRom(int index)
    {
        Vector3 p0 = points[getIndex(index + 0)].position; // Vector3 has an implicit conversion to Vector2
        Vector3 p1 = points[getIndex(index + 1)].position;
        Vector3 p2 = points[getIndex(index + 2)].position;
        Vector3 p3 = points[getIndex(index + 3)].position;

        float t0 = 0.0f;
        float t1 = GetT(t0, p0, p1);
        float t2 = GetT(t1, p1, p2);
        float t3 = GetT(t2, p2, p3);

        for (float t = t1; t < t2; t += ((t2 - t1) / amountOfPoints))
        {
            Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
            Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

            Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

            if (newPoints.Count == 0 || newPoints[newPoints.Count - 1] != C)
            {
                newPoints.Add(C);
            }
        }
    }

    float GetT(float t, Vector3 p0, Vector3 p1)
    {
        float a = Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f) + Mathf.Pow((p1.z - p0.z), 2.0f);
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, alpha);

        return (c + t);
    }

    //Visualize the points
    void OnDrawGizmos()
    {
        Color r = Color.red;
        r.a = 0.5f;
        Color g = Color.green;
        g.a = 0.7f;
        Gizmos.color = r;
        foreach (Vector3 temp in newPoints)
        {
            Vector3 pos = new Vector3(temp.x, temp.y, temp.z);
            Gizmos.DrawSphere(pos, 0.1f);
        }
        for (int i = 0; i < points.Length - 4; i++)
        {
            Vector3 v0 = points[i + 0].position;
            Vector3 v1 = points[i + 1].position;
            Vector3 v2 = points[i + 2].position;
            Vector3 v3 = points[i + 3].position;
            Gizmos.DrawLine(v0, v2);

            Vector3 v20 = v2 - v0;
            v20.Normalize();
            Gizmos.DrawLine(v1 - v20, v1 + v20);
            Gizmos.color = g;

            Gizmos.DrawLine(v1, v3);
            Vector3 v31 = v3 - v1;
            v31.Normalize();
            Gizmos.DrawLine(v2 - v31, v2 + v31);
        }

        Color b = Color.blue;
        b.a = 0.5f;

        Gizmos.color = b;
        foreach (Vector3 temp in leftPoints)
        {
            Vector3 pos = new Vector3(temp.x, temp.y, temp.z);
            Gizmos.DrawSphere(pos, 0.1f);
        }

        Color y = Color.yellow;
        y.a = 0.5f;

        Gizmos.color = y;
        foreach (Vector3 temp in rightPoints)
        {
            Vector3 pos = new Vector3(temp.x, temp.y, temp.z);
            Gizmos.DrawSphere(pos, 0.1f);
        }

    }

    void ChangeLanes(int track)
    {
        if (track == 0)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].position = leftPoints[i];
            }
        }

        if (track == 1)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].position = rightPoints[i];
            }
        }

        if (track == 2)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].position = midPoints[i];
            }
        }

        newPoints.Clear();
        for (int i = 0; i < points.Length; i++)
        {
            CatmulRom(i);
        }
    }
}
