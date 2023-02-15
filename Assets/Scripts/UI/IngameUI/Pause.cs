using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject menu, checkbox; // 메뉴바(설정, 메인으로, 재시작), 체크박스(yes or no)
    public List<GameObject> child_panels; // 자식 패널들 자동으로 끄기
    string dir; // 목적지 = Scene (Main or Ingame)
    public Text text;
    bool is_pause_pushed = false; // pause가 눌린 상태면 true
    public void Push()
    {
        if (!is_pause_pushed) // ∥ 상태에서 다시 누르면 멈춤
        {
            Gamemanager.Instance.uimanager.TimeStop();

            if (!Gamemanager.Instance.buffmanager.is_korean_speed)
                text.text = "▶";

            is_pause_pushed = true;
            menu.SetActive(true);
        }
        else // ▶ 상태에서 다시 누르면 재생
        {
            Gamemanager.Instance.uimanager.TryTimeRestart();

            text.text = "I I";

            foreach(var cp in child_panels)
                cp.SetActive(false);

            is_pause_pushed = false;
            menu.SetActive(false);
        }
    }
    public void PushToMain()
    {
        dir = "Menu";
        checkbox.SetActive(true);
    }
    public void PushToRestart()
    {
        dir = "Ingame";
        checkbox.SetActive(true);
    }
    public void PushYes()
    {
        Gamemanager.Instance.buildgame.OnClickLoadScene(dir);
    }
    public void PushNo()
    {
        checkbox.SetActive(false);
    }
}