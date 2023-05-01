using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image barFill;
    public float progress = 0f;

    void Update()
    {
        barFill.fillAmount = progress;
    }
}