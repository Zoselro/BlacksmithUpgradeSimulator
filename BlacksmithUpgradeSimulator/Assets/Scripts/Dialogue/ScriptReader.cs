using System.Collections.Generic;
using UnityEngine;

public class ScriptReader : MonoBehaviour
{
    private List<DialogueData> dialogueDataList = new List<DialogueData>();

    private TextAsset dialogueJson = null;
    private string path = "Dialogue/GameScript";
    ScriptReaderDialogueLines dialoguesLines = null;

    public ScriptReaderDialogueLines LoadDialogueData()
    {
        dialogueJson = Resources.Load<TextAsset>(path);
        dialoguesLines = JsonUtility.FromJson<ScriptReaderDialogueLines>(dialogueJson.text);

        return dialoguesLines;
    }

    public string ReadPlayer(string eventId)
    {
        dialogueDataList.Clear(); // 대사 리스트 초기화

        foreach (DialogueData dialogueData in dialoguesLines.lines)
        {
            if(dialogueData.eventID == eventId)
            {
                dialogueDataList.Add(dialogueData);
            }
        }

        int ran = Random.Range(0, dialogueDataList.Count);

        return dialogueDataList[ran].text;
    }

    public string ReadNPC(string NpcType, string NpcTendency, NpcState npcState, Gender gender)
    {
        dialogueDataList.Clear(); // 대사 리스트 초기화

        foreach (DialogueData dialogueData in dialoguesLines.lines)
        {
            if (dialogueData.NpcTendency == NpcTendency && dialogueData.NpcType == NpcType && dialogueData.NpcState == npcState.ToString() && dialogueData.Gender == gender.ToString())
            {
                dialogueDataList.Add(dialogueData);
            }

        }
        if (dialogueDataList.Count == 0)
        {
            Debug.LogError($"대사 없음: {NpcType} {NpcTendency} {npcState}");
            return null;
        }

        int ran = Random.Range(0, dialogueDataList.Count);

        return dialogueDataList[ran].text;
    }
}
