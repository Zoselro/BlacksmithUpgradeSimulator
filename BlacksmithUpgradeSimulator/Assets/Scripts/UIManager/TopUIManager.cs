using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopUIManager : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private TextMeshProUGUI npcVisitCount;
    [SerializeField] private TextMeshProUGUI progressDaysText;
    [SerializeField] private TextMeshProUGUI dayOfWeekText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject topUIPanel;
    public void TopBarDisPlay()
    {
        npcVisitCount.text = "¹æ¹® ¼ö : " + gm.Visitors + "¸í";
        dayOfWeekText.text = gm.Weekday;
        progressDaysText.text = gm.Days + " ÀÏÂ÷";
        goldText.text = "°ñµå : " + gm.Gold.ToString("N0");
    }
    public void ResetData()
    {
        goldText.text = "°ñµå : 0";
    }

    public void SetGoldText(int gold)
    {
        if(gold < 0)
        {
            gold = 0;
        }
        goldText.text = $"°ñµå : {gold}";
    }

    public void ActiveTopUIPanel(bool active)
    {
        topUIPanel.gameObject.SetActive(active);
    }
}
