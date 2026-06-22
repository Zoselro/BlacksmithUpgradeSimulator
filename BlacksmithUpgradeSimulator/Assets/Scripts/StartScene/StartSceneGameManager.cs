using TMPro;
using UnityEngine;

public class StartSceneGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    string version;
    private void Start()
    {
        SoundManager.Inst.PlayBGM(EBgm.Title_music_Ver1);
        version = Application.version;
        versionText.text = "Ver." + version;
    }
}
