using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LibPdInstance))]
public class MusicManager : MonoBehaviour {

    private LibPdInstance pdInstance;

	private void Start() {
		this.pdInstance = GetComponent<LibPdInstance>();
		this.pdInstance.Bind("obstacles");
	}

	public void OnReceiveFloat(string name, float value) {
		Debug.Log($"Event received from patch! Name: {name}, Value: {value}");
	}

}
