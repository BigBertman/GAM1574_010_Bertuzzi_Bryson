using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Catmul : MonoBehaviour
{
    public GameObject controlPointPrefab;
    public List<Vector3> leftPoints = new List<Vector3>();
    public List<Vector3> rightPoints = new List<Vector3>();
    public List<Vector3> middlePoints = new List<Vector3>();

    public int SplineNumber;

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

        if (SplineNumber == 1)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        if (SplineNumber == 2)
        {
            transform.position = new Vector3(20.0f, 0, 0);
        }
        if (SplineNumber == 3)
        {
            transform.position = new Vector3(20.0f * Mathf.Cos(60 * Mathf.Deg2Rad), 0, -20.0f * Mathf.Sin(60 * Mathf.Deg2Rad));
        }

        points = new Transform[12];

        for (int i = 0; i < points.Length; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * 360 / points.Length * i) * 20.0f;
            float z = Mathf.Sin(Mathf.Deg2Rad * 360 / points.Length * i) * 20.0f;
            Vector3 pos = new Vector3(x, 0, z) + gameObject.transform.position;
            GameObject cp = GameObject.Instantiate(controlPointPrefab, pos, Quaternion.identity);
            middlePoints.Add(pos);
            points[i] = cp.transform;
        }

        newPoints.Clear();

        for (int i = 0; i < points.Length; i++)
        {
            CatmulRom(i);
        }

        newPoints.RemoveAt(newPoints.Count - 1);

        for (int i = 0; i < newPoints.Count; i++)
        {
            Vector3 dir = newPoints[(i + 1) % newPoints.Count] - newPoints[i];
            Vector3 left = Vector3.Cross(dir, Vector3.up).normalized * 1.0f;
            Vector3 leftPoint = newPoints[i] + left;
            Vector3 rightPoint = newPoints[i] - left;
            leftPoints.Add(leftPoint);
            rightPoints.Add(rightPoint);
        }
    }

    int getIndex(int index)
    {
        return index % points.Length;
    }

    public void CatmulRom(int index)
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
        Color r = Color.green;
        r.a = 0.5f;
        Gizmos.color = r;

        for (int i = 0; i < newPoints.Count; i++)
        {
            Vector3 pos = newPoints[i];
            Vector3 pos1 = newPoints[(i + 1) % newPoints.Count];
            Gizmos.DrawLine(pos, pos1);
        }

        //foreach (Vector3 temp in newPoints)
        //{
        //    Vector3 pos = new Vector3(temp.x, temp.y, temp.z);
        //    Gizmos.DrawSphere(pos, 0.3f);
        //}

        Color b = Color.blue;
        b.a = 0.5f;

        Gizmos.color = b;

        for (int i = 0; i < leftPoints.Count; i++)
        {
            Vector3 pos = leftPoints[i];
            Vector3 pos1 = leftPoints[(i + 1) % leftPoints.Count];
            Gizmos.DrawLine(pos, pos1);
        }

        //foreach (Vector3 temp in leftPoints)
        //{
        //    Vector3 pos = new Vector3(temp.x, temp.y, temp.z);
        //    Gizmos.DrawSphere(pos, 0.3f);
        //}

        Color y = Color.red;
        y.a = 0.5f;

        Gizmos.color = y;

        for (int i = 0; i < rightPoints.Count; i++)
        {
            Vector3 pos = rightPoints[i];
            Vector3 pos1 = rightPoints[(i + 1) % rightPoints.Count];
            Gizmos.DrawLine(pos, pos1);
        }

        //foreach (Vector3 temp in rightPoints)
        //{
        //    Vector3 pos = new Vector3(temp.x, temp.y, temp.z);
        //    Gizmos.DrawSphere(pos, 0.3f);
        //}
    }



    //public void ChangeLanes(int track)
    //{
    //    if (track == 0)
    //    {
    //        for (int i = 0; i < points.Length; i++)
    //        {
    //            points[i].position = leftPoints[i];
    //        }
    //    }

    //    if (track == 1)
    //    {
    //        for (int i = 0; i < points.Length; i++)
    //        {
    //            points[i].position = rightPoints[i];
    //        }
    //    }

    //    if (track == 2)
    //    {
    //        for (int i = 0; i < points.Length; i++)
    //        {
    //            points[i].position = middlePoints[i];
    //        }
    //    }

    //    newPoints.Clear();
    //    for (int i = 0; i < points.Length; i++)
    //    {
    //        CatmulRom(i);
    //    }
    //}


    public Vector3 getPoint(int index, int track)
    {
        if (track == 0)
        {
            return leftPoints[index];
        }
        else if (track == 1)
        {
            return newPoints[index];
        }
        else
        {
            return rightPoints[index];
        }
    }


}



