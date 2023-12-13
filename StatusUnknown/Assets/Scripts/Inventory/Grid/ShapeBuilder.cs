namespace Inventory.Grid
{
    using System.Collections.Generic;
    using Core.SingletonsSO;
    using UnityEngine;
    using UnityEngine.UIElements;

    public static class ShapeBuilder
    {
        public static VisualElement[] BuildShape(Shape shape, VisualElement verticalParent, VisualTreeAsset slotTemplate)
        {
            List<VisualElement> slotsList = new List<VisualElement>();
            
            verticalParent.Clear();
            for (int y = 0; y < shape.shapeSize.y; y++)
            {
                VisualElement horizontalParent = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row
                    }
                };
                verticalParent.Insert(y, horizontalParent);
                
                for (int x = 0; x < shape.shapeSize.x; x++)
                {
                    VisualElement slotView = slotTemplate.Instantiate();
                    
                    horizontalParent.Insert(x, slotView);
                    slotsList.Add(slotView);

                    if (shape.IsValidPosition(new Vector2Int(x, y)))
                    {
                        slotView.name = $"{x},{y}";
                    }
                    else
                    {
                        slotView.AddToClassList(UIHandler.Instance.uiSettings.hiddenStyle);
                    }
                }
            }

            return slotsList.ToArray();
        }
    }
}
