using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("DialogueBox")]
    [SerializeField] private DialogueBoxUI dialogueBoxUI;
    [SerializeField] private Image[] characterImageUI;

    private Dir currentDir;

    public void Show(string name, string text, Sprite sprite, Speaker speak, Dir dir)
    {
        dialogueBoxUI.Show(name, text, dir);

        characterImageUI[(int)dir].sprite = sprite;
    }
}
