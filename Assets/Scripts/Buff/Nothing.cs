using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nothing : Buff
{
    public string buff_name { get; } = "Nothing";
    public string buff_explain { get; } = "";
    public string buff_effect { get; } = "이 버프는 아무 효과도 없습니다. 과연 아무 효과도 없을까요?";
    public void BuffOn()
    {
        // 업적
    }
}
