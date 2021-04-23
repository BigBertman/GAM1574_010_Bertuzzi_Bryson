using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Catmul[] splines;
    public GameObject splinePrefab;
    public GameObject[] swarmleaderPrefab;
    public string[][] maskNames;

    // Use this for initialization
    void Start()
    {
        maskNames = new string[5][];

        maskNames[0] = new string[] { "flock1", "flock2", "flock3", "flock4" };
        maskNames[1] = new string[] { "flock0", "flock2", "flock3", "flock4" };
        maskNames[2] = new string[] { "flock0", "flock1", "flock3", "flock4" };
        maskNames[3] = new string[] { "flock0", "flock1", "flock2", "flock4" };
        maskNames[4] = new string[] { "flock0", "flock1", "flock2", "flock3" };

        splines = new Catmul[5]; // TODO - change
        splines[0] = Instantiate(splinePrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Catmul>(); // Middle
        splines[1] = Instantiate(splinePrefab, new Vector3(-30.0f, 0.0f, 30.0f), Quaternion.identity).GetComponent<Catmul>(); // Top Left
        splines[2] = Instantiate(splinePrefab, new Vector3(30.0f, 0.0f, 30.0f), Quaternion.identity).GetComponent<Catmul>(); // Top Right
        splines[3] = Instantiate(splinePrefab, new Vector3(-30.0f, 0.0f, -30.0f), Quaternion.identity).GetComponent<Catmul>(); // Bottom Left
        splines[4] = Instantiate(splinePrefab, new Vector3(30.0f, 0.0f, -30.0f), Quaternion.identity).GetComponent<Catmul>(); // Bottom Right
                                                                                                                              // swarmleaderPrefab = new GameObject[5];

        // TODO add code here

        for (int i = 0; i < 5; i++) // TO DO - change code
        {
            splines[i].GenerateSpline();
        }
        // spawn the flocks on the tracks.  Track 0 is where the player begins.
        for (int i = 1; i < 5; i++)
        {
            GameObject AI = Instantiate(swarmleaderPrefab[i], splines[i].lanes[1, 0], Quaternion.identity);
            AI.GetComponent<Movement>().waypoints = splines[i].sp;
            AI.GetComponent<Movement>().SetValue("TrackManager", this);
            AI.GetComponent<Movement>().SetValue("TrackIndex", i);
        }

        for (int i = 0; i < 1; i++) // TO DO CHANGE CODE 
        {
            GameObject Player = Instantiate(swarmleaderPrefab[i], splines[i].lanes[1, 0], Quaternion.identity);
            Player.GetComponent<Movement>().waypoints = splines[i].sp;
            Player.GetComponent<Movement>().SetValue("TrackManager", this);
            Player.GetComponent<Movement>().SetValue("TrackIndex", i);

            // TO DO - Spawn the swarm leader
            // TODO - Get the follow track script, and tell it about the track manager (so it can find more tracks), and the spline.
            // make sure to set the mask on the flock, and to say which is the player. 
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
