using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Vip : Floor
{
    public float vip_threshold = 0.5f;
    public override void Set(int floor_level, float min_respawntime = 10, float max_respawntime = 15, int max_wait_cnt = 5)
    {
        floor_color = FID.VIP;
        dirty_index = 0;
        is_cleaner_called = false;

        this.floor_level = floor_level;
        floor_level_text.text = floor_level.ToString() + "F";
        this.min_respawntime = min_respawntime;
        this.max_respawntime = max_respawntime;
        this.max_wait_cnt = max_wait_cnt;

        make_human_coroutine = StartCoroutine(MakeHuman());
    }
    public override IEnumerator MakeHuman()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(min_respawntime, max_respawntime) * Gamemanager.Instance.buildgame.night_regen);

            Gamemanager.Instance.objectpool.GetVip(vip_threshold).GetComponent<Human>().Set(this);
        }
    }
    public override IEnumerator MakeRush()
    {
        while (Gamemanager.Instance.buildgame.timer_arr[FID.VIP].rush_on)
        {
            yield return new WaitForSeconds(2 * Gamemanager.Instance.buildgame.night_regen);

            Gamemanager.Instance.objectpool.GetVip(vip_threshold).GetComponent<Human>().Set(this);
        }
    }
    public IEnumerator MakeHumanForGoHome()
    {
        while (Gamemanager.Instance.buildgame.is_go_to_home)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));

            Gamemanager.Instance.objectpool.GetVip(vip_threshold).GetComponent<Human>().Set(this);
        }
    }
    public override void SetDirtyRate(float dirty_size)
    {
        dirty_index += dirty_size;
        if (dirty_index > 100) dirty_index = 100;
        if (dirty_index < 0) dirty_index = 0;
        dirty_text.text = ((int)dirty_index).ToString();
        dirty_text_box.color = new Color(1, (100-dirty_index)/100, (100-dirty_index)/100);

        dirty_bar.transform.localScale = new Vector2(1, dirty_index / 100);
        if (dirty_index > Gamemanager.Instance.buildingmanager.cleaner_limit_size && !is_cleaner_called)
            StartCoroutine(CallClean());
    }
}