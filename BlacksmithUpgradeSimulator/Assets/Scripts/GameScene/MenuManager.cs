using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;

    public void ShowMenu(bool active)
    {
        menuPanel.SetActive(active);
    }
}
