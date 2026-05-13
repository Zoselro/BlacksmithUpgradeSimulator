using UnityEngine;
using UnityEngine.Events;

public class UnityEventCall : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private UnityEvent onWelcome;
    [SerializeField] private UnityEvent playNewDay;
    [SerializeField] private UnityEvent onSettlementWindow;
    [SerializeField] private UnityEvent onShowContent;
    [SerializeField] private UnityEvent npcVisitPopup;
    public void WelcomeEvent()
    {
        onWelcome.Invoke();
    }

    public void PlayNpcVisitSequence()
    {
        uiManager.PlayWelcomeSequence(false);
    }

    public void PlayNewDay()
    {
        playNewDay.Invoke();
    }

    public void OnSettlementWindow()
    {
        onSettlementWindow.Invoke();
    }

    public void OnShowContent()
    {
        onShowContent.Invoke();
    }
    
    public void NpcVisit()
    {
        npcVisitPopup.Invoke();
    }
}
