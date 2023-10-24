using UnityEngine;

[CreateAssetMenu(fileName = "Module", menuName = "Status Unknown/Systems/module")]
public class ModuleStats : ScriptableObject
{
    public string ID;
    [TextArea(3, 20)] public string Description;
    public int Size;
    public int Effectors_Amount;
}
