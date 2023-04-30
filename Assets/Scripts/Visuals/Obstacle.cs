using UnityEngine;
using System.Collections.Generic;
using Auxiliars;

public class Obstacle : MonoBehaviour {

    public float speed = 1f;
    private Camera mainCam;
    private bool interactable = false;

    private void OnEnable()
    {
        this.mainCam = EntityFetcher.s_MainCamera;

        // Check if the obstacle is interactable
        if (this.gameObject.tag == "Interactable")
        {
            interactable = true;
        }
    }

    private void Update()
    {
        Vector3 pos = this.transform.position;
        pos.z -= Time.fixedDeltaTime * speed;
        this.transform.position = pos;

        // Check if player is pressing space bar and if the obstacle is interactable
        if (interactable && Input.GetKeyDown(KeyCode.Space))
        {
            CollectObstacle();
        }

        if (this.ShouldDestroy())
        {
            Destroy(this.gameObject);
        }
    }

    private bool ShouldDestroy()
    {
        float depth = this.transform.position.z;
        float maxDepth = this.mainCam.transform.position.z;
        return depth <= maxDepth;
    }

    private void CollectObstacle()
    {
        // Add logic to collect the interactable obstacle here
        // For example, increment a score variable or trigger an animation
        Destroy(this.gameObject);
    }

}