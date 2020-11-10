using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    [SerializeField]
    private Image[] scenes;

    private int selectNumber;
    private int length;
    [SerializeField]
    private float interval; //カーソル移動のインターバルフレーム
    private float timer;
    //private float eTimer;

    [SerializeField]
    private InputManager Input;
    [SerializeField]
    private FadeScene sceneManager;

    [SerializeField]
    private string[] sceneNames;

    // Start is called before the first frame update
    void Start()
    {
        length = scenes.Length - 1;
        selectNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneManager.IsFadeIn || sceneManager.IsFadeOut) return;

        timer++;
        float v = -Input.GetL_Stick_Vertical();
        if (timer >= interval && v != 0)
        {
            selectNumber += (int)(1 * (v / Mathf.Abs(v)));
            selectNumber = (0 < selectNumber) ? selectNumber : 0;
            selectNumber = (length > selectNumber) ? selectNumber : length;
            timer = 0;
        }
        if (v == 0)
            timer = interval;

        //Aかメニューボタンで選択中のシーンへ
        if (Input.GetA_ButtonDown() || Input.GetMenu_ButtonDown())
        {
            if (selectNumber == length)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
#endif
            }
            else
                sceneManager.ChangeNextScene(sceneNames[selectNumber]);
        }

        //Bボタンでタイトルへ
        if (Input.GetB_ButtonDown())
            sceneManager.ChangeNextScene("Title");

        ActiveButton(selectNumber);
    }

    //選択中のボタンの演出を書く
    private void ActiveButton(int number)
    {
        //eTimer++;
        foreach (var s in scenes)
            s.color = Color.white;
        //if (eTimer % 10 == 0)
        //    scenes[number].color = Color.white;
        //else
            scenes[number].color = Color.yellow;
    }
}
