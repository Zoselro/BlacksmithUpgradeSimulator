using TMPro;
using UnityEngine;

public class DialogueBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI contentUI;
    [SerializeField] private GameObject[] contentBoxs;
    [SerializeField] private TextMeshProUGUI endContentBox;

    private Dir currentDir;

    public void Show(string name, string content, Dir dir)
    {
        nameUI.text = name;
        contentUI.text = content;
        if (currentDir != dir)
        {
            contentBoxs[(int)currentDir].SetActive(false);
            currentDir = dir;
            contentBoxs[(int)currentDir].SetActive(true);
        }
    }

    public void SetEndContentBox(string text)
    {
        endContentBox.text = text;
    }
}
