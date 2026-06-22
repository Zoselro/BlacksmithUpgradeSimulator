using UnityEngine;
using TMPro;

public class TextSender : MonoBehaviour
{
    [Header("Dialogs")]
    [SerializeField] private string[] dialogStrings;
    [SerializeField] private TextMeshProUGUI textObj;

    public void SendText(string[] dialogs)
    {
        TypingManager.instance.Typing(dialogs, textObj);
    }
}
