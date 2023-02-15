using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPurifier : EventObject
{
    // ÀºÇà Ãþ¿¡ Á¸Àç
    // ÀÏÁ¤ ÀçÈ­ È¹µæ or ÀÏÁ¤ ÀçÈ­ ÀÒÀ½

    public void Awake()
    {
        use_count = 10;
        earn_things = 0;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);

        if (use_count > 0)
        {
            yield return human.StartCoroutine(human.ActAtWP(this));
            use_count -= Gamemanager.Instance.buffmanager.use_size_of_bevarage;

            human.animator.SetBool("isWaiting", false);

            if (!delivery_called && use_count <= 0)
            {
                delivery_called = true;
                FF.StartCoroutine(FF.MakeDelivery(kind_of, id, H_Delivery.DeliveryItem.WATERBUCKET, cur_floor));
            }
        }
    }
}
