using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private int score;
    public static GameManager Instance { get; private set; }
    public float LevelTime { get => levelTime; private set => this.levelTime = value; }

	[SerializeField]
	private float levelTime; //We use a float instead of a timer because we don't want to stop


	private void Start() {
		this.LevelTime = 0f;
	}

	private void Update() {
		this.LevelTime += Time.deltaTime;
	}
    private void Awake() {
        Instance = this;
    }
    public void AddPoints(int points) {
        score += points;
        Debug.Log($"Score: {score}");
    }

}
