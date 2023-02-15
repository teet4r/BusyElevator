using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatron : Buff
{
    int count = 1;
    public string buff_name { get; } = "순찰 5분컷";
    public string buff_explain { get; } = "순찰하고 똥싸러가야지";
    public string buff_effect { get; } = "가드의 이동속도가 증가합니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.patron_speed_rate += Mathf.Pow(0.5f, count++);
        Gamemanager.Instance.employeemanager.PatronSpeedUp();
    }
}
