using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private EnhanceManager enhanceManager;


    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI menuText;

    private bool wasEnhancingBeforeMenu; // 메뉴가 열리기 전에 강화 중이었는지 여부를 저장하는 변수

    public void ShowMenu(bool active)
    {
        menuText.text = "메뉴";
        menuPanel.SetActive(active); // 메뉴 패널의 활성화 여부 설정
        optionUI.SetActive(!active); // 옵션 UI는 처음에 비활성화
        menuUI.SetActive(active); // 메뉴 UI는 활성화 여부에 따라 설정
        gm.TryAnimation(active); // 메뉴가 활성화될 때 게임 매니저에게 일시정지를 요청하는 메서드 호출

        if (active)
        {
            wasEnhancingBeforeMenu = enhanceManager.IsEnhancing; // 메뉴가 열리기 전에 강화 중이었는지 여부 저장
            Debug.Log($"메뉴 활성화: wasEnhancingBeforeMenu = {wasEnhancingBeforeMenu}");
            if (wasEnhancingBeforeMenu)
            {
                gm.TryEnhance(true); // 메뉴가 활성화될 때 게임 매니저에게 일시정지를 요청하는 메서드 호출
            }
        }
        else
        {
            if (wasEnhancingBeforeMenu)
            {
                gm.TryEnhance(false); // 메뉴가 비활성화될 때 게임 매니저에게 일시정지를 해제하는 메서드 호출
            }
        }
    }

    public void OnContinueButton()
    {
        ShowMenu(false);
    }

    public void OnSettingButton()
    {
        menuText.text = "설정";
        optionUI.SetActive(true);
        menuUI.SetActive(false);
        Debug.Log("설정 버튼이 클릭되었습니다.");
    }

    public void OnExitButton()
    {
        // 게임 종료 로직을 여기에 작성
        Debug.Log("게임이 종료됩니다.");
        SceneManager.LoadScene("GameStart");
    }

    public void SettingComplete(bool active)
    {
        menuPanel.SetActive(active);
        gm.TryAnimation(active); // 메뉴가 비활성화될 때 게임 매니저에게 애니메이션을 재개하는 메서드 호출

        if (active)
        {
            wasEnhancingBeforeMenu = enhanceManager.IsEnhancing; // 메뉴가 열리기 전에 강화 중이었는지 여부 저장
            Debug.Log($"메뉴 활성화: wasEnhancingBeforeMenu = {wasEnhancingBeforeMenu}");
            if (wasEnhancingBeforeMenu)
            {
                gm.TryEnhance(true); // 메뉴가 활성화될 때 게임 매니저에게 일시정지를 요청하는 메서드 호출
            }
        }
        else
        {
            if (wasEnhancingBeforeMenu)
            {
                gm.TryEnhance(false); // 메뉴가 비활성화될 때 게임 매니저에게 일시정지를 해제하는 메서드 호출
            }
        }
        Debug.Log("설정이 완료되었습니다.");
    }
}
