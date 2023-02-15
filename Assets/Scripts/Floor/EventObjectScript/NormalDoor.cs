using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDoor: EventObject
{
    public void Awake()
    {
        earn_things = 25;
        spend_things = -20;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        yield return human.StartCoroutine(human.ActAtND(this));
    }
}
