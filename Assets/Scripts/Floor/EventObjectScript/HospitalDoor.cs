using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalDoor : EventObject
{
    // 병원층에 존재
    // 일정 재화 획득
    // H_Sick이 들어올 경우 H_Patient 확정 생성
    // 기타 다른 사람이 들어올경우 > ??
    // 도둑이 들어올 경우 > ??
    public void Awake()
    {
        earn_things = 30;
        spend_things = -100;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        yield return human.StartCoroutine(human.ActAtHD(this));
    }
}
