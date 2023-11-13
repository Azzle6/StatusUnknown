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
        private SerializedProperty content;
        private SerializedProperty size;
        private VisualElement[] squares;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();
            VisualElement gridContainer = new VisualElement();

            gridContainer.style.flexDirection = FlexDirection.Row;
            gridContainer.style.alignSelf = Align.Center;
            gridContainer.style.backgroundColor = new StyleColor(new Color(255,255,255, 0.5f));
            gridContainer.style.marginBottom = 10;
            gridContainer.style.marginTop = 10;
            
            content = property.FindPropertyRelative("content");
            size = property.FindPropertyRelative("shapeSize");

            var anchorField = new PropertyField(property.FindPropertyRelative("anchor"));
            
            var sizeField = new PropertyField(property.FindPropertyRelative("shapeSize"));
            sizeField.RegisterValueChangeCallback(e =>
            {
                this.DrawGridContent(gridContainer, property);
                property.serializedObject.ApplyModifiedProperties();
            });
            
            this.DrawGridContent(gridContainer, property);

            Button fillGrid = new Button(() =>
            {
                this.FillAll();
                property.serializedObject.ApplyModifiedProperties();
            });
            fillGrid.text = "Fill grid";
            
            container.Add(sizeField);
            container.Add(gridContainer);
            container.Add(anchorField);
            container.Add(fillGrid);

            return container;
        }

        private void DrawGridContent(VisualElement gridContainer, SerializedProperty property)
        {
            List<VisualElement> newSquares = new List<VisualElement>();
            content.arraySize = size.vector2IntValue.x * size.vector2IntValue.y;
            gridContainer.Clear();
            if (content.arraySize > 0)
            {
                for (int x = 0; x < size.vector2IntValue.x; x++)
                {
                    VisualElement newColumn = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Column
                        }
                    };
                    gridContainer.Add(newColumn);
                    for (int y = 0; y < size.vector2IntValue.y; y++)
                    {
                        SerializedProperty valueProperty =
                            content.GetArrayElementAtIndex(
                                GridHelper.GetIndexFromGridPosition(new Vector2Int(x, y), size.vector2IntValue.x));
                        
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
            this.squares = newSquares.ToArray();
        }

        private void FillAll()
        {
            content.arraySize = size.vector2IntValue.x * size.vector2IntValue.y;
            if (content.arraySize > 0)
            {
                for (int x = 0; x < size.vector2IntValue.x; x++)
                {
                    for (int y = 0; y < size.vector2IntValue.y; y++)
                    {
                        SerializedProperty valueProperty =
                            content.GetArrayElementAtIndex(
                                GridHelper.GetIndexFromGridPosition(new Vector2Int(x, y), size.vector2IntValue.x));
                        valueProperty.boolValue = true;
                    }
                }
            }

            foreach (VisualElement square in this.squares)
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
