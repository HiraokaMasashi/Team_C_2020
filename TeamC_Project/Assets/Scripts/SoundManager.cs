using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Volume
{
    public float Master;
    public float BGM;
    public float SE;
}

public class SoundManager : MonoBehaviour
{
    AudioClip[] bgm;
    AudioClip[] se;

    Dictionary<string, int> bgmIndex = new Dictionary<string, int>();
    Dictionary<string, int> seIndex = new Dictionary<string, int>();

    AudioSource bgmAudioSource;
    AudioSource seAudioSource;

    Volume volume = new Volume();
    string path;
    string fileName = "volume_data.json";

    List<float> playingList = new List<float>();

    [SerializeField, Range(1, 30), Header("SE同時再生数上限")]
    int maxPlayingSeCount = 10;

    public float MasterVolume
    {
        set
        {
            volume.Master = value;
            bgmAudioSource.volume = volume.BGM * volume.Master;
            seAudioSource.volume = volume.SE * volume.Master;
        }
        get
        {
            return volume.Master;
        }
    }

    public float BgmVolume
    {
        set
        {
            volume.BGM = value;
            bgmAudioSource.volume = volume.BGM * volume.Master;
        }
        get
        {
            return volume.BGM;
        }
    }

    public float SeVolume
    {
        set
        {
            volume.SE = value;
            seAudioSource.volume = volume.SE * volume.Master;
        }
        get
        {
            return volume.SE;
        }
    }

    #region Singleton

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    Debug.LogError(typeof(SoundManager) + "is nothing");
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        seAudioSource = gameObject.AddComponent<AudioSource>();

        bgm = Resources.LoadAll<AudioClip>("Sound/BGM");
        se = Resources.LoadAll<AudioClip>("Sound/SE");

        for (int i = 0; i < bgm.Length; i++)
        {
            bgmIndex.Add(bgm[i].name, i);
        }

        for (int i = 0; i < se.Length; i++)
        {
            seIndex.Add(se[i].name, i);
        }

        path = Application.persistentDataPath + "/" + fileName;

        if (File.Exists(path))
            LoadVolume();
        else
        {
            InitVolume();
            SaveVolume();
        }
    }

    void Update()
    {
        //再生中のSEの再生時間を更新(カウントダウン)
        for (int i = 0; i < playingList.Count; i++)
        {
            playingList[i] -= Time.deltaTime;
            
            //終わっていたらリストから削除
            if (playingList[i] <= 0)
            {
                playingList.Remove(playingList[i]);
            }
        }
    }

    public int GetBgmIndex(string name)
    {
        if (bgmIndex.ContainsKey(name))
        {
            return bgmIndex[name];
        }
        else
        {
            Debug.LogError("指定された名前のBGMファイルが存在しません。");
            return 0;
        }
    }

    public int GetSeIndex(string name)
    {
        if (seIndex.ContainsKey(name))
        {
            return seIndex[name];
        }
        else
        {
            Debug.LogError("指定された名前のSEファイルが存在しません。");
            return 0;
        }
    }

    //BGM再生
    public void PlayBgm(int index)
    {
        index = Mathf.Clamp(index, 0, bgm.Length);

        bgmAudioSource.clip = bgm[index];
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = BgmVolume * MasterVolume;
        bgmAudioSource.Play();
    }

    public void PlayBgmByName(string name)
    {
        PlayBgm(GetBgmIndex(name));
    }

    //public void PlayBgmByClip(AudioClip clip)
    //{
    //    bgmAudioSource.clip = clip;
    //    bgmAudioSource.loop = true;
    //    bgmAudioSource.volume = BgmVolume * MasterVolume;
    //    bgmAudioSource.Play();
    //}

    public void StopBgm()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
    }

    //SE再生
    public void PlaySe(int index)
    {
        //再生中のSE数が上限数に達していたら鳴らさない
        if (playingList.Count >= maxPlayingSeCount) return;

        index = Mathf.Clamp(index, 0, se.Length);

        seAudioSource.PlayOneShot(se[index], SeVolume * MasterVolume);

        //再生したSEの再生時間の長さを取得し再生中リストに追加
        float len = se[index].length;
        playingList.Add(len);
    }

    public void PlaySe(int index,bool isLoop)
    {
        //再生中のSE数が上限数に達していたら鳴らさない
        if (playingList.Count >= maxPlayingSeCount) return;

        index = Mathf.Clamp(index, 0, se.Length);

        seAudioSource.clip = se[index];
        seAudioSource.volume = SeVolume * MasterVolume;
        seAudioSource.Play();


        //再生したSEの再生時間の長さを取得し再生中リストに追加
        float len = se[index].length;
        playingList.Add(len);
        seAudioSource.loop = isLoop;
    }

    public void PlaySeByName(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    public void PlaySeByName(string name,bool isLoop)
    {
        PlaySe(GetSeIndex(name),isLoop);
    }

    //public void PlaySeByClip(AudioClip clip)
    //{
    //    seAudioSource.clip = clip;
    //    seAudioSource.loop = false;
    //    seAudioSource.volume = SeVolume * MasterVolume;
    //    seAudioSource.Play();
    //}

    public void StopSe()
    {
        seAudioSource.Stop();
        seAudioSource.clip = null;
    }

    /// <summary>
    /// データの保存
    /// </summary>
    public void SaveVolume()
    {
        string json = JsonUtility.ToJson(volume);

        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    /// <summary>
    /// データの読込
    /// </summary>
    public void LoadVolume()
    {
        StreamReader streamReader;
        streamReader = new StreamReader(path);
        string data = streamReader.ReadToEnd();
        streamReader.Close();

        volume = JsonUtility.FromJson<Volume>(data);
    }

    private void InitVolume()
    {
        volume.Master = 0.8f;
        volume.BGM = 1.0f;
        volume.SE = 1.0f;
    }
}
