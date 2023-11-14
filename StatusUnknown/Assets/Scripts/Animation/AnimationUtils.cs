
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class AnimationUtils
{
    [MenuItem("Assets/Add root motion curve")]
    public static void AddMotionCurve()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets).Cast<AnimationClip>();

        foreach (AnimationClip clip in clips)
        {
            var bindings = AnimationUtility.GetCurveBindings(clip);

            foreach (EditorCurveBinding sourceBinding in bindings)
            {
                if (sourceBinding.path != "")
                {
                    // We are only looking at the root component
                    continue;
                }

                var property = sourceBinding.propertyName;

                if (property.StartsWith("m_LocalPosition."))
                {
                    property = property.Replace("m_LocalPosition.", "MotionT.");
                }
                else
                {
                    // Not interested in this property
                    continue;
                }

                var binding = new EditorCurveBinding();
                binding.path = "";
                binding.type = typeof(Animator);
                binding.propertyName = property;

                var curve = AnimationUtility.GetEditorCurve(clip, sourceBinding);

                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }
        }
    }
    [MenuItem("My Commands/Add root motion curve")]
    public static void RemoveMotionCurve()
    {

    }
}
