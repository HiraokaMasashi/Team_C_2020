using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour
{
    [SerializeField]
    private InputManager input;

    // Update is called once per frame
    void Update()
    {
        DebugController();
    }

    /// <summary>
    /// コントローラー操作のデバッグ表示
    /// </summary>
    private void DebugController()
    {
        if (input.GetA_ButtonDown())
        {
            Debug.Log("Push A");
            FadeScene.Instance.ChangeNextScene("Select");
        }
        if (input.GetB_ButtonDown())
        {
            Debug.Log("Push B");
        }
        if (input.GetX_ButtonDown())
        {
            Debug.Log("Push X");
        }
        if (input.GetY_ButtonDown())
        {
            Debug.Log("Push Y");
        }
        if (input.GetL_ButtonDown())
        {
            Debug.Log("Push LB");
        }
        if (input.GetR_ButtonDown())
        {
            Debug.Log("Push RB");
        }
        if (input.GetView_ButtonDown())
        {
            Debug.Log("Push View");
        }
        if (input.GetMenu_ButtonDown())
        {
            Debug.Log("Push Menu");
        }
        if (input.GetL_StickDown())
        {
            Debug.Log("Push L_Stick");
        }
        if (input.GetR_StickDown())
        {
            Debug.Log("Push R_Stick");
        }
        if (Mathf.Abs(input.GetD_Pad_Horizontal()) >= 0.1f)
        {
            Debug.Log("Push D_Pad_H");
        }
        if (Mathf.Abs(input.GetD_Pad_Vertical()) >= 0.1f)
        {
            Debug.Log("Push D_Pad_V");
        }
        if (Mathf.Abs(input.GetL_Stick_Horizontal()) >= 0.1f)
        {
            Debug.Log("Topple L_Stick_H");
        }
        if (Mathf.Abs(input.GetL_Stick_Vertical()) >= 0.1f)
        {
            Debug.Log("Topple L_Stick_V");
        }
        if (Mathf.Abs(input.GetR_Stick_Horizontal()) >= 0.1f)
        {
            Debug.Log("Topple R_Stick_H");
        }
        if (Mathf.Abs(input.GetR_Stick_Vertical()) >= 0.1f)
        {
            Debug.Log("Topple R_Stick_V");
        }
        float trigger = input.GetL_R_Trigger();
        if (trigger >= 0.1f)
        {
            Debug.Log("Push R_Trigger");
        }
        else if (trigger <= -0.1f)
        {
            Debug.Log("Push L_Trigger");
        }
    }
}
