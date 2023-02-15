using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOvertime :  Debuff
{
    public string debuff_name { get; } = "이야 야근이다";
    public string debuff_explain { get; } = "늘 새로워 짜릿해";
    public string debuff_effect { get; } = "밤 시간에 사람들이 더 이상 줄어들지 않습니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.human_create_in_night_rate = 1;
    }
}
