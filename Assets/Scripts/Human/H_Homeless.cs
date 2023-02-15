using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Homeless : Human
{
    public float sleep_time = 10f;
    public bool is_caught { get; set; }

    Coroutine contamination_coroutine;
    Coroutine sleep_coroutine;
    public override void Set(Floor currentfloor_script)
    {
        name = "homeless";
        color = PID.HOMELESS;
        dirty_size = Random.Range(15f, 20f);
        speed = Random.Range(2f, 4f);
        exp = 0;
        gold = 0;
        population = 1;
        is_caught = false;
        animator.speed = speed / 3;

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
        Gamemanager.Instance.objectpool.ReturnHomeless(this);
    }
    protected override IEnumerator Move()
    {
        contamination_coroutine = StartCoroutine(PersistentContamination(currentfloor_script));
        sleep_coroutine = StartCoroutine(Sleep());

        while (true)
        {
            Debug.DrawRay(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left * 0.3f, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left, 0.3f, layermask);

            if (hit.collider != null)
            { // 사람 혹은 벽과 충돌함 멈춰야함
                rigid.velocity = Vector2.zero;

                if (hit.collider.tag == "Endline") // 벽과 충돌
                {
                    animator.SetBool("isWaiting", true);
                    StopCoroutine(sleep_coroutine);
                    currentfloor_script.ReadyToGetElevator(this);
                    is_first = true;
                    yield break;
                }
                else
                    animator.SetBool("isWaiting", rigid.velocity == Vector2.zero);
            }
            else // 충돌한 것이 없음 
            {
                rigid.velocity = Vector2.left * speed; // move
                animator.SetBool("isWaiting", false);
            }

            yield return null;
        }
    }
    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));

        if (state == HumanState.WAITING && Random.Range(0, 100) < 30)
        {
            StopCoroutine(move_coroutine);
            rigid.velocity = Vector2.zero;
            StopCoroutine(angry_timer);
            StopCoroutine(contamination_coroutine);

            animator.SetBool("isSleeping", true);
            yield return new WaitForSeconds(sleep_time);
            animator.SetBool("isSleeping", false);

            move_coroutine = StartCoroutine(Move());
            angry_timer = StartCoroutine(AngryTimer(time_who_can_wait)); // ????
        }
    }
    public IEnumerator PersistentContamination(Floor floor_script) // 현재 위치하고 있는 층을 지속적으로 더럽힘
    {
        while(true)
        {
            floor_script.SetDirtyRate(1);
            yield return new WaitForSeconds(1);
        }
    }

    public override IEnumerator MovetoElevator(Vector2 start, Vector2 end)
    {
        rigid.position = start;
        animator.SetBool("isWaiting", false);

        StopCoroutine(contamination_coroutine);

        is_caught = true;

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime * (1 + Gamemanager.Instance.buffmanager.elevator_on_speed_rate));
            yield return null;
        }

        is_caught = false;
    }

    public override void ActOffElevator(ElevatorClass elevator)
    {
        // 층 더럽힘
        destinationfloor_script.SetDirtyRate(dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));

        //곰팡이 벽지
        if (destinationfloor_script.dirty_index > 90)
            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.mold_pay_size);

        StartCoroutine(PersistentContamination(destinationfloor_script));

        speech_bubble.gameObject.SetActive(false); // 말풍선 삭제
        des_bubble_text.text = "";
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";

        // 오른쪽으로 움직이기 시작
        move_right = StartCoroutine(MoveRight(elevator.rigid.position));
    }
}
