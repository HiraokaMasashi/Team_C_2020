using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmVolume : MonoBehaviour
{
    Slider slider;
    SoundManager soundManager;

    void Start()
    {
        slider = GetComponent<Slider>();
        soundManager = SoundManager.Instance;
        slider.value = soundManager.BgmVolume;
    }

    void LateUpdate()
    {
        slider.value = soundManager.BgmVolume;
    }

    public void OnValueChanged()
    {
        soundManager.BgmVolume = slider.value;
        if (soundManager.MasterVolume <= slider.value)
            soundManager.MasterVolume = slider.value;
    }
}
