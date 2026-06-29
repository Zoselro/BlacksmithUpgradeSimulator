using TMPro;
using UnityEngine;

public class StartSceneUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;

    public void SetVersionTest(string text)
    {
        versionText.text = $"ºôµå ¹öÀü : {text}";
    }
}
