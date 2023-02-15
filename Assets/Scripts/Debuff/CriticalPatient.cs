using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPatient :Debuff
{
    public string debuff_name { get; } = "중환자";
    public string debuff_explain { get; } = "그래도 산책은 못참지 ㅋㅋ";
    public string debuff_effect { get; } = "환자의 이동속도가 더더욱 느려집니다";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.patient_speed_rate = -0.5f;
    }
}
