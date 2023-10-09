namespace Inventory
{
    using UnityEngine;
    using UnityEngine.UIElements;
    public class GridView : MonoBehaviour
    {
        [SerializeField]
        private GridDataSO gridDataSo;
        [SerializeField]
        private UIDocument uiDocument;

        public void DisplayGrid(GridDataSO dataSo)
        {
            this.gridDataSo = dataSo;
            this.uiDocument.enabled = true;
        }
    }
}
