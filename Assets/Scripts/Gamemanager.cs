using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager
{
    static Gamemanager instance = null;

    public BuildGame buildgame { get; private set; }
    public ObjectPool objectpool { get; private set; }
    public ElevatorManager elevatormanager { get; private set; }
    public EmployeeManager employeemanager { get; private set; }
    public BuffManager buffmanager { get; private set; }
    public UIManager uimanager { get; private set; }
    public BuildingManager buildingmanager { get; private set; }
    public static Gamemanager Instance
    {
        get
        {
            if (instance == null)
                instance = new Gamemanager();
            return instance;
        }
    }

    public void Set()
    {
        if (instance == null)
            instance = new Gamemanager();
        buildgame = Camera.main.GetComponent<BuildGame>();
        objectpool = Camera.main.GetComponent<ObjectPool>();
        elevatormanager = Camera.main.GetComponent<ElevatorManager>();
        employeemanager = Camera.main.GetComponent<EmployeeManager>();
        buffmanager = Camera.main.GetComponent<BuffManager>();
        uimanager = Camera.main.GetComponent<UIManager>();
        buildingmanager = Camera.main.GetComponent<BuildingManager>();
    }
}