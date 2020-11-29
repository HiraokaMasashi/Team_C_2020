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
        STAGE_5,
    }
    private ChargeMode currentMode;//現在のチャージ状態

    //[SerializeField]
    //private int[] stageChargeMaxCount;//段階ごとのチャージ量
    //[SerializeField]
    //private Color[] stageSliderColor;//段階ごとのゲージ色

    //private InputManager inputManager;

    //[SerializeField]
    //private float chargeAngle = 30.0f;//チャージに必要な角度
    //private float previousAngle;//前の角度

    //private float chargeCount;//チャージ量

    [SerializeField]
    private Slider[] chargeSliders;
    //private Player player;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //previousAngle = 0.0f;
        //chargeCount = 0;
        //chargeSlider.maxValue = 2;
        //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[0];
        currentMode = ChargeMode.STAGE_1;
        //player = GetComponent<Player>();

        for(int i=0; i<chargeSliders.Length; i++)
        {
            chargeSliders[i].value = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsGameStart) return;

        Charge();
    }

    /// <summary>
    /// 弾のチャージ処理
    /// </summary>
    private void Charge()
    {
        int chargeNum = (int)currentMode;
        chargeSliders[chargeNum].value += Time.deltaTime;

        ////連射しているなら、チャージしない
        //if (player.IsRapidFire) return;

        ////左スティックを倒している角度を取得
        //float x = inputManager.GetL_Stick_Horizontal();
        //float y = inputManager.GetL_Stick_Vertical();

        //Vector2 v = new Vector2(x, y);
        //Vector2 dt = v - Vector2.zero;

        //float rad = Mathf.Atan2(dt.x, dt.y);
        //float degree = rad * Mathf.Rad2Deg;

        ////360度方向に変換
        //if (degree < 0)
        //{
        //    degree += 360;
        //}

        ////前の角度と現在の角度が一定以上離れていたらカウントアップ
        //if (Mathf.Abs(degree - previousAngle) >= chargeAngle)
        //{
        ChangeSliderValue();
        //    previousAngle = degree;
        //}
    }

    /// <summary>
    /// スライダーの値を代入
    /// </summary>
    /// <param name="count">代入する値</param>
    private void ChangeSliderValue()
    {
        switch (currentMode)
        {
            case ChargeMode.STAGE_1:
                if (chargeSliders[0].value < chargeSliders[0].maxValue) return;
                //chargeCount = 0;
                //chargeSlider.value = 0;
                //chargeSlider.maxValue = stageChargeMaxCount[1];
                //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[0];
                //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[1];
                currentMode = ChargeMode.STAGE_2;
                break;

            case ChargeMode.STAGE_2:
                if (chargeSliders[1].value < chargeSliders[1].maxValue) return;
                //chargeCount = 0;
                //chargeSlider.value = 0;
                //chargeSlider.maxValue = stageChargeMaxCount[2];
                //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[1];
                //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[2];
                currentMode = ChargeMode.STAGE_3;
                break;

            case ChargeMode.STAGE_3:
                if (chargeSliders[2].value < chargeSliders[2].maxValue) return;
                //chargeCount = 0;
                //chargeSlider.value = 0;
                //chargeSlider.maxValue = stageChargeMaxCount[2];
                //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[2];
                //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[3];
                currentMode = ChargeMode.STAGE_4;
                break;

            case ChargeMode.STAGE_4:
                if (chargeSliders[3].value < chargeSliders[3].maxValue) return;
                //chargeCount = 0;
                //chargeSlider.value = 0;
                //chargeSlider.maxValue = stageChargeMaxCount[2];
                //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[3];
                //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[4];
                currentMode = ChargeMode.STAGE_5;
                break;

            case ChargeMode.STAGE_5:
                //if (chargeSliders[4].value < chargeSliders[4].maxValue) return;
                //chargeSlider.value = 0;
                //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = stageSliderColor[4];
                //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = stageSliderColor[5];
                //currentMode = ChargeMode.STAGE_6;
                break;

            //case ChargeMode.STAGE_6:
            //    break;

            default:
                break;
        }
    }

    public void DecreaseCharge()
    {
        //chargeCount = 0;
        float chargeAmount = chargeSliders[(int)currentMode].value;
        chargeSliders[(int)currentMode].value = 0;
        currentMode--;
        chargeSliders[(int)currentMode].value -= chargeSliders[(int)currentMode].maxValue - chargeAmount;
        //currentMode = ChargeMode.STAGE_1;
        //previousAngle = 0.0f;
        //chargeSlider.value = 0;
        //chargeSlider.maxValue = 2;
        //Color sliderBackColor, sliderColor;

        //if(currentMode == ChargeMode.STAGE_1)
        //{
        //    sliderBackColor = Color.white;
        //    sliderColor = stageSliderColor[0];
        //}
        //else
        //{
        //    sliderBackColor = stageSliderColor[(int)currentMode -1];
        //    sliderColor = stageSliderColor[(int)currentMode];
        //}

        //chargeSlider.transform.GetChild(0).GetComponent<Image>().color = sliderBackColor;
        //chargeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = sliderColor;
    }

    public bool GetCanShot()
    {
        bool canShot = false;

        if (currentMode != ChargeMode.STAGE_1)
            canShot = true;

        return canShot;
    }

    ///// <summary>
    ///// 現在のチャージ段階を返す
    ///// </summary>
    ///// <returns></returns>
    //public ChargeMode GetChargeMode()
    //{
    //    return currentMode;
    //}
}
