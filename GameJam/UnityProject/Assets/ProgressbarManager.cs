using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressbarManager : MonoBehaviour
{
    private bool isSliderSet;
    private Slider slider;

    private void Update()
    {
        if (isSliderSet != true) {
            slider = GetComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 30;
            slider.value = 0;
        }
        slider.value += InfoManager.instance._fixTimer * 10;
    }
}
