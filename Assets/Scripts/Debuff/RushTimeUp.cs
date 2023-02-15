using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushTimeUp :  Debuff
{
    public string debuff_name { get; } = "투게이트 러쉬";
    public string debuff_explain { get; } = "그만 와 제발";
    public string debuff_effect { get; } = "러쉬 지속시간이 증가합니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.rush_time_size += 0.5f;
    }
}
