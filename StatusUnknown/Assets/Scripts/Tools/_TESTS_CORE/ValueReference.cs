using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[InlineProperty]
[LabelWidth(75)]
public abstract class ValueReference<TValue, TAsset> where TAsset : ValueAssetSO<TValue>
{
    [HorizontalGroup("Reference", MaxWidth = 100), ValueDropdown("valueList"), HideLabel][SerializeField] protected bool useValue = true;
    [ShowIf("useValue", Animate = false), HorizontalGroup("Reference"), HideLabel][SerializeField] protected TValue _value;
    [HideIf("useValue", Animate = false), HorizontalGroup("Reference"), OnValueChanged(nameof(UpdateAsset)), HideLabel][SerializeField] protected TAsset assetReference;
    [ShowIf("@assetReference != null && useValue == false"), LabelWidth(100)][SerializeField] protected bool editAsset = false;
    [ShowIf("@assetReference != null && useValue == false"), EnableIf("editAsset")][InlineEditor(InlineEditorObjectFieldModes.Hidden)][SerializeField] protected TAsset _assetReference; // inline attribute to be editable

    private static ValueDropdownList<bool> valueList = new ValueDropdownList<bool>()
    {
        {"Value", true },
        {"Reference", false },
    };

    public TValue value
    {
        get
        {
            if (assetReference == null || useValue) return _value;
            else return assetReference.value; 
        }
    }

    protected void UpdateAsset()
    {
        _assetReference = assetReference;
    }

    public static implicit operator TValue(ValueReference<TValue, TAsset> valueReference)
    {
        return valueReference.value;
    }
}
