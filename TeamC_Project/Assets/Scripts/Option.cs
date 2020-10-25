using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField]
    private FadeScene fadeScene;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ExitOption())
            fadeScene.ChangeNextScene("Select");
    }

    private bool ExitOption()
    {
        if (inputManager.GetB_ButtonDown() || inputManager.GetView_ButtonDown())
            return true;
        else return false;
    }
}
