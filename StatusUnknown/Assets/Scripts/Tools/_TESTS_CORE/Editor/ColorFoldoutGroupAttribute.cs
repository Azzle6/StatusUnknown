using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine; 

/// <summary>
/// Wraps a set of data into a colored box with label
/// </summary>
public class ColorFoldoutGroupAttribute : PropertyGroupAttribute
{
    public float R, G, B, A; 

    public ColorFoldoutGroupAttribute(string path) : base(path) { }

    public ColorFoldoutGroupAttribute(string path, float r, float g, float b, float a) : base(path)
    {
        R = r;                       
        G = g;
        B = b;
        A = a;
    }

    protected override void CombineValuesWith(PropertyGroupAttribute other)
    {
        var otherAttribute = (ColorFoldoutGroupAttribute)other;

        // between incoming attrib and our current instance
        // this is similar to logic operations on 0/1 bit, but with floats
        this.R = Mathf.Max(otherAttribute.R, this.R); 
        this.G = Mathf.Max(otherAttribute.G, this.G);
        this.B = Mathf.Max(otherAttribute.B, this.B);
        this.A = Mathf.Max(otherAttribute.A, this.A);
    }
}

// keep your drawers as small and modular as possible
// they should focus on one specific drawing logic and then do this.CallNextDrawer(label)
public class ColorFoldoutGroupAttributeDrawer : OdinGroupDrawer<ColorFoldoutGroupAttribute>
{
    // with no persistency, non-serialized data like this one will always be reset to default after every script compilation or update
    private LocalPersistentContext<bool> IsVisible;

    protected override void Initialize()
    {
        this.IsVisible = this.GetPersistentValue<bool>("ColorFoldoutGroupAttributeDrawer.IsVisible", GeneralDrawerConfig.Instance.ExpandFoldoutByDefault); 
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        GUIHelper.PushColor(new Color(this.Attribute.R, this.Attribute.G, this.Attribute.B, this.Attribute.A)); 
        SirenixEditorGUI.BeginBox();
        SirenixEditorGUI.BeginBoxHeader(); 
        GUIHelper.PopColor(); // poping color here so that it is limited to the box and head but leaves text with default color

        // now, every subsequent instance will take the value from the previous one.
        // if "positions".foldout = true, then the next instance will have it true. May not be what you want..
        this.IsVisible.Value = SirenixEditorGUI.Foldout(this.IsVisible.Value, label); 

        SirenixEditorGUI.EndBoxHeader(); // ending BoxHeader so that only the label is highlited

        // Basic foldout logic of opening/closing/displaying
        if (SirenixEditorGUI.BeginFadeGroup(this, this.IsVisible.Value))
        {
            for (int i = 0; i < this.Property.Children.Count; i++)
            {
                this.Property.Children[i].Draw();
            }
        }
        SirenixEditorGUI.EndFadeGroup();
        SirenixEditorGUI.EndBox();

        // you can see the call chain by using the [ShowDrawerChain] attribute on any property
        // this is how conditional attributes like [ShowIf(result)] work
    }

}
