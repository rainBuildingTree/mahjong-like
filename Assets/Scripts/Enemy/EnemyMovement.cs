using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    /* MEMBER VARIABLES *///==================================================
    EnemyWaypointManager waypointManager;

    const float MoveSpeed = 1; // moves 'MoveSpeed' distance per a second

    List<Transform> waypoints;

    protected float targetDistance;
    protected float progressRate = 0f;
    protected float _knockbackAmounnt = 0.01f;



    /* UNITY EVENT FUNCTIONS *///==================================================
    void Awake() {
        GetWaypoints();
    }



    /* PUBLIC METHODS *///==================================================
    public void InitializeMovement() {
        JumpToFirstWaypoint();
        StartCoroutine(FollowWaypoints());
    }



    /* PRIVATE METHODS *///==================================================
    void GetWaypoints() {
        waypoints = new List<Transform>();
        waypointManager = FindObjectOfType<EnemyWaypointManager>();

        foreach (Transform t in waypointManager.transform) {
            waypoints.Add(t);
        }
    }
    void JumpToFirstWaypoint() {
        transform.position = waypoints[0].position;
    }
    public void Knockback() {
        progressRate -= _knockbackAmounnt;
        if (progressRate < 0f)
            progressRate = 0f;
    }



    /* IENUMERATORS *///==================================================
    IEnumerator FollowWaypoints() {
        yield return new WaitForEndOfFrame();

        foreach (Transform waypoint in waypoints) {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = waypoint.position;
            targetDistance = Vector3.Distance(startPosition, targetPosition);
            progressRate = 0f;

            while (progressRate < 1f) {
                progressRate = progressRate + Time.deltaTime * (MoveSpeed / targetDistance);
                if (progressRate > 1f)
                    progressRate = 1f;

                transform.position = Vector3.Lerp(startPosition, targetPosition, progressRate);

                yield return new WaitForEndOfFrame();
            }
        }

        gameObject.SetActive(false);
        yield return null;
    }
}
