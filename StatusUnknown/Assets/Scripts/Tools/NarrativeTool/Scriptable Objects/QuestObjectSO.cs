using StatusUnknown.Utils.AssetManagement;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    public abstract class QuestObjectSO : ScriptableObject
    {
        [SerializeField] protected string _Name;
        [SerializeField, TextArea(5,20)] protected string _Description;   
    }
}
