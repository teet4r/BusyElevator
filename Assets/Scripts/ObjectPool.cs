using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NormalHumanColor
{
    public const int INDEX_BEGIN = 0;

    public const int RED = 0;
    public const int BLUE = 1;
    public const int GREEN = 2;
    public const int YELLOW = 3;

    public const int INDEX_END = 3;
}

[System.Serializable]
public class SickSprite
{
    public Sprite[] sprite;
}

// *******************************************
// *         attached in Main Camera         *
// *******************************************
public class ObjectPool : MonoBehaviour
{
    // ÀÎ½ºÆåÅÍ¿¡¼­¸¸ º¸ÀÓ
    [SerializeField]
    private GameObject[] origin_normal;
    [SerializeField]
    private GameObject[] origin_fat;
    [SerializeField]
    private GameObject origin_vip_m;
    [SerializeField]
    private GameObject origin_vip_f;
    [SerializeField]
    private GameObject origin_master;
    [SerializeField]
    private GameObject origin_cleaner;
    [SerializeField]
    private GameObject origin_thief;
    [SerializeField]
    private GameObject origin_mechanic;
    [SerializeField]
    private GameObject origin_sick;
    [SerializeField]
    private GameObject origin_guard;
    [SerializeField]
    private GameObject origin_delivery;
    [SerializeField]
    private GameObject origin_sanitation;
    [SerializeField]
    private GameObject origin_homeless;

    // È°¼ºÈ­µÇÁö ¾ÊÀº °´Ã¼µéÀ» ÀúÀå
    List<Queue<GameObject>> normal_pool;
    List<Queue<GameObject>> fat_pool;
    Queue<GameObject> vip_m_pool;
    Queue<GameObject> vip_f_pool;
    Queue<GameObject> master_pool;
    Queue<GameObject> cleaner_pool;
    Queue<GameObject> thief_pool;
    Queue<GameObject> mechanic_pool;
    Queue<GameObject> sick_pool;
    Queue<GameObject> guard_pool;
    Queue<GameObject> delivery_pool;
    Queue<GameObject> sanitation_pool;
    Queue<GameObject> homeless_pool;

    // ºñÈ°¼ºÈ­µÈ °´Ã¼µé ¿©±â¿¡ ´ë±â
    public GameObject waiting_room;

    // 2Â÷¿ø ¹è¿­
    public SickSprite[] sick_sprites;
    // Red
    // Blue
    // Green
    // Yellow
    // vip
    //------------------------------------
    // ¸ðÈ÷Ä­
    // ¸ðÈ÷Ä­ µÅÁö
    // °¡¸£¸¶
    // °¡¸£¸¶ µÅÁö
    // °«ÆÄ
    // °«ÆÄ µÅÁö
    // ¹Ù°¡Áö
    // ¹Ù°¡Áö µÅÁö
    // ·ÕÇì¾î 
    // ·ÕÇì¾î µÅÁö
    // ¼ôÇì¾î
    // ¼ôÇì¾î µÅÁö
    // ¸Ó¸®¶ì
    // ¸Ó¸®¶ì µÅÁö
    // vip³²¼º
    // vip¿©¼º

    protected void Awake()
    {
        normal_pool = new List<Queue<GameObject>>();
        for (int i = NormalHumanColor.INDEX_BEGIN; i <= NormalHumanColor.INDEX_END; i++)
            normal_pool.Add(new Queue<GameObject>());

        fat_pool = new List<Queue<GameObject>>();
        for (int i = NormalHumanColor.INDEX_BEGIN; i <= NormalHumanColor.INDEX_END; i++)
            fat_pool.Add(new Queue<GameObject>());

        vip_m_pool      = new Queue<GameObject>();
        vip_f_pool      = new Queue<GameObject>();
        master_pool     = new Queue<GameObject>();
        cleaner_pool    = new Queue<GameObject>();
        thief_pool      = new Queue<GameObject>();
        mechanic_pool   = new Queue<GameObject>();
        sick_pool       = new Queue<GameObject>();
        guard_pool      = new Queue<GameObject>();
        delivery_pool   = new Queue<GameObject>();
        sanitation_pool = new Queue<GameObject>();
        homeless_pool   = new Queue<GameObject>();
    }
    public GameObject GetNormal(int floor_color, float fat_threshold) // 1Ãþ¿ë
    {
        GameObject obj;

        if (Random.Range(0, 1f) > fat_threshold + Gamemanager.Instance.buffmanager.fat_human_appear_rate) // Normal
        {
            if (normal_pool[floor_color].Count == 0)
                return Instantiate(origin_normal[floor_color]);
            obj = normal_pool[floor_color].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            if (fat_pool[floor_color].Count == 0)
                return Instantiate(origin_fat[floor_color]);
            obj = fat_pool[floor_color].Dequeue();
            obj.SetActive(true);
        }
        return obj;
    }
    public GameObject GetRed(float fat_threshold)
    {
        GameObject obj;
        if (Random.Range(0, 1f) > fat_threshold + Gamemanager.Instance.buffmanager.fat_human_appear_rate) // Normal
        {
            if (normal_pool[NormalHumanColor.RED].Count == 0)
                return Instantiate(origin_normal[NormalHumanColor.RED]);
            obj = normal_pool[NormalHumanColor.RED].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else // Fat
        {
            if (fat_pool[NormalHumanColor.RED].Count == 0)
                return Instantiate(origin_fat[NormalHumanColor.RED]);
            obj = fat_pool[NormalHumanColor.RED].Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }
    public GameObject GetBlue(float fat_threshold)
    {
        GameObject obj;
        if (Random.Range(0, 1f) > fat_threshold + Gamemanager.Instance.buffmanager.fat_human_appear_rate) // Normal
        {
            if (normal_pool[NormalHumanColor.BLUE].Count == 0)
                return Instantiate(origin_normal[NormalHumanColor.BLUE]);
            obj = normal_pool[NormalHumanColor.BLUE].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else // Fat
        {
            if (fat_pool[NormalHumanColor.BLUE].Count == 0)
                return Instantiate(origin_fat[NormalHumanColor.BLUE]);
            obj = fat_pool[NormalHumanColor.BLUE].Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }
    public GameObject GetGreen(float fat_threshold)
    {
        GameObject obj;
        if (Random.Range(0, 1f) > fat_threshold + Gamemanager.Instance.buffmanager.fat_human_appear_rate) // Normal
        {
            if (normal_pool[NormalHumanColor.GREEN].Count == 0)
                return Instantiate(origin_normal[NormalHumanColor.GREEN]);
            obj = normal_pool[NormalHumanColor.GREEN].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else // Fat
        {
            if (fat_pool[NormalHumanColor.GREEN].Count == 0)
                return Instantiate(origin_fat[NormalHumanColor.GREEN]);
            obj = fat_pool[NormalHumanColor.GREEN].Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }
    public GameObject GetYellow(float fat_threshold)
    {
        GameObject obj;
        if (Random.Range(0, 1f) > fat_threshold + Gamemanager.Instance.buffmanager.fat_human_appear_rate) // Normal
        {
            if (normal_pool[NormalHumanColor.YELLOW].Count == 0)
                return Instantiate(origin_normal[NormalHumanColor.YELLOW]);
            obj = normal_pool[NormalHumanColor.YELLOW].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else // Fat
        {
            if (fat_pool[NormalHumanColor.YELLOW].Count == 0)
                return Instantiate(origin_fat[NormalHumanColor.YELLOW]);
            obj = fat_pool[NormalHumanColor.YELLOW].Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }
    public GameObject GetVip(float vip_threshold)
    {
        GameObject obj;
        if (Random.Range(0, 1f) > vip_threshold) // Male
        {
            if (vip_m_pool.Count == 0)
                return Instantiate(origin_vip_m);
            obj = vip_m_pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else // Female
        {
            if (vip_f_pool.Count == 0)
                return Instantiate(origin_vip_f);
            obj = vip_f_pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
    }
    public GameObject GetMaster()
    {
        if (master_pool.Count == 0)
            return Instantiate(origin_master);
        GameObject obj = master_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetCleaner()
    {
        if (cleaner_pool.Count == 0)
            return Instantiate(origin_cleaner);
        GameObject obj = cleaner_pool.Dequeue();
        // SetActive ¾ø¾î¾ß´ï
        return obj;
    }
    public GameObject GetThief()
    {
        if (thief_pool.Count == 0)
            return Instantiate(origin_thief);
        GameObject obj = thief_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetMechanic()
    {
        if (mechanic_pool.Count == 0)
            return Instantiate(origin_mechanic);
        GameObject obj = mechanic_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetSick()
    {
        if (sick_pool.Count == 0)
        {
            Gamemanager.Instance.uimanager.ShowExplain(ref Gamemanager.Instance.buildgame.is_sick_appear, PID.SICK);
            return Instantiate(origin_sick);
        }
        GameObject obj = sick_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetGuard()
    {
        if (guard_pool.Count == 0)
            return Instantiate(origin_guard);
        GameObject obj = guard_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetDelivery()
    {
        if (delivery_pool.Count == 0)
            return Instantiate(origin_delivery);
        GameObject obj = delivery_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetSanitation()
    {
        if (sanitation_pool.Count == 0)
            return Instantiate(origin_sanitation);
        GameObject obj = sanitation_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    public GameObject GetHomeless()
    {
        if (homeless_pool.Count == 0)
            return Instantiate(origin_homeless);
        GameObject obj = homeless_pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnNormalRed(H_Normal_Red normal_red)
    {
        if (normal_red == null)
            return;

        normal_red.gameObject.SetActive(false);
        normal_red.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.RED].Enqueue(normal_red.gameObject);
    }
    public void ReturnNormalBlue(H_Normal_Blue normal_blue)
    {
        if (normal_blue == null)
            return;

        normal_blue.gameObject.SetActive(false);
        normal_blue.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.BLUE].Enqueue(normal_blue.gameObject);
    }
    public void ReturnNormalGreen(H_Normal_Green normal_green)
    {
        if (normal_green == null)
            return;

        normal_green.gameObject.SetActive(false);
        normal_green.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.GREEN].Enqueue(normal_green.gameObject);
    }
    public void ReturnNormalYellow(H_Normal_Yellow normal_yellow)
    {
        if (normal_yellow == null)
            return;

        normal_yellow.gameObject.SetActive(false);
        normal_yellow.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.YELLOW].Enqueue(normal_yellow.gameObject);
    }
    public void ReturnFatRed(H_Fat_Red fat_red)
    {
        if (fat_red == null)
            return;

        fat_red.gameObject.SetActive(false);
        fat_red.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.RED].Enqueue(fat_red.gameObject);
    }
    public void ReturnFatBlue(H_Fat_Blue fat_blue)
    {
        if (fat_blue == null)
            return;

        fat_blue.gameObject.SetActive(false);
        fat_blue.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.BLUE].Enqueue(fat_blue.gameObject);
    }
    public void ReturnFatGreen(H_Fat_Green fat_green)
    {
        if (fat_green == null)
            return;

        fat_green.gameObject.SetActive(false);
        fat_green.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.GREEN].Enqueue(fat_green.gameObject);
    }
    public void ReturnFatYellow(H_Fat_Yellow fat_yellow)
    {
        if (fat_yellow == null)
            return;

        fat_yellow.gameObject.SetActive(false);
        fat_yellow.transform.position = waiting_room.transform.position;
        normal_pool[NormalHumanColor.YELLOW].Enqueue(fat_yellow.gameObject);
    }
    public void ReturnVIPM(H_VIP_M VIP_M)
    {
        if (VIP_M == null)
            return;

        VIP_M.gameObject.SetActive(false);
        VIP_M.transform.position = waiting_room.transform.position;
        vip_m_pool.Enqueue(VIP_M.gameObject);
    }
    public void ReturnVIPF(H_VIP_F VIP_F)
    {
        if (VIP_F == null)
            return;

        VIP_F.gameObject.SetActive(false);
        VIP_F.transform.position = waiting_room.transform.position;
        vip_f_pool.Enqueue(VIP_F.gameObject);
    }
    public void ReturnMaster(H_Master master)
    {
        if (master == null)
            return;

        master.gameObject.SetActive(false);
        master.transform.position = waiting_room.transform.position;
        master_pool.Enqueue(master.gameObject);
    }
    public void ReturnCleaner(H_Cleaner cleaner)
    {
        if (cleaner == null)
            return;

        cleaner.gameObject.SetActive(false);
        cleaner.transform.position = waiting_room.transform.position;
        cleaner_pool.Enqueue(cleaner.gameObject);
    }
    public void ReturnThief(H_Thief thief)
    {
        if (thief == null)
            return;

        thief.gameObject.SetActive(false);
        thief.transform.position = waiting_room.transform.position;
        thief_pool.Enqueue(thief.gameObject);
    }
    public void ReturnMechanic(H_Mechanic mechanic)
    {
        if (mechanic == null)
            return;

        mechanic.gameObject.SetActive(false);
        mechanic.transform.position = waiting_room.transform.position;
        mechanic_pool.Enqueue(mechanic.gameObject);
    }
    public void ReturnSick(H_Sick sick)
    {
        if (sick == null)
            return;

        sick.gameObject.SetActive(false);
        sick.transform.position = waiting_room.transform.position;
        sick_pool.Enqueue(sick.gameObject);
    }
    public void ReturnGuard(H_Guard guard)
    {
        if (guard == null)
            return;

        guard.gameObject.SetActive(false);
        guard.transform.position = waiting_room.transform.position;
        guard_pool.Enqueue(guard.gameObject);
    }
    public void ReturnDelivery(H_Delivery delivery)
    {
        if (delivery == null)
            return;

        delivery.gameObject.SetActive(false);
        delivery.transform.position = waiting_room.transform.position;
        delivery_pool.Enqueue(delivery.gameObject);
    }
    public void ReturnSanitation(H_Sanitation sanitation)
    {
        if (sanitation == null)
            return;

        sanitation.gameObject.SetActive(false);
        sanitation.transform.position = waiting_room.transform.position;
        sanitation_pool.Enqueue(sanitation.gameObject);
    }
    public void ReturnHomeless(H_Homeless homeless)
    {
        if (homeless == null)
            return;

        homeless.gameObject.SetActive(false);
        homeless.transform.position = waiting_room.transform.position;
        homeless_pool.Enqueue(homeless.gameObject);
    }

    public void DestroyAllObjects()
    {
        GameObject e;
        for (int color = NormalHumanColor.INDEX_BEGIN; color <= NormalHumanColor.INDEX_END; color++)
        {
            while (normal_pool[color].Count != 0)
            {
                e = normal_pool[color].Dequeue();
                DestroyImmediate(e);
            }
            while (fat_pool[color].Count != 0)
            {
                e = fat_pool[color].Dequeue();
                DestroyImmediate(e);
            }
        }
        while (vip_m_pool.Count != 0)
        {
            e = vip_m_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (vip_f_pool.Count != 0)
        {
            e = vip_f_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (master_pool.Count != 0)
        {
            e = master_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (cleaner_pool.Count != 0)
        {
            e = cleaner_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (thief_pool.Count != 0)
        {
            e = thief_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (mechanic_pool.Count != 0)
        {
            e = mechanic_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (sick_pool.Count != 0)
        {
            e = sick_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (guard_pool.Count != 0)
        {
            e = guard_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (delivery_pool.Count != 0)
        {
            e = delivery_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (sanitation_pool.Count != 0)
        {
            e = sanitation_pool.Dequeue();
            DestroyImmediate(e);
        }
        while (homeless_pool.Count != 0)
        {
            e = homeless_pool.Dequeue();
            DestroyImmediate(e);
        }
    }
}
