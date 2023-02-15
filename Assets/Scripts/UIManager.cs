using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached on Main Camera
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;    //캔버스

    [SerializeField]
    Button multiple_btn; //배속버튼
    [SerializeField]
    Button pause_btn; // 일시정지버튼

    [SerializeField]
    GameObject explain_panel;
    public int stop_panels_count { get; set; }

    ExplainText explain_text;

    public bool is_explain_on; // 인게임 설명 사용 결정 , true라면 설명창이 보임
    public bool is_bgm_on; 
    public bool is_effectsound_on;

    void Awake()
    {
        stop_panels_count = 0;
    }
    void Start()
    {
        explain_text = new ExplainText();
    }
    public void KoreanSpeedOn()
    {
        multiple_btn.GetComponent<MultipleSpeed>().multiple_text.text = "X3";
        multiple_btn.enabled = false;
    }
    public void TryTimeRestart() // 어느 방법으로든 게임 속도를 원상태로 돌려함
    {
        stop_panels_count--;
        if (stop_panels_count == 0)
            Time.timeScale = Gamemanager.Instance.buildgame.timespeed;
    }
    public void TimeStop() // 게임 속도 정지
    {
        if (!Gamemanager.Instance.buffmanager.is_korean_speed)
        {
            stop_panels_count++;
            Time.timeScale = 0f;
        }
    }
    public void ShowExplain(ref bool is_appear, int explain_idx) // 현재 설명을 이전에 본적이 있는지, 설명 인덱스
    {
        if (!is_appear && is_explain_on) // 타이머 설명창 생성
        {
            TimeStop();
            GameObject ex_panel = Instantiate(explain_panel, canvas.transform);
            ex_panel.GetComponent<ExplainPanel>().Set(explain_text.explain_arr[explain_idx]);
            is_appear = true;
        }
    }
}
public class ExplainText
{
    public int man_size = 15;
    public string[][] explain_arr;

    public ExplainText()
    {
        explain_arr = new string[man_size][];

        for (int i = 0; i < man_size; i++) 
            explain_arr[i] = new string[3];

        explain_arr[PID.THIEF][0] = "도둑";
        explain_arr[PID.THIEF][1] = "어렸을 때부터 바늘을 훔쳤다는군요.";
        explain_arr[PID.THIEF][2] = "목적지에 도착하면 일정량 골드를 뺏습니다. 은행에선 더 많은 돈을 잃습니다. 경비를 배치하십시오. 밤에 출몰합니다.";

        explain_arr[1][0] = "러쉬";
        explain_arr[1][1] = "효율적으로 엘리베이터를 배치하세요!";
        explain_arr[1][2] = "타이머의 시간이 다되면 해당 색깔의 모든 층에서 사람이 마구 생성됩니다.";

        explain_arr[PID.SANITATION][0] = "위생관";
        explain_arr[PID.SANITATION][1] = "훈련소 조교 출신이라 하네요.";
        explain_arr[PID.SANITATION][2] = "건물을 이동하면서 위생검사를 합니다. 더러움 관리에 주의하십시오.";

        explain_arr[PID.HOMELESS][0] = "노숙자";
        explain_arr[PID.HOMELESS][1] = "차가운 대리석 바닥을 선호합니다.";
        explain_arr[PID.HOMELESS][2] = "현재 있는 층에 주기적으로 더러움을 추가합니다. 행동을 할 때는 더욱 많은 더러움을 추가합니다. 누워서 자기도 합니다.";

        explain_arr[PID.SICK][0] = "식중독";
        explain_arr[PID.SICK][1] = "이런 배아픈 사람이 생겼네요";
        explain_arr[PID.SICK][2] = "특수 상호삭용 중 일정 확률로 배탈이 발생합니다. 병원으로 이송하세요.";
    }
}
