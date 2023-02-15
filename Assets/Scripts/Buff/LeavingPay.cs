using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingPay : Buff
{
    public string buff_name { get; } = "퇴근비";
    public string buff_explain { get; } = "히히 못가";
    public string buff_effect { get; } = "1층으로 나가는 사람들이 골드를 지불합니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.go_to_home_pay_size += 10;
    }
}
