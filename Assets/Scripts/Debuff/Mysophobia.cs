using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mysophobia :  Debuff
{
    public string debuff_name { get; } = "결벽증";
    public string debuff_explain { get; } = "소독 불합격, 위생 불합격, 아무튼 불합격";
    public string debuff_effect { get; } = "위생관이 더 꼼꼼해 집니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.sanitation_check_size -= 20;
        if (Gamemanager.Instance.buffmanager.sanitation_check_size < 20)
            Gamemanager.Instance.buffmanager.sanitation_check_size = 0;
    }
}
