using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour {

	[SerializeField]
	private MeshRenderer meshRenderer;
	private Material material;

	private void Start() {
		this.material = this.meshRenderer.material;
	}

	private void Update() { 

	}

	private void HandleInput() {
		//Raycast forward and check if we can parry
	}


}
