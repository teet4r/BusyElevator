using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Clean :  Floor
{
    public int cleaners_destination;    // cleaner°¡ °¡·Á´Â ÃþÀÇ µµÂøÁö
    public override void Set(int floor_level, float min_respawntime = 3, float max_respawntime = 4, int max_wait_cnt = 5)
    {
        floor_color = FID.CLEAN;
        dirty_index = 0;
        is_cleaner_called = false;

        this.floor_level = floor_level;
        floor_level_text.text = floor_level.ToString() + "F";
        this.min_respawntime = min_respawntime;
        this.max_respawntime = max_respawntime;
        this.max_wait_cnt = max_wait_cnt;
    }

    public void Call_Clean(int destination)
    {
        cleaners_destination = destination;
        var human = Gamemanager.Instance.employeemanager.GetCleaner();

        if(human)
        {
            Gamemanager.Instance.buildgame.floor_of[destination].is_cleaner_called = true;
            human.Set(this);
        }
    }
}
