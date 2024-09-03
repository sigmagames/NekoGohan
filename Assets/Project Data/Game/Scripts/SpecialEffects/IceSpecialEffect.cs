using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Watermelon;

namespace Watermelon.BubbleMerge
{
    public class IceSpecialEffect : BubbleSpecialEffect
    {
        [SerializeField] GameObject graphicsObject;
        [SerializeField] SpriteRenderer iceSpriteRenderer;
        [SerializeField] Sprite flyingObjectSprite;

        [Space]
        [SerializeField] string crackParticleName;

        [Space]
        [SerializeField] bool useVelocity;
        [SerializeField] float minVelocity;

        [Space]
        [SerializeField] int health = 1;
        public int Health { set { health = value; } get { return health; } }

        [SerializeField] Sprite[] crackSprites;

        private int currentHealth;
        private bool isIceActive;

        private TweenCase kinematicTweenCase;

        public override bool IsMergeAllowed()
        {
            return !isIceActive;
        }

        public void UpdateSpriteIce()
        {
            if(crackSprites.IsInRange(currentHealth - 1))
            {
                iceSpriteRenderer.sprite = crackSprites[currentHealth - 1];
            }
        }

        public void PlayFadeAnimation()
        {
            iceSpriteRenderer.color = iceSpriteRenderer.color.SetAlpha(0.0f);
            iceSpriteRenderer.DOFade(1.0f, 0.4f);
        }

        public void LaunchAndDisablePhysics(Vector3 force, float delay = 0.3f)
        {
            kinematicTweenCase.KillActive();

            Rigidbody2D bubleRididbody = linkedBubble.RB;

            bubleRididbody.isKinematic = false;
            bubleRididbody.AddForce(force, ForceMode2D.Impulse);

            kinematicTweenCase = Tween.DoFloat(1.0f, 0.0f, delay, (value) =>
            {
                bubleRididbody.velocity = bubleRididbody.velocity * value;
            }).OnComplete(() =>
            {
                bubleRididbody.isKinematic = true;
            });
        }

        public override void OnBubbleCollided(BubbleBehavior bubbleBehavior)
        {
            if (useVelocity && bubbleBehavior.RB.velocity.magnitude < minVelocity) return;
            if (bubbleBehavior.BubbleSpecialEffect != null && bubbleBehavior.BubbleSpecialEffect.EffectType == Type.Ice) return;

            Hit();

#if UNITY_IOS || UNITY_ANDROID
            Vibration.Vibrate(VibrationIntensity.Light);
#endif
        }

        public void Hit()
        {
            currentHealth--;

            AudioController.PlaySound(AudioController.Sounds.iceCrackSound);

            UpdateSpriteIce();

            ParticlesController.PlayParticle(crackParticleName).SetPosition(transform.position);

            if (currentHealth <= 0)
            {
                kinematicTweenCase.KillActive();

                linkedBubble.RB.isKinematic = false;
                linkedBubble.IsMagnetActive = true;

                graphicsObject.SetActive(false);

                isIceActive = false;

                DisableEffect();
            }
        }

        public override void OnBubbleMerged()
        {
            DisableEffect();
        }

        public override void OnBubbleDisabled()
        {
            kinematicTweenCase.KillActive();

            graphicsObject.SetActive(false);

            Destroy(gameObject);
        }

        public override void OnCreated()
        {
            isIceActive = true;

            linkedBubble.RB.isKinematic = true;
            linkedBubble.RB.velocity = Vector2.zero;
            linkedBubble.IsMagnetActive = false;

            currentHealth = health;

            iceSpriteRenderer.sprite = crackSprites[crackSprites.Length - 1];
        }

        public override bool IsBubbleActive()
        {
            return !isIceActive;
        }

        public void OnRequirementMet(RequirementBehavior requirementBehavior, RequirementCallback completeRequirement)
        {
            UIGame gameUI = UIController.GetPage<UIGame>();

            gameUI.FlyingObjects.Activate(transform.position, flyingObjectSprite, requirementBehavior, () =>
            {
                completeRequirement?.Invoke(false);
            });
        }

        public override bool IsDragAllowed()
        {
            return false;
        }
    }
}