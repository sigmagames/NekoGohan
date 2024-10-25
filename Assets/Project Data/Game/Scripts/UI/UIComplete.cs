using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using Watermelon.BubbleMerge;

namespace Watermelon
{
    public class UIComplete : UIPage
    {
        [SerializeField] UIFadeAnimation backgroundFade;

        [Space]
        [SerializeField] UIScaleAnimation levelCompleteLabel;

        [Space]
        [SerializeField] CurrencyUIPanelSimple currencyPanel;
        
        [SerializeField] UIScaleAnimation rewardLabel;

        [SerializeField] Image rewardImage;
        [SerializeField] TextMeshProUGUI rewardAmountText;

        [Space]
        [SerializeField] Image resultImage;

        [Space]
        [SerializeField] UIFadeAnimation multiplyRewardButtonFade;
        [SerializeField] Button multiplyRewardButton;
        [SerializeField] UIFadeAnimation noThanksButtonText;
        [SerializeField] Button noThanksButton;
        [SerializeField] UIFadeAnimation continueButtonFade;
        [SerializeField] Button continueButton;
        [SerializeField] Button quitInMenuButton;
        [SerializeField] RectTransform safeAreaRectTransform;

        public static float HideDuration => 0.25f;

        private TweenCase noThanksAppearTween;
        private int coinsHash = FloatingCloud.StringToHash("Coins");

        private int currentReward = 0;

        public override void Initialise()
        {
            multiplyRewardButton.onClick.AddListener(MultiplyRewardButton);
            noThanksButton.onClick.AddListener(ContinueButton);
            continueButton.onClick.AddListener(ContinueButton);
            quitInMenuButton.onClick.AddListener(QuitInMenuButton);

            currencyPanel.Initialise();

            Currency currency = CurrenciesController.GetCurrency(CurrencyType.Coins);
            rewardImage.sprite = currency.Icon;
            NotchSaveArea.RegisterRectTransform(safeAreaRectTransform);
        }

        #region Show/Hide
    public override void PlayShowAnimation()
    {
        // Hide all initial elements
        rewardLabel.Hide(immediately: true);
        multiplyRewardButtonFade.Hide(immediately: true);
        multiplyRewardButton.interactable = true;
        noThanksButtonText.Hide(immediately: true);
        noThanksButton.interactable = false;
        continueButtonFade.Hide(immediately: true);

        // Show animation for the background and level complete label
        backgroundFade.Show(duration: 0.3f);
        levelCompleteLabel.Show();

        // Display the result image from the level's recipe requirements
        resultImage.sprite = LevelController.Level.Requirements.Recipe.ResultPreview;

        // Show the continue button with a fade effect
        continueButtonFade.Show(0.3f);

        // Set the current reward from the level's coin reward
        currentReward = LevelController.Level.CoinsReward;

        // Show the reward label with a push scale animation
        ShowRewardLabel(currentReward, false, 0.3f, delegate
        {
            rewardLabel.RectTransform.DOPushScale(Vector3.one * 1.1f, Vector3.one, 0.2f, 0.2f).OnComplete(delegate
            {
                FloatingCloud.SpawnCurrency(coinsHash, rewardLabel.RectTransform, currencyPanel.RectTransform, 10, "", () =>
                {
                    CurrenciesController.Add(CurrencyType.Coins, currentReward);

                    // Show the multiply reward button after the currency animation completes
                    multiplyRewardButtonFade.Show();
                });
            });
        });

        // Check if the current level is the final stage (stage 20)
        int stageNumber = SaveController.LevelId;
        Debug.Log("Current Stage Number: " + stageNumber);
        if (stageNumber == 19)
        {
            // Disable the continue button for the final stage
            continueButton.interactable = false;
        }
    }


        public override void PlayHideAnimation()
        {
            backgroundFade.Hide(HideDuration);

            Tween.DelayedCall(HideDuration, delegate
            {
                canvas.enabled = false;
                isPageDisplayed = false;

                UIController.OnPageClosed(this);
            });
        }
        #endregion

        #region RewardLabel

        public void ShowRewardLabel(float rewardAmounts, bool immediately = false, float duration = 0.3f, Action onComplted = null)
        {
            rewardLabel.Show(immediately: immediately);

            if (immediately)
            {
                rewardAmountText.text = "+" + rewardAmounts;
                onComplted?.Invoke();

                return;
            }

            rewardAmountText.text = "+" + 0;

            Tween.DoFloat(0, rewardAmounts, duration, (float value) =>
            {
                rewardAmountText.text = "+" + (int)value;
            }).OnComplete(delegate
            {
                onComplted?.Invoke();
            });
        }

        #endregion

        #region Buttons

        public void MultiplyRewardButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            if (noThanksAppearTween != null && noThanksAppearTween.IsActive)
            {
                noThanksAppearTween.Kill();
            }

            AdsManager.ShowRewardBasedVideo((bool success) =>
            {
                if (success)
                {
                    int rewardMult = 3;

                    noThanksButton.interactable = false;
                    noThanksButtonText.Hide(immediately: true);
                    multiplyRewardButtonFade.Hide(immediately: true);
                    multiplyRewardButton.interactable = false;

                    ShowRewardLabel(currentReward * rewardMult, false, 0.3f, delegate
                    {
                        FloatingCloud.SpawnCurrency(coinsHash, rewardLabel.RectTransform, currencyPanel.RectTransform, 10, "", () =>
                        {
                            CurrenciesController.Add(CurrencyType.Coins, currentReward * rewardMult);

                            noThanksButton.interactable = true;
                            continueButton.gameObject.SetActive(true);
                            continueButtonFade.Show(immediately:true);
                        });
                    });
                }
                else
                {
                    ContinueButton();
                }
            });
        }

        public void ContinueButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            GameController.NextLevel();

            UIController.HidePage<UIComplete>();
        }

        public void QuitInMenuButton()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            GameController.CloseLevel();
            UIController.HidePage<UIComplete>();
            UIController.ShowPage<UIMainMenu>();

            LivesManager.AddLife();
        }

        #endregion
    }
}
