using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Mechanic : Human
{
    /// <<수리>>
    /// 수리를 원하는 엘레베이터가 수리 층에 도착
    /// 수리공을 오른쪽에서 생성
    /// 해당 엘레베이터까지 이동
    /// 게이지 바를 통한 수리 현황 제시
    /// 수리가 완료되면 엘리베이터 출발 및 수리공 오른쪽으로 퇴장

    public int id;
    public int pay;
    public GameObject fixing_image; // 수리 중임을 보여주는 이미지
    public ElevatorClass elevator;  // 수리해야하는 엘리베이터
    public Vector2 end_point;
    public int fixing_speed;// 수리 속도 (숙련도)
    public int fix_elevator_count; // 지금까지 수리한 엘리베이터 수
    public bool is_working = false;

    public Mechanic_Info mech_info;

    public override void Set(Floor currentfloor_script)
    {
        spriteRenderer.flipX = false;
        population = 0;
        animator.speed = speed / 3;

        is_INF_human = true;

        this.currentfloor_script = currentfloor_script;

        StartMove(currentfloor_script.spawn_point_up, true);
    }
    public override void StartMove(Vector2 spawn_point, bool going_left) // 수리를 위한 이동 로직
    {
        rigid.position = spawn_point;
        end_point = (Vector2)(currentfloor_script as F_Repairshop).repair_point[elevator.line].transform.position;

        if (gameObject.activeSelf)
            move_coroutine = StartCoroutine(Move());
    }
    protected override IEnumerator Move() // 수리 전
    {
        while (end_point != rigid.position)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end_point, speed * Time.deltaTime); // move
            yield return null;
        }

        StartCoroutine(FixElevator()); // 수리 시작
    }

    IEnumerator FixElevator() // 수리 로직
    {
        fixing_image.SetActive(true);
        animator.SetBool("isFixing", true);

        for (int i = 0; i < 100; i++)  //  숙련도가 높을 수록 빨리 수리, 한번에 내구도 1 수리
        {
            if (elevator.cur_durability >= elevator.max_durability || Gamemanager.Instance.buildgame.gold < 1)
                break;

            elevator.AddDurability(fixing_speed, false);

            if (Random.Range(0,100) < Gamemanager.Instance.buffmanager.reinforce_fail_threshold) // 강화 실패 디버프 (음주 수리)
            {
                //Gamemanager.Instance.buildgame.SetAlarm(elevator.name + " 수리 실패!!!");
                elevator.AddDurability(-30, false);

                if (elevator.cur_durability < 10)
                    elevator.cur_durability = 10;

                break; //수리 중지
            }

            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, -1);

            yield return new WaitForSeconds(1f);
        }

        // 수리 끝
        elevator.FinishFix();

        mech_info.employee_fix_elevator_count.text = "수리횟수 : " + ++fix_elevator_count + "번";
        fixing_image.SetActive(false);
        spriteRenderer.flipX = true;
        animator.SetBool("isFixing", false);

        yield return StartCoroutine(MoveAtoB(rigid.position,currentfloor_script.spawn_point_down));

        Gamemanager.Instance.employeemanager.remain_mechanic_count++;
        ReturnHuman();
    }
    protected override IEnumerator MoveAtoB(Vector2 start, Vector2 end) // 수리 후
    {
        rigid.position = start;

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime);
            yield return null;
        }
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.employeemanager.ReturnMechanic(id);
    }
}