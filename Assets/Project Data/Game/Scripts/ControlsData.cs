using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon.BubbleMerge
{
    [CreateAssetMenu(menuName = "Content/Data/Controls Settings", fileName = "Controls Settings")]
    public class ControlsData : ScriptableObject
    {
        private static ControlsData instance;

        public float controlsPower = 3f;
        public static float ControlsPower => instance.controlsPower;

        public AnimationCurve controlsCurve;
        public static AnimationCurve ControlsCurve => instance.controlsCurve;

        public void Init()
        {
            instance = this;
        }
    }
}