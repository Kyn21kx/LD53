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
	private Score scoreRef;

	private void Start() {
		this.parryRef = GetComponent<Parry>();
		this.scoreRef = GetComponent<Score>();
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
		switch (other.tag) {
			case "Obstacle":
				Damage(other);
				break;
			case "Score":
				this.ScorePoint(other);
				break;
			default:
				throw new System.NotImplementedException($"Trigger for tag {other.tag} not implemented!");
		}
	}

	private void Damage(Collider other) {
		ParryResult res = this.parryRef.CurrParryState;
		if (res == ParryResult.NONE || res == ParryResult.MISSED) {
			Debug.Log("Damaged!");
			this.scoreRef.ResetCombo();
			this.material.color = Color.red;
			this.damageVfxTimer.Reset();
			this.scoreRef.DropPacket();
		}
		bool inLayer = SpartanMath.IsInLayerMask(other.gameObject.layer, this.parryRef.ParryMask);
		if (parryRef.IsBlocking && inLayer) { 
			parryRef.DeactivateForceField();
			float value = res == ParryResult.PERFECT ? 5.0f : 1.0f;
			var instance = Instantiate(this.parryRef.parryIndicatorPrefab).GetComponent<ParryIndicator>();
			instance.transform.parent = EntityFetcher.s_Canvas.transform;
			instance.Initialize(res);
			this.ScorePoint((Collider)other, value);
			Destroy(other.gameObject);
		}
	}

	private void ScorePoint(Collider other, float value = 1.0f) {
		this.scoreRef.ScoreValue += value;
		this.scoreRef.Combo++;
		Destroy(other.gameObject);
	}

}
