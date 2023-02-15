using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPoisoning :  Debuff
{
    public string debuff_name { get; } = "식중독";
    public string debuff_explain { get; } = "누군가 정수기에 독을 풀었다!";
    public string debuff_effect { get; } = "식당에서 배가 아픈사람이 더욱 자주 생깁니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.illed_human_rate += 0.3f;
    }
}
