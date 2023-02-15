using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWave : Debuff
{
    public string debuff_name { get; } = "폭염";
    public string debuff_explain { get; } = "100년 만에 최악의 폭염 어쩌구";
    public string debuff_effect { get; } = "자판기 및 정수기의 소모량이 증가합니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.use_size_of_bevarage += 1;
    }
}
