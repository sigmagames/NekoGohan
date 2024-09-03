using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon.BubbleMerge;

namespace Watermelon
{
    [System.Serializable]
    public class ObjectData
    {
        public Branch branch;
        public GameObject prefab;

        private Pool pool;
        public Pool Pool => pool;

        public void Initialise(Transform containerTransform)
        {
            pool = new Pool(new PoolSettings(prefab.name, prefab, 1, true, containerTransform));
        }
    }
}
