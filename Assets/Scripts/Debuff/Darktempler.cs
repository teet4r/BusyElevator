using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darktempler : Debuff
{
    public string debuff_name { get; } = "도크템플러";
    public string debuff_explain { get; } = "아둔 토리다스";
    public string debuff_effect { get; } = "밤에 도둑이 더욱 많이 생성됩니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.thief_appear_in_night_rate += 0.15f;
    }
}
