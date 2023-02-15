using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    BuildGame buildgame_script;
    ElevatorManager elevator_manager_script;

    bool is_touching = false; // 현재 화면을 터치 중이면 true
    Rigidbody2D camera_rigid; // 카메라 이동을 위한 물리법칙 적용
    Vector2 start_touch; // 터치가 시작된 위치
    Vector2 right_before_touch; // 터치가 시작되기 직전에 터치되었던 위치

    [SerializeField]
    Management_detail managedetail;
    [SerializeField]
    Management manage;


    ElevatorClass chase_elevator = null; // 쫒고있는 엘리베이터
    Coroutine chase_selected_elevator_coroutine;

    protected void Awake()
    {
        buildgame_script = GetComponent<BuildGame>();
        elevator_manager_script = GetComponent<ElevatorManager>();
        camera_rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(CameraMoving());
    }
    IEnumerator CameraMoving() // 화면 드래그로 인한 카메라 위치 변경 
    {
        while (true)
        {
            if (Input.touchCount == 1)
            {
                Touch tempTouchs = Input.GetTouch(0);
                if (tempTouchs.phase == TouchPhase.Began && !is_touching)
                {    //해당 터치가 시작됐다면.
                    is_touching = true;
                    camera_rigid.velocity = Vector3.zero;
                    right_before_touch = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.

                }
                else if (tempTouchs.phase == TouchPhase.Moved)
                {    //해당 터치가 움직이는 중이라면.
                    Vector2 touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                    camera_rigid.AddForce(new Vector3(0, right_before_touch.y - touchedPos.y, 0) * 300, ForceMode2D.Force);
                    right_before_touch = touchedPos;
                }
                else if (tempTouchs.phase == TouchPhase.Ended) // 끝났다면
                {
                    Vector2 touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                    right_before_touch = touchedPos;
                    is_touching = false;
                }
            }
            if (gameObject.transform.position.y < 2) // 최하층 돌파
            {
                gameObject.transform.position = new Vector3(0, 2, -10);
                camera_rigid.velocity = Vector3.zero;
            }
            else if (gameObject.transform.position.y > (buildgame_script.building_top_floor - 2) * 2) // 최고층 돌파
            {
                gameObject.transform.position = new Vector3(0, (buildgame_script.building_top_floor - 2) * 2, -10);
                camera_rigid.velocity = Vector3.zero;
            }
            yield return null;
        }
    }
    public void SelectElevator(ElevatorClass elevator_script) // 엘리베이터 레벨업을 위해 엘리베이터를 선택했을때 해당 엘리베이터를 따라다니는 코루틴시작시켜주는 함수
    {
        chase_elevator = elevator_script;
        chase_selected_elevator_coroutine = StartCoroutine(ChaseElevator(elevator_script.gameObject));
    }
    IEnumerator ChaseElevator(GameObject elevator)
    {
        while (true)
        {
            if (elevator.transform.position.y < 2) // 최하층 돌파
                gameObject.transform.position = new Vector3(camera_rigid.position.x, 2, -10);
            else if (elevator.transform.position.y > (buildgame_script.building_top_floor - 2) * 2) // 최고층 돌파
                gameObject.transform.position = new Vector3(camera_rigid.position.x, (buildgame_script.building_top_floor - 2) * 2, -10);
            else
                camera_rigid.position = new Vector3(camera_rigid.position.x, elevator.transform.position.y, -10);
            yield return null;
        }
    }
    public void StopChaseElevator() // 위 코루틴을 외부에서 정지 시켜주는 함수
    {
        if (chase_elevator != null)
        {
            StopCoroutine(chase_selected_elevator_coroutine);
            chase_elevator = null;
        }
    }

    public void PushManagePanelOffButton()
    {
        StopChaseElevator();

        if (managedetail.gameObject.activeSelf)
            managedetail.Save();

        if (manage.elevator_btn_selected || manage.employee_btn_selected || manage.building_btn_selected)
        {
            if (manage.btn_idx == 0)
                manage.PushElevator();
            else if (manage.btn_idx == 1)
                manage.PushEmployee();
            else if (manage.btn_idx == 2)
                manage.PushBuilding();
        }
    }
}
