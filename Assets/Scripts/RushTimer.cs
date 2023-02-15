using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RushTimer : MonoBehaviour
{
    public Text timer_text;
    public bool rush_on;
    public Image clock;
    public int color;

    public void Start()
    {
        GetComponent<Image>().color = Color.gray;
    }
    public void StartTimer()
    {
        GetComponent<Image>().color = Color.white;
        timer_text.color = Color.white;
        StartCoroutine(FloorTimer());
    }

    IEnumerator FloorTimer()
    {
        int random_timer;
        while (true)
        {
            int t;
            random_timer = (int)(Random.Range(50, 60) * (1 + Gamemanager.Instance.buffmanager.rush_distance_size));
            for (t = 0; t < random_timer - 3; t++)
            {
                timer_text.text = (random_timer - t).ToString();
                yield return new WaitForSeconds(1f);
            }
            for (; t < random_timer; t++)
            {
                timer_text.text = (random_timer - t).ToString();
                timer_text.fontSize += 5;
                timer_text.color = Color.red;
                yield return new WaitForSeconds(1f);
            }

            rush_on = true;
            timer_text.text = "!";
            StartCoroutine(ExpandtionContraction());
            StartCoroutine(MoveClock());

            foreach (Floor floor in Gamemanager.Instance.buildgame.floors[color])
                for (int i = 0; i < 3; i++)
                    StartCoroutine(floor.MakeRush());

            yield return new WaitForSeconds(Gamemanager.Instance.buildgame.one_hour * 5 * (1 + Gamemanager.Instance.buffmanager.rush_time_size));

            rush_on = false;
            timer_text.color = Color.white;
            timer_text.fontSize = 60;
        }
    }
    IEnumerator ExpandtionContraction() // 글자 확장 수축 반복
    {
        while (rush_on)
        {
            for (int i = 0; i < 5; i++)
            {
                timer_text.fontSize += 20;
                yield return new WaitForSeconds(0.05f);
            }
            for (int i = 0; i < 5; i++)
            {
                timer_text.fontSize -= 20;
                yield return new WaitForSeconds(0.05f);
            }
        }
        timer_text.fontSize = 60;
    }
    IEnumerator MoveClock()
    {
        while (rush_on)
        {
            clock.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15f, 15f)));
            yield return new WaitForSeconds(0.1f);
        }

        clock.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
