using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour {

	public float speed = 1f;

	private void Update() {
		Vector3 pos = this.transform.position;
		pos.z -= Time.fixedDeltaTime * speed;
		this.transform.position = pos;
	}

}