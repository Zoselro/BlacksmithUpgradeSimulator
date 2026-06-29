using UnityEngine;

public class StartSceneGameManager : MonoBehaviour
{
    [SerializeField] private StartSceneUIManager uiManager;
    string version;
    private void Start()
    {
        SoundManager.Inst.PlayBGM(EBgm.Title_music_Ver1);
        version = Application.version;
        uiManager.SetVersionTest(version);
    }
}
