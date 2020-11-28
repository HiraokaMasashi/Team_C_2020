using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPGauge : MonoBehaviour
{
    [SerializeField]
    private Image gauge;
    [SerializeField]
    private Image frame;

    [SerializeField]
    private float upTime = 1.0f;

    private Health bossHealth;

    public void DisplayGauge()
    {
        if (bossHealth == null) return;
        float currentHP = (float)bossHealth.Hp / bossHealth.MaxHp;
        if (gauge.fillAmount >= currentHP)
        {
            gauge.fillAmount = currentHP;
            return;
        }

        gauge.fillAmount += Time.deltaTime * upTime;
    }

    public void SetBossHealth(Health health)
    {
        bossHealth = health;
    }
}
