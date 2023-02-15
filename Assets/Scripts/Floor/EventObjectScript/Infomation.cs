using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infomation : EventObject
{
    // 1층에 존재
    // 아직 상호작용의 명확한 정체성 없음
    // 

    void Awake()
    {
        earn_things = 5;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        yield return human.StartCoroutine(human.ActAtInfo(this));
        human.animator.SetBool("isWaiting", false);
    }
}
