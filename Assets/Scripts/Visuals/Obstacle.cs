using UnityEngine;
using System.Collections.Generic;
using Auxiliars;

public class Obstacle : MonoBehaviour {
	private Camera mainCam;

	private void OnEnable() {
		this.mainCam = EntityFetcher.s_MainCamera;
	}

	private void Update() {
		Vector3 pos = this.transform.position;
		pos.z -= Time.deltaTime * EntityFetcher.s_GameManager.travelSpeed;
		this.transform.position = pos;
		if (this.ShouldDestroy()) {
			Destroy(this.gameObject);
		}
	}

	private bool ShouldDestroy() {
		float depth = this.transform.position.z;
		float maxDepth = this.mainCam.transform.position.z;
		return depth <= maxDepth;
	}

}