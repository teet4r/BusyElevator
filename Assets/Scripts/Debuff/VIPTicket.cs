using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIPTicket : Debuff
{
    public string debuff_name { get; } = "VIP 티켓";
    public string debuff_explain { get; } = "내가 마 느그 서장이랑 마";
    public string debuff_effect { get; } = "VIP들이 시설물 이용시 비용을 내지 않습니다. 회장은 원래 안냅니다. 그것이 회장이니까.";

    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.vip_ticket = 0;
    }
}
