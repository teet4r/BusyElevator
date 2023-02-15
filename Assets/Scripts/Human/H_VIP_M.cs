using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_VIP_M : H_VIP, ActInterface
{
    public override void Set(Floor currentfloor_script)
    {
        is_man = true;
        color = FID.VIP;
        dirty_size = Random.Range(1f, 2f);
        speed = Random.Range(1f, 3f);
        exp = Random.Range(3, 5);
        gold = Random.Range(15, 20);
        population = 3;
        animator.speed = speed / 3;

        this.currentfloor_script = currentfloor_script;

        // Normal, Fat, VIP만 이렇게 움직임
        if (Gamemanager.Instance.buildgame.is_lunch_time)
            destinationfloor_script = Gamemanager.Instance.buildgame.GetClosestFID(currentfloor_script.floor_level, FID.REST);
        else if (Gamemanager.Instance.buildgame.is_go_to_home && currentfloor_script.floor_level != 1)
            destinationfloor_script = Gamemanager.Instance.buildgame.GetClosestFID(currentfloor_script.floor_level, FID.FIRST);
        else
        {
            destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
                destinations,
                currentfloor_script.floor_level,
                // params
                color,
                FID.BANK
            );
            if (destinationfloor_script == null)
            {
                ReturnHuman();
                return;
            }
        }
        destination_floor = destinationfloor_script.floor_level;


        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EMPTY];
        des_bubble_text.text = destination_floor.ToString();
        

        spriteRenderer.sortingLayerName = "HumanAfter";
        speech_bubble.sortingLayerName = "HumanAfter";

        if (destination_floor > currentfloor_script.floor_level)
        {
            gameObject.layer = Mask.Up;
            StartMove(currentfloor_script.spawn_point_up, true);
        }
        else
        {
            gameObject.layer = Mask.Down;
            StartMove(currentfloor_script.spawn_point_down, true);
        }
        Gamemanager.Instance.buildgame.ChangeCurrentWaitingHumanCount(population);

    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnVIPM(this);
    }
}