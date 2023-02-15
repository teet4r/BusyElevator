using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaddyAwake :  Buff
{
    public string buff_name { get; } = "아빠 안잔다";
    public string buff_explain { get; } = "분명 눈을 감고 있을텐데?";
    public string buff_effect { get; } = "가드가 뒤로도 볼 수 있습니다. //아직 구현 X//";
    public void BuffOn()
    {
        //Gamemanager.Instance.buffmanager.vvvvvip_speed_rate = 3f;
    }
}
