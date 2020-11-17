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

    public bool IsFadeIn
    {
        get;
        private set;
    } = false;

    public bool IsFadeOut
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        IsFadeIn = true;
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

    private void FadeIn()
    {
        if (!IsFadeIn) return;

        fadeColor.a -= fadeSpeed * Time.deltaTime;
        fadeImage.color = fadeColor;

        if (fadeColor.a <= 0.0f)
        {
            fadeColor.a = 0.0f;
            IsFadeIn = false;
            fadeImage.enabled = false;
        }
    }

    private void FadeOut()
    {
        if (!IsFadeOut) return;

        fadeColor.a += fadeSpeed * Time.deltaTime;
        fadeImage.color = fadeColor;

        if (fadeColor.a >= 1.0f)
        {
            fadeColor.a = 1.0f;
            IsFadeOut = false;
            if (nextSceneName != "Select" && nextSceneName != "Option")
                SoundStop();
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void SoundStop()
    {
        if (soundManager == null) return;

        soundManager.StopBgm();
    }

    public void ChangeNextScene(string SceneName)
    {
        //フェードイン中は実行しない
        if (IsFadeIn) return;

        IsFadeOut = true;
        fadeImage.enabled = true;
        nextSceneName = SceneName;
    }
}
