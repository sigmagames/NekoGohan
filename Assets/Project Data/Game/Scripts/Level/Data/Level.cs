using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Watermelon.BubbleMerge
{
    [CreateAssetMenu(fileName = "Level_001", menuName = "Content/Data/Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] string note;

        [SerializeField] int bubblesOnTheFieldAmount;
        public int BubblesOnTheFieldAmount => bubblesOnTheFieldAmount;

        [SerializeField] int turnsLimit;
        public int TurnsLimit => turnsLimit;

        [SerializeField] int coinsReward = 20;
        public int CoinsReward => (int)(coinsReward * (LevelController.IsLevelCompletedForTheFirstTime ? 1f : 0.3f));

        [SerializeField] GeneralLevelTarget requirements = new GeneralLevelTarget();
        public GeneralLevelTarget Requirements => requirements;

        [SerializeField] bool canBeUsedInRandomizer = true;
        public bool CanBeUsedInRandomizer => canBeUsedInRandomizer;

        [SerializeField] ItemSave[] items;
        [SerializeField] LevelShapeType levelShapeType;
        [SerializeField] LevelBackgroundType levelBackType;

        private List<BubbleSpawnData> spawnQueue;
        public List<BubbleSpawnData> SpawnQueue => spawnQueue;
        public ItemSave[] Items => items;

        public int ItemsAmount => items.Length;

        public LevelShapeType LevelShapeType => levelShapeType;
        public LevelBackgroundType LevelBackType => levelBackType;

        public void Init()
        {
            spawnQueue = new List<BubbleSpawnData>();

            var receipt = requirements.Recipe;

            for (int j = 0; j < receipt.Ingridients.Count; j++)
            {
                var ingridient = receipt.Ingridients[j];

                int count = ingridient.amount * (int)Mathf.Pow(2, ingridient.stageId);

                for (int k = 0; k < count; k++)
                {
                    spawnQueue.Add(new BubbleSpawnData()
                    {
                        branch = ingridient.branch,
                        stageId = 0,
                        iceHP = 0,
                        boxHP = 0,
                    });
                }
            }

            spawnQueue.Shuffle();

            // finding all bubbles without effect
            List<BubbleSpawnData> bubblesWithoutEffects = new List<BubbleSpawnData>();

            for (int i = 0; i < spawnQueue.Count; i++)
            {
                if (!spawnQueue[i].HasEffect)
                {
                    bubblesWithoutEffects.Add(spawnQueue[i]);
                }
            }

            bubblesWithoutEffects.Shuffle();

            // ice bubbles initialization
            for (int i = 0; i < requirements.IceBubblesPerLevel && bubblesWithoutEffects.Count > 0; i++)
            {
                bubblesWithoutEffects[0].iceHP = requirements.IceBubblesHealth;
                bubblesWithoutEffects.RemoveAt(0);
            }

            // box bubbles initialization
            for (int i = 0; i < requirements.BoxesPerLevel && bubblesWithoutEffects.Count > 0; i++)
            {
                bubblesWithoutEffects[0].boxHP = requirements.BoxesHealth;
                bubblesWithoutEffects.RemoveAt(0);
            }
        }

        public bool GetNextSpawnData(out BubbleSpawnData data)
        {
            data = default;
            if (spawnQueue.IsNullOrEmpty())
                return false;

            data = spawnQueue[0];
            spawnQueue.RemoveAt(0);

            return true;
        }

        public bool TryGetSimilarSpawnData(BubbleData data, out BubbleSpawnData similarSpawnData)
        {
            similarSpawnData = default;

            if (spawnQueue.IsNullOrEmpty())
                return false;

            for (int i = 0; i < spawnQueue.Count; i++)
            {
                var testData = spawnQueue[i];

                if (testData.branch == data.branch)
                {
                    similarSpawnData = testData;

                    spawnQueue.RemoveAt(i);

                    return true;
                }
            }

            return false;
        }

        public void AddBubbleToQueue(BubbleData data)
        {
            int count = (int)Mathf.Pow(2, data.stageId);

            for (int i = 0; i < count; i++)
            {
                spawnQueue.Insert(Random.Range(0, spawnQueue.Count), new BubbleSpawnData
                {
                    stageId = 0,
                    branch = data.branch,
                    boxHP = 0,
                    iceHP = 0
                });
            }
        }
    }

    [System.Serializable]
    public struct Requirement
    {
        public Branch branch;
        public int stageId;
        public int amount;

        public Requirement(Branch branch, int stageId, int amount = 0)
        {
            this.branch = branch;
            this.stageId = stageId;
            this.amount = amount;
        }
    }
}