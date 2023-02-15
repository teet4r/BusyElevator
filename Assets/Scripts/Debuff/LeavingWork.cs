using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingWork : Debuff
{
    public string debuff_name { get; } = "칼!퇴!";
    public string debuff_explain { get; } = "누구도 날 막지 못해";
    public string debuff_effect { get; } = "퇴근 시간에 사람들이 더욱 많이 생깁니다";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.go_to_home_rate += 1.5f;
    }
}
