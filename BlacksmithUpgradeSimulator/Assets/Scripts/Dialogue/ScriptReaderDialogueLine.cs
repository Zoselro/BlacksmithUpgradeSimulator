using System;

[Serializable]
public class ScriptReaderDialogueLines
{
    public DialogueData[] lines;
}

[Serializable]
public class DialogueData
{
    public string text;
    public string eventID;
    public string NpcTendency;
    public string NpcState;
    public string NpcType;
    public string Gender;
}