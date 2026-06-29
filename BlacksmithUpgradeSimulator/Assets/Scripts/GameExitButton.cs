using UnityEngine;

public class GameExitButton : MonoBehaviour
{
    public void OnClickExitButton()
    {
        SoundManager.Inst.PlaySFX(ESfx.Button_Click);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}