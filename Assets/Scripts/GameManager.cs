using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public float LevelTime { get => levelTime; private set => this.levelTime = value; }

	[SerializeField]
	private float levelTime; //We use a float instead of a timer because we don't want to stop


	private void Start() {
		this.LevelTime = 0f;
	}

	private void Update() {
		this.LevelTime += Time.deltaTime;
	}

}
