using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticlaManager))]
public class Screw : MonoBehaviour
{
    public bool IsUseScrew
    {
        get;
        private set;
    } = false;

    private bool existScrew = false;

    private InputManager inputManager;
    private ParticlaManager particlaManager;

    private GameObject screw;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        particlaManager = GetComponent<ParticlaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        IsUseScrew = inputManager.GetR_Button();

        if (IsUseScrew)
        {
            Debug.Log("スクリュー使用中");
            GenerateScrew();
        }
        else
        {
            StopScrew();
        }
    }

    private void GenerateScrew()
    {
        if (existScrew) return;

        //スクリューパーティクル生成
        screw = particlaManager.GenerateParticle();
        screw.transform.position = transform.position;
        screw.transform.rotation = transform.rotation;
        //あたり判定を付ける
        screw.GetComponent<BoxCollider>().enabled = true;
        particlaManager.StartParticle(screw);
        existScrew = true;
    }

    private void StopScrew()
    {
        if (!existScrew) return;

        //パーティクルの生成を止める
        particlaManager.StopParticle(screw);
        //あたり判定をはずす
        screw.GetComponent<BoxCollider>().enabled = false;
        existScrew = false;
    }
}
