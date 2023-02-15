using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorClass : MonoBehaviour
{
    // gameObject layer
    public static class MovingStateForLayer
    {
        public const int ASCENDING = 12; // UpElevator
        public const int DESCENDING = 13; // DownElevator
        public const int FIXING = 8;  // 수리 전후 
    }

    public int line;                    // index: 0, 1, 2
    public int max_volume;                    // 태울 수 있는 최대 인원 수
    public int cur_volume;                    // 현재 태우고 있는 인원 수
    public int cur_floor;                    // 현재 엘리베이터가 멈춰있는 층
    public int available_top_floor;                    // 현재 설정해놓은 운행층 중 최고층
    public int available_bottom_floor;                    // 현재 설정해놓은 운행층 중 최하층

    public Dictionary<string, int> skill_dict;

    public int vip_female_cnt;      // 현재 타고 있는 김여사 수
    public int master_cnt;          // 현재 타고 있는 사장 수
    public int thief_cnt;           // 현재 타고 있는 도둑 수

    public int save_moving_state;        // 수리 하기 직전의 상태
    public Vector2 save_position;        // 수리 하기 직전의 상태

    public int level;                    // 엘리베이터 레벨
    public int cur_point;                    // 사용 가능한 포인트
    public float cur_exp;                   // 현재 경험치
    public float cur_speed;                   // 현재 운행 속도
    public float max_exp;                  // 레벨업하기위한 최대 경험치
    public float on_and_off_time;                   // 사람이 타고 내리는 시간 간격(고정값, 버프 디버프로만 영향)

    //3개 모두 곱연산
    public float crash_speed_rate;                // 한 라인에서 두 대 이상의 엘리베이터가 충돌했을 때 감소하는 속도 비율 -0.5
    public float overload_speed_rate;                // max_weight를 넘겼을때 줄어드는 속도 비율  -0.5
    public float broken_speed_rate;             // 내구도가 0이되어 파괴되었을때 줄어드는 속도 비율  -0.5

    /// <내구도 설명>
    /// 0이 되면 이동 불가
    /// 사람이 타고 내릴때 일정량(사람마다 차이를 두지 않음) 감소
    /// 수리로 복원가능
    /// 수리는 일정량의 골드 소모, 소모량은 현재/최대 비율로 산정
    public float cur_durability;                // 현재 내구도 
    public float max_durability;                // 최대 내구도 레벨 업으로 증가 가능
    public float temp_durability_size;  //임시값
    public float auto_fix_per_second;
    public Coroutine auto_fix_coroutine;

    public GameObject selected_point; // 엘리베이텨 선택 표시 오브젝트

    public bool is_loading;                 // 사람을 태우고 있으면 false
    public bool is_overload;                // 최대 수용량의 80%를 넘으면 true
    public bool is_waiting;                 // 한 층에서 사람들을 태우거나 내릴때만 true
    public bool is_crashing;                // 엘리베이터끼리 충돌하면 true
    public bool is_fixing;                  // 엘리베이터가 수리중이면 true
    public bool is_automating = false;      // 엘리베이터가 자동 운행 중이면 true

    SpriteRenderer sprite;
    public Sprite[] elevator_sprite;              // 현재 승객의 수에 대한 이미지 0% 33% 66% 100%(이상)

    public GameObject thief_bubble;     // 해당 엘리베이터에 도둑이 있다는 것을 보여주는 이미지
    public GameObject master_bubble;    // 해당 엘리베이터에 회장이 있다는 것을 보여주는 이미지

    protected Floor floor_script;

    public EleSimpleInfo elevator_info; // 현재 엘리베이터의 상태를 간략하게 보여주는 버튼 , 엘리베이터 매니져에서 연결해줌

    protected Coroutine move_coroutine;
    protected Coroutine wait_coroutine;         // 사람들을 태우거나 내릴때 수행
    protected Coroutine auto_coroutine;
    public int save_top_floor, save_bottom_floor; // 오토 이전 상태 저장

    public Vector2 end_point;
    Vector2 save_end_point;

    public Rigidbody2D rigid { get; protected set; }
    protected Dictionary<int, Queue<GameObject>> humans_in_elevator;          // 현재 엘리베이터가 태우고 있는 사람들 리스트

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        humans_in_elevator = new Dictionary<int, Queue<GameObject>>();
        sprite = GetComponent<SpriteRenderer>();
        floor_script = null;
    }

    protected void Start()
    {
        auto_fix_coroutine = StartCoroutine(AutoFix());
    }

    public void Set(int line, int building_bottom_floor, int building_top_floor)
    {
        this.line = line;
        end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;

        float durability_color_int = (255 - (100 - (int)((cur_durability / max_durability) * 100)) * 2) / 255;
        sprite.color = new Color(durability_color_int, durability_color_int, durability_color_int);

        save_top_floor = available_top_floor = building_top_floor;
        save_bottom_floor = available_bottom_floor = building_bottom_floor;

        max_volume = 5 / Gamemanager.Instance.buffmanager.elevator_volume_half;

        skill_dict = new Dictionary<string, int>()
        {
            { "speed", 0 },
            { "volume", 0 },
            { "durability", 0 }
        };

        selected_point.SetActive(true);

        // 생성되자마자 움직임
        move_coroutine = StartCoroutine(Move());
    }

    public void Reset(int setting_top_floor, int setting_bottom_floor) // 새로 설정된 값
    {
        end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;

        save_top_floor = available_top_floor = setting_top_floor;
        save_bottom_floor = available_bottom_floor = setting_bottom_floor;
    }

    //---------------------------------------------------------------------------
    IEnumerator Move()
    {
        while (!is_fixing)
        {
            yield return null;

            rigid.position = Vector2.MoveTowards(
                rigid.position,
                end_point,
                // Buff & Debuff
                cur_speed *                   
                // 현재속도
                overload_speed_rate *   
                // 과적재 속도 감소
                crash_speed_rate *      
                // 충돌 속도 감소
                ((cur_volume == 0) ? 1 + Gamemanager.Instance.buffmanager.empty_elevator_speed_rate : 1) * 
                // 빈 엘리베이터 속도 증가 
                ((cur_floor >= Gamemanager.Instance.buildgame.building_top_floor / 2) ? 1 + Gamemanager.Instance.buffmanager.acrophobia_speed_rate : 1) *
                // 고소공포증 속도 감소
                (1 + vip_female_cnt * Gamemanager.Instance.buffmanager.vip_female_speed_rate) *            
                // 김여사 속도 증가
                (1 + master_cnt * Gamemanager.Instance.buffmanager.vvvvvip_speed_rate) *                  
                // 사장 속도 증가
                ((end_point.y > rigid.position.y) ? 1 - Gamemanager.Instance.buffmanager.gravity_elevator_rate : 1 + Gamemanager.Instance.buffmanager.gravity_elevator_rate) * 
                // 중력 속도 증감소
                Time.deltaTime
                );
        }
    }
    // 충돌 처리 스크립트
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 층에 멈췄을 때
        if (collision.gameObject.tag == "CheckBox")
        {
            floor_script = collision.gameObject.GetComponentInParent<Floor>();
            cur_floor = floor_script.floor_level;

            if (available_bottom_floor <= cur_floor && cur_floor <= available_top_floor) // 현재 층에 멈출수 있는가
            {
                if (move_coroutine != null) StopCoroutine(move_coroutine); // 최초실행용 최초에는 move_coroutine이 null
                wait_coroutine = StartCoroutine(Wait());
            }
            else
            {
                if (cur_floor >= available_top_floor)
                    gameObject.layer = MovingStateForLayer.DESCENDING;
                if (cur_floor <= available_bottom_floor)
                    gameObject.layer = MovingStateForLayer.ASCENDING;

                end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;
            }
        }
        else if (collision.gameObject.tag == "Elevator" && !Gamemanager.Instance.buffmanager.is_ghost_elevator && !is_crashing) // 엘리베이터 간 충돌
        {
            is_crashing = true;
            crash_speed_rate = 0.5f;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Elevator") // 엘리베이터 간 충돌 해제
        {
            is_crashing = false;
            crash_speed_rate = 1f;
        }
    }
    // 충돌 처리 스크립트
    IEnumerator Wait() // 사람들 내리고 타기 위한 코루틴
    {
        if (!humans_in_elevator.ContainsKey(cur_floor)) // 없음
            humans_in_elevator.Add(cur_floor, new Queue<GameObject>());

        while (humans_in_elevator[cur_floor].Count > 0)
        {
            yield return floor_script.StartCoroutine(floor_script.OpenDoors(line));

            Human hs = humans_in_elevator[cur_floor].Dequeue().GetComponent<Human>();

            AddDurability(-temp_durability_size, false); // 내구도 변화
            AddVolume(-hs.population);

            // 경험치 추가
            cur_exp += hs.exp;
            if (cur_exp >= max_exp)
            {
                level++;
                elevator_info.ChangeLevel();
                cur_exp -= max_exp;
                max_exp *= 1.2f; // 최대 경험치량 변경
                cur_point++;
            }

            hs.state = HumanState.WALKING;
            hs.ActOffElevator(this);

            yield return new WaitForSeconds(on_and_off_time); // off time
        }

        // 층에 있는 사람 태움
        if ((!floor_script.is_using_up && gameObject.layer == MovingStateForLayer.ASCENDING) ||
            (!floor_script.is_using_down && gameObject.layer == MovingStateForLayer.DESCENDING)) // 사용가능
        {
            floor_script.StartUseFloor(gameObject.layer); // 현재 층 사용한다고 알림

            Human waiter = (gameObject.layer == MovingStateForLayer.ASCENDING) ? floor_script.up_first : floor_script.down_first;
            // 알맞는 사람을 데려옴

            while (waiter != null && waiter.state == HumanState.WAITING && cur_volume + waiter.population <= max_volume) // 대기중인 사람 있음
            {
                if (available_bottom_floor <= waiter.destination_floor && waiter.destination_floor <= available_top_floor)
                {
                    waiter.state = HumanState.BOARDING;

                    yield return floor_script.StartCoroutine(floor_script.OpenDoors(line));

                    //1. 목적지까지 운행가능한가, 2. 엘리베이터에 남은 자리가 있는가
                    /// ********** 사람이 엘리베이터에 탐
                    /// ********** 엘리베이터에 탐으로써 해야할 행동 정의

                    // 엘리베이터의 상태에 따라 초기화할 첫 대기자를 선택
                    if (gameObject.layer == MovingStateForLayer.ASCENDING)
                    {
                        if (floor_script.up_first == null) // 그 찰나의 순간에 사람이 화나서 가버렸다면
                            break;
                        floor_script.up_first = null;
                    }
                    else
                    {
                        if (floor_script.down_first == null)
                            break;
                        floor_script.down_first = null;
                    }

                    // 엘리베이터를 향해 이동
                    // 완전히 탈때까지 대기

                    if (waiter.gameObject.activeSelf) // 사람이 존재하면(중간에 사라지지 않으면) 태움
                    {
                        is_loading = true;
                        yield return waiter.StartCoroutine(waiter.MovetoElevator(waiter.transform.position, rigid.position));
                        is_loading = false;

                        if (!humans_in_elevator.ContainsKey(waiter.destination_floor)) // 없음
                            humans_in_elevator.Add(waiter.destination_floor, new Queue<GameObject>());

                        humans_in_elevator[waiter.destination_floor].Enqueue(waiter.gameObject);

                        waiter.ActInElevator(this); // 엘리베이터 탔을 때 행동 지정
                        waiter.transform.position = Gamemanager.Instance.objectpool.waiting_room.transform.position;

                        AddDurability(-temp_durability_size, false); // 내구도 변화
                        AddVolume(waiter.population);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.3f); // 사람 없어지고 바로 체크하면 엘베 혼자 휭 가버릴테니 조금은 기다려주자
                        break; // 태우다가 그 사람이 사라지면 break
                    }
                }
                else break; // 태울 사람이 없으면 break

                waiter = (gameObject.layer == MovingStateForLayer.ASCENDING) ? floor_script.up_first : floor_script.down_first;
                // 새로운 대기자를 불러옴
            }
        }
        yield return floor_script.StartCoroutine(floor_script.CloseDoors(line));
        floor_script.FinishUseFloor(gameObject.layer); // 층 사용이 끝났다고 알림

        if (cur_durability <= 0)
        {
            broken_speed_rate = 0f; // 이동 불가
            sprite.color = Color.gray;
            elevator_info.ShowWarningState();
        }
        else
        {
            if (cur_floor >= available_top_floor)
                gameObject.layer = MovingStateForLayer.DESCENDING;
            if (cur_floor <= available_bottom_floor)
                gameObject.layer = MovingStateForLayer.ASCENDING;

            end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;

            move_coroutine = StartCoroutine(Move());
        }
    }

    // 상태변화 스크립트
    public void AddDurability(float durability_size, bool is_auto_fix)
    {
        //내구도 설정 및 이미지 변경
        cur_durability += durability_size + (is_auto_fix ? 0 : Gamemanager.Instance.buffmanager.durability_decrease_rate);
        {
            if (cur_durability < 0)
                cur_durability = 0;
            else if (cur_durability > max_durability)
                cur_durability = max_durability;

            float durability_color_int = (255 - (100 - cur_durability / max_durability * 100) * 2) / 255;
            sprite.color = new Color(durability_color_int, durability_color_int, durability_color_int);

            elevator_info.ShowDurabilityProgress();
        } // 이미지 변경
    } 
    public void AddVolume(int population)
    {
        cur_volume += population;

        if (cur_volume <= max_volume * 0.75f && is_overload)
        {
            is_overload = false;
            overload_speed_rate = 1f;
            elevator_info.ShowWarningState();
        } // 과적재 체크
        else if (cur_volume > max_volume * 0.75f && !is_overload)
        {
            is_overload = true;
            overload_speed_rate = 0.5f - Gamemanager.Instance.buffmanager.overload_speed_rate;
            elevator_info.ShowWarningState();
        } // 과적재 체크

        sprite.sprite = elevator_sprite[Mathf.FloorToInt((float)cur_volume / max_volume * 100) / 25];
    }
    // 상태변화 스크립트










    // 수리 스크립트
    public void GoToFix(H_Mechanic mechanic_script) // 수리를 하러 이동
    {
        is_fixing = true;

        if (wait_coroutine != null) StopCoroutine(wait_coroutine);

        sprite.color = Color.gray;

        save_end_point = end_point;
        save_position = rigid.position; // 수리 직전 상태를 저장
        save_moving_state = gameObject.layer;

        gameObject.layer = MovingStateForLayer.FIXING; // up, down과 충돌하지않는 독단적인 레이어

        elevator_info.ShowIsFixing(true); // 현재 수리 중임을 알려줌

        F_Repairshop repairshop = Gamemanager.Instance.buildgame.floors[FID.REPAIRSHOP][0] as F_Repairshop; // 빌딩에는 정비소가 단 하나
        repairshop.is_line_repair_using[line] = true; // 해당 라인에서 이 엘리베이터를 수리
        // 수리 상태

        StartCoroutine(MoveForFix(mechanic_script));
    }
    IEnumerator MoveForFix(H_Mechanic mechanic_script) // 수리 전후로 이동하기 위한 코루틴
    {
        var repairshop = Gamemanager.Instance.buildgame.floors[FID.REPAIRSHOP][0] as F_Repairshop;

        Vector2 fix_end_point = repairshop.repair_point[line].transform.position;
        // 고정값으로 이동
        while (rigid.position != fix_end_point)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, fix_end_point, 1.5f * Time.deltaTime); // move 느릿느릿 이동
            yield return null;
        }

        // 정비공 호출하기 전 해당 층 불켜기
        repairshop.TurnOnLight(line);
        repairshop.Call_Mechanic(this, mechanic_script); // 정비공 호출
    }
    public void FinishFix() // 수리 끝
    {
        broken_speed_rate = 1;
        Vector2 fix_end_point = save_position; // 원래 자리로 돌아감

        F_Repairshop repairshop = Gamemanager.Instance.buildgame.floors[FID.REPAIRSHOP][0] as F_Repairshop;
        repairshop.is_line_repair_using[line] = false; // 해당 라인 다른 엘리베이터가 이용 가능

        // 자리로 복귀하기 전 해당 층 불끄기
        repairshop.TurnOffLight(line);
        StartCoroutine(MoveForReturn(fix_end_point)); // 수리 직전 자리로 복귀
    }
    IEnumerator MoveForReturn(Vector2 fix_end_point) // 수리 전후로 이동하기 위한 코루틴
    {
        // 고정값으로 이동
        while (rigid.position != fix_end_point)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, fix_end_point, 1.5f * Time.deltaTime * (1 + Gamemanager.Instance.buffmanager.elevator_speed_rate_for_fix)); // move 느릿느릿 이동
            yield return null;
        }

        // 원래 상태로 복귀
        sprite.color = Color.white;
        is_fixing = false;
        elevator_info.ShowIsFixing(false);
        gameObject.layer = save_moving_state;
        end_point = save_end_point;
        move_coroutine = StartCoroutine(Move());
    }
    // 수리 스크립트

    public void StartAuto()
    {
        is_automating = true;
        save_top_floor = available_top_floor;
        save_bottom_floor = available_bottom_floor; // 상태 저장
        auto_coroutine = StartCoroutine(Auto());
        elevator_info.ChangeAuto();
    }
    public void StopAuto()
    {
        StopCoroutine(auto_coroutine);
        is_automating = false;
        available_top_floor = save_top_floor;
        available_bottom_floor = save_bottom_floor;
        elevator_info.ChangeAuto();
    }
    // 오토 스크립트
    IEnumerator Auto()
    {
        while(true)
        {
            available_top_floor = Gamemanager.Instance.buildgame.building_top_floor;
            available_bottom_floor = Gamemanager.Instance.buildgame.building_bottom_floor;
            yield return null;
        }
    }

    // 자동으로 엘베 내구도 수리
    // 최솟값은 0.3
    IEnumerator AutoFix()
    {
        while (!Gamemanager.Instance.buildgame.is_gameover)
        {
            AddDurability(Mathf.Max(0.3f, auto_fix_per_second), true);

            yield return new WaitForSeconds(1f);
        }
    }
}