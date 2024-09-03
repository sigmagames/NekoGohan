#pragma warning disable 649

using UnityEngine;

namespace Watermelon.BubbleMerge
{
    [System.Serializable]
    public class ItemSave
    {
        [SerializeField] private Item type;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 scale;

        public Item Type { get => type; }
        public Vector3 Position { get => position; }
        public Vector3 Rotation { get => rotation; }
        public Vector3 Scale { get => scale; }

        public ItemSave(Item type, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.type = type;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}