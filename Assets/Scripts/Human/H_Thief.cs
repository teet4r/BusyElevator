using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Thief : Human, ActInterface
{
    public bool is_caught { get; set; }
    public override void Set(Floor currentfloor_script)
    {
        name = "thief";
        color = PID.THIEF;
        dirty_size = Random.Range(5f, 6f);
        speed = Random.Range(2f, 4f);
        exp = 0;
        gold = 0;
        population = 0;
        is_caught = false;
        animator.speed = speed / 3;
        time_who_can_wait *= Gamemanager.Instance.buffmanager.thief_patience_rate;

        this.currentfloor_script = currentfloor_script;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
            destinations,
            currentfloor_script.floor_level,
            // params
            FID.RED,
            FID.BLUE,
            FID.GREEN,
            FID.YELLOW,
            FID.MASTER,
            FID.HOSPITAL,
            FID.VIP,
            FID.BANK
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
        speech_bubble.gameObject.SetActive(false);
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnThief(this);
    }

    public override IEnumerator MovetoElevator(Vector2 start, Vector2 end)
    {
        is_caught = true;

        rigid.position = start;
        animator.SetBool("isWaiting", false);

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime * (1 + Gamemanager.Instance.buffmanager.elevator_on_speed_rate));
            yield return null;
        }

        is_caught = false;
    }


    /// <summary>
    /// Override Functions of ActInterface
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// 
    public override IEnumerator ActAtATM(ATM atm)
    {
        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EXCITED];
        // 행동 정의
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, -(int)((0.1 + Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate) * Gamemanager.Instance.buildgame.gold));
        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtVM(VendingMachine vm)
    {
        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EXCITED];

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, (int)(vm.spend_things * (1 + Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate)));// -10 gold
        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtHD(HospitalDoor hd)
    {
        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EXCITED];

        is_caught = true;
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, (int)(hd.spend_things * (1 + Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate))); // -100 gold

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtSAD(SmokingAreaDoor sad) // Smoking Area Door
    {
        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EXCITED];

        is_caught = true;
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, (int)(-(Random.Range(1, 10)) * (1 + Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate)));

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtWP(WaterPurifier wp)
    {
        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtBS(BookShelf bs)
    {
        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtND(NormalDoor nd)
    {
        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EXCITED];

        is_caught = true;
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, (int)(nd.spend_things * (1 + Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate)));

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }

    public override void ActInElevator(ElevatorClass elevator)
    {
        StopCoroutine(move_coroutine);
        //도둑 수 갱신
        elevator.thief_cnt++;

        elevator.thief_bubble.SetActive(true); // 이미지 켜
    }

    public override void ActOffElevator(ElevatorClass elevator)
    {
        des_bubble_text.text = "";

        Gamemanager.Instance.buildgame.floor_of[destination_floor].SetDirtyRate(dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));

        //도둑 갱신
        elevator.thief_cnt--;

        if (elevator.thief_cnt <= 0) // 도둑이 없으면
            elevator.thief_bubble.SetActive(false); // 이미지 꺼

        speech_bubble.gameObject.SetActive(false);
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";

        // 움직임
        if (gameObject.activeSelf)
            move_right = StartCoroutine(MoveRight(elevator.rigid.position));
    }


    /// <summary>
    /// Override Functions of reacting with humans each other of ActInterface
    /// </summary>
    /// <returns></returns>
    public override IEnumerator ActWithGuard(Human guard)
    {
        if (!is_caught && state != HumanState.BOARDING)
        {
            is_caught = true;
            guard.StartCoroutine(guard.ActWithThief(this));
            speech_bubble.gameObject.SetActive(true);
            des_bubble_text.text = "";
            speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.ARREST];
            if (move_coroutine != null)
            {
                rigid.velocity = Vector2.zero;
                StopCoroutine(move_coroutine);
            }
            if (move_right != null)
                StopCoroutine(move_right);
            if (angry_timer != null)
                StopCoroutine(angry_timer);
            yield return StartCoroutine(Fadeout());
            ReturnHuman();
        }
    }
}