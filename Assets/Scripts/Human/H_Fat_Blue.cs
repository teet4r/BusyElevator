using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Fat_Blue : H_Fat
{
    public override void Set(Floor currentfloor_script)
    {
        color = FID.BLUE;
        dirty_size = Random.Range(6f, 10f);
        speed = Random.Range(1f, 2f);
        exp = Random.Range(1, 3);
        gold = Random.Range(5, 10);
        population = 2;
        kind = Random.Range(0, 7);
        animator.SetFloat("Blend", kind);
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
        Gamemanager.Instance.objectpool.ReturnFatBlue(this);
    }
}
