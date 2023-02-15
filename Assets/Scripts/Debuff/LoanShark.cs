using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanShark :  Debuff
{
    public string debuff_name { get; } = "사채업자";
    public string debuff_explain { get; } = "돈을 빌려가라 이말이야";
    public string debuff_effect { get; } = "3000G를 강제로 받습니다. 20일간 300골드씩 자동으로 갚습니다. 갚을 수 없다면 게임오버 됩니다.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buildgame.StartDebuffLoanShark();
    }
}
