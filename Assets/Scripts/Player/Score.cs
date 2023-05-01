using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI comboText;

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
            this.combo = value;
            if (value < 3) {
                this.comboText.text = string.Empty;
                return;
            }
            this.comboText.text = $"Combo: x{value}";
		}
	}
    private float scoreValue;
    private int combo;

	private void Start() {
        this.ScoreValue = 0;
        this.Combo = 0;
	}

	public void ResetCombo() {
        this.Combo = 0;
	}

}
