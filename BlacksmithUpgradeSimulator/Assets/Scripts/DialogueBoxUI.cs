using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI contentUI;
    [SerializeField] private GameObject[] contentBox;
    [SerializeField] private Button nextBtn;
    [SerializeField] private GameObject contentBoxObj;
    [SerializeField] private GameObject endContentBoxObj;

    private Dir currentDir;

    public void Show(string name, string content, Dir dir)
    {
        nameUI.text = name;
        contentUI.text = content;

        if(currentDir != dir)
        {
            contentBox[(int)currentDir].SetActive(false);
            currentDir = dir;
            contentBox[(int)currentDir].SetActive(true);
        }
    }

    public void ShowContentBox(bool active)
    {
        contentBoxObj.SetActive(active);
    }

    public void ShowNextBtn(bool active)
    {
        nextBtn.gameObject.SetActive(active);
    }

    public void ShowEndContentBoxObj(bool active)
    {
        endContentBoxObj.SetActive(active);
    }
}
