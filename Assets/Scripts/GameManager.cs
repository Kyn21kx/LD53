using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {

	public int DistanceCovered => (int)(Time.time / travelSpeed);
	public float travelSpeed = 6f;

	private TextMeshProUGUI distanceText;

	private void Start() {

	}

	private void Update() {
		this.distanceText.text = $"Distance: {this.DistanceCovered}";
	}

}
