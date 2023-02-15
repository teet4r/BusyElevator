using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Management : MonoBehaviour
{
    public Button manage_panel_off_button; // 전부 끄는 투명 버튼
    public Button elevator_manage_button; // 오른쪽(엘리베이터 관리 버튼)
    public Button employee_manager_button; // 왼쪽하단(직원 관리 버튼)
    public Button building_manager_button; // 왼쪽상단(건물 관리 버튼)
    public GameObject elevator_manager_panel;
    public GameObject employee_manager_panel;
    public GameObject building_manager_panel;
    public int btn_idx;
    public bool elevator_btn_selected = false;
    public bool employee_btn_selected = false;
    public bool building_btn_selected = false;

    public void PushElevator()
    {
        if (building_btn_selected) // 기존에 켜져있는 놈은 끈다
            PushBuilding();
        if (employee_btn_selected)
            PushEmployee();

        btn_idx = 0; // 현재 내가 누른 버튼의 이름

        if (!elevator_btn_selected) // 버튼 눌린 상태 아님
        {
            elevator_btn_selected = true;
            elevator_manage_button.enabled = false;
            StartCoroutine(ManagerPanelMove(false, elevator_manage_button, elevator_manager_panel)); // 왼쪽으로 이동
            manage_panel_off_button.gameObject.SetActive(true);
        }
        else
        {
            elevator_btn_selected = false;
            elevator_manage_button.enabled = false;
            StartCoroutine(ManagerPanelMove(true, elevator_manage_button, elevator_manager_panel)); // 오른쪽으로 이동
            manage_panel_off_button.gameObject.SetActive(false);
        }
    }
    public void PushEmployee()
    {
        if (elevator_btn_selected)
            PushElevator();
        if (building_btn_selected)
            PushBuilding();

        btn_idx = 1;

        if (!employee_btn_selected) // 버튼 눌린 상태 아님
        {
            employee_btn_selected = true;
            employee_manager_button.enabled = false;
            StartCoroutine(ManagerPanelMove(true, employee_manager_button, employee_manager_panel)); // 오른족으로 이동
            manage_panel_off_button.gameObject.SetActive(true);
        }
        else
        {
            employee_btn_selected = false;
            employee_manager_button.enabled = false;
            StartCoroutine(ManagerPanelMove(false, employee_manager_button, employee_manager_panel)); // 왼ㅉㅎㄱ으로 이동
            manage_panel_off_button.gameObject.SetActive(false);
        }
    }
    public void PushBuilding()
    {
        if (elevator_btn_selected)
            PushElevator();
        if (employee_btn_selected)
            PushEmployee();

        btn_idx = 2;

        if (!building_btn_selected) // 버튼 눌린 상태 아님
        {
            building_btn_selected = true;
            building_manager_button.enabled = false;
            StartCoroutine(ManagerPanelMove(true, building_manager_button,building_manager_panel)); // 오른족으로 이동
            manage_panel_off_button.gameObject.SetActive(true);
        }
        else
        {
            building_btn_selected = false;
            building_manager_button.enabled = false;
            StartCoroutine(ManagerPanelMove(false, building_manager_button, building_manager_panel)); // 왼ㅉㅎㄱ으로 이동
            manage_panel_off_button.gameObject.SetActive(false);
        }
    }
    IEnumerator ManagerPanelMove(bool is_move_right, Button pushed_btn, GameObject panel) // 객체가 오른쪽으로 움직여야하면 true
    {
        RectTransform rt1, rt2;
        rt1 = panel.GetComponent<RectTransform>();
        rt2 = pushed_btn.GetComponent<RectTransform>();

        if (is_move_right)
            for (int i = 0; i < 5; i++)  
            {
                rt1.transform.localPosition += Vector3.right * 180;
                rt2.transform.localPosition += Vector3.right * 180;
                yield return null;
            }
        else
            for (int i = 0; i < 5; i++)
            {
                rt1.transform.localPosition += Vector3.left * 180;
                rt2.transform.localPosition += Vector3.left * 180;
                yield return null;
            }
        pushed_btn.enabled = true;
    }
}
