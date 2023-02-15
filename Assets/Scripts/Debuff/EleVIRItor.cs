using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleVIRItor : Debuff
{
    public string debuff_name { get; } = "엘리 \"비리\" 터";
    public string debuff_explain { get; } = "누가 엘리베이터를 나무로 만들었니?";
    public string debuff_effect { get; } = "엘리베이터의 내구도가 더더욱 빨리 감소합니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.durability_decrease_rate = 4f;
    }
}
