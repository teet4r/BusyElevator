using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    // tag = EventObject
    // Layer = AfterHuman(11번) = 내린 이후 사람과만 충돌


    // 필요할지 몰라서 일단 아무거나 적어둠
    public Floor cur_floor;
    public int kind_of; // object 종류 (층마다 0부터 시작)
    public int id; // object 번호 (종류마다 0부터 시작)
    public int use_count; // 사용 가능 횟수
    public int spend_things; // 소모 재화
    public int earn_things; // 얻는 재화
    public bool delivery_called = false; // 이미 배달부를 불렀으면 true, 중복 호출 방지
    protected F_First FF;

    private void Start()
    {
        FF = Gamemanager.Instance.buildgame.floors[FID.FIRST][0] as F_First;
    }
    // 객체별 행동
    // Human 에서 특정 Move가 끝날때마다 조건부로(충돌 등) 호출될 예정
    public virtual IEnumerator EventOn(Human human) 
    {
        yield return null;
    }
}
