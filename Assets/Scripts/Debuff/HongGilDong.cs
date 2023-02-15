using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongGilDong :  Debuff
{
    public string debuff_name { get; } = "È«±æµ¿";
    public string debuff_explain { get; } = "µ¿¿¡ ¹øÂ½! ¼­¿¡ ¹øÂ½! ´Ï µ·µµ ¹øÂ½!";
    public string debuff_effect { get; } = "µµµÏÀÌ µ·À» ¾ÆÁÖ ¾ß¹«Áö°Ô ÈÉÃÄ°©´Ï´Ù";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate += 0.15f;
    }
}
