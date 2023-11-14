using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Auxiliars;

public class GameManager : MonoBehaviour {
	//OH, GOD D:
	private const float AVERAGE_DELTA_TIME = 0.102093f;
	
	public int DistanceCovered => (int)(gameTime * travelSpeed);
	public float GameTime => gameTime;
	public float travelSpeed = 6f;
	private float gameTime;

	public bool Paused { get; private set; }
	public bool GameOver { get; private set; }

	[SerializeField]
	private Image distanceImg;
	[SerializeField]
	private GameObject gameOverGoodImg;
	[SerializeField]
	private GameObject gameOverBadImg;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject gameOverMenu;
	private MusicManager musicManager;
	private CameraShake shakeRef;
	private PostProcessVolume postProcessVolume;
	public float maxDistance;

	private void Awake() {
		if (EntityFetcher.s_GameManager == null) {
			EntityFetcher.s_Instance.Initialize();
		}
	}

	private void Start() {
		this.gameTime = 0f;
		this.Paused = false;
		this.GameOver = false;
		this.pauseMenu.SetActive(this.Paused);
		this.gameOverGoodImg.SetActive(false);
		this.gameOverBadImg.SetActive(false);
		this.musicManager = GetComponent<MusicManager>();
		this.shakeRef = EntityFetcher.s_MainCamera.GetComponent<CameraShake>();
		this.postProcessVolume = EntityFetcher.s_MainCamera.GetComponent<PostProcessVolume>();
		float distanceToCover = EntityFetcher.s_TerrainSplit.ScaleToWorld.z;
		this.travelSpeed = (distanceToCover / musicManager.TimeToOneMeasure) / AVERAGE_DELTA_TIME;
		this.maxDistance = Random.Range(distanceToCover * 100f, distanceToCover * 200f);
		this.distanceImg.fillAmount = 0f;
		//this.maxDistance = 300;
	}

	private void Update() {
		this.gameTime += Time.deltaTime;
		this.gameOverMenu.SetActive(this.GameOver);
		if (this.GameOver) return;
		this.HandleInput();
		this.distanceImg.fillAmount = Mathf.Clamp01(this.DistanceCovered / this.maxDistance);
		if (SpartanMath.ArrivedAt(this.distanceImg.fillAmount, 1f, 0.05f)) {
			this.distanceImg.fillAmount = 1f;
			this.EndGame(false);
		}
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

	public void EndGame(bool lost) {
		ColorGrading settings = this.postProcessVolume.profile.GetSetting<ColorGrading>();
		this.GameOver = true;
		Destroy(EntityFetcher.s_Player);
		if (!lost) {
			//Good
			this.gameOverGoodImg.SetActive(true);
			return;
		}
		this.shakeRef.Shake(1.5f);
		this.gameOverBadImg.SetActive(true);
		//Bad
		//Decrease saturation to like, -200
		settings.saturation.value = -200f;
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

	public void ReloadScene() {
		SceneManager.LoadScene(0, LoadSceneMode.Single);
	}

}
