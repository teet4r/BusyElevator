using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateRush :  Buff
{
    public string buff_name { get; } = "고장난 자명종";
    public string buff_explain { get; } = "시간이 느리게 가는 거 같은데?";
    public string buff_effect { get; } = "러쉬 간격이 증가합니다.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.rush_distance_size += 0.5f;
    }
}
