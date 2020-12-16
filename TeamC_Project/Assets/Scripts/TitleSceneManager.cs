﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform titleLogo;
    [SerializeField]
    private Text pressLogo;
    [SerializeField]
    private RectTransform screwLogo;
    [SerializeField]
    private Transform bubbleParticle;
    [SerializeField]
    private RectTransform buttons;
    private float rotationSpeed;
    private bool rotateStop;
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
        rotationSpeed = 4;
        titleLogo.transform.position -= Vector3.up * 2000;
        buttons.transform.position -= Vector3.up * 500;
        //bubbleParticle.position -= Vector3.up * 60;
        moveEnd = false;
        rotateStop = false;
        soundManager = SoundManager.Instance;
    }

    private void Start()
    {
        soundManager.PlayBgmByName(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.IsFadeIn) return;
        if (sceneManager.IsFadeOut)
        {
            rotationSpeed += 0.05f;
            screwLogo.eulerAngles += Vector3.back * rotationSpeed;
            return;
        }

        //ムービー再生
        if (!moveEnd)
        {
            //ボタンが押されたらムービー終了
            if (PressAnyButton())
            {
                moveEnd = true;
                titleLogo.anchoredPosition = new Vector3(titleLogo.anchoredPosition.x, 150);
                screwLogo.eulerAngles = new Vector3(0,0,4);
                //bubbleParticle.position += Vector3.up * 100;
                buttons.anchoredPosition = Vector3.zero;
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

            //一定間隔で泡がのぼる
            //bubbleParticle.position += Vector3.up * 0.2f;
            //if ((int)Time.time % 10 == 0)
            //{
            //    bubbleParticle.position = new Vector3(0, -100, -10);
            //}

            //ムービー終了後にボタンを押すと次のシーンへ
            if (PressAnyButton())
            {
                rotationSpeed = 3;
                soundManager.PlaySeByName(se);
                sceneManager.ChangeNextScene("Select");
            }


            if (GameExit())
            {
                sceneManager.GameQuitFadeOut();
            }
        }
    }

    //タイトルで流すムービー演出を書く
    private void Opening()
    {
        //泡パーティクルを動かす
        //bubbleParticle.position += Vector3.up * 0.2f;

        //タイトルロゴを動かす
        titleLogo.position += Vector3.up * 2;

        if (!rotateStop)
        {
            //スクリュー画像を回す
            screwLogo.eulerAngles += Vector3.back * rotationSpeed;
        }

        //ロゴが指定の位置に来たら終了
        if (titleLogo.anchoredPosition.y >= 150)
        {
            titleLogo.anchoredPosition = new Vector3(titleLogo.anchoredPosition.x, 150);
            rotationSpeed -=0.03f;
            pressLogo.gameObject.SetActive(true);
            if (screwLogo.eulerAngles.z <= 0)
            {
                rotateStop = true;
            }

            if (buttons.anchoredPosition.y <= 0)
            {
                buttons.position += Vector3.up * 2.77f;
            }
            else
            {
                buttons.anchoredPosition = Vector3.zero;
                moveEnd = true;
            }
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
