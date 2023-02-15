using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : EventObject
{
    // ½Ä´ç Ãþ¿¡ Á¸Àç
    // µ·¹Þ°ÚÁö?
    public float addict_threshold = 0.1f;

    void Awake()
    {
        use_count = 15;
        earn_things = 3;
        spend_things = -10;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);

        if (use_count > 0)
        {
            yield return human.StartCoroutine(human.ActAtVM(this));
            use_count -= Gamemanager.Instance.buffmanager.use_size_of_bevarage;

            human.animator.SetBool("isWaiting", false);

            if (!delivery_called && use_count <= 0)
            {
                delivery_called = true;
                FF.StartCoroutine(FF.MakeDelivery(kind_of, id, H_Delivery.DeliveryItem.BEVERAGESBOXES, cur_floor));
            }
        }
    }
}