using TMPro;
using UnityEngine;

public class TestVersionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    string version;
    private void Start()
    {
        SoundManager.Inst.PlayBGM(EBgm.Title_music_Ver2);
        version = Application.version;
        versionText.text = "Ver." + version;
    }
}
