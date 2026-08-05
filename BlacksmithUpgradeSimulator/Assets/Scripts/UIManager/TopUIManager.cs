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
        npcVisitCount.text = "방문 수 : " + gm.GetVisitors() + "명";
        dayOfWeekText.text = gm.Weekday;
        progressDaysText.text = gm.GetDay() + " 일차";
        goldText.text = gm.GetGold().ToString("N0");
    }
    public void ResetData()
    {
        goldText.text = 0.ToString();
    }

    public void SetGoldText(int gold)
    {
        if(gold < 0)
        {
            gold = 0;
        }
        goldText.text = gold.ToString("N0");
    }

    public void ActiveTopUIPanel(bool active)
    {
        topUIPanel.gameObject.SetActive(active);
    }
}
