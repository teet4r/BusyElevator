using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinMissing : Debuff
{
    public string debuff_name { get; } = "쓰레기통이 없네";
    public string debuff_explain { get; } = "나하나 쯤이야~";
    public string debuff_effect { get; } = "층이 더욱 빠르게 더러워집니다";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.dirty_increase_rate += 0.3f;
    }
}
