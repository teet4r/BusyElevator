using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Text start_txt;
    public Image title_img;
    public Button setting_btn, help_btn, achievevment_btn, start_btn;
    public GameObject[] ui_panel;

    [SerializeField]
    GameObject leftdoor, rightdoor;

    // 0 setting
    // 1 help
    // 2 achieve

    int cur_on_panel_idx;
    //현재 켜져있는 패널 인덱스

    Coroutine blink, boingboing;

    string log;

    void Awake()
    {
        start_btn.gameObject.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(OpenDoors());

        Gamemanager.Instance.Set();
        SoundManager.Instance.PlayBgm("Menu3");

        blink = StartCoroutine(Blink());
        boingboing = StartCoroutine(BoingBoing());
    }
    public void PushGameStart() // 존나큰 버튼
    {
        start_btn.gameObject.SetActive(false); // 연출을 위해 꺼
        start_txt.gameObject.SetActive(false);
        //StartCoroutine(ZoomIn());
        StartCoroutine(CloseDoors());
        //StartCoroutine(UIMove());
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

        // 문 다 열리면 시작 버튼 누를 수 있음
        start_btn.gameObject.SetActive(true);
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

        StopCoroutine(blink);
        SceneManager.LoadScene("Ingame");
    }
    IEnumerator ZoomIn()
    {
        for (int i = 0; i < 50; i++)
        {
            Camera.main.orthographicSize -= 0.1f;
            yield return null;
        }
        StopCoroutine(blink);
        SceneManager.LoadScene("Ingame");
    }
    IEnumerator UIMove()
    {
        for (int i = 0; i < 50; i++)
        {
            title_img.transform.position += Vector3.up * 15;
            setting_btn.transform.position += Vector3.right * 7;
            help_btn.transform.position += Vector3.right * 7;
            achievevment_btn.transform.position += Vector3.right * 7;
            yield return null;
        }
    }
    IEnumerator Blink()
    {
        float r = start_txt.color.r;
        float g = start_txt.color.r;
        float b = start_txt.color.r;

        while (true)
        {
            for (float a = 1; a >= 0; a -= 0.03f)
            {
                start_txt.color = new Color(r, g, b, a);
                yield return null;
            }
            for (float a = 0; a <= 1; a += 0.03f)
            {
                start_txt.color = new Color(r, g, b, a);
                yield return null;
            }
        }
    }
    IEnumerator BoingBoing()
    {
        while(true)
        {
            for (int i = 1; i < 50; i++)
            {
                title_img.transform.localScale -= new Vector3(0.01f, 0.01f, 0) * 6/i;
                yield return null;
            }
            for (int i = 1; i < 50; i++) 
            {
                title_img.transform.localScale += new Vector3(0.01f, 0.01f, 0) * 6/i;
                yield return null;
            }
            
        }
    }
    public void TurnOnSetting() // 누르면 발동
    {
        cur_on_panel_idx = 0;
        setting_btn.enabled = false;
        help_btn.enabled = false;
        achievevment_btn.enabled = false;
        ui_panel[cur_on_panel_idx].SetActive(true);
    }
    public void TurnOnHelp() // 누르면 발동
    {
        cur_on_panel_idx = 1;
        setting_btn.enabled = false;
        help_btn.enabled = false;
        achievevment_btn.enabled = false;
        ui_panel[cur_on_panel_idx].SetActive(true);
    }
    public void TurnOnArchievement() // 누르면 발동
    {
        cur_on_panel_idx = 2;
        setting_btn.enabled = false;
        help_btn.enabled = false;
        achievevment_btn.enabled = false;
        ui_panel[cur_on_panel_idx].SetActive(true);
    }
    public void ExitButtonPush() // 각 창에 설정된 x버튼(뒤로가기)
    {
        ui_panel[cur_on_panel_idx].SetActive(false);

        setting_btn.enabled = true;
        help_btn.enabled = true;
        achievevment_btn.enabled = true;
    }
}
