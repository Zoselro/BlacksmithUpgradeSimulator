using UnityEngine;
using UnityEngine.Events;

public class ObjectAnimation : MonoBehaviour
{   
    [SerializeField] private UnityEvent onWelcome;
    [SerializeField] private UnityEvent onFadeIn;
    [SerializeField] private UnityEvent onFadeOut;
    public void WelcomeEvent()
    {
        onWelcome.Invoke();
    }

    public void ShowFadeIOutImage()
    {
        onFadeIn.Invoke();
    }

    public void ShowFadeInImage()
    {
        onFadeOut.Invoke();
    }
}
