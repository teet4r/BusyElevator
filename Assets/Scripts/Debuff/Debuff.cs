using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Debuff
{
    public string debuff_name { get; }
    public string debuff_explain { get; }
    public string debuff_effect { get; }
    public void DebuffOn();
}
