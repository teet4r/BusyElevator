using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuffManager : MonoBehaviour
{
    private List<Buff> Buffs;
    private List<Debuff> Debuffs;

    [SerializeField]
    private GameObject buff_panel;
    [SerializeField]
    private Buff_Choice_UI[] buff; // 버프 선택 버튼

    [SerializeField]
    private GameObject debuff_panel;
    [SerializeField]
    private Buff_Choice_UI debuff; // 디버프 선택 버튼

    private Buff[] pickedbuff;

    public void Start()
    {
        Buffs = new List<Buff>();
        Debuffs = new List<Debuff>();


        Buffs.Add(new Oxyclean());          // 옥시크린
        Buffs.Add(new EmptyElevator());     // 쾌적한 엘리베이터
        Buffs.Add(new Ghost());             // 유체화
        Buffs.Add(new MsKim());             // 김여사
        Buffs.Add(new CompanyJogging());    // 사내조깅
        Buffs.Add(new Telecommuting());     // 재택근무
        Buffs.Add(new VVVVVIP());           // VVVVVIP
        Buffs.Add(new BonusDay());          // 보너스 받는 날
        Buffs.Add(new LeavingPay());        // 퇴근비
        Buffs.Add(new GuardPatron());       // 순찰5분컷
        Buffs.Add(new Nothing());           // nothing
        Buffs.Add(new Gravity());           // 중력
        Buffs.Add(new DaddyAwake());        // 아빠 안잔다
        Buffs.Add(new FastLoad());          // 빠른 탑승
        Buffs.Add(new FixMoveUp());         // 렉카
        Buffs.Add(new LateRush());          // 고장난 자명종
        Buffs.Add(new LateRush());          // 휴식 = 힐링



        Debuffs.Add(new Surfeit());         // 과식
        Debuffs.Add(new Overload());        // 과적재는 사고의 원인
        Debuffs.Add(new FoodPoisoning());   // 식중독
        Debuffs.Add(new BinMissing());      // 쓰레기통이 없네?
        Debuffs.Add(new CleanerTip());      // 청소비
        Debuffs.Add(new HongGilDong());     // 홍길동
        Debuffs.Add(new SocialDistance());  // 사회적 거리두기
        Debuffs.Add(new LeavingWork());     // 칼 퇴
        Debuffs.Add(new Darktempler());     // 도크템플러
        Debuffs.Add(new KoreanSpeed());     // 한국인속도
        Debuffs.Add(new Acrophobia());      // 고소공포증
        Debuffs.Add(new DruckReinforce());  // 음주수리
        Debuffs.Add(new IronBowl());        // 철밥통
        Debuffs.Add(new CriticalPatient()); // 중환자
        Debuffs.Add(new MoldWall());        // 곰팡이 벽지
        Debuffs.Add(new EleVIRItor());      // 엘리"비리"터
        Debuffs.Add(new VIPTicket());       // VIP티켓
        Debuffs.Add(new OverTimeMeal());    // 야식제공
        Debuffs.Add(new WorkOvertime());    // 이야 야근이다
        Debuffs.Add(new Arsenal());         // 아스날
        Debuffs.Add(new HeatWave());        // 폭염
        Debuffs.Add(new LoanShark());       // 사채업자
        Debuffs.Add(new Mysophobia());      // 결벽증
        Debuffs.Add(new Overlap());         // 중복
        Debuffs.Add(new RushTimeUp());      // 투게이트 러쉬
        Debuffs.Add(new UnLuckyDay());      // 운수 나쁜 날

        //Debuffs.Add(new BodyGuard());// 보디가드

        pickedbuff = new Buff[3];
    }

    public void PickAndShowBuff() // 랜덤한 세개의 버프를 제시
    {
        for (int i = 0; i < 3; i++) // 랜덤한 세개의 버프를 선택 및 제시
        {
            int random_idx = Random.Range(0, Buffs.Count);
            pickedbuff[i] = Buffs[random_idx];
            Buffs.RemoveAt(random_idx);
        }

        buff[0].Setting(pickedbuff[0], 0);
        buff[1].Setting(pickedbuff[1], 1);
        buff[2].Setting(pickedbuff[2], 2);

        buff_panel.SetActive(true);
    }
    int debuff_count;

    public void ChooseBuff() // 버튼 클릭 시 실행되는 액션리스너 함수(버프 선택)
    {
        int choosen_buff_idx = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<Buff_Choice_UI>().id;
        // 선택한 버튼의 인덱스를 가져옴

        for (int i = 0; i < 3; i++)
            if(i != choosen_buff_idx)
                Buffs.Add(pickedbuff[i]); // 선택받지 못한 버프들은 다시 넣어둠

        debuff_count = unlucky_day_count;
        pickedbuff[choosen_buff_idx].BuffOn(); // 선택한 버프 실행
        buff_panel.SetActive(false);
        ApplyRandomDebuff();
    }

    void ApplyRandomDebuff() // 디버프는 아무 디버프나 랜덤하게 적용
    {
        debuff_panel.SetActive(true);
        int random_idx = Random.Range(0, Debuffs.Count);

        Debuffs[random_idx].DebuffOn();

        debuff_count--;

        debuff.Setting(Debuffs[random_idx]);

        if (!is_overlap_on)
            Debuffs.RemoveAt(random_idx); // 적용 후 리스트에서 제거
    }
    public void DebuffCheckPush() // 디버프를 확인하고 확인버튼을 누름
    {
        debuff_panel.SetActive(false);

        if (debuff_count > 0)
            ApplyRandomDebuff();
        else
            Gamemanager.Instance.uimanager.TryTimeRestart();
    }

    ////////////////////////////////////////////버프////////////////////////////////////////////

    public float cleaning_rate { get; set; } = 0f;
    public float empty_elevator_speed_rate { get; set; } = 0f;
    public bool is_ghost_elevator { get; set; } = false;
    public int human_create_in_night_rate { get; set; } = 0; // 0 or 1
    public float vip_female_speed_rate { get; set; } = 0;
    public bool is_there_night_rest { get; set; } = false;
    public float human_speed_rate { get; set; } = 0f;
    public int go_to_work_rate { get; set; } = 5;
    public float vvvvvip_speed_rate { get; set; } = 0f;

    // 보너스 받는 날은 변수가 없음
    public int go_to_home_pay_size { get; set; } = 0;
    public float patron_speed_rate { get; set; } = 0; // 생성될때 + 현재 고용되어있는 사람들 모두 변경
    public bool nothing { get; set; } = false;
    public float gravity_elevator_rate { get; set; } = 0;
    public float elevator_on_speed_rate { get; set; } = 0;
    public float elevator_speed_rate_for_fix { get; set; } = 0;
    public float rush_distance_size { get; set; } = 0;
    public int healing_size { get; set; } = 1;
    public int supply_size { get; set; } = 10;

    ////////////////////////////////////////////버프////////////////////////////////////////////
    //.
    //.
    //.
    //.
    //.
    ////////////////////////////////////////////디버프////////////////////////////////////////////
    public float fat_human_appear_rate { get; set; } = 0f;
    public float overload_speed_rate { get; set; } = 0f;
    public float illed_human_rate { get; set; } = 0;
    public float dirty_increase_rate { get; set; } = 0f;
    public int cleaner_pay_size { get; set; } = 0;
    public float thief_proficiency_increase_rate { get; set; } = 0;
    public int elevator_volume_half { get; set; } = 1;
    public float go_to_home_rate { get; set; } = 1f;
    public float thief_appear_in_night_rate { get; set; } = 0f;
    public bool is_korean_speed { get; set; } = false;
    public float acrophobia_speed_rate { get; set; } = 0f;
    public int reinforce_fail_threshold { get; set; } = 0;
    public bool is_iron_craw_on { get; set; } = false;
    public float pay_increase_rate { get; set; } = 0f;
    public float patient_speed_rate { get; set; } = 0f;
    public int mold_pay_size { get; set; } = 0;
    public float durability_decrease_rate { get; set; } = 0f;
    public int vip_ticket { get; set; } = 1; // 체크되면 0
    public int plus_floor_size { get; set; } = 3;
    public float thief_patience_rate { get; set; } = 1;
    public int use_size_of_bevarage { get; set; } = 1;
    public float delivery_speed_rate { get; set; } = 0;
    public int sanitation_check_size { get; set; } = 0;
    public bool is_overlap_on { get; set; } = false;
    public float rush_time_size { get; set; } = 0;
    public bool is_severance_pay_on { get; set; } = false;
    public int unlucky_day_count { get; set; } = 1;

    // 이름 보디가드
    // 설명 매트릭스 요원이야 뭐야
    // 효과 회장과 가까이 접근할 수 없습니다. 엘리베이터에 회장이 타면 모두 내쫒습니다.

    ////////////////////////////////////////////디버프////////////////////////////////////////////
}

// 비누 버프 노숙자 지속 더러움 해제
// 꿀잠 디버프 노숙자 숙면시간 증가