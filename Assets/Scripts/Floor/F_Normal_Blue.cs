using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Normal_Blue : F_Normal
{
    float fat_threshold;
    public override void Set(int floor_level, float min_respawntime = 2, float max_respawntime = 5, int max_wait_cnt = 7)
    {
        floor_color = FID.BLUE;
        dirty_index = 0;
        is_cleaner_called = false;

        fat_threshold = 0.2f;

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

            Gamemanager.Instance.objectpool.GetBlue(fat_threshold).GetComponent<Human>().Set(this);
        }
    }
    public override IEnumerator MakeRush()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 3f));

        Gamemanager.Instance.objectpool.GetBlue(fat_threshold).GetComponent<Human>().Set(this);

        while (Gamemanager.Instance.buildgame.timer_arr[FID.BLUE].rush_on)
        {
            yield return new WaitForSeconds(Random.Range(min_respawntime, max_respawntime) * Gamemanager.Instance.buildgame.night_regen);

            Gamemanager.Instance.objectpool.GetBlue(fat_threshold).GetComponent<Human>().Set(this);
        }
    }
    public IEnumerator MakeHumanForGoHome()
    {
        while (Gamemanager.Instance.buildgame.is_go_to_home)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));

            Gamemanager.Instance.objectpool.GetBlue(fat_threshold).GetComponent<Human>().Set(this);
        }
    }
}
