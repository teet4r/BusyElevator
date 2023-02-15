using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Setting : MonoBehaviour
{
    public GameObject setting_panel;

    public Button explain_set_btn;
    public Button bgm_set_btn;
    public Button effectsound_set_btn;

    public Text explain_state;
    public Text bgm_state;
    public Text effectsound_state;

    public void PushSettingButton() // 세팅 버튼 클릭(패널 활성화)
    {
        if (setting_panel.activeSelf)
            setting_panel.SetActive(false);
        else
            setting_panel.SetActive(true);
    }
    public void PushExplainSettingButton() // 설명 활성화 유무 클릭
    {
        if(Gamemanager.Instance.uimanager.is_explain_on) // On이였음
        {
            explain_state.text = "OFF";
            explain_set_btn.GetComponent<Image>().color = Color.gray;
            Gamemanager.Instance.uimanager.is_explain_on = false;
        }
        else
        {
            explain_state.text = "ON";
            explain_set_btn.GetComponent<Image>().color = Color.white;
            Gamemanager.Instance.uimanager.is_explain_on = true;
        }
    }
    public void PushBgmSettingButton() // 브금 활성화 유무 클릭
    {
        if (Gamemanager.Instance.uimanager.is_bgm_on) // On이였음
        {
            SoundManager.Instance.OffBgm();
            bgm_state.text = "OFF";
            bgm_set_btn.GetComponent<Image>().color = Color.gray;
            Gamemanager.Instance.uimanager.is_bgm_on = false;
        }
        else
        {
            SoundManager.Instance.OnBgm();
            bgm_state.text = "ON";
            bgm_set_btn.GetComponent<Image>().color = Color.white;
            Gamemanager.Instance.uimanager.is_bgm_on = true;
        }
    }
    public void PushEffectSoundSettingButton() // 효과음 활성화 유무 클릭
    {
        if (Gamemanager.Instance.uimanager.is_effectsound_on) // On이였음
        {
            effectsound_state.text = "OFF";
            effectsound_set_btn.GetComponent<Image>().color = Color.gray;
            Gamemanager.Instance.uimanager.is_effectsound_on = false;
        }
        else
        {
            effectsound_state.text = "ON";
            effectsound_set_btn.GetComponent<Image>().color = Color.white;
            Gamemanager.Instance.uimanager.is_effectsound_on = true;
        }
    }
}
