using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastLoad :  Buff
{
    public string buff_name { get; } = "빠른 탑승";
    public string buff_explain { get; } = "일렬 종대로 탓!";
    public string buff_effect { get; } = "사람들이 엘리베이터에 타는 속도가 감소합니다.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.elevator_on_speed_rate += 0.2f;
    }
}
