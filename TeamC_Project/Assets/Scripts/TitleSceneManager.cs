using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform titleLogo;
    [SerializeField]
    private Text pressLogo;
    private bool moveEnd;
    [SerializeField]
    private InputManager Input;
    [SerializeField]
    private FadeScene sceneManager;

    private SoundManager soundManager;
    [SerializeField]
    private string bgm;
    [SerializeField]
    private string se;

    void Awake()
    {
        pressLogo.gameObject.SetActive(false);
        titleLogo.transform.position += Vector3.up * 775;
        moveEnd = false;
        soundManager = SoundManager.Instance;
        soundManager.PlayBgmByName(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.IsFadeIn || sceneManager.IsFadeOut) return;

        //ムービー再生
        if (!moveEnd)
        {
            //ボタンが押されたらムービー終了
            if (PressAnyButton())
            {
                moveEnd = true;
                titleLogo.anchoredPosition = new Vector3(titleLogo.anchoredPosition.x, 100);
                pressLogo.gameObject.SetActive(true);
            }
            Opening();
            return;
        }
        else
        {
            if ((int)Time.time % 2 == 0)
                pressLogo.gameObject.SetActive(false);
            else
                pressLogo.gameObject.SetActive(true);

            //ムービー終了後にボタンを押すと次のシーンへ
            if (PressAnyButton())
            {
                soundManager.PlaySeByName(se);
                sceneManager.ChangeNextScene("Select");
            }

            if (GameExit())
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
#endif
            }
        }
    }

    //タイトルで流すムービー演出を書く
    private void Opening()
    {
        //タイトルロゴを動かす
        titleLogo.position -= Vector3.up * 2;

        //ロゴが指定の位置に来たら終了
        if (titleLogo.anchoredPosition.y <= 100)
        {
            titleLogo.anchoredPosition = new Vector3(titleLogo.anchoredPosition.x, 100);
            pressLogo.gameObject.SetActive(true);
            moveEnd = true;
        }
    }

    //複数のボタンの入力を受け付ける
    private bool PressAnyButton()
    {
        if (Input.GetA_ButtonDown() || Input.GetB_ButtonDown()
        || Input.GetX_ButtonDown() || Input.GetY_ButtonDown()
        || Input.GetMenu_ButtonDown())
            return true;
        else
            return false;
    }

    private bool GameExit()
    {
        if (Input.GetView_ButtonDown())
            return true;
        else return false;
    }
}
