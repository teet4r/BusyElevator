using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal :  Debuff
{
    public string debuff_name { get; } = "Arsenal";
    public string debuff_explain { get; } = "축구에는 이유를 설명할 수 없지만, 그냥 그런 것이 존재한다.";
    public string debuff_effect { get; } = "층 생성 수가 3개에서 4개로 증가합니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.plus_floor_size += 1;
    }
}
