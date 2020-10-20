using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeVolume : MonoBehaviour
{
    Slider slider;
    SoundManager sm;

    void Start()
    {
        slider = GetComponent<Slider>();
        sm = SoundManager.Instance;
        slider.value = sm.SeVolume;
    }

    public void OnValueChanged()
    {
        sm.SeVolume = slider.value;
    }
}
