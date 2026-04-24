using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtn : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
