using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Management_detail : MonoBehaviour
{

    public InputField elevator_name;
    public Text speed, volume, floor, level, point, max_durability;
    public Text speed_point, volume_point, durability_point;
    public Image exp_bar;
    public Button manage;
    public Button fix;
    public Button auto;
    public Button[] skillpoint;
    public InputField top_floor_input, bottom_floor_input;
    public float exp, max_exp;

    ElevatorClass ele;

    Coroutine showing_coroutine;

    public void Setting(ElevatorClass e)
    {
        manage.enabled = false;

        elevator_name.text = e.gameObject.name;

        ele = e;
        speed.text = "속도 : " + e.cur_speed + "km/s";
        volume.text = "최대 인원 : " + e.max_volume + "명";
        max_durability.text = "최대 내구도 : " + e.max_durability;
        floor.text = "운행층 : ";
        point.text = "Point : " + e.cur_point;

        speed_point.text = ele.skill_dict["speed"].ToString();
        volume_point.text = ele.skill_dict["volume"].ToString();
        durability_point.text = ele.skill_dict["durability"].ToString();

        top_floor_input.text = ele.save_top_floor.ToString();
        bottom_floor_input.text = ele.save_bottom_floor.ToString();

        top_floor_input.enabled = !ele.is_automating;
        bottom_floor_input.enabled = !ele.is_automating; // 자동 중이었으면(true) 해당 버튼 활성화

        if (ele.is_automating) // 자동중
        {
            top_floor_input.image.color = Color.gray;
            bottom_floor_input.image.color = Color.gray;
        }
        else
        {
            top_floor_input.image.color = Color.white;
            bottom_floor_input.image.color = Color.white;
        }

        showing_coroutine = StartCoroutine(ShowInfo());
    }
    IEnumerator ShowInfo() // 수시적으로 변하는 경험치와 레벨 및 수리가능여부를 실시간으로 반환
    {
        while (true)
        {

            exp_bar.fillAmount = ele.cur_exp / ele.max_exp;
            level.text = "Level " + ele.level;

            foreach (Button bt in skillpoint)
            {
                if (ele.cur_point > 0)
                {
                    bt.enabled = true;
                    bt.GetComponent<Image>().color = Color.white;
                }
                else
                {
                    bt.enabled = false;
                    bt.GetComponent<Image>().color = Color.gray;
                }
            }

            point.text = "Point : " + ele.cur_point;
            if (!(Gamemanager.Instance.buildgame.floor_of[0] as F_Repairshop).is_line_repair_using[ele.line] &&
                Gamemanager.Instance.employeemanager.remain_mechanic_count > 0 &&
                !ele.is_loading)
            {
                fix.enabled = true;
                fix.GetComponent<Image>().color = Color.white;
            }
            else
            {
                fix.enabled = false;
                fix.GetComponent<Image>().color = Color.gray;
            }
            yield return null;
        }
    }
    public void Use_point()
    {
        string btn = EventSystem.current.currentSelectedGameObject.name;

        switch (btn)
        {
            case "Speed_add_btn":
                ele.cur_speed += 0.3f;
                speed.text = "속도 : " + ele.cur_speed + "km/s";
                ele.cur_point--;
                speed_point.text = (++ele.skill_dict["speed"]).ToString();
                break;
            case "Voluem_add_btn":
                ele.max_volume += 1;
                volume.text = "최대 인원 : " + ele.max_volume + "명";
                ele.cur_point--;
                volume_point.text = (++ele.skill_dict["volume"]).ToString();
                break;
            case "Durability_add_btn":
                ele.max_durability += 10;
                max_durability.text = "최대 내구도 : " + ele.max_durability;
                ele.cur_point--;
                durability_point.text = (++ele.skill_dict["durability"]).ToString();
                break;
        }
    }
    public void PushFixBtn() // 강제 수리 이동
    {
        Gamemanager.Instance.employeemanager.remain_mechanic_count--;
        ele.GoToFix(Gamemanager.Instance.employeemanager.GetMechanic());
        Save();
    }
    public void PushAutoBtn()
    {
        if (ele.is_automating) // 자동 해제
        {
            top_floor_input.image.color = Color.white;
            bottom_floor_input.image.color = Color.white;
            ele.StopAuto();
        }
        else
        {
            top_floor_input.image.color = Color.gray;
            bottom_floor_input.image.color = Color.gray;
            ele.StartAuto();
        }

        top_floor_input.enabled = !ele.is_automating;
        bottom_floor_input.enabled = !ele.is_automating; // 자동 중이었으면(true) 해당 버튼 활성화
    }
    public void Save()
    {
        ele.gameObject.name = elevator_name.text;
        ele.elevator_info.ChangeName();
        ele.Reset(Mathf.Min(int.Parse(top_floor_input.text), Gamemanager.Instance.buildgame.building_top_floor),
                    Mathf.Max(int.Parse(bottom_floor_input.text), Gamemanager.Instance.buildgame.building_bottom_floor));
        manage.enabled = true;
        Camera.main.GetComponent<CameraMove>().StopChaseElevator();
        StopCoroutine(showing_coroutine);
        gameObject.SetActive(false);
    }
}