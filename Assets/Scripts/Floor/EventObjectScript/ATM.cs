using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATM : EventObject
{
    // 은행 층에 존재
    // 일정 재화 획득 or 일정 재화 잃음

    public void Awake()
    {
        earn_things = 20;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        // 하고 싶은 행동 정의
        yield return human.StartCoroutine(human.ActAtATM(this));
        human.animator.SetBool("isWaiting", false);
    }
}
