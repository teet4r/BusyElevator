using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VVVVVIP :  Buff
{
    public string buff_name { get; } = "VVVVVIP";
    public string buff_explain { get; } = "그...엘리베이터가 더 빠를 순 없나?";
    public string buff_effect { get; } = "회장을 태우면 속도가 급격하게 빨라집니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.vvvvvip_speed_rate = 3f;
    }
}
