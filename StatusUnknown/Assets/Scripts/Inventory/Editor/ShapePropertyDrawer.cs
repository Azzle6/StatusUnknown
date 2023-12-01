namespace Inventory.Editor
{
    using System.Collections.Generic;
    using Core.Helpers;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UIElements;

    [CustomPropertyDrawer(typeof(Shape))]
    public class ShapePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement[] squares = new VisualElement[]{};
            
            VisualElement container = new VisualElement();
            VisualElement gridContainer = new VisualElement();

            gridContainer.style.flexDirection = FlexDirection.Row;
            gridContainer.style.alignSelf = Align.Center;
            gridContainer.style.backgroundColor = new StyleColor(new Color(255,255,255, 0.5f));
            gridContainer.style.marginBottom = 10;
            gridContainer.style.marginTop = 10;
            
            var content = property.FindPropertyRelative("content");
            var size = property.FindPropertyRelative("shapeSize");

            var anchorField = new PropertyField(property.FindPropertyRelative("anchor"));
            
            var sizeField = new PropertyField(property.FindPropertyRelative("shapeSize"));
            sizeField.RegisterValueChangeCallback(e =>
            {
                this.DrawGridContent(gridContainer, property, content, size, ref squares);
                property.serializedObject.ApplyModifiedProperties();
            });
            
            this.DrawGridContent(gridContainer, property, content, size, ref squares);

            Button fillGrid = new Button(() =>
            {
                this.FillAll(content, size, ref squares);
                property.serializedObject.ApplyModifiedProperties();
            });
            fillGrid.text = "Fill grid";
            
            container.Add(sizeField);
            container.Add(gridContainer);
            container.Add(anchorField);
            container.Add(fillGrid);

            return container;
        }

        private void DrawGridContent(VisualElement gridContainer, SerializedProperty property, SerializedProperty content, SerializedProperty size, ref VisualElement[] squares)
        {
            List<VisualElement> newSquares = new List<VisualElement>();
            Vector2Int curSize = size.vector2IntValue;
            content.arraySize = curSize.x * curSize.y;
            gridContainer.Clear();
            if (content.arraySize > 0)
            {
                for (int x = 0; x < curSize.x; x++)
                {
                    VisualElement newColumn = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Column
                        }
                    };
                    gridContainer.Add(newColumn);
                    for (int y = 0; y < curSize.y; y++)
                    {
                        SerializedProperty valueProperty =
                            content.GetArrayElementAtIndex(
                                GridHelper.GetIndexFromGridPosition(new Vector2Int(x, y), curSize.x));
                        
                        VisualElement button = this.InstantiateSquare();
                        ChangeSquareVisual(button, valueProperty.boolValue);
                        
                        button.RegisterCallback<MouseEnterEvent>(e =>
                        {
                            if (Mouse.current.leftButton.isPressed)
                            {
                                valueProperty.boolValue = true;
                                ChangeSquareVisual(button, true);
                            }
                            else if (Mouse.current.rightButton.isPressed)
                            {
                                valueProperty.boolValue = false;
                                ChangeSquareVisual(button, false);
                            }
                            property.serializedObject.ApplyModifiedProperties();
                        });
                        
                        button.RegisterCallback<ClickEvent>(e =>
                        {
                            valueProperty.boolValue = true;
                            ChangeSquareVisual(button, true);
                            property.serializedObject.ApplyModifiedProperties();
                        });
                        
                        button.RegisterCallback<ContextClickEvent>(e =>
                        {
                            valueProperty.boolValue = false;
                            ChangeSquareVisual(button, false);
                            property.serializedObject.ApplyModifiedProperties();
                        });
                        
                        newColumn.Add(button);
                        button.Bind(property.serializedObject);
                        newSquares.Add(button);
                    }
                }
            }
            squares = newSquares.ToArray();
        }

        private void FillAll(SerializedProperty content, SerializedProperty size, ref VisualElement[] squares)
        {
            Vector2Int newSize = size.vector2IntValue;
            content.arraySize = newSize.x * newSize.y;
            if (content.arraySize > 0)
            {
                for (int x = 0; x < newSize.x; x++)
                {
                    for (int y = 0; y < newSize.y; y++)
                    {
                        SerializedProperty valueProperty =
                            content.GetArrayElementAtIndex(
                                GridHelper.GetIndexFromGridPosition(new Vector2Int(x, y), newSize.x));
                        valueProperty.boolValue = true;
                    }
                }
            }

            foreach (VisualElement square in squares)
                ChangeSquareVisual(square, true);
        }

        private VisualElement InstantiateSquare()
        {
            VisualElement result = new VisualElement();
            IStyle style = result.style;
            style.height = 20;
            style.width = 20;
            style.marginBottom = 3;
            style.marginLeft = 3;
            style.marginRight = 3;
            style.marginTop = 3;
            style.backgroundColor = new StyleColor(new Color(255,255,255, 0.8f));
            result.focusable = true;

            return result;
        }

        private void ChangeSquareVisual(VisualElement square, bool selected)
        {
            square.style.backgroundColor =
                new StyleColor(selected ? new Color(0, 255, 0, 0.8f) : new Color(255, 255, 255, 0.8f));
        }
    }
}
