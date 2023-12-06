namespace Module
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using Core.SingletonsSO;
    using Definitions;
    using Inventory.Grid;
    using Inventory.Item;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ModuleItemView : ItemView
    {
        public ModuleItemView(ModuleData itemData, Vector2Int gridPosition, GridView gridView) : base(itemData, gridPosition,
            gridView)
        {
            this.ModuleItemData = itemData;
            this.GenerateCustomView();
        }

        public ModuleData ModuleItemData;
        private Dictionary<E_ModuleOutput, VisualElement> outputsVisual;

        protected override void GenerateCustomView()
        {
            this.outputsVisual = new Dictionary<E_ModuleOutput, VisualElement>();
            foreach (var outputInfo in this.ModuleItemData.definition.outputs)
            {
                VisualElement triggerElement = UIHandler.Instance.uiSettings.triggerTemplate.Instantiate();
                triggerElement.style.backgroundImage = UIHandler.Instance.iconsReferences.moduleOutputReferences[outputInfo.moduleTriggerType].texture;
                
                this.ViewRoot.Add(triggerElement);
                float slotWidth = UIHandler.Instance.uiSettings.slotWidth;
                Vector2 directionDisplacement;
                float iconRotation = 0;
                switch (outputInfo.direction)
                {
                    case E_Direction.UP:
                        directionDisplacement = new Vector2(slotWidth/2f, 0);
                        iconRotation = 270;
                        break;
                    case E_Direction.DOWN:
                        directionDisplacement = new Vector2(slotWidth/2f, slotWidth);
                        iconRotation = 90;
                        break;
                    case E_Direction.LEFT:
                        directionDisplacement = new Vector2(0, slotWidth/2f);
                        iconRotation = 180;
                        break;
                    case E_Direction.RIGHT:
                        directionDisplacement = new Vector2(slotWidth, slotWidth/2f);
                        iconRotation = 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                triggerElement.transform.position = ((Vector2)outputInfo.localPosition * slotWidth + directionDisplacement) - (Vector2.one * UIHandler.Instance.uiSettings.triggerWidth/2);
                triggerElement.style.rotate = new StyleRotate(new Rotate(iconRotation));
                
                this.outputsVisual.Add(outputInfo.moduleTriggerType, triggerElement);
            }
        }

        public void SetLinkView(E_ModuleOutput linkedOutput, bool isLinked)
        {
            this.outputsVisual[linkedOutput].style.backgroundColor = isLinked ? new StyleColor(Color.green) : new StyleColor(Color.red);
        }
    }
}
