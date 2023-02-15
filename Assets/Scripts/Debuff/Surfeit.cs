using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfeit :  Debuff
{
    public string debuff_name { get; } = "과식";
    public string debuff_explain { get; } = "이 집 김치 잘하네";
    public string debuff_effect { get; } = "비만인 사람이 생성될 확률이 증가합니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.fat_human_appear_rate += 0.2f;
    }
}
