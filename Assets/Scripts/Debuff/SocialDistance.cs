using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialDistance :  Debuff
{
    public string debuff_name { get; } = "사회적 거리두기";
    public string debuff_explain { get; } = "Good~ Good~ Stay right there";
    public string debuff_effect { get; } = "엘리베이터 수용량이 절반으로 감소합니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.elevator_volume_half = 2;
        Gamemanager.Instance.elevatormanager.HalfVolume();
    }
}
