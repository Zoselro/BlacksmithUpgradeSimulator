using TMPro;
using UnityEngine;

public class TestVersionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    string version;
    private void Start()
    {
        version = Application.version;
        versionText.text = "Ver." + version;
    }
}
