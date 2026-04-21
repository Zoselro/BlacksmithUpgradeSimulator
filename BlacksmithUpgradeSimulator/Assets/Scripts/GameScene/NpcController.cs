using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
