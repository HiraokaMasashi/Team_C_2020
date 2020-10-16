﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    SoundManager sm;
    [SerializeField]
    string bgmName;
    [SerializeField]
    string seName;

    void Start()
    {
        sm = SoundManager.Instance;
        sm.PlayBgmByName(bgmName);
    }

    public void OnClick()
    {
        sm.PlaySeByName(seName);
    }
}
