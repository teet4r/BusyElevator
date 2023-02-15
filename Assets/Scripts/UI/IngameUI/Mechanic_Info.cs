using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mechanic_Info : MonoBehaviour
{
    H_Mechanic mechanic; // 현재 정보를 가지고 있는 사람의 스크립트

    public Text employee_name; // 이름
    public Text employee_birth_place; // 출생지
    public Text employee_date_of_join; // 입사날짜
    public Text employee_career; // 경력
    public int career;
    public Text employee_fix_elevator_count; // 수리 횟수
    public Text employee_proficiency; // 숙련도(일에 영향)
    public Text level; // 레벨로 일단은 표현
    public Text promotion_gold; // 승진에 필요한 골드
    public int promotion_gold_size;

    public Button fire_btn;         // 해고버튼
    public Button promotion_btn;    // 승진버튼

    int id;

    public void SettingMechanicInfo(H_Mechanic mechanic) // 최초 세팅
    {
        // id 부여
        this.id = mechanic.id;
        mechanic.mech_info = this;
        this.mechanic = mechanic; // 스크립트 동기화

        promotion_gold_size = 300;
        promotion_gold.text = promotion_gold_size + " G";
        
        // 랜덤 이름 생성
        employee_name.text = Gamemanager.Instance.employeemanager.first_name[Random.Range(0, Gamemanager.Instance.employeemanager.first_name.Length - 1)]
                     + " " + Gamemanager.Instance.employeemanager.second_name[Random.Range(0, Gamemanager.Instance.employeemanager.second_name.Length - 1)];

        // 간략 설명 부여
        // 출생지
        employee_birth_place.text = "출생지 : " + Gamemanager.Instance.employeemanager.birth_place[Random.Range(0, Gamemanager.Instance.employeemanager.birth_place.Length - 1)];
        // 입사날
        employee_date_of_join.text = "입사일 : " + Gamemanager.Instance.buildgame.day + "일";
        // 경력
        employee_career.text = "경력일수 : " + career + "일";
        // 청소를 완료한 층 수
        employee_fix_elevator_count.text = "수리횟수 : " + mechanic.fix_elevator_count + "번";
        // 숙련도
        employee_proficiency.text = "숙련도 : " + mechanic.fixing_speed;

        if (Gamemanager.Instance.buffmanager.is_iron_craw_on) // 디버프 철밥통이 적용 중이면 해고버튼 사용 금지
            fire_btn.enabled = false;
    }
    public void Promotion()
    {
        if (Gamemanager.Instance.buildgame.gold >= promotion_gold_size)
        {
            Gamemanager.Instance.buildgame.PlusGold(-promotion_gold_size);

            // pay 30% 증가
            mechanic.pay = (int)(1.3 * mechanic.pay);

            // 숙련도 80% 증가
            mechanic.fixing_speed = (int)(1.15f * mechanic.fixing_speed);
            employee_proficiency.text = "숙련도 : " + Mathf.Floor(mechanic.fixing_speed * 100) * 0.01;

            // 승진 골드 50% 증가
            promotion_gold_size = (int)(1.5 * promotion_gold_size);
            promotion_gold.text = promotion_gold_size + " G";
        }
    }
    public void PlusCareer()
    {
        employee_career.text = "경력일수 : " + ++career + "일";
    }
    public void IronBowl()
    {
        mechanic.pay = (int)(1.5 * mechanic.pay);
        fire_btn.enabled = false;
    }
    public void Fire() // 넌 해고야!
    {
        if (Gamemanager.Instance.buffmanager.is_severance_pay_on)
            Gamemanager.Instance.buildgame.PlusGold(-100 * (career + 1) * (mechanic.fix_elevator_count+ 1));

        Gamemanager.Instance.employeemanager.FireMechanic(id);
        Destroy(gameObject);
    }
}
