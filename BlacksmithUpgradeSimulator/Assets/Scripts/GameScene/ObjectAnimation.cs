using UnityEngine;
using UnityEngine.Events;

public class ObjectAnimation : MonoBehaviour
{   
    [SerializeField] private UnityEvent onWelcome;
    [SerializeField] private UnityEvent onFadeOut;
    [SerializeField] private UnityEvent onFadeIn;
    public void WelcomeEvent()
    {
        onWelcome.Invoke();
    }

    public void ShowFadeIOutImage()
    {
        onFadeOut.Invoke();
    }

    public void ShowFadeInImage()
    {
        onFadeIn.Invoke();
    }
}
