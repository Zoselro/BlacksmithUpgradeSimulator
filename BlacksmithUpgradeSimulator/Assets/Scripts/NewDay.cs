using UnityEngine;

public class NewDay : MonoBehaviour
{
    public void StartDay()
    {
        GameManager.Inst.ResetGame();
    }
}
