using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EleSimpleInfo : MonoBehaviour
{
    public Text level_txt;
    public Text name_txt;
    public Text state_txt; // 수리중 등 일 때 해당 버튼을 이용 못한다는 사유를 적어주는 것
    public Text auto_state_txt; // 자동 운행 중이면 초록색, 아니면 회색
    public Image waring_img;
    public Image durability_bar_img;
    public Image screen_img; // 가림막 이미지, 버튼이 이용불가할 때 덮어씌움

    ElevatorClass elevatorclass_script;

    public void Init(ElevatorClass elevatorclass_script)
    {
        this.elevatorclass_script = elevatorclass_script;
        elevatorclass_script.elevator_info = this;

        level_txt.text = "Lv" + 1;
        name_txt.text = elevatorclass_script.gameObject.name; // 최대 6글자
        screen_img.enabled = false;
        state_txt.text = "";
        auto_state_txt.color = Color.gray;
    }

    //======================================실시간으로 변하는 값들======================================================
    public void ChangeLevel()
    {
        // 레벨이 올라갈때마다 변경
        level_txt.text = "Lv" + elevatorclass_script.level;
    }
    public void ChangeName()
    {
        // 이름이 바뀔때마다 변경
        name_txt.text = elevatorclass_script.gameObject.name; // 최대 5글자(한글기준, 영어는 좀 길어도 될듯)
    }
    public void ChangeAuto()
    {
        // 오토 버튼을 누를때마다 변경
        auto_state_txt.color = (elevatorclass_script.is_automating) ? Color.green : Color.gray;
    }
    public void ShowIsFixing(bool is_fixing) // 수리를 하려 움직이는 중인가?
    {
        // 엘리베이터가 수리버튼을 누른 후 부터 완전하게 복귀하기 전까지의 상태
        if (is_fixing) // 현재 엘리베이터가  수리 전후(수리 버튼 클릭부터 수리 후 복귀까지)라면 값을 변경 할 수 없음
        {
            state_txt.text = "수리";
            //button.enabled = false;
            screen_img.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // 반투명 회색
        }
        else
        {
            state_txt.text = "";
            //button.enabled = true;
            screen_img.color = new Color(0, 0, 0, 0); // 투명
        }
    }
    public void ShowDurabilityProgress()
    {
        //정비소에서 수리 중일때 혹은 내구도가 소모될때 게이지 변환 보여줌
        durability_bar_img.fillAmount = (elevatorclass_script.cur_durability / elevatorclass_script.max_durability);

        if ((elevatorclass_script.cur_durability / elevatorclass_script.max_durability) > 0.5) // 내구도 50% 이상
            durability_bar_img.color = new Color(
                -2 * ((elevatorclass_script.cur_durability / elevatorclass_script.max_durability) - 1),
                1,
                0);
        else // 내구도 50% 미만
            durability_bar_img.color = new Color(
                1,
                2 * (elevatorclass_script.cur_durability / elevatorclass_script.max_durability),
                0);
    }
    public void ShowWarningState()
    {
        waring_img.color =
        (
            //1. 과적재
            elevatorclass_script.is_overload ||
            //2. 도둑 탑승 << 아직 적용안됨
            elevatorclass_script.thief_cnt > 0 ||
            //3. 엘리페이터 파손
            elevatorclass_script.cur_durability == 0
        //3. etc(미구현)
        )
        ? Color.red : Color.gray;
    }

    public void Push()
    {
        Gamemanager.Instance.elevatormanager.managepanel.SetActive(true);
        Gamemanager.Instance.elevatormanager.managepanel.GetComponent<Management_detail>().Setting(elevatorclass_script);
        Camera.main.GetComponent<CameraMove>().SelectElevator(elevatorclass_script); // 카메라가 따라다닐 엘리베이터를 넘겨줌
    }
}
