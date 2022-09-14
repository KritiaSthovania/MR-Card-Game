using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Waypoints
{
    public static Transform[] mapWaypoints;

    public static GameObject enemySpawn;
}

namespace map
{
    public class GameMap : MonoBehaviour
    {
        // Array of waypoints placed on the path of the enemies
        [SerializeField]
        private Transform[] mapWaypoints;

        [SerializeField]
        private GameObject enemySpawn;

        // Need to define the spawns here too TODO

        void Start()
        {
            Debug.Log(gameObject.GetComponent<Collider>().bounds.size);
            SetMapWaypoints();

            Debug.Log("The map waypoints and enemy spawn were set.");
        }

        // The variable used to access the value of the map waypoints from the enemies class
        private void SetMapWaypoints()
        {
            Waypoints.mapWaypoints = mapWaypoints;
            Waypoints.enemySpawn = enemySpawn;
        }
    }
}
