using UnityEngine;

namespace Watermelon.BubbleMerge
{
    [System.Serializable]
    public class LevelBackground
    {
        [SerializeField] private LevelBackgroundType type;
        [SerializeField] private GameObject prefab;

        public LevelBackgroundType Type => type;
        public GameObject Prefab => prefab;
    }
}