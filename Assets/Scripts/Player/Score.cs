using Auxiliars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Score : MonoBehaviour {

	private const float TARGET_SATURATION = 18.5f;
	private const float LOW_SATURATION = -200f;

	[SerializeField]
	private TextMeshProUGUI scoreText;
	[SerializeField]
	private TextMeshProUGUI comboText;
	[SerializeField]
	private GameObject comboImg;
	[SerializeField]
	private TextMeshProUGUI packetsText;
	[SerializeField]
	private GameObject packetPrefab;
	[SerializeField]
	private GameObject packetPivot;

	private PostProcessVolume volume;
	private CameraShake shakeRef;
	private Stack<PacketRotate> packetRotateRef;
	private int saturationIndex;

	public float ScoreValue { 
		get => this.scoreValue; 
		set { 
			this.scoreValue = value;
			this.scoreText.text = $"Score: {value}";
		}
	}
	public int Combo {
		get => this.combo;
		set {
			if (value < 3) {
				if (this.combo > 10) {
					this.CrashCombo();
				}
				this.comboImg.SetActive(false);
				this.comboText.text = string.Empty;
				this.combo = value;
				return;
			}
			this.comboImg.SetActive(true);
			this.combo = value;
			this.comboText.text = $"{value}";
		}
	}

	public int Packets {
		get => this.packets;
		set {
			if (value <= 0) {
				EntityFetcher.s_GameManager.EndGame(true);
			}
			this.packets = value;
			this.packetsText.text = $"{value}";
		}
	}
	private float scoreValue;
	private int combo;
	[SerializeField]
	private int packets;

	private void Start() {
		this.packetRotateRef = new Stack<PacketRotate>();
		this.volume = EntityFetcher.s_MainCamera.GetComponent<PostProcessVolume>();
		this.saturationIndex = -1;
		for (int i = 0; i < this.volume.profile.settings.Count; i++) {
			var setting = this.volume.profile.settings[i];
			if (setting is ColorGrading) {
				this.saturationIndex = i;
				break;
			}
		}
		this.volume = EntityFetcher.s_MainCamera.GetComponent<PostProcessVolume>();
		this.shakeRef = EntityFetcher.s_MainCamera.GetComponent<CameraShake>();
		this.ScoreValue = 0;
		this.Combo = 0;
		this.Packets = packets;
		const float START_X = -3;
		//Initialize packet visuals
		//Spawn around a pivot
		for (int i = 0; i < packets; i++) {
			//NEVER DO THISSS
			Vector3 offsetPosition = new Vector3(START_X + (1.5f * i), 3f, -2f);
			var instance = Instantiate(this.packetPrefab, offsetPosition, Quaternion.identity);
			var prRef = instance.GetComponent<PacketRotate>();
			prRef.startingPos = offsetPosition;
			this.packetRotateRef.Push(prRef);
		}
	}

	private void Update() {
		ColorGrading settings = this.GetColorGradingSettings();
		if (!SpartanMath.ArrivedAt(settings.saturation, TARGET_SATURATION)) {
			settings.saturation.value = SpartanMath.Lerp(settings.saturation.value, TARGET_SATURATION, Time.deltaTime);
			return;
		}
		settings.saturation.value = TARGET_SATURATION;
	}

	public void ResetCombo() {
		this.Combo = 0;
	}

	public ColorGrading GetColorGradingSettings() {
		return EntityFetcher.s_GameManager.GetPostProcessingSettingsByIndex<ColorGrading>(this.saturationIndex);
	}

	private void CrashCombo() {
		//Decrease saturation
		ColorGrading settings = this.GetColorGradingSettings();
		settings.saturation.value = LOW_SATURATION;
		//Camera shake
		this.shakeRef.Shake(0.5f);
	}

	public void DropPacket() {
		if (this.packetRotateRef.Count < 1) return;
		this.Packets--;
		PacketRotate toDestroy = this.packetRotateRef.Pop();
		if (toDestroy == null) return;
		Destroy(toDestroy.gameObject);
		//Move them along?
	}

}
