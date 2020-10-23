using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBullet : MonoBehaviour
{
    //チャージ状態
    public enum ChargeMode
    {
        STAGE_1,
        STAGE_2,
        STAGE_3,
        STAGE_4,
    }
    private ChargeMode currentMode;//現在のチャージ状態

    [SerializeField]
    private int[] stageChargeMaxCount;//段階ごとのチャージ量
    [SerializeField]
    private Color[] stageSliderColor;//段階ごとのゲージ色

    private InputManager inputManager;

    [SerializeField]
    private float chargeAngle = 30.0f;//チャージに必要な角度
    private float previousAngle;//前の角度

    private int chargeCount;//チャージ量

    [SerializeField]
    private Slider chargeSlider;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        previousAngle = 0.0f;
        chargeCount = 0;
        chargeSlider.maxValue = stageChargeMaxCount[0];
        chargeSlider.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        currentMode = ChargeMode.STAGE_1;
    }

    // Update is called once per frame
    void Update()
    {
        Charge();
    }

    /// <summary>
    /// 弾のチャージ処理
    /// </summary>
    private void Charge()
    {
        //左スティックを倒している角度を取得
        float x = inputManager.GetL_Stick_Horizontal();
        float y = inputManager.GetL_Stick_Vertical();

        Vector2 v = new Vector2(x, y);
        Vector2 dt = v - Vector2.zero;

        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        //360度方向に変換
        if (degree < 0)
        {
            degree += 360;
        }

        //前の角度と現在の角度が一定以上離れていたらカウントアップ
        if (Mathf.Abs(degree - previousAngle) >= chargeAngle)
        {
            ChangeSliderValue();
            previousAngle = degree;
        }
    }

    /// <summary>
    /// スライダーの値を代入
    /// </summary>
    /// <param name="count">代入する値</param>
    private void ChangeSliderValue()
    {
        chargeCount++;
        chargeSlider.value = chargeCount;

        switch (currentMode)
        {
            case ChargeMode.STAGE_1:
                if (chargeCount < stageChargeMaxCount[0]) return;
                chargeCount = 0;
                chargeSlider.value = 0;
                chargeSlider.maxValue = stageChargeMaxCount[1];
                chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[0];
                chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[1];
                Debug.Log("1段階目チャージ");
                currentMode = ChargeMode.STAGE_2;
                break;

            case ChargeMode.STAGE_2:
                if (chargeCount < stageChargeMaxCount[1]) return;
                chargeCount = 0;
                chargeSlider.value = 0;
                chargeSlider.maxValue = stageChargeMaxCount[2];
                chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[1];
                chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[2];
                Debug.Log("2段階目チャージ");
                currentMode = ChargeMode.STAGE_3;
                break;

            case ChargeMode.STAGE_3:
                if (chargeCount < stageChargeMaxCount[2]) return;
                Debug.Log("3段階目チャージ");
                currentMode = ChargeMode.STAGE_4;
                break;

            case ChargeMode.STAGE_4:
                break;

            default:
                break;
        }
    }

    public void ResetCharge()
    {
        chargeCount = 0;
        currentMode = ChargeMode.STAGE_1;
        previousAngle = 0.0f;
        chargeSlider.value = 0;
        chargeSlider.maxValue = stageChargeMaxCount[0];
        chargeSlider.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[0];
    }

    /// <summary>
    /// 現在のチャージ段階を返す
    /// </summary>
    /// <returns></returns>
    public ChargeMode GetChargeMode()
    {
        return currentMode;
    }
}
