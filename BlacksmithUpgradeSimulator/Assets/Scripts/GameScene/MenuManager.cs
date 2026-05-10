using UnityEngine;
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
        TryEnhance(); // 메뉴가 활성화될 때 게임 매니저에게 일시정지를 요청하는 메서드 호출
    }

    public void TryEnhance()
    {
        if (gm == null)
            return;

        // 게임 매니저에게 일시정지를 요청

    }
}
