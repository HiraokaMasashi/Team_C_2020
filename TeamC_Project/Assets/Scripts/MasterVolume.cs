using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    Slider slider;
    SoundManager sm;

    void Start()
    {
        slider = GetComponent<Slider>();
        sm = SoundManager.Instance;
        slider.value = sm.MasterVolume;

        //Masterを初期選択
        slider.Select();
    }

    public void OnValueChanged()
    {
        sm.MasterVolume = slider.value;
    }
}