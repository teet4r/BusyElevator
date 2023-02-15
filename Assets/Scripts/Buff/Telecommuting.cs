using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telecommuting : Buff
{
    public string buff_name { get; } = "재택근무";
    public string buff_explain { get; } = "특) 게임가능";
    public string buff_effect { get; } = "출근하는 사람의 수가 줄어듭니다";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.go_to_work_rate--; 
    }
}
