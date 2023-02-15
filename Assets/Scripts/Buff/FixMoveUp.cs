using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMoveUp :  Buff
{
    public string buff_name { get; } = "렉카";
    public string buff_explain { get; } = "유튜브 렉카 말고요.";
    public string buff_effect { get; } = "엘리베이터가 수리를 하러 갈때 속도가 빨라집니다.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.elevator_speed_rate_for_fix += 0.2f;
    }
}
