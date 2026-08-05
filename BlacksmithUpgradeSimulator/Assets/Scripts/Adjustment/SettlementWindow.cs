using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettlementWindow : MonoBehaviour
{

    [SerializeField] private GameDataManager gameDataManager;
    [SerializeField] private TextMeshProUGUI dayCnt;
    [SerializeField] private TextMeshProUGUI successCnt;
    [SerializeField] private TextMeshProUGUI greatSuccessCnt;
    [SerializeField] private TextMeshProUGUI failCnt;
    [SerializeField] private TextMeshProUGUI goldCnt;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject settlementWindowObj;
    [SerializeField] private GameObject settlementWindow;
    [SerializeField] private GameObject startNextDayTextObj;
    [SerializeField] private TextMeshProUGUI startNextDayText;

    public void SetupSettlementUI()
    {
        dayCnt.text = gameDataManager.GetDay().ToString() + " 일차 정산 일지";
        successCnt.text = gameDataManager.GetCurrentSuccessCount().ToString();
        greatSuccessCnt.text = gameDataManager.GetCurrentGreatSuccessCount().ToString();
        failCnt.text = gameDataManager.GetCurrentFailCount().ToString();
        goldCnt.text = gameDataManager.GetCurrentGold().ToString();
    }

    public void StartNextDayText(string startNextDayText)
    {
        this.startNextDayText.text = startNextDayText;
    }

    //public void FadeAnimatorSpeed(float speed)
    //{
    //    fadeAnimator.speed = speed;
    //}
}
