using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsKim :Buff
{
    public string buff_name { get; } = "김여사";
    public string buff_explain { get; } = "아니 누가 엘리베이터에 핸들을 달아놨어?";
    public string buff_effect { get; } = "탑승 중인 vip여성 수에 비례하여 속도가 소폭 증가합니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.vip_female_speed_rate += 0.05f;
    }
}
