using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtn : MonoBehaviour
{
    public void GameStart()
    {
        SoundManager.Inst.PlaySFX(ESfx.Start_Button);
        SceneManager.LoadScene("GameScene");
    }
}
