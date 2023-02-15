using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruckReinforce :  Debuff
{
    public string debuff_name { get; } = "음주 수리";
    public string debuff_explain { get; } = "어이 술가져와~";
    public string debuff_effect { get; } = "수리 도중 수리가 멈추고 내구도가 감소할 수 있습니다(10보다 작아지진 않습니다)";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.reinforce_fail_threshold = 1;
    }
}
