using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeverancePay :  Debuff
{
    public string debuff_name { get; } = "퇴직금";
    public string debuff_explain { get; } = "일한 만큼 받아야지";
    public string debuff_effect { get; } = "해고 시 일 횟수, 고용 날짜에 따라 돈을 지불합니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.is_severance_pay_on = true;
    }
}
