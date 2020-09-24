using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Awake()
    {
        //InputManagerが既にあれば削除
        if (GameObject.Find("InputManager") != this.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }

        //破棄させない
        DontDestroyOnLoad(this);
    }

    #region ボタンを押したかの判定処理
    /// <summary>
    /// XboxOneのAボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetA_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 0");
    }
    /// <summary>
    /// XboxOneのBボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetB_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 1");
    }
    /// <summary>
    /// XboxOneのXボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetX_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 2");
    }
    /// <summary>
    /// XboxOneのYボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetY_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 3");
    }
    /// <summary>
    /// XboxOneのLボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetL_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 4");
    }
    /// <summary>
    /// XboxOneのRボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetR_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 5");
    }
    /// <summary>
    /// XboxOneのViewボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetView_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 6");
    }
    /// <summary>
    /// XboxOneのMenuボタンを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetMenu_ButtonDown()
    {
        return Input.GetKeyDown("joystick button 7");
    }
    /// <summary>
    /// XboxOneのLスティックを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetL_StickDown()
    {
        return Input.GetKeyDown("joystick button 8");
    }
    /// <summary>
    /// XboxOneのRスティックを押したかの判定
    /// </summary>
    /// <returns>押したかどうかを返す</returns>
    public bool GetR_StickDown()
    {
        return Input.GetKeyDown("joystick button 9");
    }
    #endregion

    #region ボタンを離したかの判定処理
    /// <summary>
    /// XboxOneのAボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetA_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 0");
    }
    /// <summary>
    /// XboxOneのBボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetB_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 1");
    }
    /// <summary>
    /// XboxOneのXボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetX_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 2");
    }
    /// <summary>
    /// XboxOneのYボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetY_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 3");
    }
    /// <summary>
    /// XboxOneのLボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetL_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 4");
    }
    /// <summary>
    /// XboxOneのRボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetR_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 5");
    }
    /// <summary>
    /// XboxOneのViewボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetView_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 6");
    }
    /// <summary>
    /// XboxOneのMenuボタンを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetMenu_ButtonUp()
    {
        return Input.GetKeyUp("joystick button 7");
    }
    /// <summary>
    /// XboxOneのLスティックを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetL_StickUp()
    {
        return Input.GetKeyUp("joystick button 8");
    }
    /// <summary>
    /// XboxOneのRスティックを離したかの判定
    /// </summary>
    /// <returns>離したかどうかを返す</returns>
    public bool GetR_StickUp()
    {
        return Input.GetKeyUp("joystick button 9");
    }
    #endregion

    #region ボタンを押しているかの判定処理
    /// <summary>
    /// XboxOneのAボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetA_Button()
    {
        return Input.GetKey("joystick button 0");
    }
    /// <summary>
    /// XboxOneのBボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetB_Button()
    {
        return Input.GetKey("joystick button 1");
    }
    /// <summary>
    /// XboxOneのXボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetX_Button()
    {
        return Input.GetKey("joystick button 2");
    }
    /// <summary>
    /// XboxOneのYボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetY_Button()
    {
        return Input.GetKey("joystick button 3");
    }
    /// <summary>
    /// XboxOneのLボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetL_Button()
    {
        return Input.GetKey("joystick button 4");
    }
    /// <summary>
    /// XboxOneのRボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetR_Button()
    {
        return Input.GetKey("joystick button 5");
    }
    /// <summary>
    /// XboxOneのViewボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetView_Button()
    {
        return Input.GetKey("joystick button 6");
    }
    /// <summary>
    /// XboxOneのMenuボタンを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetMenu_Button()
    {
        return Input.GetKey("joystick button 7");
    }
    /// <summary>
    /// XboxOneのLスティックを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetL_Stick()
    {
        return Input.GetKey("joystick button 8");
    }
    /// <summary>
    /// XboxOneのRスティックを押しているかの判定
    /// </summary>
    /// <returns>押しているかどうかを返す</returns>
    public bool GetR_Stick()
    {
        return Input.GetKey("joystick button 9");
    }
    #endregion

    #region 入力値の取得処理
    /// <summary>
    /// XboxOneのLスティックのHorizontalを取得
    /// </summary>
    /// <returns>横の入力値を返す</returns>
    public float GetL_Stick_Horizontal()
    {
        return Input.GetAxis("L_Stick_H");
    }
    /// <summary>
    /// XboxOneのLスティックのVerticalを取得
    /// </summary>
    /// <returns>縦の入力値を返す</returns>
    public float GetL_Stick_Vertical()
    {
        return Input.GetAxis("L_Stick_V");
    }
    /// <summary>
    /// XboxOneのRスティックのHorizontalを取得
    /// </summary>
    /// <returns>横の入力値を返す</returns>
    public float GetR_Stick_Horizontal()
    {
        return Input.GetAxis("R_Stick_H");
    }
    /// <summary>
    /// XboxOneのRスティックのVerticalを取得
    /// </summary>
    /// <returns>縦の入力値を返す</returns>
    public float GetR_Stick_Vertical()
    {
        return Input.GetAxis("R_Stick_V");
    }

    /// <summary>
    /// XboxOneのDパッドのHorizontalを取得
    /// </summary>
    /// <returns>横の入力値を返す</returns>
    public float GetD_Pad_Horizontal()
    {
        return Input.GetAxis("D_Pad_H");
    }
    /// <summary>
    /// XboxOneのDパッドのVerticalを取得
    /// </summary>
    /// <returns>縦の入力値を返す</returns>
    public float GetD_Pad_Vertical()
    {
        return Input.GetAxis("D_Pad_V");
    }

    /// <summary>
    /// XboxOneのLRトリガーの入力値を取得
    /// </summary>
    /// <returns>LRトリガーの入力値を返す</returns>
    public float GetL_R_Trigger()
    {
        return Input.GetAxis("L_R_Trigger");
    }
    #endregion
}
