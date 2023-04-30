using Auxiliars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

	private SpartanTimer damageVfxTimer;
	[SerializeField]
	private MeshRenderer bodyRenderer;
	private Material material;

	private void Start() {
		this.damageVfxTimer = new SpartanTimer(TimeMode.Framed);
		this.material = this.bodyRenderer.material;
	}

	private void Update() { 
		//TODO: Maybe add enums for player states
		if (damageVfxTimer.Started) {
			float ms = damageVfxTimer.CurrentTimeMS;
			if (ms > 200f) {
				damageVfxTimer.Stop();
				//Turn off the damage Vfx
				this.material.color = Color.white;
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		//TODO: Replace with switch expression
		if (!other.CompareTag("Obstacle")) return;
		//Damage the player
		Damage();
	}

	private void Damage() {
		Debug.Log("Damaged!");
		this.material.color = Color.red;
		this.damageVfxTimer.Reset();
	}


}
