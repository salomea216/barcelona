using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float followDistance = 5f; // Distance at which the enemy will start following
    public float speed = 2f; // Speed at which the enemy moves towards the player

    private Transform player;

    private void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return; // Exit if player is not found

        // Check the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // If the player is within the follow distance
        if (distance < followDistance)
        {
            //Debug.Log($"Distance to player: {distance}");
            // Move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Optionally, make the enemy face the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
