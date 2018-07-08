using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour 
{
    private static Bonus _bonus;

    public Text bonusText;
    public Text bonusTimeText;
    public string bonusName;
    public float timeBonusIsActive = 15.0f;
    float bonusTimeLeft = 0.0f;


    public Bonus()
    {
        bonusName = "none";
    }

    void Start()
    {
        bonusName = "none";
        ClearBonusTimeText();
        SetBonusText();

        _bonus = this;
    }


    public static Bonus GetBonus()
    {
        return _bonus;
    }

    public void SetBonus(string bonusName)
    {
        this.bonusName = bonusName;
        SetBonusText();

        if (bonusName != "none")
        {
            this.bonusTimeLeft = this.timeBonusIsActive;
            SetBonusTimeText(this.bonusTimeLeft);
        }
    }

    void Update()
    {

        if(bonusName != "none")
        {
            bonusTimeLeft -= Time.deltaTime;
            SetBonusTimeText(bonusTimeLeft);

            if (bonusTimeLeft < 0)
            {
                bonusName = "none";
                ClearBonusTimeText();
                SetBonusText();
            }
        }
        else
        {
            ClearBonusTimeText();
        }

    }

    void SetBonusText()
    {
        bonusText.text = "Active Bonus : " + bonusName.ToString();
    }

    void SetBonusTimeText(float time)
    {
        bonusTimeText.text = "Bonus Time: " + time.ToString();
    }

    void ClearBonusTimeText()
    {
        bonusTimeText.text = "";
    }
}
