using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeManager : MonoBehaviour
{

    // 직원 특징
    // 임금을 가지고 있음
    // 일정 시간마다 임금을 줘야함
    // 고용하기 위해선 일정 골드가 있어야함
    // 임금을 주지 못하면 어케하지 >> 해결!
    int cleaner_id = 0;
    public Dictionary<int, H_Cleaner> cleaner_dic { get; private set; }
    int guard_id = 0;
    public Dictionary<int, H_Guard> guard_dic { get; private set; }
    int mechanic_id = 0;
    public Dictionary<int, H_Mechanic> mechanic_dic { get; private set; }

    public int remain_mechanic_count;

    public GameObject cleaners_info;
    public Transform cleaners_contents; // 직원 관리 패널(메인 화면에서 좌측 아래 버튼 누르면 나오는 패널) 내에 존재하는 스크롤 뷰의 Content
    public GameObject guards_info;
    public Transform guards_contents; // 직원 관리 패널(메인 화면에서 좌측 아래 버튼 누르면 나오는 패널) 내에 존재하는 스크롤 뷰의 Content
    public GameObject mechanices_info;
    public Transform mechanices_contents; // 직원 관리 패널(메인 화면에서 좌측 아래 버튼 누르면 나오는 패널) 내에 존재하는 스크롤 뷰의 Content

    [SerializeField]
    int cleaner_hire_gold_size;
    public Button cleaner_hire_button; // 각 탭별 고용 버튼
    public Text cleaner_hire_gold;
    [SerializeField]
    int guard_hire_gold_size;
    public Button guard_hire_button;
    public Text guard_hire_gold;
    [SerializeField]
    int mechanic_hire_gold_size;
    public Button mechanic_hire_button;
    public Text mechanic_hire_gold;

    public Button cleaners_tab;
    public Button guards_tab;
    public Button mechanices_tab;

    public Image manager_tab_img; // 배경 변경
    public Sprite[] manager_tab_images;

    public GameObject cleaner_info_sample; // 직원 관리 내에 직원마다 구현될 패널의 샘플값(origin)
    public GameObject guard_info_sample; // 직원 관리 내에 직원마다 구현될 패널의 샘플값(origin)
    public GameObject mechanic_info_sample; // 직원 관리 내에 직원마다 구현될 패널의 샘플값(origin)

    // 직원 이름 랜덤 생성 풀
    public readonly string[] first_name = { "바릭", "존", "미스타", "김", "이", "최", "박", "셰퍼드", "다까무라", "우에하라", "키즈나", "늑대와", "스카예브", "포포푸브", "시지누", "정", "채", "묠" };
    public readonly string[] second_name = { "아부에", "웡", "푸이웅", "덕배", "만수", "이룸", "도베르만", "안무르", "웤", "미세스", "유리", "밤준", "정판", "스판", "캐리건", "라이더" };

    // 직원 출생지 랜덤 생성 풀
    public readonly string[] birth_place = { "서울역" , "낯선 무인도","청소도구함", "대서양 해적선", "검단산 막걸리집",
    "화성", "아빠 뱃속", "이세카이" ,"지하미궁","66번 국도" , "차 행성","사건의 지평선","칼바람 협곡","황새","피라미드",
        "운고로","지브롤터","헤네시스","페리온","슬리피 우드", "할리스 커피","칼림도어","자유시장","백악관"};

    protected void Awake()
    {
        cleaner_dic = new Dictionary<int, H_Cleaner>();
        guard_dic = new Dictionary<int, H_Guard>();
        mechanic_dic = new Dictionary<int, H_Mechanic>();
    }
    private void Start()
    {
        cleaner_hire_gold.text = cleaner_hire_gold_size + " G";
        guard_hire_gold.text = guard_hire_gold_size + " G";
        mechanic_hire_gold.text = mechanic_hire_gold_size + " G";

        StartCoroutine(ShowButtonEnable());
    }
    IEnumerator ShowButtonEnable()
    {
        while (true && !Gamemanager.Instance.buildgame.is_gameover)
        {
            cleaner_hire_button.enabled = Gamemanager.Instance.buildgame.gold >= cleaner_hire_gold_size;
            guard_hire_button.enabled = Gamemanager.Instance.buildgame.gold >= guard_hire_gold_size;
            mechanic_hire_button.enabled = Gamemanager.Instance.buildgame.gold >= mechanic_hire_gold_size && mechanic_dic.Count < 3;

            yield return null;
        }
    }
    // 청소층
    // 청소층 (max 인부) > (노동중인 인부) 일 때만 청소부 생성 가능 << 청소층에서 선언하여 사용할 것
    public void HireCleaner()
    {
        GameObject cleaner = Gamemanager.Instance.objectpool.GetCleaner();
        cleaner.SetActive(false);
        H_Cleaner cleaner_script = cleaner.GetComponent<H_Cleaner>();

        Gamemanager.Instance.buildgame.PlusGold(-cleaner_hire_gold_size);
        cleaner_hire_gold_size += 500;
        cleaner_hire_gold.text = cleaner_hire_gold_size + " G";

        cleaner_script.color = FID.CLEAN;
        cleaner_script.is_working = false;
        cleaner_script.speed = Random.Range(1f, 2f);
        cleaner_script.cleaning_speed = Random.Range(5f, 15f); // 첫 고용시 결정, 해고하고 다시 고용하면 재변경
        cleaner_script.pay = (int)(50 * (1 + Gamemanager.Instance.buffmanager.pay_increase_rate)); // 연봉 협상

        cleaner_script.id = cleaner_id; // 아이디 동기화
        // 청소부 고용
        cleaner_dic.Add(cleaner_id++, cleaner_script);

        // 패널에 객체 추가(UI)
        Instantiate(cleaner_info_sample, cleaners_contents).GetComponent<Cleaner_Info>().SettingCleanerInfo(cleaner_script); // 부모 지정 생성 및 초기화
    }
    public void FireCleaner(int id)
    {
        cleaner_dic.Remove(id);
        cleaner_hire_gold_size -= 500;
        cleaner_hire_gold.text = cleaner_hire_gold_size + " G";
    }
    public H_Cleaner GetCleaner() // 현재 사용가능한 청소부를 받아옴
    {
        foreach (var cleaner in cleaner_dic)        // val = <cleaner_id, cleaner_script> 
            if (!cleaner.Value.is_working) // 호출 가능
            {
                cleaner.Value.is_working = true;
                return cleaner.Value;
            }

        return null; // 부를 사람 없음
    }
    public void ReturnCleaner(int id)
    {
        cleaner_dic[id].gameObject.SetActive(false);
        cleaner_dic[id].is_working = false;
    }
    public void HireGuard()
    {
        GameObject guard = Gamemanager.Instance.objectpool.GetGuard();
        H_Guard guard_script = guard.GetComponent<H_Guard>();

        Gamemanager.Instance.buildgame.PlusGold(-guard_hire_gold_size);
        guard_hire_gold_size += 1000;
        guard_hire_gold.text = guard_hire_gold_size + " G";

        guard_script.color = PID.GUARD;
        guard_script.speed = Random.Range(1f, 2f) * (1 + Gamemanager.Instance.buffmanager.patron_speed_rate);
        guard_script.pay = (int)(100 * (1 + Gamemanager.Instance.buffmanager.pay_increase_rate)); // 연봉 협상

        guard_script.id = guard_id; // 아이디 동기화
        // 가드 고용
        guard_dic.Add(guard_id++, guard_script);

        // 패널에 객체 추가(UI)
        Instantiate(guard_info_sample, guards_contents).GetComponent<Guard_Info>().SettingGuardInfo(guard_script); // 부모 지정 생성 및 초기화

        //가드는 고용하자마자 1층에서 바로 생성되게 할 거임
        guard_script.Set(Gamemanager.Instance.buildgame.floors[FID.FIRST][0]);
    }
    public void FireGuard(int id)
    {
        guard_dic.Remove(id);
        guard_hire_gold_size -= 1000;
        guard_hire_gold.text = guard_hire_gold_size + " G";
    }

    public void HireMechanic()
    {
        GameObject mechanic = Gamemanager.Instance.objectpool.GetMechanic();
        H_Mechanic mechanic_script = mechanic.GetComponent<H_Mechanic>();
        remain_mechanic_count++;

        Gamemanager.Instance.buildgame.PlusGold(-mechanic_hire_gold_size);
        mechanic_hire_gold_size += 2500;
        mechanic_hire_gold.text = mechanic_hire_gold_size + " G";

        mechanic_script.color = FID.REPAIRSHOP;
        mechanic_script.speed = 2f;
        mechanic_script.fixing_speed = Random.Range(4, 8);
        mechanic_script.fix_elevator_count = 0;
        mechanic_script.pay = (int)(100 * (1 + Gamemanager.Instance.buffmanager.pay_increase_rate)); // 연봉 협상

        mechanic_script.id = mechanic_id; // 아이디 동기화
        // 가드 고용
        mechanic_dic.Add(mechanic_id++, mechanic_script);

        // 패널에 객체 추가(UI) 변경 필요 프리텝 요망
        Instantiate(mechanic_info_sample, mechanices_contents).GetComponent<Mechanic_Info>().SettingMechanicInfo(mechanic_script); // 부모 지정 생성 및 초기화
    }
    public void FireMechanic(int id)
    {
        remain_mechanic_count--;
        mechanic_dic.Remove(id);
        mechanic_hire_gold_size -= 2500;
        mechanic_hire_gold.text = mechanic_hire_gold_size + " G";
    }
    public H_Mechanic GetMechanic() // 현재 사용가능한 메카닉을 받아옴
    {
        foreach (var mechanic in mechanic_dic)        // val = <mechanic_id, mechanic_script> 
            if (!mechanic.Value.is_working) // 호출 가능
            {
                mechanic.Value.is_working = true;
                return mechanic.Value;
            }

        return null; // 부를 사람 없음
    }
    public void ReturnMechanic(int id)
    {
        mechanic_dic[id].gameObject.SetActive(false);
        mechanic_dic[id].is_working = false;
    }
    public void PushCleanersInfo() // 청소부들의 정보칸을 누름
    {
        cleaners_info.SetActive(true);
        guards_info.SetActive(false);
        mechanices_info.SetActive(false);
        //이미지 변경
        manager_tab_img.sprite = manager_tab_images[0];
    }
    public void PushGuardsInfo() // 가드들의 정보칸을 누름
    {
        cleaners_info.SetActive(false);
        guards_info.SetActive(true);
        mechanices_info.SetActive(false);
        //이미지 변경
        manager_tab_img.sprite = manager_tab_images[1];
    }
    public void PushMechanicsInfo() // 정비공들의 정보칸을 누름
    {
        cleaners_info.SetActive(false);
        guards_info.SetActive(false);
        mechanices_info.SetActive(true);
        //이미지 변경
        manager_tab_img.sprite = manager_tab_images[2];
    }
    public void EmployeesCareerUp()//하루가 지나서 경력이 증가, 실시간으로 변하게 하기 위한 함수
    {
        for (int i = 0; i < cleaners_contents.childCount; i++)
            cleaners_contents.GetChild(i).GetComponent<Cleaner_Info>().PlusCareer();
        for (int i = 0; i < guards_contents.childCount; i++)
            guards_contents.GetChild(i).GetComponent<Guard_Info>().PlusCareer();
        for (int i = 0; i < mechanices_contents.childCount; i++)
            mechanices_contents.GetChild(i).GetComponent<Mechanic_Info>().PlusCareer();
    }
    public void EmployeesPayUp()//디버프 철밥통용
    {
        for (int i = 0; i < cleaners_contents.childCount; i++)
            cleaners_contents.GetChild(i).GetComponent<Cleaner_Info>().IronBowl();
        for (int i = 0; i < guards_contents.childCount; i++)
            guards_contents.GetChild(i).GetComponent<Guard_Info>().IronBowl();
        for (int i = 0; i < mechanices_contents.childCount; i++)
            mechanices_contents.GetChild(i).GetComponent<Mechanic_Info>().IronBowl();
    }
    public void PatronSpeedUp() // 버프 순찰 5분컷용
    {
        for (int i = 0; i < guards_contents.childCount; i++)
            guards_contents.GetChild(i).GetComponent<Guard_Info>().PatronUp();
    }
    public int PayDay()
    {
        int pay_sum = 0;

        foreach (var cleaner in cleaner_dic)        // val = <cleaner_id, cleaner_script> 
            pay_sum += cleaner.Value.pay;

        foreach (var guard in guard_dic)            // val = <guard_id, guard_script> 
            pay_sum += guard.Value.pay;

        foreach (var mechanic in mechanic_dic)      // val = <mechanic_id, mechanic_script> 
            pay_sum += mechanic.Value.pay;

        Gamemanager.Instance.buildgame.PlusGold(-pay_sum);

        return pay_sum;
    }
}
