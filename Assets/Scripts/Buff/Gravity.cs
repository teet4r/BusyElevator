using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : Buff
{
    int count = 1;
    public string buff_name { get; } = "중력";
    public string buff_explain { get; } = "엘리베이터도 중력을 느껴!";
    public string buff_effect { get; } = "엘리베이터가 올라갈 때는 느려지지만 내려갈 때는 빨라집니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.gravity_elevator_rate += Mathf.Pow(0.5f, count++);
    }
}
