using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBowl : Debuff
{
    public string debuff_name { get; } = "철밥통";
    public string debuff_explain { get; } = "맨 오브 철밥통";
    public string debuff_effect { get; } = "모든 노동자의 임금이 증가하며 해고 할 수 없어집니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.is_iron_craw_on = true;
        Gamemanager.Instance.buffmanager.pay_increase_rate = 0.3f;
        Gamemanager.Instance.employeemanager.EmployeesPayUp();
    }
}
