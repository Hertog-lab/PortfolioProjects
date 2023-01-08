using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    private bool isSliderSet;
    private Slider slider;

    private void Update()
    {
        if (isSliderSet != true)
        {
            slider = GetComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = InfoManager.instance._disconectedSatalites;
            slider.value = 0;
        }
        slider.value += InfoManager.instance._connectedSatalites;
    }
}
