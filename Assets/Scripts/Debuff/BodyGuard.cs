using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGuard : Debuff
{
    public string debuff_name { get; } = "보디가드";
    public string debuff_explain { get; } = "나는 빨간약으로 주세요...";
    public string debuff_effect { get; } = "대기줄에서 사장이 차지하는 크기가 커지며 엘리베이터도 혼자 이용합니다";
    public void DebuffOn()
    {
        // 사장 쪽에서 건들여야도미
    }
}
