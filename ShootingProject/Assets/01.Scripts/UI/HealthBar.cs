using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;

    public void SetFill(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp(current / max, 0, 1);
    }
}
