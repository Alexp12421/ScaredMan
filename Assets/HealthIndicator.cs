using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthIndicator : MonoBehaviour
{
    public RectTransform hpBar;

    void Start()
    {
        if (hpBar == null)
            Debug.Log("No Hp bar");
    }

    public void setHpbar(float currentHp, float maxHp) 
    {
        float value = currentHp / maxHp;

        hpBar.localScale = new Vector3 (value, hpBar.localScale.y, hpBar.localScale.z);
    }
}
