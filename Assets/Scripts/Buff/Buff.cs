using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Buff 
{
    public string buff_name { get; }
    public string buff_explain{ get; }
    public string buff_effect { get; }
    public void BuffOn();
}
