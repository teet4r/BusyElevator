using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : EventObject
{
    public void Awake()
    {
        use_count = 15;
        earn_things = 0;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);

        if (use_count > 0)
        {
            yield return human.StartCoroutine(human.ActAtCounter(this));
            use_count -= 1;

            human.animator.SetBool("isWaiting", false);

            if (!delivery_called && use_count <= 0)
            {
                delivery_called = true;
                FF.StartCoroutine(FF.MakeDelivery(kind_of, id, H_Delivery.DeliveryItem.DELIVERYBOXES, cur_floor));
            }
        }
    }
}
