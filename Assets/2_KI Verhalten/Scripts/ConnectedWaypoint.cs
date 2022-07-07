using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedWaypoint : Waypoint
{
    [SerializeField] protected float connectivityRadius = 50f;

    private List<ConnectedWaypoint> connections;

    public void Start()
    {
        // Grab all waypoint objects in the scene
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        
        // create a list of waypoints I can refer to later
        connections = new List<ConnectedWaypoint>();
        
        // check if they're connected waypoints
        for (int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();
            
            //if we found a waypoint
            if (nextWaypoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if (connections.Count == 0)
        {
            Debug.LogError("No waypoints found");
            return null;
        }
        else if (connections.Count == 1 && connections.Contains(previousWaypoint))
        {
            // if it has only one waypoint and its the previous one? just use that.
            return previousWaypoint;
        }
        else
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;

        }
    }
}
