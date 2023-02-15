using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Sanitation : Human
{
    public override void Set(Floor currentfloor_script)
    {
        name = "sanitation";
        color = PID.SANITATION;
        dirty_size = 0;
        speed = 3f;
        exp = 20;
        gold = 0;
        population = 2;

        is_INF_human = true;

        this.currentfloor_script = currentfloor_script;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
            destinations,
            currentfloor_script.floor_level,
            // params
            FID.HOSPITAL,
            FID.VIP,
            FID.BANK,
            FID.REST
        );
        if (destinationfloor_script == null)
        {
            ReturnHuman();
            return;
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
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnSanitation(this);
    }
    IEnumerator Checking()
    {
        animator.SetBool("isChecking", true);
        for (int i = 0; i < 2; i++)
        {
            speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.CHECK1];
            yield return new WaitForSeconds(0.5f);
            speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.CHECK2];
            yield return new WaitForSeconds(0.5f);
            speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.CHECK3];
            yield return new WaitForSeconds(0.5f);
        }

        speech_bubble.gameObject.SetActive(false);
        animator.SetBool("isChecking", false);

        if (destinationfloor_script.dirty_index > 70 - Gamemanager.Instance.buffmanager.sanitation_check_size)
        {
            Debug.Log("너무 더럽군요");
        }
    }
    public override IEnumerator ActAtATM(ATM atm)
    {
        speech_bubble.gameObject.SetActive(true);

        yield return StartCoroutine(Checking());
    }
    public override IEnumerator ActAtVM(VendingMachine vm)
    {
        speech_bubble.gameObject.SetActive(true);

        yield return StartCoroutine(Checking());
    }
    public override IEnumerator ActAtSAD(SmokingAreaDoor sad) // Smoking Area Door
    {
        speech_bubble.gameObject.SetActive(true);

        yield return StartCoroutine(Checking());

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtWP(WaterPurifier wp)
    {
        speech_bubble.gameObject.SetActive(true);

        yield return StartCoroutine(Checking());
    }
    public override IEnumerator ActAtBS(BookShelf bs)
    {
        speech_bubble.gameObject.SetActive(true);

        yield return StartCoroutine(Checking());
    }
    public override void ActInElevator(ElevatorClass elevator)
    {
        StopCoroutine(move_coroutine);
    }

    public override void ActOffElevator(ElevatorClass elevator)
    {
        Gamemanager.Instance.buildgame.floor_of[destination_floor].SetDirtyRate(dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));

        // 내린 층의 더러움 수치가 일정량 이상이면
        if (Gamemanager.Instance.buildgame.floor_of[destination_floor].dirty_index > 70 - Gamemanager.Instance.buffmanager.sanitation_check_size)
        {

        }

        speech_bubble.gameObject.SetActive(false);
        des_bubble_text.text = "";
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";

        // 움직임
        if (gameObject.activeSelf)
            move_right = StartCoroutine(MoveRight(elevator.rigid.position));
    }
    
}
