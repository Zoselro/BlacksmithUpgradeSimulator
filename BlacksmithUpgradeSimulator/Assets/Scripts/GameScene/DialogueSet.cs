public class DialogueSet
{
    private DialogueLine[] dialogueLines;
    public DialogueLine[] Dialogues => dialogueLines;
    public delegate void Endfunction();
    private Endfunction endfunc;

    public void SetDialogueLines(DialogueLine[] dialogueLines)
    {
        this.dialogueLines = dialogueLines;
    }

    public void SetEndFunc(Endfunction func)
    {
        endfunc = func;
    }

    public void EndFuncExecution()
    {
        endfunc();
    }
}
