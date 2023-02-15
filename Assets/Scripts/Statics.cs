using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FID // Floor ID
{
    public const int INDEX_BEGIN = 0;

    public const int RED        = 0;
    public const int BLUE       = 1;
    public const int GREEN      = 2;
    public const int YELLOW     = 3;
    public const int VIP        = 4;
    public const int FIRST      = 5;
    public const int BANK       = 6;
    public const int REST       = 7;
    public const int MASTER     = 8;
    public const int HOSPITAL   = 9;
    public const int CLEAN      = 10;
    public const int REPAIRSHOP = 11;
    public const int CONVENIENCE = 12;

    public const int INDEX_END = 12;
}

public static class PID
{
    public const int INDEX_BEGIN = 0;

    public const int THIEF      = 0;
    public const int GUARD      = 1;
    public const int SICK       = 2;
    public const int SANITATION = 3;
    public const int HOMELESS   = 4;
    public const int DELIVERY   = 5;

    public const int INDEX_END = 5;
}

public static class Bubble
{
    public const int INDEX_BEGIN = 0;

    public const int EMPTY      = 0;
    public const int PANIC      = 1;
    public const int ARREST     = 2;
    public const int EXCITED    = 3;
    public const int CHECK1     = 4;
    public const int CHECK2     = 5;
    public const int CHECK3     = 6;
    public const int ANGRY      = 7;

    public const int INDEX_END = 7;
}

public static class HumanState
{
    public const int WAITING    = 0;
    public const int BOARDING   = 1; // 엘리베이터 탑승 중
    public const int WALKING    = 2; // 내려서 사라질 때까지
}
