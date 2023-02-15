using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldWall :Debuff
{
    public string debuff_name { get; } = "곰팡이벽지";
    public string debuff_explain { get; } = "어우 곰팡이 냄새!!!";
    public string debuff_effect { get; } = "심하게 더러워진 층을 사람들이 이용할때마다 돈을 잃습니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.mold_pay_size = -10;
    }
}
