using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxyclean : Buff
{
    public string buff_name { get; } = "æ≥æ≥ΩœΩœ ø¡X≈©∏∞";
    public string buff_explain { get; } = "π¨¿∫∂ß, ∞ı∆Œ¿Ã, ±‚∏ß∂ß∏¶ «—πÊø°!";
    public string buff_effect { get; } = "√ªº“∫Œ¿« √ªº“ »ø¿≤¿Ã ¡ı∞°«’¥œ¥Ÿ";
    public void BuffOn()
    {
         Gamemanager.Instance.buffmanager.cleaning_rate += 0.2f;
    }
}
