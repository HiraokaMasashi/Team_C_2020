using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    Slider slider;
    SoundManager soundManager;

    void Start()
    {
        slider = GetComponent<Slider>();
        soundManager = SoundManager.Instance;
        slider.value = soundManager.MasterVolume;

        //Masterを初期選択
        slider.Select();
    }

    public void OnValueChanged()
    {
        soundManager.MasterVolume = slider.value;
    }
}