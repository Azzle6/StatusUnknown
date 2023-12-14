namespace UI.Global
{
    using UnityEngine.UIElements;
    using System.Threading.Tasks;
    using UnityEngine;
    using DG.Tweening;
    using System.Collections.Generic;

    public static class UIFeedbackHelper
    {
        public async static void BumpElement(VisualElement element, float delay = 0.2f)
        {
            element.RemoveFromClassList("backToNormalElement");
            element.AddToClassList("bumpElement");
            await Task.Delay((int)(delay * 1000));
            element.RemoveFromClassList("bumpElement");
            element.AddToClassList("backToNormalElement");

        }
    }
}