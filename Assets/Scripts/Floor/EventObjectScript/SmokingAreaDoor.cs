using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokingAreaDoor : EventObject
{
    // 식당 층에 존재
    // 여긴 걍 없어지기만할까
    // 돈받긴 좀 뭐한데
    public void Awake()
    {
        earn_things = 0;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);

        yield return human.StartCoroutine(human.ActAtSAD(this));
    }
}
