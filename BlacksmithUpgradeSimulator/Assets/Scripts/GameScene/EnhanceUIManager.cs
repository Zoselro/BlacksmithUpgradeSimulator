using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceUIManager : MonoBehaviour
{
    [Header("EnhanceActivePanel")]
    [SerializeField] private GameObject enhanceActivePanel;
    [SerializeField] private Button weaponInfoBtn;
    public Button WeaponInfoBtn => weaponInfoBtn;
    [SerializeField] private Button enhanceBtn;
    public Button EnhanceBtn => enhanceBtn;

    [Header("WeaponDataPanel")]
    [SerializeField] private GameObject weaponDataPanel;
    [SerializeField] private Image weaponImg;
    [SerializeField] private TextMeshProUGUI weaponTypeText;
    [SerializeField] private TextMeshProUGUI weaponRankText;
    [SerializeField] private TextMeshProUGUI weaponLevelText;
    [SerializeField] private Button confirmButton;

    [Header("MiniGame")]
    [SerializeField] private GameObject MiniGame;
    [SerializeField] private GameObject enhanceProgressBar;
    [SerializeField] private RectTransform enhanceProgressBarRect;

    [Header("EnhanceProbability")]
    [SerializeField] private GameObject enhanceProbabilityUI;
    [SerializeField] private TextMeshProUGUI enhanceProbability;

    public void OnClickActiveEnhanceActivePanel(bool active)
    {
        enhanceActivePanel.gameObject.SetActive(active);
        weaponDataPanel.gameObject.SetActive(!active);
    }

    public void EnhanceActivePanel(bool active)
    {
        enhanceActivePanel.gameObject.SetActive(active);
    }

    public void Initialized(int weaponLevel, Sprite weaponImg, string type, string rank)
    {
        this.weaponImg.sprite = weaponImg;
        weaponTypeText.text = "무기 등급 : " + rank;
        weaponRankText.text = "무기 종류 : " + type;
        weaponLevelText.text = "강화 단계 : + " + weaponLevel;
    }

    public void SetActiveMiniGame(bool active)
    {
        MiniGame.SetActive(active);
    }

    public void EnhanceProgressBar(float progress)
    {
        enhanceProgressBarRect.localScale = new Vector3(progress, 1f, 1f);
    }

    public void ActiveEnhanceUIBar(bool active, string probability)
    {
        enhanceProgressBar.gameObject.SetActive(active);
        enhanceProbabilityUI.gameObject.SetActive(active);

        enhanceProbability.text = $"성공 확률 : {probability.ToString()}%";
    }

    public void ActiveEnhanceUIBar(bool active)
    {
        enhanceProgressBar.gameObject.SetActive(active);
        enhanceProbabilityUI.gameObject.SetActive(active);
    }

    public void ActiveConfirmButton(bool active)
    {
        confirmButton.gameObject.SetActive(active);
    }
}
