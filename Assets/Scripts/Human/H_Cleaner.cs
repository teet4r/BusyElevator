using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Cleaner : Human
{
    public int id;
    public int pay; // 고용할때 책정
    public float cleaning_speed; // 숙련도
    public int cleanup_floors_count = 0; // 지금껏 청소한 층 수
    public bool is_working = false;

    public Cleaner_Info cleaner_info;

    Coroutine clean_coroutine;

    public override void Set(Floor currentfloor_script)
    {
        gameObject.SetActive(true);
        animator.speed = speed / 3;
        population = 1;

        is_INF_human = true;

        spriteRenderer.flipX = false;

        this.currentfloor_script = currentfloor_script;
        destination_floor = (currentfloor_script as F_Clean).cleaners_destination;
        destinationfloor_script = Gamemanager.Instance.buildgame.floor_of[destination_floor];

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
    public override void StartMove(Vector2 spawn_point, bool going_left)
    {
        layermask = 1 << gameObject.layer; // 현재 오브젝트와 같은 레이어와만 충돌
        rigid.position = spawn_point;

        if (gameObject.activeSelf)
            move_coroutine = StartCoroutine(MoveToEndLine());
    }
    protected override IEnumerator Move()
    {
        cleaner_info.employee_state.text = "현상태 : " + destination_floor + "층으로 이동중";

        while (true)
        {
            Debug.DrawRay(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left * 0.3f, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.2f, 0, 0), Vector2.left, 0.2f, layermask);
            // 현재 게임 오브젝트와 같은 레이어만 검색하게 함

            if (hit.collider != null)
            { // 사람 혹은 벽과 충돌함 멈춰야함
                yield return null;

                if (hit.collider.tag == "Endline") // 벽과 충돌
                {
                    animator.SetBool("isWaiting", true);
                    rigid.velocity = Vector2.zero;
                    currentfloor_script.ReadyToGetElevator(this);
                    yield break;
                }
                else
                {
                    rigid.velocity = hit.collider.GetComponent<Rigidbody2D>().velocity;
                    animator.SetBool("isWaiting", rigid.velocity == Vector2.zero);
                }
            }
            else // 충돌한 것이 없음 
            {
                rigid.velocity = Vector2.left * speed; // move
                animator.SetBool("isWaiting", false);
            }
            yield return null;
        }
    }
    public override void ReturnHuman()
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.cleaner_pay_size); // 청소하기 전에 돈내는 디버프(청소비)
        StartCoroutine(Clean());
    }
    IEnumerator Clean()
    {
        spriteRenderer.flipX = true;
        // 오른쪽으로 이동
        yield return StartCoroutine(MoveAtoB(rigid.position, destinationfloor_script.spawn_point_up));

        cleaner_info.employee_state.text = "현상태 : " + destination_floor + "층 청소중";

        Floor destiantion_floor_script = Gamemanager.Instance.buildgame.floor_of[destination_floor]; // 도착지

        for (int i = 0; i < 8; i++)  // 8번 청소 실행 숙련도와 버프에 따라 다름
        {
            destiantion_floor_script.SetDirtyRate(-cleaning_speed * (1 + Gamemanager.Instance.buffmanager.cleaning_rate));

            yield return new WaitForSeconds(1f);
        }

        destiantion_floor_script.is_cleaner_called = false; // 청소 끝
        cleaner_info.employee_cleanup_floors_count.text = "청소횟수 : " + ++cleanup_floors_count + "번";

        gameObject.SetActive(false);
        cleaner_info.employee_state.text = "현상태 : 대기중";
        Gamemanager.Instance.employeemanager.ReturnCleaner(id);
    }
    public override void ActOffElevator(ElevatorClass elevator)
    {
        speech_bubble.gameObject.SetActive(false); // 말풍선 삭제
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";
        rigid.position = elevator.transform.position;

        StartCoroutine(Clean());
    }
    public override void ActInElevator(ElevatorClass elevator)
    {
        //대기 인구수 제거
        StopCoroutine(move_coroutine);
    }
}
