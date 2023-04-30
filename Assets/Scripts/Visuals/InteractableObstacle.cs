using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObstacle : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Space;
    public int points = 10;
    private bool canInteract = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddPoints(points);
            Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(interactKey))
        {
            // Trigger interaction and give points
            // This is just an example, you can modify this to fit your needs
            GameManager.Instance.AddPoints(points);
            Destroy(gameObject);
        }
    }
}