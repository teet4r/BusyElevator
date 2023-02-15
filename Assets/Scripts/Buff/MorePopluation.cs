using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorePopulation :  Buff
{
    public string buff_name { get; } = "서플라이디포";
    public string buff_explain { get; } = "Additional supply depots required.";
    public string buff_effect { get; } = "3일마다 늘어나는 인구 한계치가 증가합니다.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.supply_size += 4;
    }
}
