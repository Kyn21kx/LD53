using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class GameManager : MonoBehaviour {
	//OH, GOD D:
	private const float AVERAGE_DELTA_TIME = 0.102093f;
	
	public int DistanceCovered => (int)(Time.time * travelSpeed) / 10;
	public float travelSpeed = 6f;

	public bool Paused { get; private set; }
	public bool GameOver { get; private set; }

	[SerializeField]
	private TextMeshProUGUI distanceText;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject gameOverMenu;
	private MusicManager musicManager;
	private CameraShake shakeRef;
	private PostProcessVolume postProcessVolume;

	private void Start() {
		this.Paused = false;
		this.GameOver = false;
		this.pauseMenu.SetActive(this.Paused);
		this.musicManager = GetComponent<MusicManager>();
		this.shakeRef = EntityFetcher.s_MainCamera.GetComponent<CameraShake>();
		this.postProcessVolume = EntityFetcher.s_MainCamera.GetComponent<PostProcessVolume>();
		float distanceToCover = EntityFetcher.s_TerrainSplit.ScaleToWorld.z;
		this.travelSpeed = (distanceToCover / musicManager.TimeToOneMeasure) / AVERAGE_DELTA_TIME;
	}

	private void Update() {
		this.gameOverMenu.SetActive(this.GameOver);
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
		this.shakeRef.Shake(1.5f);
		Destroy(EntityFetcher.s_Player);
		//Decrease saturation to like, -200
		ColorGrading settings = this.postProcessVolume.profile.GetSetting<ColorGrading>();
		settings.saturation.value = -200f;
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

	public T GetPostProcessingSettingsByIndex<T>(int index) where T : PostProcessEffectSettings {
		if (index == -1 || index >= this.postProcessVolume.profile.settings.Count) return null;
		return this.postProcessVolume.profile.settings[index] as T;
	}
}
