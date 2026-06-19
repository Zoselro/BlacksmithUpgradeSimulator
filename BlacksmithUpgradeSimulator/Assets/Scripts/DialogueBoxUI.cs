using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class DialogueBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI contentUI;
    [SerializeField] private GameObject[] contentBoxs;
    [SerializeField] private Button nextBtn;
    [SerializeField] private GameObject contentBoxObj;
    [SerializeField] private GameObject endContentBoxObj;
    [SerializeField] private TextMeshProUGUI welcomText;

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

    // 이름과 대사, 활성화 여부를 설정하는 메서드
    //public void PlayWelcomeSequence(bool active, string text)
    //{
    //    welcomText.text = text;
    //    popUpImg.SetActive(active);
    //    dialogueBoxUI.ShowContentBox(!active);
    //}

    public void ShowContentBox(bool active)
    {
        contentBoxObj.SetActive(active);
    }

    public void ShowContentBox(bool active, string text)
    {
        welcomText.text = text;
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
