using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ***********************
// attached in Main Camera
// ***********************

public class BuildGame : MonoBehaviour
{
    public float timespeed; // 게임 배속에 관여
    public bool is_gameover { get; private set; }

    // -----------------설명용-----------------

    public bool is_attendance_appear;
    public bool is_thief_appear;
    public bool is_sanitation_appear;
    public bool is_homeless_appear;
    public bool is_rush_appear;
    public bool is_sick_appear;

    //--------------------------------------

    public int day;
    public int hour;

    // 각 특수 몹 등장 날짜
    public int sanitation_day;
    public int homeless_day;
    public int[] rushtimer_days;
    public int delivery_day;

    public int gold { get; private set; } // 사람이 층에서 내릴때 지불하는 돈
    public int one_hour { get; private set; } = 2; // 인게임 1시간당 현실 시간

    public int night_regen; // 밤 사람 리젠률
    public bool is_go_to_work_time; // 출근 시간이면 true 7 ~ 9
    public bool is_lunch_time;      // 밥먹을 시간이면 true 11 ~ 13
    public bool is_go_to_home;      // 퇴근 시간이면 true 18 ~ 22
    public bool is_night;           // 밤 시간이면 도둑이 출몰할 수 잇음

    // <player life 관여 변수> ************************************************************************************
    // 인스펙터에서 변경 요망
    [SerializeField]
    private float cur_life;
    [SerializeField]
    private float max_life;
    public int waiting_cnt { get; set; } = 0;
    public int waiting_threshold;
    public float life_reduction_interval;

    public float auto_heal_per_second;
    public Coroutine auto_heal_coroutine;
    // </player life 관여 변수> ***********************************************************************************

    public int building_top_floor { get; private set; } = -1; // 지하는 정비소
    public int building_bottom_floor { get; private set; } = 1;

    public bool is_scenechange_ad_ready = true; // 화면 전환 광고 준비되었는지
    public string next_scene_name; // 광고 달아서 어쩔 수 없이 추가

    public Sprite[] bubble; // 여기저기서 쓰이는 버블
    /// <summary>
    /// 빈 말풍선
    /// 당황
    /// 체포
    /// 신남
    /// 검사1
    /// 검사2
    /// 검사3
    /// 화남
    /// </summary>

    // <prefabs> *************************************************************************************************
    // 인스펙터에서만 참조 가능
    [SerializeField]
    private GameObject[] N_floors; // normal floors
    [SerializeField]
    private GameObject F_floor;
    [SerializeField]
    private GameObject R_floor;
    [SerializeField]
    private GameObject C_floor;
    [SerializeField]
    private GameObject V_floor;
    [SerializeField]
    private GameObject M_floor;
    [SerializeField]
    private GameObject B_floor;
    [SerializeField]
    private GameObject hospital_floor;
    [SerializeField]
    private GameObject repairshop_floor;
    [SerializeField]
    private GameObject counter_floor;
    [SerializeField]
    private GameObject under;
    [SerializeField]
    private GameObject roof;
    [SerializeField]
    private GameObject floorSet;
    [SerializeField]
    private Sprite[] roofs;      // 지붕 이미지 저장 
    SpriteRenderer _roof; // << 얘는 하나뿐이라 걍 여기다가 저장함
    // </prefabs> ************************************************************************************************

    public List<List<Floor>> floors { get; protected set; } // FID로 인덱스 참조 >> 해당 층의 스크립트 가져옴
    public List<Floor> floor_of { get; protected set; } // FloorLevel로 인덱스 참조 >> 해당 층의 스크립트 가져옴

    public GameObject plus_Ele;

    // <UI> ******************************************************************************************************
    public Text day_txt, hour_txt, gold_txt;
    public Text cur_waiting_human_state_txt;// 현재 대기 인원 / 대기 임계점을 화면에 표시
    public Text alarm_txt; // 특정 사건이 일어나면 화면에 표시(층 수 추가, 이벤트 발생 등등)
    public Queue<string> alarm_queue; // 특정 사건들을 담고 있는 큐 차례로 출력
    public Image Hp;

    public GameObject gameover_panel;

    [SerializeField]
    GameObject leftdoor, rightdoor;

    // </UI> *****************************************************************************************************

    public GameObject spawn_point;

    ElevatorManager elevator_manager_script;
    EmployeeManager employee_manager_script;
    BuffManager buff_manager_script;

    Coroutine time_coroutine;
    Coroutine life_coroutine;

    public RushTimer[] timer_arr;
    List<int> timer_idx;

    protected void Awake()
    {
        SoundManager.Instance.PlayBgm("Menu1");

        StartCoroutine(OpenDoors());

        is_gameover = false;
        Gamemanager.Instance.Set();

        timer_idx = new List<int>() { 0, 1, 2, 3, 4 };

        gameover_panel.SetActive(false);

        //Camera.main.orthographicSize = 0; //연출용
        timespeed = Time.timeScale = 1;
        day = 1;
        hour = 3;
        gold = 10000;
        night_regen = 1;
        elevator_manager_script = GetComponent<ElevatorManager>();
        employee_manager_script = GetComponent<EmployeeManager>();
        buff_manager_script = GetComponent<BuffManager>();

        alarm_queue = new Queue<string>();

        // 생성된 모든 층들은 여기에 저장
        floors = new List<List<Floor>>();
        for (int i = FID.INDEX_BEGIN; i <= FID.INDEX_END; i++) // FID인덱스로 바로 접근할 수 있게끔
            floors.Add(new List<Floor>());
        floor_of = new List<Floor>();

        // 지하
        Instantiate(under).transform.position = new Vector2(0, -4);
        Instantiate(under).transform.position = new Vector2(0, -6);

        // 지하 1층(0층)
        GameObject f_temp = Instantiate(repairshop_floor);
        f_temp.name = repairshop_floor.name; // 이름 동기화, 안하면 뒤에 (clone) 붙음
        Floor f_temp_f = f_temp.GetComponent<Floor>();
        f_temp.transform.position = new Vector2(0, 2 * building_top_floor); // 층 위치 설정
        f_temp_f.Set(++building_top_floor); // 층 초기화
        floors[FID.REPAIRSHOP].Add(f_temp_f); // floors배열에는 스크립트 저장
        floor_of.Add(f_temp_f);


        //************** 고정 *******************
        //1층
        MakeFloor(F_floor, FID.FIRST);

        //2층
        MakeFloor(C_floor, FID.CLEAN);

        // 3층
        MakeFloor(N_floors[0], FID.RED);

        // 4층
        MakeFloor(N_floors[0], FID.RED);

        // 5층
        MakeFloor(R_floor, FID.REST);

        // 지붕
        GameObject rp = Instantiate(roof);
        rp.transform.position = new Vector2(0, 2 * building_top_floor + 1);
        _roof = rp.GetComponent<SpriteRenderer>();
        _roof.sprite = roofs[2];
        //************** 고정 *******************

        // 엘리베이터 생성
        for (int line = 0; line < 3; line++)
            elevator_manager_script.MakeElevator(line);


        rushtimer_days = new int[5] // 러쉬 켜지는 날짜들
        {
            Random.Range(7, 10) -5,
            Random.Range(15, 21)-8,
            Random.Range(21, 28)-10,
            Random.Range(28, 35)-13,
            Random.Range(35, 42)-16
        };
        homeless_day = Random.Range(5, 7);
        sanitation_day = Random.Range(7, 15);
        delivery_day = 10;

        time_coroutine = StartCoroutine(TimeGo());
        life_coroutine = StartCoroutine(Life());
        cur_waiting_human_state_txt.text = waiting_cnt + " / " + waiting_threshold;
    }

    protected void Start()
    {
        auto_heal_coroutine = StartCoroutine(AutoHeal());
    }

    public IEnumerator OpenDoors()
    {
        var leftdoor_pos = leftdoor.transform.localPosition;
        var rightdoor_pos = rightdoor.transform.localPosition;

        // 문이 열리는 속도가 서서히 느려짐
        for (int i = 0; i <= 110; i += 10)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 110; i <= 220; i += 8)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 220; i <= 330; i += 6)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 330; i <= 440; i += 4)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 440; i <= 540; i += 2)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
    }
    public IEnumerator CloseDoors()
    {
        var leftdoor_pos = leftdoor.transform.localPosition;
        var rightdoor_pos = rightdoor.transform.localPosition;

        // 문이 닫히는 속도가 서서히 느려짐
        for (int i = 0; i <= 110; i += 10)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 110; i <= 220; i += 8)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 220; i <= 330; i += 6)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 330; i <= 440; i += 4)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 440; i <= 540; i += 2)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
    }

    IEnumerator TimeGo()
    {
        while (true)
        {
            while (hour != 24)
            {
                day_txt.text = day.ToString();
                hour_txt.text = hour + ":00";
                gold_txt.text = gold.ToString();
                yield return new WaitForSeconds(one_hour); // 1시간 
                hour++;

                if (hour == 2)
                {
                    is_lunch_time = false;
                    if (day % 3 == 1) // 1 4 7 ...
                    {
                        alarm_queue.Enqueue("출근 5시간전!");
                        alarm_queue.Enqueue("출근 4시간전!");
                        alarm_queue.Enqueue("출근 3시간전!");
                        alarm_queue.Enqueue("출근 2시간전!");
                        alarm_queue.Enqueue("출근 1시간전!");
                        StartCoroutine(ShowAlarm());
                    }
                }
                else if (hour == 7) // 출근 // 이미지 변경 // 낮 리젠 적용
                {
                    is_night = false;
                    is_go_to_work_time = true;
                    night_regen = 1;

                    if (day % 3 == 1) // 1 4 7 ...
                        for (int i = 0; i < Gamemanager.Instance.buffmanager.go_to_work_rate; i++)
                            (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeHumanInMorning()); // 출근용 사람을 만드는 코루틴 4개 추가

                    _roof.sprite = roofs[0];
                }
                else if (hour == 9)
                {
                    is_go_to_work_time = false;
                }
                else if (hour == 11)//점심
                {
                    is_lunch_time = true;
                }
                else if (hour == 13)
                {
                    is_lunch_time = false; //점심 끝

                    if (day % 3 == 0) // 3 6 9 ...
                    {
                        alarm_queue.Enqueue("퇴근 5시간전!");
                        alarm_queue.Enqueue("퇴근 4시간전!");
                        alarm_queue.Enqueue("퇴근 3시간전!");
                        alarm_queue.Enqueue("퇴근 2시간전!");
                        alarm_queue.Enqueue("퇴근 1시간전!");
                        StartCoroutine(ShowAlarm());
                    }
                }
                else if (hour == 18) // 퇴근 이미지 변경
                {
                    is_go_to_home = true;

                    if (day % 3 == 0) // 3 6 9 ...
                        for (int go_to_home_count = 0; go_to_home_count < Gamemanager.Instance.buffmanager.go_to_home_rate; go_to_home_count++)
                        {//퇴근자 형성 코루틴, 디버프 적용시 2배 적용
                            for (int i = 0; i < floors[FID.RED].Count; i++)
                                (floors[FID.RED][i] as F_Normal_Red).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.BLUE].Count; i++)
                                (floors[FID.BLUE][i] as F_Normal_Blue).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.GREEN].Count; i++)
                                (floors[FID.GREEN][i] as F_Normal_Green).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.YELLOW].Count; i++)
                                (floors[FID.YELLOW][i] as F_Normal_Yellow).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.VIP].Count; i++)
                                (floors[FID.VIP][i] as F_Vip).MakeHumanForGoHome();
                        }

                    _roof.sprite = roofs[1];
                }
                else if (hour == 20)
                {
                    is_go_to_home = false;
                }
                else if (hour == 22) // 밤 리젠 적용
                {
                    // 2 or 1
                    night_regen = 2 - Gamemanager.Instance.buffmanager.human_create_in_night_rate;
                    is_night = true;

                    if (day > 4)
                    {
                        Gamemanager.Instance.uimanager.ShowExplain(ref is_thief_appear, PID.THIEF);

                        for (int i = 0; i < 3; i++)
                            (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeThief()); // 도둑 추가 코루틴 3개

                        alarm_queue.Enqueue("도둑 출현");
                        StartCoroutine(ShowAlarm());
                    }
                    _roof.sprite = roofs[2];
                }
                else if (hour == 24)
                {
                    is_lunch_time = Gamemanager.Instance.buffmanager.is_there_night_rest; // 야식제공 버프
                    if (day % 3 == 0) // 3 6 9 ...
                    {
                        Gamemanager.Instance.uimanager.TimeStop();
                        Gamemanager.Instance.buffmanager.PickAndShowBuff(); // 3개의 버프 제시
                        alarm_queue.Enqueue("임금 " + employee_manager_script.PayDay()+ "원 지불"); // 임금지불
                        StartCoroutine(ShowAlarm());

                        waiting_threshold += Gamemanager.Instance.buffmanager.supply_size;
                        cur_waiting_human_state_txt.text = waiting_cnt + " / " + waiting_threshold;

                        for (int build_count = 0; build_count < Gamemanager.Instance.buffmanager.plus_floor_size; build_count++) // 건물 생성
                        {
                            if (Random.Range(0, 100) < 70)
                            {
                                // 일반층 생성
                                int nfloor_idx = Random.Range(0, N_floors.Length);
                                MakeFloor(N_floors[nfloor_idx], nfloor_idx);
                            }
                            else
                            {
                                // 특수층 생성
                                int idx = Random.Range(0, 100);
                                if (idx < 20)
                                    MakeFloor(hospital_floor, FID.HOSPITAL);
                                else if (idx < 45)
                                    MakeFloor(R_floor, FID.REST);
                                else if (idx < 50)
                                    MakeFloor(M_floor, FID.MASTER);
                                else if (idx < 60)
                                    MakeFloor(B_floor, FID.BANK);
                                else if (idx < 80)
                                    MakeFloor(counter_floor, FID.CONVENIENCE);
                                else
                                    MakeFloor(V_floor, FID.VIP);
                            }
                            _roof.transform.position = new Vector2(0, 2 * building_top_floor + 1);
                        }
                    }
                    if (day % 6 == 0)
                    {
                        Gamemanager.Instance.uimanager.TimeStop();
                        plus_Ele.SetActive(true);
                    }
                }
            }
            hour = 0;
            day++;
            employee_manager_script.EmployeesCareerUp();//경력 증가

            // 50층까지 버텨야 함
            if (day > 50)
            {
                StartCoroutine(GameOver());
                yield break;
            }

            // -------------------특수 이벤트 날짜---------------------
            if (day == homeless_day) // 최초 노숙자 생성
            {
                Gamemanager.Instance.uimanager.ShowExplain(ref is_homeless_appear, PID.HOMELESS);

                (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeHomeless());
            }
            if (day == sanitation_day) // 최초 위생관 생성
            {
                Gamemanager.Instance.uimanager.ShowExplain(ref is_sanitation_appear, PID.SANITATION);

                (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeSanitation());
            }
            foreach (var r_day in rushtimer_days)
                if (day == r_day) // 타이머 시작
                {
                    Gamemanager.Instance.uimanager.ShowExplain(ref is_rush_appear, 1);

                    int random_idx = Random.Range(0, timer_idx.Count);

                    timer_arr[timer_idx[random_idx]].StartTimer();

                    timer_idx.RemoveAt(random_idx);

                    alarm_queue.Enqueue("새로운 타이머 시작!");
                    StartCoroutine(ShowAlarm());
                }
        }
    }

    IEnumerator Life()
    {
        while (true)
        {
            if (cur_life > max_life)
                cur_life = max_life;
            if (waiting_cnt > waiting_threshold)
                cur_life -= waiting_cnt - waiting_threshold;
            if (cur_life <= 0)
            {
                cur_life = 0;
                Hp.fillAmount = cur_life / max_life;

                Gamemanager.Instance.uimanager.stop_panels_count = 9999999;

                StartCoroutine(GameOver());
                yield break;
            }
            Hp.fillAmount = cur_life / max_life; // 지속적으로 화면에 그려줌
            yield return new WaitForSeconds(life_reduction_interval);
        }
    }

    public void PlusHp(float healing_size)
    {
        cur_life += healing_size;
        if (cur_life < 0)
            cur_life = 0;
        else if (cur_life > max_life)
            cur_life = max_life;
    }

    public void ChangeCurrentWaitingHumanCount(int population) // 현재 대기 인원 변경
    {
        waiting_cnt += population;
        // UI
        cur_waiting_human_state_txt.text = waiting_cnt + " / " + waiting_threshold;
    }

    public void MakeFloor(GameObject floor_prefab, int floor_color)
    {
        GameObject f_temp = Instantiate(floor_prefab);
        f_temp.name = floor_prefab.name; // 이름 동기화, 안하면 뒤에 (clone) 붙음
        Floor f_temp_f = f_temp.GetComponent<Floor>();
        f_temp.transform.position = new Vector2(0, 2 * building_top_floor); // 층 위치 설정
        f_temp_f.Set(++building_top_floor); // 층 초기화
        floors[floor_color].Add(f_temp_f); // floors배열에는 스크립트 저장
        floor_of.Add(f_temp_f);
    }

    // arguments: int 현재 층, int[] 내릴 수 있는 층 색깔들
    // 내릴 수 있는 층 색깔들 중에 랜덤으로 하나 리턴
    public Floor GetDestinationExceptCurrent(List<Floor> destinations, int current_floorlevel, params int[] possible_FIDs)
    {
        destinations.Clear();
        // 가고 싶은 층색깔 중 현재 층을 제외한 모든 층을 대상으로 랜덤 픽
        foreach (var possible_FID in possible_FIDs)
            foreach (var floor_script in floors[possible_FID])
                if (current_floorlevel != floor_script.floor_level)
                    destinations.Add(floor_script);
        return destinations.Count == 0 ? null : destinations[Random.Range(0, destinations.Count)];
    }

    // 현재 층으로부터 가장 가까운 FID층을 찾음
    // 자신이 서있는 층이 제일 가깝다면 그 다음으로 가까운 층을 찾음
    public Floor GetClosestFID(int currentfloor_level, int dest_FID)
    {
        if (floors[dest_FID].Count == 0)
            return null;

        int idx = 0, closest_dis = int.MaxValue;
        for (int i = 0; i < floors[dest_FID].Count; i++)
        {
            if (currentfloor_level == floors[dest_FID][i].floor_level)
                continue;

            int d = Mathf.Abs(currentfloor_level - floors[dest_FID][i].floor_level);
            if (d < closest_dis)
            {
                closest_dis = d;
                idx = i;
            }
        }
        return floors[dest_FID][idx];
    }

    public IEnumerator ShowAlarm() // 내용을 인자로 받음
    {
        while (alarm_queue.Count > 0)
        {
            alarm_txt.text = alarm_queue.Dequeue(); // 내용
            alarm_txt.color = new Color(alarm_txt.color.r, alarm_txt.color.g, alarm_txt.color.b, 1); // 불투명도 조절
            yield return StartCoroutine(AlarmFadeOut());
            yield return new WaitForSeconds(0.01f); // 게임 중지시를 위한 스크립트
        }
    }
    IEnumerator AlarmFadeOut()// 알람 서서히 사라지게하기
    {
        for (int i = 0; i < 33; i++)
        {
            alarm_txt.color = new Color(alarm_txt.color.r, alarm_txt.color.g, alarm_txt.color.b, 1 - 0.03f * i);
            yield return null;
        }
        alarm_txt.color = new Color(alarm_txt.color.r, alarm_txt.color.g, alarm_txt.color.b, 0);
    }

    public void PlusGold(int gold)
    {
        this.gold += gold;

        if(this.gold < 0)
        {
            PlusHp(this.gold / 20 * 5);
            this.gold = 0;
        }

        gold_txt.text = this.gold.ToString();
    }
    public void PlusGoldFromHuman(Human hs, int gold)
    {
        this.gold += gold;
        gold_txt.text = this.gold.ToString();

        StartCoroutine(hs.CoinEffect(gold));
    }

    /// <summary>
    /// Gameover Functions Below This Row
    /// </summary>
    IEnumerator GameOver()
    {
        is_gameover = true;
        Time.timeScale = 0; // 게임 정지

        // **************************************** 패널 효과 시작 ****************************************
        gameover_panel.SetActive(true);

        // 위에서 떨어짐
        float x = gameover_panel.transform.localPosition.x;
        for (float y = gameover_panel.transform.localPosition.y; y >= -500; y -= 50f)
        {
            gameover_panel.transform.localPosition = new Vector2(x, y);
            yield return null;
        }

        StartCoroutine(SlimeEffect(gameover_panel));
        // **************************************** 패널 효과 종료 ****************************************

    }

    IEnumerator SlimeEffect(GameObject panel)
    {
        float origin_width = panel.transform.localScale.x;
        float origin_height = panel.transform.localScale.y;
        for (float Delta = 2.0f; Delta >= 1f; Delta -= 0.5f)
        {
            // 짜부시키는 모션
            for (float delta = 0f; delta < 50f; delta += 5f)
            {
                panel.transform.localScale = new Vector2(origin_width + Delta * delta * 1.3f, origin_height - Delta * delta * 0.7f);
                yield return null;
            }

            // 늘이는 모션
            for (float delta = 50f; delta >= -50f; delta -= 10f)
            {
                panel.transform.localScale = new Vector2(origin_width + Delta * delta * 1.3f, origin_height - Delta * delta * 0.7f);
                yield return null;
            }

            // 원상태로 복귀
            for (float delta = -50f; delta <= 0f; delta += 5f)
            {
                panel.transform.localScale = new Vector2(origin_width + Delta * delta * 1.3f, origin_height - Delta * delta * 0.7f);
                yield return null;
            }
        }
    }

    public void OnClickLoadScene(string name)
    {
        next_scene_name = name;

        StartCoroutine(OnClickLoadScene_(name));
    }
    IEnumerator OnClickLoadScene_(string name)
    {
        yield return StartCoroutine(CloseDoors());
        SceneManager.LoadScene(name);
    }
   
    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        StartCoroutine(OnClickLoadScene_(next_scene_name));
    }
    public void StartDebuffLoanShark()
    {
        PlusGold(3000);
        StartCoroutine(PayBackDept());
    }
    IEnumerator PayBackDept()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(24 * one_hour);

            if (gold < 200)
            {
                StartCoroutine(GameOver());
                yield break;
            }

            gold -= 200;
        }
    }

    // 자동으로 체력 회복
    // 최솟값은 1
    IEnumerator AutoHeal()
    {
        while (!is_gameover) {
            PlusHp(Mathf.Max(1f, auto_heal_per_second));

            yield return new WaitForSeconds(1f);
        }
    }
}