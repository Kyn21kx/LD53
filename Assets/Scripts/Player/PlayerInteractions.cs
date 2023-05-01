using Auxiliars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Parry))]
public class PlayerInteractions : MonoBehaviour {

	private SpartanTimer damageVfxTimer;
	[SerializeField]
	private MeshRenderer bodyRenderer;
	private Material material;
	private Parry parryRef;

	private void Start() {
		this.parryRef = GetComponent<Parry>();
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
		Damage(other);
	}

	private void Damage(Collider other) {
		Debug.Log("Damaged!");
		this.material.color = Color.red;
		this.damageVfxTimer.Reset();
		bool inLayer = SpartanMath.IsInLayerMask(other.gameObject.layer, this.parryRef.ParryMask);
		if (parryRef.IsBlocking && inLayer) { 
			parryRef.DeactivateForceField();
			Destroy(other.gameObject);
		}
	}


}
