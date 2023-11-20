namespace Module
{
    using System;
    using Core.Helpers;
    using Core.SingletonsSO;
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
        }

        public ModuleData ModuleItemData;

        protected override void GenerateCustomView()
        {
            foreach (var outputInfo in this.ModuleItemData.definition.outputs)
            {
                VisualElement triggerElement = UIHandler.Instance.uiSettings.triggerTemplate.Instantiate();
                this.ViewRoot.Add(triggerElement);
                float slotWidth = UIHandler.Instance.uiSettings.slotWidth;
                Vector2 directionDisplacement;
                switch (outputInfo.direction)
                {
                    case E_Direction.UP:
                        directionDisplacement = new Vector2(slotWidth / 2, 0);
                        break;
                    case E_Direction.DOWN:
                        directionDisplacement = new Vector2(slotWidth / 2, 1);
                        break;
                    case E_Direction.LEFT:
                        directionDisplacement = new Vector2(0, slotWidth / 2);
                        break;
                    case E_Direction.RIGHT:
                        directionDisplacement = new Vector2(0, slotWidth / 2);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                triggerElement.transform.position = (Vector2)outputInfo.localPosition * slotWidth + directionDisplacement;
            }
        }
    }
}
