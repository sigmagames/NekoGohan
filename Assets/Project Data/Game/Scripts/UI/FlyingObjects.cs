using UnityEngine;
using Watermelon.BubbleMerge;

namespace Watermelon
{
    [System.Serializable]
    public class FlyingObjects
    {
        [SerializeField] Transform containerTransform;

        [Space]
        [SerializeField] ObjectData defaultCase;
        [SerializeField] ObjectData[] customCases;

        public void Initialise()
        {
            defaultCase.Initialise(containerTransform);
            for (int i = 0; i < customCases.Length; i++)
            {
                customCases[i].Initialise(containerTransform);              
            }
        }

        private ObjectData GetObjectData(Branch branch)
        {
            for(int i = 0; i < customCases.Length; i++)
            {
                if (customCases[i].branch == branch)
                    return customCases[i];
            }

            return defaultCase;
        }

        public FlyingObjectBehavior Activate(Vector3 position, Sprite icon, RequirementBehavior requirementBehavior, SimpleCallback onCompleted)
        {
            ObjectData objectData = GetObjectData(requirementBehavior.Requirement.branch);

            GameObject flyingObject = objectData.Pool.GetPooledObject(true);
            flyingObject.transform.position = position;

            FlyingObjectBehavior flyingObjectBehavior = flyingObject.GetComponent<FlyingObjectBehavior>();
            flyingObjectBehavior.Initialise(objectData, requirementBehavior, onCompleted);
            flyingObjectBehavior.Activate(icon);

            return flyingObjectBehavior;
        }

    }
}
