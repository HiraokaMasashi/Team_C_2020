using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private InputManager Input;
    [SerializeField]
    private Transform option; //Sliderの親オブジェクト
    private Slider[] options; //Sliderを格納する変数
    [SerializeField]
    private GameObject select; //選択中であることを示すオブジェクト
    private RectTransform selectBase; 
    private Vector3 basePos; //↑の初期位置
    private int selectNumber; //選択中の番号
    private int length; //要素の上限数
    [SerializeField]
    private string testSound; //SEの音量を確かめるために流す音
    [SerializeField]
    private float interval; //カーソル移動のインターバル
    private float vTimer,hTimer;
    [SerializeField]
    private Text summary;
    [SerializeField]
    private string[] textes;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        selectNumber = 0;
        length = option.transform.childCount;
        options = new Slider[length];
        for(int i = 0; i < length;i++)
        {
            options[i] = option.transform.GetChild(i).GetComponent<Slider>();
        }

        selectBase = select.GetComponent<RectTransform>();
        basePos = selectBase.position;

        soundManager = SoundManager.Instance;
        soundManager.LoadVolume();
        options[0].value = soundManager.MasterVolume;
        options[1].value = soundManager.BgmVolume;
        options[2].value = soundManager.SeVolume;
    }

    // Update is called once per frame
    void Update()
    {
        vTimer++;
        hTimer++;
        float v = -Input.GetL_Stick_Vertical();
        float h = Input.GetL_Stick_Horizontal();
        float vAbs = Mathf.Abs(v);
        float hAbs = Mathf.Abs(h);

        if(vAbs >= 0.5f && vTimer > interval)
        {
            selectNumber += (int)(1 * (v / vAbs));
            selectNumber = length <= selectNumber ? length - 1 : selectNumber;
            selectNumber = 0 > selectNumber ? 0 : selectNumber;
            vTimer = 0;
        }
        if(hAbs >= 0.5f && hTimer > interval)
        {
            float num = (float)((int)(1 * (h / hAbs))) / 10;
            if(selectNumber != 1)
            {
                if(!(options[selectNumber].value == 1 && num > 0))
                soundManager.PlaySeByName(testSound);
            }
            options[selectNumber].value += num;
            hTimer = 0;
        }
        select.transform.position = basePos - Vector3.up * selectBase.sizeDelta.y * selectNumber;

        if (v == 0)
            vTimer = interval;
        if (h == 0)
            hTimer = interval;

        summary.text = textes[selectNumber];
    }
}
