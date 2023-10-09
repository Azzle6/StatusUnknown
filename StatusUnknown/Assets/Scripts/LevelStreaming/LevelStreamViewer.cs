
namespace LevelStreaming
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelStreamViewer : MonoBehaviour
    {
        [SerializeField] Camera viewCamera;
        [SerializeField] List<LevelStreamVolume> levelStreamVolumes;

        private void Update()
        {
            CheckVolumeInView();
        }

        private void CheckVolumeInView()
        {
            Plane[] viewPlanes = GeometryUtility.CalculateFrustumPlanes(viewCamera);
            for (int i = 0; i < levelStreamVolumes.Count; i++)
            {
                bool volumeInView = GeometryUtility.TestPlanesAABB(viewPlanes, levelStreamVolumes[i].Bounds);
                LoadLevelStreamVolume(levelStreamVolumes[i], volumeInView);
            }
        }
        void LoadLevelStreamVolume(LevelStreamVolume volume, bool load)
        {
            if (volume.IsLoaded == load) return;

            if (load)
                volume.LoadScene();
            if (!load)
                volume.UnLoadScene();
        }
    }
}