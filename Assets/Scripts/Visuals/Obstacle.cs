using UnityEngine;
using System.Collections.Generic;
using Auxiliars;

public class Obstacle : MonoBehaviour {
	private Camera mainCam;
	private SpartanTimer timer;

	private void OnEnable() {
		this.mainCam = EntityFetcher.s_MainCamera;
		this.timer = new SpartanTimer(TimeMode.Framed);
		this.timer.Start();
	}

	private void Update() {
		Vector3 pos = this.transform.position;
		pos.z -= Time.deltaTime * EntityFetcher.s_GameManager.travelSpeed;
		this.transform.position = pos;
		if (this.ShouldDestroy()) {
			this.timer.Stop();
			Destroy(this.gameObject);
		}
	}

	private bool ShouldDestroy() {
		float depth = this.transform.position.z;
		//float maxDepth = -19.97f;
		float maxDepth = this.mainCam.transform.position.z;
		return depth <= maxDepth;
	}

}