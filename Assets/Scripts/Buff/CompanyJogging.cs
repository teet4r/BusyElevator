using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyJogging :Buff
{
    public string buff_name { get; } = "사내조깅";
    public string buff_explain { get; } = "잠시만요 저까지만 탈게요~";
    public string buff_effect { get; } = "사람들의 속도가 빨라집니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.human_speed_rate += 0.3f;
    }

}
