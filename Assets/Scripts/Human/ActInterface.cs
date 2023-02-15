using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActInterface
{
    IEnumerator ActAtATM(ATM atm);
    /// <요약>
    ///  Human
    ///  돈 획득
    ///  ----------------
    ///  Normal
    ///  돈 인출
    ///  ----------------
    ///  Fat
    ///  돈 인출
    ///  ----------------
    ///  Vip
    ///  돈 인출
    ///  ----------------
    ///  Mechnic
    ///  구현 안함
    ///  ----------------
    ///  Cleaner
    ///  구현 안함
    ///  ----------------
    ///  Patient
    ///  돈 인출
    ///  ----------------
    ///  Sick
    ///  구현 안함
    ///  ----------------
    ///  Thief
    ///  돈 절도
    ///  ----------------
    ///  Master
    ///  구현 안함
    /// <요약>
    IEnumerator ActAtVM(VendingMachine vm); // Vending Machine
    /// <요약>
    ///  Human
    ///  구현 안함
    ///  ----------------
    ///  Normal
    ///  돈 사용
    ///  (일정확률 Sick 생성)
    ///  ----------------
    ///  Fat
    ///  돈 사용
    ///  (일정확률 Sick 생성)
    ///  ----------------
    ///  Vip
    ///  돈 사용
    ///  (일정확률 Sick 생성)
    ///  ----------------
    ///  Mechnic
    ///  구현 안함
    ///  ----------------
    ///  Cleaner
    ///  구현 안함
    ///  ----------------
    ///  Patient
    ///  ????
    ///  ----------------
    ///  Sick
    ///  구현 안함
    ///  ----------------
    ///  Thief
    ///  구현 안함
    ///  ----------------
    ///  Master
    ///  구현 안함
    /// <요약>
    IEnumerator ActAtHD(HospitalDoor hd);
    /// <요약>
    ///  Human
    ///  구현 안함
    ///  ----------------
    ///  Normal
    ///  ????
    ///  ----------------
    ///  Fat
    ///  ????
    ///  ----------------
    ///  Vip
    ///  ????
    ///  ----------------
    ///  Mechnic
    ///  구현 안함
    ///  ----------------
    ///  Cleaner
    ///  구현 안함
    ///  ----------------
    ///  Patient
    ///  구현 안함
    ///  ----------------
    ///  Sick
    ///  Patient로 변경되는 듯한 느낌을 줌
    ///  돈 지불
    ///  ----------------
    ///  Thief
    ///  구현 안함
    ///  ----------------
    ///  Master
    ///  구현 안함
    /// <요약>
    IEnumerator ActAtSAD(SmokingAreaDoor sad); // Smoking Area Door
    /// <summary>
    /// 나중에 생각나면 추가
    /// </summary>
    /// <param name="elevator"></param>
    IEnumerator ActAtExit(Exit exit);
    IEnumerator ActAtInfo(Infomation info);
    IEnumerator ActAtWP(WaterPurifier wp); // Water Purifier
    IEnumerator ActAtBS(BookShelf bs); // Book Shelf
    IEnumerator ActAtND(NormalDoor nd); // Normal Door
    IEnumerator ActAtFFExit(FirstFloorExit ffe); // Fisrt Floor Exit
    IEnumerator ActAtRestExit(RestExit re);
    IEnumerator ActAtCounter(Counter c);

    /// <summary>
    /// 사람끼리의 상호작용
    /// </summary>
    /// <returns></returns>
    IEnumerator ActWithGuard(Human other); // 가드와의 상호작용
    IEnumerator ActWithThief(Human other); // 도둑과의 상호작용

    void ActInElevator(ElevatorClass elevator);
    /// <요약>
    ///  Human
    ///  무브 코루틴 정지
    ///  ----------------
    ///  Normal
    ///  구현 안함
    ///  ----------------
    ///  Fat
    ///  구현 안함
    ///  ----------------
    ///  Vip
    ///  구현 안함
    ///  **Vip_F**
    ///  여성vip가 탔다고 알림 
    ///  ----------------
    ///  Mechnic
    ///  구현 안함
    ///  ----------------
    ///  Cleaner
    ///  구현 안함
    ///  ----------------
    ///  Patient
    ///  구현 안함
    ///  ----------------
    ///  Sick
    ///  구현 안함
    ///  ----------------
    ///  Thief
    ///  도둑이 탔다고 알림
    ///  ----------------
    ///  Master
    ///  사장이 탔다고 알림
    /// <요약>
    void ActOffElevator(ElevatorClass elevator);
    /// <요약>
    ///  Human
    ///  비용 지불
    ///  층 더럽힘
    ///  Hp 회복?
    ///  말풍선 제거
    ///  레이어 변경
    ///  이동
    ///  ----------------
    ///  Normal
    ///  구현 안함
    ///  ----------------
    ///  Fat
    ///  구현 안함
    ///  ----------------
    ///  Vip
    ///  **Vip_F**
    ///  여성vip가 내렸다고 알림
    ///  비용지불
    ///  레이어 변경
    ///  이동
    ///  ----------------
    ///  Mechnic
    ///  구현 안함
    ///  ----------------
    ///  Cleaner
    ///  구현 안함
    ///  ----------------
    ///  Patient
    ///  구현 안함
    ///  ----------------
    ///  Sick
    ///  구현 안함
    ///  ----------------
    ///  Thief
    ///  도둑이 내렸다고 알림
    ///  레이어 변경
    ///  이동
    ///  ----------------
    ///  Master
    ///  사랑이 내렸다고 알림
    ///  레이어 변경
    ///  이동
    /// <요약>
}