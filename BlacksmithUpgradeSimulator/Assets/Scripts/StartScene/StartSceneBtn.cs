using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtn : MonoBehaviour
{
    public void GameStart()
    {
        SoundManager.Inst.PlaySFX(ESfx.Start_Button);
        // New game -> intro first, saved game -> continue directly
        SceneManager.LoadScene(GameDataManager.IsNewGame() ? "IntroScene" : "GameScene");
    }
}
