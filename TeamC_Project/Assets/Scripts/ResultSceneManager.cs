using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text resultText;

    private InputManager inputManager;
    [SerializeField]
    private FadeScene fadeScene;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        if (GameManager.GetResult() == GameManager.ResultMode.GAMECLEAR)
        {
            resultText.text = "Game Clear!";
        }
        else if (GameManager.GetResult() == GameManager.ResultMode.GAMEOVER)
        {
            resultText.text = "Game Over..";
        }
    }

    // Update is called once per frame
    void Update()
    {
        NextScene();
    }

    private void NextScene()
    {
        if (inputManager.GetA_ButtonDown())
        {
            fadeScene.ChangeNextScene("Title");
        }
    }
}
