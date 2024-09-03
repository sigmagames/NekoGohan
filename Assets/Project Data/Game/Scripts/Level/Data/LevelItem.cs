#pragma warning disable 649

using UnityEngine;

namespace Watermelon.BubbleMerge
{
    [System.Serializable]
    public class LevelItem
    {
        [SerializeField] private Item type;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Texture2D editorTexture; //used in level editor

        public Item Type => type;

        public GameObject Prefab => prefab;
    }
}