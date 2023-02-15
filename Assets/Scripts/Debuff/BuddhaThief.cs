using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddhaThief :  Debuff
{
    public string debuff_name { get; } = "∫Œ√≥ µµµœ";
    public string debuff_explain { get; } = "ﬂ≤“¥‹Ùﬁ´È—‹ÙŸ∞";
    public string debuff_effect { get; } = "µµµœ¿Ã ¥ı ø¿∑£Ω√∞£ ±‚¥Ÿ∏± ºˆ ¿÷Ω¿¥œ¥Ÿ.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.thief_patience_rate += 1;
    }
}
