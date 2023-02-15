using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff_Choice_UI : MonoBehaviour
{
    public int id;

    public Text buff_name;
    public Text buff_explain;
    public Text buff_effect;

    public void Setting(Buff buff, int id)
    {
        this.id = id;       
        buff_name.text = buff.buff_name;
        buff_explain.text = buff.buff_explain;
        buff_effect.text = buff.buff_effect;
    }
    public void Setting(Debuff debuff)
    {
        buff_name.text = debuff.debuff_name;
        buff_explain.text = debuff.debuff_explain;
        buff_effect.text = debuff.debuff_effect;
    }
}
