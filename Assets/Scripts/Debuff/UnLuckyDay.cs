using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLuckyDay :  Debuff
{
    public string debuff_name { get; } = "운수 나쁜 날";
    public string debuff_explain { get; } = "설렁탕도 안 사왔어!";
    public string debuff_effect { get; } = "앞으로 디버프가 하나 더 등장합니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.unlucky_day_count++;
    }
}
