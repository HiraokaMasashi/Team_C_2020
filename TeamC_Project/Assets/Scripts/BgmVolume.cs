using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmVolume : MonoBehaviour
{
    Slider slider;
    SoundManager soundManager;
    VolumeManager volemeManager;

    void Awake()
    {
        volemeManager = VolumeManager.Instance;
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        soundManager = SoundManager.Instance;
        slider.value = soundManager.BgmVolume;
    }

    public void OnValueChanged()
    {
        soundManager.BgmVolume = slider.value;
        volemeManager.BgmVolume = slider.value;
    }
}
