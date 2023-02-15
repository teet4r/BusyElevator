using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload :Debuff
{
    public string debuff_name { get; } = "과적재는 사고의 원인";
    public string debuff_explain { get; } = "자 가상자리로 이동하시고 숨 참으세요";
    public string debuff_effect { get; } = "과적재 시 속도가 더욱 감소합니다";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.overload_speed_rate -= 0.3f;
    }
}
