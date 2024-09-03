using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Watermelon.BubbleMerge;

namespace Watermelon
{
    public class UIGame : UIPage
    {
        [SerializeField] TMP_Text movesLeft;
        [SerializeField] TextMeshProUGUI levelText;

        [SerializeField] Image requirementsResultImage;
        public Image RequirementsResultImage => requirementsResultImage;

        [SerializeField] RectTransform safeAreaRectTransform;
        [SerializeField] RectTransform requirementsParent;
        public RectTransform RequirementsParent => requirementsParent;

        [SerializeField] FlyingObjects flyingObjects;
        public FlyingObjects FlyingObjects => flyingObjects;

        [SerializeField] InGameSettingsPanel inGameSettings;
        [SerializeField] UILevelQuitPopUp exitPopUp;

        [Space]
        [SerializeField] TMP_Text comboText;

        [Header("Power Ups")]
        [SerializeField] PUUIController powerUpsUIController;
        public PUUIController PowerUpsUIController => powerUpsUIController;

        [Header("Dev")]
        [SerializeField] TMP_InputField levelInputDev;

        [SerializeField] GameObject devOverlay;

        public override void Initialise()
        {
            flyingObjects.Initialise();

            LevelController.OnTurnChanged += OnTurnChanged;

            if (DevPanelEnabler.IsDevPanelDisplayed())
            {
                ShowDevOverlay();
            }
            else
            {
                HideDevOverlay();
            }

            NotchSaveArea.RegisterRectTransform(safeAreaRectTransform);
        }

        private void OnEnable()
        {
            exitPopUp.OnConfirmExitEvent += ExitPopUpConfirmExitButton;
            exitPopUp.OnCancelExitEvent += ExitPopCloseButton;
        }

        private void OnDisable()
        {
            exitPopUp.OnConfirmExitEvent -= ExitPopUpConfirmExitButton;
            exitPopUp.OnCancelExitEvent += ExitPopCloseButton;
        }

        #region Show/Hide
        public override void PlayHideAnimation()
        {
            UILevelNumberText.Hide(false);

            inGameSettings.HidePanel();

            UIController.OnPageClosed(this);
        }

        public override void PlayShowAnimation()
        {
            UILevelNumberText.Show(false);

            UIController.OnPageOpened(this);

            inGameSettings.ShowPanel();
        }
        #endregion

        public void OnLevelStarted(int level)
        {
            levelText.text = string.Format("LEVEL {0}", level + 1);

            powerUpsUIController.OnLevelStarted(level);
        }

        private void OnTurnChanged()
        {
            movesLeft.text = (LevelController.TurnsLimit - LevelController.Turn).ToString();
        }

        #region Combo

        public void ShowCombo()
        {
            comboText.DOFade(1, 0.2f);
        }

        public void HideCombo()
        {
            comboText.DOFade(0, 0.2f);
        }

        public void SetCombo(int count)
        {
            comboText.text = $"{count}!";
        }

        #endregion

        public void ShowExitPopUp()
        {
            LevelController.OnGamePopupOpened();

            // close panel
            inGameSettings.SettingsButton();

            exitPopUp.Show();
        }

        public void ExitPopCloseButton()
        {
            LevelController.OnGamePopupClosed();

            exitPopUp.Hide();
        }

        public void ExitPopUpConfirmExitButton()
        {
            if (LivesManager.IsMaxLives)
                LivesManager.RemoveLife();

            GameController.CloseLevel();

            exitPopUp.Hide();
        }

        #region Dev

        public void Reload()
        {
            LevelController.LoadLevel(SaveController.LevelId);
        }

        public void Spawn()
        {
            LevelController.LevelBehavior.SpawnRandomBubble(true, false);
        }

        public void SpawnIce()
        {
            LevelController.LevelBehavior.SpawnIceBubble();
        }

        public void OnLevelInputChanged(string input)
        {
            int level = -1;

            if (int.TryParse(input, out level))
            {
                SaveController.LevelId = Mathf.Clamp((level - 1), 0, int.MaxValue);
                GameController.OnLevelManuallyChanged();

                SaveController.MarkAsSaveIsRequired();
            }
        }

        public void HideDevOverlay()
        {
            devOverlay.SetActive(false);
        }

        public void MinusItemsButton()
        {
            LevelController.LevelBehavior.RemoveRandomBubbleDev();
        }

        public void PlusItemsButton()
        {
            LevelController.LevelBehavior.SpawnRandomBubble(true, false);
        }

        public void NextButtonDev()
        {
            SaveController.LevelId++;
            if (SaveController.LevelId > LevelController.MaxLevelReached)
                LevelController.MaxLevelReached = SaveController.LevelId;
            GameController.OnLevelManuallyChanged();
            LevelController.LoadLevel(SaveController.LevelId);

            SaveController.MarkAsSaveIsRequired();
        }

        public void PrevButtonDev()
        {
            SaveController.LevelId = Mathf.Clamp(SaveController.LevelId - 1, 0, int.MaxValue);
            GameController.OnLevelManuallyChanged();
            LevelController.LoadLevel(SaveController.LevelId);

            SaveController.MarkAsSaveIsRequired();
        }

        public void ShowDevOverlay()
        {
            devOverlay.SetActive(true);
        }

        #endregion
    }
}
