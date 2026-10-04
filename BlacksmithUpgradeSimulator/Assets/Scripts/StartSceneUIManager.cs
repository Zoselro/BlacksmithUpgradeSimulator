using TMPro;
using UnityEngine;

public class StartSceneUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;

    public void SetVersionTest(string text)
    {
        versionText.text = $"빌드 버전 : {text}";
    }
}
