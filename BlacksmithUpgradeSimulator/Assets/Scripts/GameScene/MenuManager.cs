using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    public void ShowMenu(bool active)
    {
        menuPanel.SetActive(active);
        gm.TryEnhance(active); // 메뉴가 활성화될 때 게임 매니저에게 일시정지를 요청하는 메서드 호출
    }

    public void OnContinueButton()
    {
        ShowMenu(false);
    }

    public void OnSettingButton()
    {
        // 설정 메뉴로 이동하는 로직을 여기에 작성
        Debug.Log("설정 버튼이 클릭되었습니다.");
    }

    public void OnExitButton()
    {
        // 게임 종료 로직을 여기에 작성
        Debug.Log("게임이 종료됩니다.");
        SceneManager.LoadScene("GameStart");
    }
}
