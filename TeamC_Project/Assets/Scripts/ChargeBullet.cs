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

    [SerializeField, Range(2, 5)]
    private int maxBulletCount = 5;
    [SerializeField]
    private float chargeSpeed = 1.0f;
    [SerializeField]
    private Slider[] chargeSliders;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentMode = ChargeMode.STAGE_1;

        for (int i = 0; i < maxBulletCount; i++)
        {
            chargeSliders[i].value = 0;
        }
        for (int i = maxBulletCount; i < chargeSliders.Length; i++)
        {
            chargeSliders[i].enabled = false;
            chargeSliders[i].gameObject.SetActive(false);
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
        int chargeCount = (int)currentMode;
        chargeSliders[chargeCount].value += Time.deltaTime * chargeSpeed;
        IncreaseCharge(chargeCount);
    }

    private void IncreaseCharge(int chargeCount)
    {
        if (chargeSliders[chargeCount].value < chargeSliders[chargeCount].maxValue) return;
        if (chargeCount + 1 < maxBulletCount)
            currentMode++;
    }

    public void DecreaseCharge()
    {
        float chargeAmount = chargeSliders[(int)currentMode].value;
        chargeSliders[(int)currentMode].value = 0;
        currentMode--;
        chargeSliders[(int)currentMode].value -= chargeSliders[(int)currentMode].maxValue - chargeAmount;
    }

    public bool GetCanShot()
    {
        bool canShot = false;

        if (currentMode != ChargeMode.STAGE_1)
            canShot = true;

        return canShot;
    }
}
