using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {

	public int DistanceCovered => (int)(Time.time * travelSpeed) / 10;
	public float travelSpeed = 6f;

	public bool Paused { get; private set; }
	public bool GameOver { get; private set; }

	[SerializeField]
	private TextMeshProUGUI distanceText;
	[SerializeField]
	private GameObject pauseMenu;
	private MusicManager musicManager;

	private void Start() {
		this.Paused = false;
		this.GameOver = false;
		this.pauseMenu.SetActive(this.Paused);
		this.musicManager = GetComponent<MusicManager>();
	}

	private void Update() {
		if (this.GameOver) return;
		this.HandleInput();
		this.distanceText.text = $"Distance: {this.DistanceCovered}m";
	}

	public void Pause() {
		this.Paused = true;
		this.pauseMenu.SetActive(this.Paused);
		this.musicManager.SendBang("start-stop");
		Time.timeScale = 0f;
	}

	public void Resume() {
		this.Paused = false;
		this.pauseMenu.SetActive(this.Paused);
		this.musicManager.SendBang("start-stop");
		Time.timeScale = 1f;
	}

	public void EndGame() {
		Destroy(EntityFetcher.s_Player);
		this.GameOver = true;
	}

	private void HandleInput() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (this.Paused)
				this.Resume();
			else
				this.Pause();
		}
	}
}
