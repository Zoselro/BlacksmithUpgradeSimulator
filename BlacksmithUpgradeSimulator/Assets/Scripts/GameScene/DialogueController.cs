using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DialogueUI dialogueUI;

    DialogueSet[] dialogues = null;

    int dialogueSetIndex = 0; // 대화 객체의 Index
    int dialogueLineIndex = 0; // 대화 객체 속에 있는 대사들 Index

    public void OnDialogueNext(DialogueSet dialogueSet)
    {
        // 대화가 끝나지 않았으면,
        // 다음 대사를 실행
        // index ++ 이후 가져옴.
        // UI한테 실행

        if (dialogueSet == null ||
            dialogues[dialogueSetIndex].Dialogues == null ||
            !dialogueSet.Dialogues[dialogueLineIndex].IsValid)
        {
            Debug.Log("대사 없거나 다음 대사가 존재하지 않음 -> 스킵");
            return;
        }

        // 대장장이가 무기를 건내 줄 때, NPC 활성화
        if (dialogueSetIndex == 3 && dialogueLineIndex == 0)
        {
            uiManager.ShowNpc(true);
        }
        dialogueUI.Show(dialogueSet.Dialogues[dialogueLineIndex].NicName, 
                            dialogueSet.Dialogues[dialogueLineIndex].Content, 
                            dialogueSet.Dialogues[dialogueLineIndex].Sprite, 
                            dialogueSet.Dialogues[dialogueLineIndex].Speak, 
                            dialogueSet.Dialogues[dialogueLineIndex].Dir);

        //uiManager.OutPutSprite(dialogueSet.Dialogues[dialogueLineIndex].NicName,
        //                            dialogueSet.Dialogues[dialogueLineIndex].Content,
        //                            dialogueSet.Dialogues[dialogueLineIndex].Sprite,
        //                            dialogueSet.Dialogues[dialogueLineIndex].Speak, dialogueSet.Dialogues[dialogueLineIndex].Dir);
        dialogueLineIndex++;
    }

    public void OnDialogueEnd(DialogueSet dialogue)
    {
        // 만약에 대화가 끝났으면 (현재 대사 번호랑 대사의 총 개수랑 비교) 
        // 대화 종료(UI 비활성화, 끝났을 때 이벤트 실행)
        // 대화 끝
        dialogue.EndFuncExecution();

        dialogueSetIndex++;     // 다음 대화 객체로
        dialogueLineIndex = 0;  // 대사 초기화

        if (dialogueSetIndex < dialogues.Length)
        {
            OnDialogueNext(dialogues[dialogueSetIndex]);
        }
        else
        {
            Debug.Log("모든 대화 종료");
        }
    }

    public void OnClickNextBtn()
    {
        // 다음버튼을 눌렀을 때
        // 만약에 대화가 끝났으면 (현재 대사 번호랑 대사의 총 개수랑 비교) 
        // 대화 종료(UI 비활성화, 끝났을 때 이벤트 실행)
        // 대화가 끝나지 않았으면,
        // 다음 대사를 실행
        // index ++ 이후 가져옴.
        // UI한테 실행

        if (dialogues == null) 
            return;

        // 아직 대사가 남아있으면
        if (dialogueLineIndex < dialogues[dialogueSetIndex].Dialogues.Length)
        {
            OnDialogueNext(dialogues[dialogueSetIndex]);
        }
        else
        {
            OnDialogueEnd(dialogues[dialogueSetIndex]);
        }
    }

    public void SetDialogue(DialogueSet[] dialogues, int startIndex)
    {
        this.dialogues = dialogues;
        dialogueSetIndex = startIndex;
        dialogueLineIndex = 0;

        // 시작하자마자 첫 대사 출력
        if (dialogues != null && dialogueSetIndex < dialogues.Length)
        {
            OnDialogueNext(dialogues[dialogueSetIndex]);
        }
    }
}

