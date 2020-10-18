using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    SoundManager sm;
    //[SerializeField]
    //string bgmName;
    //[SerializeField]
    //string seName;

    [SerializeField]
    AudioClip bgmClip;
    [SerializeField]
    AudioClip seClip;

    void Start()
    {
        sm = SoundManager.Instance;
        sm.PlayBgmByClip(bgmClip);
    }

    public void OnClick()
    {
        sm.PlaySeByClip(seClip);
    }
}
