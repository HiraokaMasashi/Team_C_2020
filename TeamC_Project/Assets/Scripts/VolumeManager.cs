using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

[System.Serializable]
public class Volume
{
    public float Master;
    public float BGM;
    public float SE;
}

public class VolumeManager : SingletonMonoBehaviour<VolumeManager>
{
    Volume volume = new Volume();
    string filePath;

    [HideInInspector]
    public bool isLoadfile;

    public float MastarVolume
    {
        set { volume.Master = value; }
        get { return volume.Master; }
    }
    public float BgmVolume
    {
        set { volume.BGM = value; }
        get { return volume.BGM; }
    }
    public float SeVolume
    {
        set { volume.SE = value; }
        get { return volume.SE; }
    }

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        filePath = Application.dataPath + "/volume_data.dat";
#elif UNITY_STANDALONE
        filePath = Application.persistentDataPath + "/volume_data.dat";
#endif

        if (File.Exists(filePath))
        {
            LoadVolume();
            isLoadfile = true;
        }
        else
        {
            SaveVolume();
            isLoadfile = false;
        }
    }

    void Start()
    {
        
    }

    /// <summary>
    /// データの保存
    /// </summary>
    public void SaveVolume()
    {
        string jsonstr = JsonUtility.ToJson(volume);
        //保存データを暗号化する
        byte[] data = Encoding.UTF8.GetBytes(jsonstr);
        data = Cryptor.Encrypt(data);

        FileStream fileStream = File.Create(filePath);
        fileStream.Write(data, 0, data.Length);
    }

    /// <summary>
    /// データの読込
    /// </summary>
    public void LoadVolume()
    {
        byte[] data = null;

        FileStream fileStream = File.OpenRead(filePath);
        data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);

        //読み込むデータを複合化する
        data = Cryptor.Decrypt(data);
        string jsonstr = Encoding.UTF8.GetString(data);
        volume = JsonUtility.FromJson<Volume>(jsonstr);
    }
}
