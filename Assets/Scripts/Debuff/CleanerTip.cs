using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerTip : Debuff
{
    public string debuff_name { get; } = "청소비";
    public string debuff_explain { get; } = "팁을 왜 건물에서 줘요?";
    public string debuff_effect { get; } = "청소부가 청소전에 돈을 받습니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.cleaner_pay_size = -30;
    }
}
