using UnityEngine;
using UnityEngine.Events;

public class UnityEventCall : MonoBehaviour
{   
    [SerializeField] private UnityEvent onWelcome;
    [SerializeField] private UnityEvent inFadeBackGround;
    public void WelcomeEvent()
    {
        onWelcome.Invoke();
    }

    public void FadeInBackground()
    {
        inFadeBackGround.Invoke();
    }
}
