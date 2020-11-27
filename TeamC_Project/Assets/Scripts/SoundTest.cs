using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    SoundManager sm;
    [SerializeField]
    string bgmName;
    [SerializeField]
    string seName;
    [SerializeField]
    string loopSeName;

    [SerializeField]
    AudioClip bgmClip;
    [SerializeField]
    AudioClip seClip;

    void Awake()
    {
        sm = SoundManager.Instance;
        //sm.PlayBgmByName(bgmName);
    }

    public void OnClick()
    {
        sm.PlaySeByName(seName);
    }

    public void OnClick2()
    {
        sm.PlaySeByName(loopSeName, true);
    }
}
