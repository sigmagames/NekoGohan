using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon.BubbleMerge
{
    public class UILevelPopUp : MonoBehaviour
    {
        [SerializeField] TMP_Text levelText;
        [SerializeField] Image resultPreview;

        private int LevelId { get; set; }

        public void Show(int levelId)
        {
            levelText.text = "LEVEL " + (levelId + 1);
            gameObject.SetActive(true);

            resultPreview.sprite = LevelController.Database.GetLevel(levelId).Requirements.Recipe.ResultPreview;

            LevelId = levelId;
        }

        public void PlayButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            GameController.OnLevelStart(LevelId);

            ClosePanel();

            Tween.DelayedCall(2f, LivesManager.RemoveLife);
        }

        public void ClosePanel()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            gameObject.SetActive(false);
        }
    }
}