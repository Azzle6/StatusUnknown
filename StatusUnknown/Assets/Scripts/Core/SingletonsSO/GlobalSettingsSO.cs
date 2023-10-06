using UnityEngine;

namespace Core.SingletonsSO
{
    [CreateAssetMenu(menuName = "SingletonSO", fileName = "GlobalSettingsSO", order = 0)]
    public class GlobalSettingsSO : SingletonSO<GlobalSettingsSO>
    {
        public int testVariable;
    }
}
