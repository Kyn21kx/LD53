using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotForVideo : MonoBehaviour {

    public float speed = 10f;
    public float time;
    private Vector3 direction;

    private void Start() {
        this.direction = Vector3.up;
        this.time = 0f;
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(direction, Time.deltaTime * speed, Space.Self);
        time += Time.deltaTime;
        if (time > 1f && ((int)time) % 10 == 0) {
            SetDirection();
		}
    }

    private void SetDirection() {
        this.direction = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
	}
}
