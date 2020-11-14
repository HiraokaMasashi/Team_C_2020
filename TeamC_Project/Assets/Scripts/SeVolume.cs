using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeVolume : MonoBehaviour
{
    Slider slider;
    SoundManager soundManager;

    void Start()
    {
        slider = GetComponent<Slider>();
        soundManager = SoundManager.Instance;
        slider.value = soundManager.SeVolume;
    }

    public void OnValueChanged()
    {
        soundManager.SeVolume = slider.value;
    }
}
