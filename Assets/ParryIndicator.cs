using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParryIndicator : MonoBehaviour {

	private TextMeshProUGUI textRef;
	private RectTransform rectTransform;
	private float t;
	private Color targetColor;
	private ParryResult result;

	public void Initialize(ParryResult result) {
		this.result = result;
		textRef = GetComponent<TextMeshProUGUI>();
		this.rectTransform = GetComponent<RectTransform>();
		this.textRef.text = result.ToString();
		Color initialColor = Color.red;
		switch (result) {
			case ParryResult.MISSED:
				initialColor = Color.red;
				break;
			case ParryResult.GOOD:
				initialColor = Color.cyan;
				break;
			case ParryResult.PERFECT:
				initialColor = Color.green;
				break;
		}

		this.targetColor = initialColor;
		this.textRef.color = initialColor;
		this.t = 0f;
	}



	private void Update() {
		this.t += Time.deltaTime * 0.25f;
		Vector3 position = this.rectTransform.position;
		position.y -= t * 0.25f;
		this.rectTransform.position = position;
		this.textRef.color = Color.Lerp(this.textRef.color, new Color(targetColor.r, targetColor.g, targetColor.b, 0f), t);
		if (t > 5f) {
			Destroy(gameObject);
		}
	}

}
