using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHealing : Buff
{
    public string buff_name { get; } = "휴식 = 힐링";
    public string buff_explain { get; } = "쓰읍...하...";
    public string buff_effect { get; } = "휴식활동을 하면 체력이 조금 더 회복됩니다.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.healing_size += 3; 
    }
}
