using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField,Header("メニューパネル")]
    GameObject panel;
    //[SerializeField,Header("初期選択ボタン")]
    //Button initSelect;

    private bool isPause;

    void Start()
    {
        panel.SetActive(false);
        //initSelect.Select();
    }

    void Update()
    {
        //escキーかコントローラーメニューボタン
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.JoystickButton7))
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
    public void PauseOn()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
        Debug.Log("Pause");
    }

    /// <summary>
    /// 再開する
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
        Debug.Log("Resume");
    }
}
