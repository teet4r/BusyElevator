using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acrophobia : Debuff
{
    public string debuff_name { get; } = "고소공포증";
    public string debuff_explain { get; } = "가장 큰 공포를 느낀다는 11미터 어쩌고...";
    public string debuff_effect { get; } = "건물 절반이상 올라가면 엘리베이터가 느려집니다";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.acrophobia_speed_rate -= 0.4f;
    }
}
