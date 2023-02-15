using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDay :  Buff
{
    public string buff_name { get; } = "오늘은 보너스 받는 날";
    public string buff_explain { get; } = "엘리베이터한테 월급도 주나?";
    public string buff_effect { get; } = "모든 엘리베이터에게 포인트가 3개씩 추가됩니다";
    public void BuffOn()
    {
        Gamemanager.Instance.elevatormanager.PlusPoint();
    }
}
