using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField, Header("メニューパネル")]
    GameObject panel;
    //[SerializeField,Header("初期選択ボタン")]
    //Button initSelect;

    private bool isPause;

    InputManager inputManager;
    [SerializeField]
    private FadeScene fadeScene;

    SoundManager soundManager;

    void Start()
    {
        panel.SetActive(false);
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        //initSelect.Select();
        soundManager = SoundManager.Instance;
    }

    void Update()
    {
        //ポーズ中のみの動作
        if (isPause)
        {
            //Escキー+左右どちらかのshift
            if ((Input.GetKeyDown(KeyCode.Escape)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                GameQuit();
            }

            //Viue(startの反対側にあるボタン　backって書いてあったりもする)ボタン
            if (inputManager.GetView_ButtonDown())
            {
                GameQuit();
            }

            if (inputManager.GetA_ButtonDown())
            {
                fadeScene.ChangeNextScene("Select");
                Resume();
            }
        }

        //コントローラーメニューボタン
        if (inputManager.GetMenu_ButtonDown())
        {
            PauseSwich();
        }
    }

    public void OnClickResume()
    {
        Resume();
    }

    /// <summary>
    /// ポーズと再開を切り替える
    /// </summary>
    void PauseSwich()
    {
        isPause = !isPause;

        if (isPause)
        {
            PauseOn();
        }
        else
        {
            Resume();
        }
    }

    /// <summary>
    /// ポーズする
    /// </summary>
    void PauseOn()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
        soundManager.PlaySeByName("pose1");
        Debug.Log("Pause");
    }

    /// <summary>
    /// 再開する
    /// </summary>
    void Resume()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
        Debug.Log("Resume");
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
}
