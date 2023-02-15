using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Master : Human, ActInterface
{
    public override void Set(Floor currentfloor_script)
    {
        color = FID.MASTER;
        dirty_size = 5f;
        speed = 3f;
        exp = 4;
        gold = 20;
        population = 2;
        animator.speed = speed / 3;

        is_INF_human = true;

        this.currentfloor_script = currentfloor_script;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
            destinations,
            currentfloor_script.floor_level,
            // params
            color,
             FID.RED,
            FID.BLUE,
            FID.GREEN,
            FID.YELLOW,
            FID.VIP,
            FID.MASTER,
            FID.HOSPITAL,
            FID.REST,
            FID.BANK,
            FID.FIRST
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
    public void ResetDestination(Floor currentfloor_script)
    {
        StopAllCoroutines();
        this.currentfloor_script = currentfloor_script;
        state = HumanState.WAITING;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
            destinations,
           currentfloor_script.floor_level,
            // params
            color,
             FID.RED,
            FID.BLUE,
            FID.GREEN,
            FID.YELLOW,
            FID.VIP,
            FID.MASTER,
            FID.HOSPITAL,
            FID.REST,
            FID.BANK,
            FID.FIRST
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

        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";
        speech_bubble.sortingLayerName = "HumanAfter";

        spriteRenderer.color = new Color(r, g, b, 1);

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

    public override void StartMove(Vector2 spawn_point, bool going_left)
    {
        layermask = 1 << gameObject.layer; // 현재 오브젝트와 같은 레이어와만 충돌
        rigid.position = spawn_point;

        if (gameObject.activeSelf)
            move_coroutine = StartCoroutine(MoveToEndLine());
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnMaster(this);
    }
    public override IEnumerator ActAtATM(ATM atm)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtVM(VendingMachine vm)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtHD(HospitalDoor hd)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        yield return new WaitForSeconds(2f);
        ResetDestination(destinationfloor_script);
    }
    public override IEnumerator ActAtSAD(SmokingAreaDoor sad) // Smoking Area Door
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        yield return new WaitForSeconds(2f);
        ResetDestination(destinationfloor_script);
    }
    public override IEnumerator ActAtExit(Exit exit)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        yield return new WaitForSeconds(2f);
        ResetDestination(destinationfloor_script);
    }
    public override IEnumerator ActAtInfo(Infomation info)
    {
        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtWP(WaterPurifier wp)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtBS(BookShelf bs)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtND(NormalDoor nd)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        yield return new WaitForSeconds(2f);
        ResetDestination(destinationfloor_script);
    }
    public override IEnumerator ActAtFFExit(FirstFloorExit ffe)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        yield return new WaitForSeconds(2f);
        ResetDestination(destinationfloor_script);
    }
    public override void ActOffElevator(ElevatorClass elevator)
    {
        Gamemanager.Instance.buildgame.floor_of[destination_floor].SetDirtyRate(dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));

        // 사장이 내렸다고 알림
        elevator.master_cnt--;
        if (elevator.master_cnt <= 0) // 회장이 없으면
            elevator.master_bubble.SetActive(false); // 이미지 꺼

        speech_bubble.gameObject.SetActive(false);
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";

        // 골드 추가
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, gold);

        // 움직임
        move_right = StartCoroutine(MoveRight(elevator.rigid.position));
    }

    public override void ActInElevator(ElevatorClass elevator)
    {
        StopCoroutine(move_coroutine);

        // 사장이 탓다고 알림
        elevator.master_cnt++;

        elevator.master_bubble.SetActive(true); // 이미지 켜
    }
}