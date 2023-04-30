using Auxiliars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParryResult {
	MISSED,
	GOOD,
	PERFECT
};

public class Parry : MonoBehaviour {

	private const float START_NOISE_STRENGTH = 0f;
	private const float START_FRESNEL = 20f;

	private const float END_NOISE_STRENGTH = 3f;
	private const float END_FRESNEL = 3f;

	[SerializeField]
	private MeshRenderer meshRenderer;
	private Material material;
	[SerializeField]
	private ParticleSystem explosionEffect;

	//So, we press a button, and start the timer, then stop it when we get hit
	private SpartanTimer parryTimerInterval;

	[SerializeField]
	private float animationSpeed = 2f;

	private float noiseStrength;
	private float fresnel;

	private void Start() {
		this.material = this.meshRenderer.material;
		this.meshRenderer.gameObject.SetActive(false);
		this.explosionEffect.Stop();
		this.parryTimerInterval = new SpartanTimer(TimeMode.Framed);
	}

	private void Update() {
		this.HandleInput();
		if (!this.parryTimerInterval.Started) return;
		this.ActivateForceField(Time.deltaTime);
		float ms = this.parryTimerInterval.CurrentTimeMS;
		//Deactivate it either by time, or when we have a collision
		if (ms >= 1000f)
			this.DeactivateForceField();
	}

	private void HandleInput() {
		//Raycast forward and check if we can parry
		if (Input.GetKeyDown(KeyCode.Q) && !this.parryTimerInterval.Started) {
			this.parryTimerInterval.Reset();
			this.ResetShaderParams();
		}
	}

	private void ActivateForceField(float timeStep) {
		//If we haven't gotten to the final values on the shader, keep lerping towards them
		this.AnimateShader(timeStep);
		//Get info from PD
	}

	private bool AnimateShader(float timeStep) {
		this.noiseStrength = SpartanMath.Lerp(this.noiseStrength, END_NOISE_STRENGTH, timeStep * this.animationSpeed);
		this.fresnel = SpartanMath.Lerp(this.fresnel, END_FRESNEL, timeStep * this.animationSpeed);

		this.material.SetFloat("_NoiseStrength", this.noiseStrength);
		this.material.SetFloat("_Fresnel", this.fresnel);

		return SpartanMath.ArrivedAt(noiseStrength, END_NOISE_STRENGTH) && SpartanMath.ArrivedAt(this.fresnel, END_FRESNEL);
	}

	private void ResetShaderParams() {
		this.noiseStrength = START_NOISE_STRENGTH;
		this.fresnel = START_FRESNEL;
		this.meshRenderer.gameObject.SetActive(true);
		this.explosionEffect.Stop();
	}

	private void DeactivateForceField() {
		this.parryTimerInterval.Stop();
		this.explosionEffect.Play();
		//Some fancy particle effects here maybe
		this.meshRenderer.gameObject.SetActive(false);
	}

	private bool IsParryEnabled() {
		//Raycast forward, see if the obstacle is in the proper layer, if so, parry
		return false;
	}

	public void SendParryResult(ParryResult result) {
		//We have a result, damage the player, add combo multipliers, show it to the screen and stuff
		
	}

}
