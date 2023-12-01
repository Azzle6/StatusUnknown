namespace CameraEffects
{
    using Player;
    using UnityEngine;
    public class CameraSwitch : MonoBehaviour
    {
        public GameObject[] activateCams;
        public GameObject[] disactivateCams;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerAction>() != null)
            {
                for(int i = 0; i < this.disactivateCams.Length; i++)
                {
                    this.disactivateCams[i].SetActive(false);
                }
                for (int i = 0; i < this.activateCams.Length; i++)
                {
                    this.activateCams[i].SetActive(true);
                }
            }
        }
    }*/
}
