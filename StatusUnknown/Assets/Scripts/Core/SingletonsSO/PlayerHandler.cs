namespace Core.SingletonsSO
{
    using Inventory;
    using Inventory.Item;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/SingletonSO/PlayerHandler", fileName = "PlayerHandler")]
    public class PlayerHandler : SingletonSO<PlayerHandler>
    { }
}
