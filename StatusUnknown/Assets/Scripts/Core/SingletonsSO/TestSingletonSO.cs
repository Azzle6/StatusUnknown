namespace Core.SingletonsSO
{
    using UnityEngine;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/Test", fileName = "TestSO")]
    public class TestSingletonSO : SingletonSO<TestSingletonSO>
    {
        public VisualTreeAsset slotAsset;
    }
}
