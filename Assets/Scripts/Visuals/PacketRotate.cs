using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketRotate : MonoBehaviour {

    private float speed;
    private float t;
    public Vector3 startingPos;
    private Vector3 RelativePosition { get {
            if (EntityFetcher.s_Player == null)
                return this.transform.position + this.startingPos;
            return EntityFetcher.s_Player.transform.position + this.startingPos;
        }
    }

	private void OnEnable() {
        this.speed = Random.Range(0.1f, 2f);
    }


    private void Update() {
        OnGameOver();
        t = Mathf.Clamp01(t + (Time.deltaTime * speed));
        this.transform.Rotate(Vector3.forward, t);
        //this.transform.position = Vector3.Lerp(this.transform.position, RelativePosition, Time.deltaTime);
        this.transform.position = Vector3.Lerp(transform.position, RelativePosition, Time.deltaTime * 10f);
    }

    private void OnGameOver() {
        if (EntityFetcher.s_GameManager == null) return;
        if (EntityFetcher.s_GameManager.GameOver) {
            Destroy(gameObject);
            return;
        }
    }
}
