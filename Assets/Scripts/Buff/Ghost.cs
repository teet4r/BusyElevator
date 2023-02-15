using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost :Buff
{
    public string buff_name { get; } = "유체화";
    public string buff_explain { get; } = "안에 있는 사람은 어떻게 되는거죠?";
    public string buff_effect { get; } = "엘리베이터들이 더이상 서로 충돌하지 않습니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.is_ghost_elevator = true;
    }
}
