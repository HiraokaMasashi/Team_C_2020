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
    [SerializeField]
    private Text[] menus;

    SoundManager soundManager;
    GameManager gameManager;
    private int selectNumber;
    [SerializeField]
    private float textTime;
    private float timer;
    [SerializeField]
    private Color activeColor;

    void Start()
    {
        panel.SetActive(false);
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        //initSelect.Select();
        soundManager = SoundManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //ポーズ中のみの動作
        if (isPause)
        {
            float stick = inputManager.GetL_Stick_Vertical();
            float v = Mathf.Abs(stick);

            if (Mathf.Abs(stick) >= 0.5f)
            {
                selectNumber -= (int)(1 * (v / stick));
                selectNumber = selectNumber < 0 ? 0 : selectNumber;
                selectNumber = selectNumber > 1 ? 1 : selectNumber;
            }
            //Escキー+左右どちらかのshift
            //if ((Input.GetKeyDown(KeyCode.Escape)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            //{
            //    GameQuit();
            //}

            //Viue(startの反対側にあるボタン　backって書いてあったりもする)ボタン
            //if (inputManager.GetView_ButtonDown())
            //{
            //    GameQuit();
            //}

            if (inputManager.GetA_ButtonDown())
            {
                if (selectNumber == 1)
                {
                    fadeScene.ChangeNextScene("Select");
                    Resume();
                }
                else if (selectNumber == 0)
                {
                    Resume();
                }
            }
            ActiveText();
        }

        //コントローラーメニューボタン
        if (inputManager.GetMenu_ButtonDown())
        {
            PauseSwich();
        }
    }

    private void ActiveText()
    {
        timer += Time.unscaledDeltaTime;
        if (timer <= textTime / 2)
        {
            menus[selectNumber].color = activeColor;
            menus[selectNumber].fontSize = 18;
        }
        else
        {
            menus[selectNumber].color = Color.white;
            menus[selectNumber].fontSize = 14;
        }
        if (timer >= textTime * 2)
            timer = 0;
        menus[1 - selectNumber].color = Color.white;
        menus[1 - selectNumber].fontSize = 14;
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
        if (fadeScene.IsFadeIn || fadeScene.IsFadeOut
            || gameManager.IsPerformance || gameManager.IsEnd) return;

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
    }

    /// <summary>
    /// 再開する
    /// </summary>
    void Resume()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    void GameQuit()
    {
        fadeScene.GameQuitFadeOut();
    }
}
