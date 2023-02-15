using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTimeMeal :Debuff
{
    public string debuff_name { get; } = "야식제공";
    public string debuff_explain { get; } = "야식 강제 섭취 요망";
    public string debuff_effect { get; } = "24시부터 3시까지 식당이용이 가능해집니다";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.is_there_night_rest = true;
    }
}
