using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmVolume : MonoBehaviour
{
    Slider slider;
    SoundManager sm;

    void Start()
    {
        slider = GetComponent<Slider>();
        sm = SoundManager.Instance;
        slider.value = sm.BgmVolume;
    }

    public void OnValueChanged()
    {
        sm.BgmVolume = slider.value;
    }
}
