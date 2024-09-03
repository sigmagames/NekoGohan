using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Watermelon.BubbleMerge;
using Watermelon.IAPStore;
using Watermelon.Map;

namespace Watermelon
{
    public class UIMainMenu : UIPage
    {
        public readonly float STORE_AD_RIGHT_OFFSET_X = 300F;

        [Space]
        [SerializeField] RectTransform tapToPlayRect;
        [SerializeField] UILevelPopUp levelPopUp;

        [Space]
        [SerializeField] UIScaleAnimation coinsLabelScalable;
        [SerializeField] CurrencyUIPanelSimple coinsPanel;

        [Space]
        [SerializeField] UIMainMenuButton storeButton;
        [SerializeField] UIMainMenuButton noAdsButton;

        [Space]
        [SerializeField] AddLivesPanel addLivesPanel;
        [SerializeField] LivesIndicator indicator;
        
        [Space]
        [SerializeField] RectTransform safeAreaRectTransform;
        private TweenCase tapToPlayPingPong;
        private TweenCase showHideStoreAdButtonDelayTweenCase;

        private void OnEnable()
        {
            IAPManager.OnPurchaseComplete += OnAdPurchased;
        }

        private void OnDisable()
        {
            IAPManager.OnPurchaseComplete -= OnAdPurchased;
        }

        public override void Initialise()
        {
            coinsPanel.Initialise();
            coinsPanel.AddButton.onClick.AddListener(StoreButton);

            storeButton.Init(STORE_AD_RIGHT_OFFSET_X);
            noAdsButton.Init(STORE_AD_RIGHT_OFFSET_X);

            storeButton.Button.onClick.AddListener(StoreButton);           

            if (!AdsManager.IsForcedAdEnabled())
            {
                noAdsButton.Hide(true);
            } else
            {
                noAdsButton.Button.onClick.AddListener(NoAdButton);
            }

            NotchSaveArea.RegisterRectTransform(safeAreaRectTransform);
        }

        public void ShowLevelPopup(int levelId)
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);

            levelPopUp.Show(levelId);
        }

        public void ShowAddLivesPopup()
        {
            addLivesPanel.Show();
        }

        #region Show/Hide

        public override void PlayShowAnimation()
        {
            showHideStoreAdButtonDelayTweenCase?.Kill();

            HideAdButton(true);

            ShowTapToPlay(false);

            coinsLabelScalable.Show();
            storeButton.Show(false);
            UILevelNumberText.Show(false);

            showHideStoreAdButtonDelayTweenCase = Tween.DelayedCall(0.12f, delegate
            {
                ShowAdButton(immediately: false);
            });

            SettingsPanel.ShowPanel(false);

            UIController.OnPageOpened(this);

            MapBehavior.EnableScroll();

            indicator.transform.DOScale(1, 0.5f).SetEasing(Ease.Type.BackOut);
        }

        public override void PlayHideAnimation()
        {
            if (!isPageDisplayed)
                return;

            showHideStoreAdButtonDelayTweenCase?.Kill();

            isPageDisplayed = false;

            HideTapToPlayText(false);

            coinsLabelScalable.Hide();

            HideAdButton(immediately: false);

            showHideStoreAdButtonDelayTweenCase = Tween.DelayedCall(0.1f, delegate
            {
                storeButton.Hide(immediately: false);
            });

            SettingsPanel.HidePanel(false);

            Tween.DelayedCall(0.5f, delegate
            {
                UIController.OnPageClosed(this);
            });

            MapBehavior.DisableScroll();

            indicator.transform.DOScale(0, 0.5f).SetEasing(Ease.Type.BackIn);
        }

        #endregion

        #region Tap To Play Label

        public void ShowTapToPlay(bool immediately = true)
        {
            if (tapToPlayPingPong != null && tapToPlayPingPong.IsActive)
                tapToPlayPingPong.Kill();

            if (immediately)
            {
                tapToPlayRect.localScale = Vector3.one;

                tapToPlayPingPong = tapToPlayRect.transform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

                return;
            }

            // RESET
            tapToPlayRect.localScale = Vector3.zero;

            tapToPlayRect.DOPushScale(Vector3.one * 1.2f, Vector3.one, 0.35f, 0.2f, Ease.Type.CubicOut, Ease.Type.CubicIn).OnComplete(delegate
            {

                tapToPlayPingPong = tapToPlayRect.transform.DOPingPongScale(1.0f, 1.05f, 0.9f, Ease.Type.QuadIn, Ease.Type.QuadOut, unscaledTime: true);

            });

        }

        public void HideTapToPlayText(bool immediately = true)
        {
            if (tapToPlayPingPong != null && tapToPlayPingPong.IsActive)
                tapToPlayPingPong.Kill();

            if (immediately)
            {
                tapToPlayRect.localScale = Vector3.zero;

                return;
            }

            tapToPlayRect.DOPushScale(Vector3.one * 1.2f, Vector3.zero, 0.2f, 0.35f, Ease.Type.CubicOut, Ease.Type.CubicIn);
        }

        #endregion

        #region Ad Button Label

        private void ShowAdButton(bool immediately = false)
        {
            if (AdsManager.IsForcedAdEnabled())
            {
                noAdsButton.Show(immediately);
            }
            else
            {
                noAdsButton.Hide(immediately: true);
            }
        }

        private void HideAdButton(bool immediately = false)
        {
            if (immediately || AdsManager.IsForcedAdEnabled()) noAdsButton.Hide(immediately);
        }

        private void OnAdPurchased(ProductKeyType productKeyType)
        {
            if (productKeyType == ProductKeyType.NoAds || productKeyType == ProductKeyType.StarterPack)
            {
                HideAdButton(immediately: true);
            }
        }

        #endregion

        #region Buttons

        public void TapToPlayButtonTemp()
        {
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
            levelPopUp.Show(SaveController.LevelId);
        }

        public void StoreButton()
        {
            UIController.HidePage<UIMainMenu>();

            UIMainMenu uiMainMenu = UIController.GetPage<UIMainMenu>();
            UILevelNumberText.Hide(false);

            Tween.DelayedCall(0.5f, delegate
            {
                UIController.ShowPage<UIIAPStore>();

                // reopening main menu only after store page was opened throug main menu
                UIController.OnPageClosedEvent += OnIapStoreStoreClosed;
            });

            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        private void OnIapStoreStoreClosed(UIPage page, System.Type pageType)
        {
            if (pageType.Equals(typeof(UIIAPStore)))
            {
                UIController.OnPageClosedEvent -= OnIapStoreStoreClosed;

                UIController.ShowPage<UIMainMenu>();
            }
        }

        public void NoAdButton()
        {
            IAPManager.BuyProduct(ProductKeyType.NoAds);
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        #endregion
    }


}
