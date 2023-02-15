using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDelivery :  Debuff
{
    public string debuff_name { get; } = "무거운 택배";
    public string debuff_explain { get; } = "삼다수 20통 시키신 분";
    public string debuff_effect { get; } = "배달부의 이동속도가 감소합니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.delivery_speed_rate -= 0.5f;
    }
}
