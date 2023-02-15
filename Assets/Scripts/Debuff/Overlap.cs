using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlap :  Debuff
{
    public string debuff_name { get; } = "중복";
    public string debuff_explain { get; } = "재업";
    public string debuff_effect { get; } = "앞으로의 버프와 디버프가 중복해서 나올 수 있습니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.is_overlap_on = true;
    }
}
