using UnityEngine;
using UnityEngine.UI;

public class ExplainPanel : MonoBehaviour
{
    [SerializeField]
    Button OK;
    [SerializeField]
    Text explain_name;
    [SerializeField]
    Text explain_explain;
    [SerializeField]
    Text explain_subexplain;

    public GameObject panel { get; private set; }

    public void Set(string[] explain)
    {
        explain_name.text = explain[0];
        explain_explain.text = explain[1];
        explain_subexplain.text = explain[2];

        panel = gameObject;
    }
    public void TurnOffPanel()
    {
        Gamemanager.Instance.uimanager.TryTimeRestart();

        Destroy(panel);
    }
}
