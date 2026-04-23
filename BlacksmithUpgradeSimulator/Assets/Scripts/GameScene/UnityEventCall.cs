using UnityEngine;
using UnityEngine.Events;

public class UnityEventCall : MonoBehaviour
{   
    [SerializeField] private UnityEvent onWelcome;
    [SerializeField] private UnityEvent inFadeBackGround;
    [SerializeField] private UnityEvent playNpcVistSequence;
    [SerializeField] private UnityEvent playNewDay;
    public void WelcomeEvent()
    {
        onWelcome.Invoke();
    }

    public void FadeInBackground()
    {
        inFadeBackGround.Invoke();
    }

    public void PlayNpcVisitSequence()
    {
        playNpcVistSequence.Invoke();
    }

    public void PlayNewDay()
    {
        playNewDay.Invoke();
    }
}
