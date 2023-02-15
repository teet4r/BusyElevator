using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Guard : Human
{
    public int id;
    public int pay; // 고용할때 책정
    public int caught_thief_count = 0; // 지금껏 잡은 도둑 수

    public Guard_Info guard_info;
    public override void Set(Floor currentfloor_script)
    {
        gameObject.name = "guard";
        animator.speed = speed / 3;
        population = 0;

        is_INF_human = true;

        this.currentfloor_script = currentfloor_script;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
            destinations,
            currentfloor_script.floor_level,
            // params
            FID.RED,
            FID.BLUE,
            FID.GREEN,
            FID.YELLOW,
            FID.BANK,
            FID.MASTER,
            FID.REST,
            FID.VIP,
            FID.HOSPITAL
            );
        if (destinationfloor_script == null)
        {
            ReturnHuman();
            return;
        }

        destination_floor = destinationfloor_script.floor_level;

        StartCoroutine(MoveAllFloors(new Vector2(4f, (currentfloor_script.floor_level - 1) * 2)));
    }

    /// <summary>
    /// 전층을 돌아다님
    /// </summary>
    IEnumerator MoveAllFloors(Vector2 start)
    {
        // 처음 등장했을 때 로직(1층)
        rigid.position = start;
        Vector2 end = new Vector2(-4f, (currentfloor_script.floor_level - 1) * 2);
        yield return StartCoroutine(MoveAtoB(rigid.position, end));

        while (true)
        {
            // 현재 층, 목적지 층 갱신
            currentfloor_script = destinationfloor_script;
            destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
                destinations,
                currentfloor_script.floor_level,
                // params
                FID.FIRST,
                FID.RED,
                FID.BLUE,
                FID.GREEN,
                FID.YELLOW,
                FID.BANK,
                FID.MASTER,
                FID.REST,
                FID.VIP,
                FID.HOSPITAL
                );
            if (destinationfloor_script == null)
            {
                ReturnHuman();
                yield break;
            }
            destination_floor = destinationfloor_script.floor_level;

            // 목적지가 현재 층과 멀리 떨어진 곳일수록 오래 기다렸다가 나타남
            yield return new WaitForSeconds(Mathf.Abs(currentfloor_script.floor_level - destination_floor));

            if(!Gamemanager.Instance.employeemanager.guard_dic.ContainsKey(id)) // 이미 해고가 된 사람이면
            {
                ReturnHuman();
                yield break;
            }    

            guard_info.employee_state.text = "상태 :" + destination_floor + "층 순찰중";

            // 오른쪽으로 이동
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));  // 이동하려는 방향으로 돌아섬
            yield return StartCoroutine(MoveAtoB(new Vector2(-4f, (destination_floor - 1) * 2), new Vector2(4f, (destination_floor - 1) * 2)));

            // 왼쪽으로 이동
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));  // 이동하려는 방향으로 돌아섬
            yield return StartCoroutine(MoveAtoB(new Vector2(4f, (destination_floor - 1) * 2), new Vector2(-4f, (destination_floor - 1) * 2)));
        }
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnGuard(this);
    }
    /// <summary>
    /// // 도둑과의 상호작용
    /// </summary>
    /// <returns></returns>
    public override IEnumerator ActWithThief(Human other)
    {
        guard_info.employee_caught_thief_count.text = "검거횟수 : " + ++caught_thief_count + "번";
        yield return null;
    }

    /// <summary>
    /// 도둑을 잡으면 잡은 도둑 수 올라감
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    void OnTriggerEnter2D(Collider2D collision)
    {
        var tag = collision.tag;
        if (tag == "Human")
        {
            var human = collision.GetComponent<Human>();
            human.StartCoroutine(human.ActWithGuard(this));
        }
    }
}