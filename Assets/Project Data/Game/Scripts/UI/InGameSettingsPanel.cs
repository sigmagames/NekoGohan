#pragma warning disable 0414, 0649

using System.Collections;
using UnityEngine;

namespace Watermelon
{
    public class InGameSettingsPanel : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] float initialDelay;
        [SerializeField] float elementDelay;
        [SerializeField] float elementMovementDuration;
        [SerializeField] float offsetPosition;
        [SerializeField] Ease.Type showEasing = Ease.Type.BackOut;
        [SerializeField] Ease.Type hideEasing = Ease.Type.BackIn;

        [Space]
        [SerializeField] UIScaleAnimation mainRect;

        [Header("Panel Paddings")]
        [SerializeField] float xPanelPosition;
        [SerializeField] float yPanelPosition;

        [Header("Element Paddings")]
        [SerializeField] float elementSpace;

        [SerializeField] SettingsButtonInfo[] settingsButtonsInfo;
        public SettingsButtonInfo[] SettingsButtonsInfo
        {
            get { return settingsButtonsInfo; }
        }

        private bool isActiveSettingsButton = false;
        public bool IsActiveSettingsButton => isActiveSettingsButton;

        private static bool IsPanelActive { get; set; }

        private bool isAnimationActive = false;

        private Vector2[] buttonPositions;
        public Vector2[] ButtonPositions
        {
            get { return buttonPositions; }
        }

        private void Awake()
        {
            // Disable all buttons
            for (int i = 0; i < settingsButtonsInfo.Length; i++)
            {
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(false);
            }

            InitPositions();
        }

        public void InitPositions()
        {
            Vector2 lastPosition = new Vector2(xPanelPosition, yPanelPosition);

            buttonPositions = new Vector2[settingsButtonsInfo.Length];
            for (int i = 0; i < buttonPositions.Length; i++)
            {
                if (settingsButtonsInfo[i].SettingsButton != null)
                {
                    settingsButtonsInfo[i].SettingsButton.Init(i, null);

                    if (settingsButtonsInfo[i].SettingsButton.IsActive())
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            settingsButtonsInfo[i].SettingsButton.RectTransform.gameObject.SetActive(true);
#endif

                        RectTransform button = settingsButtonsInfo[i].SettingsButton.RectTransform;

                        Vector2 buttonPosition = lastPosition;

                        lastPosition -= new Vector2(0, elementSpace);

                        button.anchoredPosition = new Vector2(xPanelPosition, buttonPosition.y);

                        buttonPositions[i] = buttonPosition;
                    }
                    else
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            settingsButtonsInfo[i].SettingsButton.RectTransform.gameObject.SetActive(false);
#endif

                        buttonPositions[i] = Vector3.zero;
                    }
                }
                else
                {
                    Debug.Log("[Settings Panel]: Button reference is missing!");
                }
            }
        }

        public void SettingsButton()
        {
            if (isAnimationActive)
                return;

            if (isActiveSettingsButton)
            {
                Hide();

                isActiveSettingsButton = false;
            }
            else
            {
                Show();

                isActiveSettingsButton = true;
            }

            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

        private void Show()
        {
            isAnimationActive = true;

            StartCoroutine(ShowAnimationCoroutine(delegate
            {
                isAnimationActive = false;
            }));
        }

        private void Hide(bool immediately = false)
        {
            if (!immediately)
            {
                isAnimationActive = true;

                StartCoroutine(HideAnimationCoroutine(delegate
                {
                    isAnimationActive = false;
                }));

                isActiveSettingsButton = false;
                return;
            }

            for (int i = settingsButtonsInfo.Length - 1; i >= 0; i--)
            {
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(false);
            }

            isActiveSettingsButton = false;
        }

        public void ShowPanel(bool immediately = false)
        {
            if (IsPanelActive)
                return;

            IsPanelActive = true;
            mainRect.Show(immediately: immediately);
        }

        public void HidePanel(bool immediately = false)
        {
            if (!IsPanelActive)
                return;

            IsPanelActive = false;

            Hide(immediately);

            OnSettingButtonsHidden(immediately);
        }

        private IEnumerator ShowAnimationCoroutine(System.Action callback)
        {
            yield return new WaitForSeconds(initialDelay);

            TweenCase lastTweenCase = null;
            for (int i = 0; i < settingsButtonsInfo.Length; i++)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive())
                    continue;

                settingsButtonsInfo[i].SettingsButton.RectTransform.anchoredPosition = ButtonPositions[i].AddToX(offsetPosition);
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(true);

                lastTweenCase = settingsButtonsInfo[i].SettingsButton.RectTransform.DOAnchoredPosition(ButtonPositions[i], elementMovementDuration).SetEasing(showEasing);

                yield return new WaitForSeconds(elementDelay);
            }

            if (lastTweenCase != null)
            {
                while (!lastTweenCase.IsCompleted)
                {
                    yield return null;
                }

                callback.Invoke();
            }
            else
            {
                callback.Invoke();
            }
        }

        private IEnumerator HideAnimationCoroutine(System.Action callback)
        {
            yield return new WaitForSeconds(initialDelay);

            TweenCase lastTweenCase = null;
            for (int i = settingsButtonsInfo.Length - 1; i >= 0; i--)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive())
                    continue;

                int index = i;
                lastTweenCase = settingsButtonsInfo[i].SettingsButton.RectTransform.DOAnchoredPosition(ButtonPositions[i].AddToX(offsetPosition), elementMovementDuration).SetEasing(hideEasing).OnComplete(delegate
                {
                    settingsButtonsInfo[index].SettingsButton.gameObject.SetActive(false);
                });

                yield return new WaitForSeconds(elementDelay);
            }

            if (lastTweenCase != null)
            {
                while (!lastTweenCase.IsCompleted)
                {
                    yield return null;
                }

                callback.Invoke();
            }
            else
            {
                callback.Invoke();
            }
        }

        private void OnSettingButtonsHidden(bool immediately = false)
        {
            mainRect.Hide(immediately:immediately);
        }
    }
}