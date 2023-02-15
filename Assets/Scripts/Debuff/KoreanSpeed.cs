using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoreanSpeed :Debuff
{
    public string debuff_name { get; } = "한국인 속도";
    public string debuff_explain { get; } = "지금 이걸 읽고 있을 시간이 없을텐데요?";
    public string debuff_effect { get; } = "게임 속도가 3배로 고정되고 일시정지가 사라집니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buildgame.timespeed = 3;
        Gamemanager.Instance.buffmanager.is_korean_speed = true;
        Gamemanager.Instance.uimanager.KoreanSpeedOn();
        Time.timeScale = Gamemanager.Instance.buildgame.timespeed;
    }
}
