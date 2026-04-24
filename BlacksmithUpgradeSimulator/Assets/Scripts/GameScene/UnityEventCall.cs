using UnityEngine;
using UnityEngine.Events;

public class UnityEventCall : MonoBehaviour
{   
    [SerializeField] private UnityEvent onWelcome;
    [SerializeField] private UnityEvent playNpcVistSequence;
    [SerializeField] private UnityEvent playNewDay;
    [SerializeField] private UnityEvent onSettlementWindow;
    [SerializeField] private UnityEvent onShowContent;
    public void WelcomeEvent()
    {
        onWelcome.Invoke();
    }

    public void PlayNpcVisitSequence()
    {
        playNpcVistSequence.Invoke();
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
}
