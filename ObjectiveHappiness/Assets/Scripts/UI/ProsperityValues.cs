using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ValueUpdater : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI slidervalues;

    void Start()
    {

    }

    void Update()
    {
        slider.value = ResourceManager.Instance.prosperity;
        slidervalues.text = slider.value.ToString("0");
    }
}