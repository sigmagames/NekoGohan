using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.BubbleMerge
{
    [System.Serializable]
    public class ArrowData
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] float startValue;
        [SerializeField] float endValue;

        [Space]
        [SerializeField] float vibrationStartValue;

        private Color defaultColor;
        private Color alphaColor;
        private Color tutorialColor;

        private bool isVibrationActivate;

        public void Initialise()
        {
            defaultColor = spriteRenderer.color;
            alphaColor = defaultColor.SetAlpha(0.0f);
            tutorialColor = defaultColor.SetAlpha(0.5f);
        }

        public void ResetVibration()
        {
            isVibrationActivate = false;
        }

        public void UpdateState(float state)
        {
            if (!isVibrationActivate && state >= vibrationStartValue)
            {
#if UNITY_IOS || UNITY_ANDROID
                Vibration.Vibrate(VibrationIntensity.Light);
#endif

                isVibrationActivate = true;
            }

            float realState = Mathf.InverseLerp(startValue, endValue, state);

            spriteRenderer.color = Color.Lerp(alphaColor, defaultColor, realState);
        }

        public void UpdateTutorialState(float state)
        {
            float realState = Mathf.InverseLerp(startValue, endValue, state);

            spriteRenderer.color = Color.Lerp(alphaColor, tutorialColor, realState);
        }
    }
}
