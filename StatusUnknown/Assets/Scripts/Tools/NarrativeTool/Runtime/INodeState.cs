
namespace StatusUnknown.Tools.Narrative
{
    public interface INodeState
    {
        void MoveNext();
        void OnEnter();
        void OnExit();
    }

    public interface INodeDialogue
    {
        void GetDialogueAnswers();
        void SetDialogueAnswers(); 
    }
}
