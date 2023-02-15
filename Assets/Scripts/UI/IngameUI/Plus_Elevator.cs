using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus_Elevator : MonoBehaviour
{
    public int idx;
    public void Push()
    {
        Gamemanager.Instance.elevatormanager.MakeElevator(idx);

        transform.parent.gameObject.SetActive(false);

        Gamemanager.Instance.uimanager.TryTimeRestart();
    }
}
