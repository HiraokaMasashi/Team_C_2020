using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScene : SingletonMonoBehaviour<FadeScene>
{
    private Image fadeImage;//フェード用のイメージ
    private Color fadeColor;//フェードカラー

    [SerializeField]
    private float fadeSpeed = 1.0f;//フェード速度

    private string nextSceneName;//次のシーンの名前

    private SoundManager soundManager;

    private static string beforeSceneName = "";//前のシーンの名前

    /// <summary>
    /// フェードイン中か
    /// </summary>
    public bool IsFadeIn
    {
        get;
        private set;
    } = true;

    /// <summary>
    /// フェードアウト中か
    /// </summary>
    public bool IsFadeOut
    {
        get;
        private set;
    } = false;

    /// <summary>
    /// ゲーム終了か
    /// </summary>
    public bool IsGameQuit
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        IsFadeIn = true;
        IsFadeOut = false;
        fadeColor = fadeImage.color;
        fadeColor.a = 1.0f;
        if (GameObject.Find("SoundManager") != null)
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }
    }

    void FixedUpdate()
    {
        FadeIn();
        FadeOut();
    }

    /// <summary>
    /// フェードイン処理
    /// </summary>
    private void FadeIn()
    {
        //フェードイン中でなければreturn
        if (!IsFadeIn) return;

        fadeColor.a -= fadeSpeed * Time.deltaTime;
        fadeImage.color = fadeColor;

        //α値が0以下になったらフェードイン終了
        if (fadeColor.a <= 0.0f)
        {
            fadeColor.a = 0.0f;
            IsFadeIn = false;
            fadeImage.enabled = false;
        }
    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    private void FadeOut()
    {
        //フェードアウト中でなければreturn
        if (!IsFadeOut) return;

        fadeColor.a += fadeSpeed * Time.deltaTime;
        fadeImage.color = fadeColor;

        //α値が1以上になったらフェードアウト終了
        if (fadeColor.a >= 1.0f)
        {
            fadeColor.a = 1.0f;
            //IsFadeOut = false;
            //次のシーンがセレクトもしくは、オプションでなければBGMを止める
            if (nextSceneName != "Select" && nextSceneName != "Option")
                SoundStop();
            //ゲーム終了でなければ指定した次のシーンへ
            if (!IsGameQuit)
                SceneManager.LoadScene(nextSceneName);
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
#endif
            }
        }
    }

    /// <summary>
    /// BGM停止処理
    /// </summary>
    private void SoundStop()
    {
        if (soundManager == null) return;

        soundManager.StopBgm();
    }

    /// <summary>
    /// シーン遷移開始処理
    /// </summary>
    /// <param name="SceneName"></param>
    public void ChangeNextScene(string SceneName)
    {
        //フェード中は実行しない
        if (IsFadeIn || IsFadeOut) return;

        IsFadeOut = true;
        fadeImage.enabled = true;
        //現在のシーンを前のシーンとして格納
        beforeSceneName = SceneManager.GetActiveScene().name;
        //次のシーン名を格納
        nextSceneName = SceneName;
    }

    /// <summary>
    /// 前のシーンの名前を返す
    /// </summary>
    /// <returns></returns>
    public static string GetBeforeSceneName()
    {
        return beforeSceneName;
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void GameQuitFadeOut()
    {
        IsGameQuit = true;
        IsFadeOut = true;
        fadeImage.enabled = true;
    }
}
