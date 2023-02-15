using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Repairshop : Floor
{
    public List<bool> is_line_repair_using; // 각 라인에서 동시에 수리 할 수 있는 엘리베이터는 각 한 대임
                                            // 해당 라인이 수리 중이라면 true

    public List<GameObject> repair_point; // 각 라인당 정비 위치
    public override void Set(int floor_level, float min_respawntime = 10, float max_respawntime = 15, int max_wait_cnt = 5)
    {
        floor_color = FID.REPAIRSHOP;
        dirty_index = 0;
        is_cleaner_called = false;

        is_line_repair_using = new List<bool>(3) { false, false, false };

        this.floor_level = floor_level;
        this.min_respawntime = min_respawntime;
        this.max_respawntime = max_respawntime;
        this.max_wait_cnt = max_wait_cnt;
    }
    public void Call_Mechanic(ElevatorClass elevator, H_Mechanic mechanic_script) // 수리해야하는 엘리베이터의 객체
    {
        mechanic_script.elevator = elevator;
        mechanic_script.gameObject.SetActive(true);
        mechanic_script.Set(this);
    }
}
