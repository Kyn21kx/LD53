using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketRotate : MonoBehaviour {

    private float speed;
    private float t;
    public Vector3 startingPos;
    private Vector3 RelativePosition => EntityFetcher.s_Player.transform.position + this.startingPos;

	private void OnEnable() {
        this.speed = Random.Range(0.1f, 2f);
    }


    private void Update() {
        if (EntityFetcher.s_GameManager.GameOver) { 
            Destroy(gameObject);
            return;
		}
        t = Mathf.Clamp01(t + (Time.deltaTime * speed));
        this.transform.Rotate(Vector3.forward, t);
        //this.transform.position = Vector3.Lerp(this.transform.position, RelativePosition, Time.deltaTime);
        this.transform.position = Vector3.Lerp(transform.position, RelativePosition, Time.deltaTime * 10f);
    }
}
