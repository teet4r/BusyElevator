using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyElevator : Buff
{
    public string buff_name { get; } = "쾌적한 엘리베이터";
    public string buff_explain { get; } = "층 전부 누르고 다니는 꼬맹이는 이제 없다";
    public string buff_effect { get; } = "엘리베이터에 사람이 없으면 속도가 대폭 증가합니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.empty_elevator_speed_rate += 1f;
    }
}
