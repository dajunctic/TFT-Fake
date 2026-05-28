using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Dajunctic
{
    public class FloatingText : BaseView
    {
        [SerializeField] private TextMeshPro textMesh;
        
        [Header("Normal Damage Styling")]
        [SerializeField] private Color physicalColor = new Color(0.9f, 0.3f, 0.26f); // Orange Red
        [SerializeField] private Color magicalColor = new Color(0.23f, 0.51f, 0.96f); // Indigo Blue
        [SerializeField] private Color trueColor = new Color(0.98f, 0.75f, 0.14f); // Golden Yellow
        [SerializeField] private float normalScale = 1.0f;
        [SerializeField] private float normalDuration = 1.0f;
        
        [Header("Critical Damage Styling")]
        [SerializeField] private Color physicalCritColor = new Color(1f, 0.2f, 0.2f); // Vibrant Red
        [SerializeField] private Color magicalCritColor = new Color(0.7f, 0.1f, 0.9f); // Vibrant Purple/Magenta
        [SerializeField] private Color trueCritColor = new Color(0.98f, 0.75f, 0.14f); // Golden Yellow
        [SerializeField] private Color critOutlineColor = new Color(1f, 0.8f, 0f); // Bright Gold
        [SerializeField] private float critScale = 1.7f;
        [SerializeField] private float critDuration = 1.4f;

        private Sequence tweenSequence;

        public void Setup(float damageAmount, DamageType damageType, bool isCritical)
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshPro>();
                if (textMesh == null) textMesh = GetComponentInChildren<TextMeshPro>();
            }

            if (textMesh == null)
            {
                Debug.LogError("[FloatingText] TextMeshPro component is missing!");
                Despawn();
                return;
            }

            // Clean up any old tween sequences if spawned again from pool
            if (tweenSequence != null && tweenSequence.IsActive())
            {
                tweenSequence.Kill();
            }

            // Set initial state
            transform.localScale = Vector3.one;
            textMesh.alpha = 1f;

            // Format damage display
            int roundedDmg = Mathf.RoundToInt(damageAmount);
            string formattedText = roundedDmg.ToString();

            // Set scale and styling based on damage type and critical status
            float targetScale = normalScale;
            float duration = normalDuration;
            Color textColor = physicalColor;

            if (damageType == DamageType.PhysicalDamage) textColor = physicalColor;
            else if (damageType == DamageType.MagicalDamage) textColor = magicalColor;
            else if (damageType == DamageType.TrueDamage) textColor = trueColor;

            if (isCritical)
            {
                if (damageType == DamageType.PhysicalDamage) textColor = physicalCritColor;
                else if (damageType == DamageType.MagicalDamage) textColor = magicalCritColor;
                else if (damageType == DamageType.TrueDamage) textColor = trueCritColor;
                else textColor = physicalCritColor;

                targetScale = critScale;
                duration = critDuration;
                
                // Rich critical styling: outline, uppercase prefix, icon (sprite 0 for Physical/True, sprite 1 for Magical)
                if (damageType == DamageType.MagicalDamage)
                {
                    formattedText = $"<sprite=1> {formattedText}";
                }
                else
                {
                    formattedText = $"<sprite=0> {formattedText}";
                }
                
                textMesh.fontStyle = FontStyles.Bold;
                textMesh.outlineWidth = 0.2f;
                textMesh.outlineColor = critOutlineColor;
            }
            else
            {
                textMesh.fontStyle = FontStyles.Normal;
                textMesh.outlineWidth = 0f;
            }

            textMesh.text = formattedText;
            textMesh.color = textColor;

            // Face the main camera
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                transform.rotation = mainCam.transform.rotation;
            }

            // Scale to target base scale first
            transform.localScale = Vector3.one * targetScale;

            // Juice up the animations using DOTween!
            tweenSequence = DOTween.Sequence();

            // Scale punch/pop on spawn
            tweenSequence.Append(transform.DOPunchScale(Vector3.one * (targetScale * 0.4f), 0.2f, 10, 1f));

            // Float Upwards smoothly
            float floatDistance = isCritical ? 2.5f : 1.5f;
            tweenSequence.Join(transform.DOMoveY(transform.position.y + floatDistance, duration).SetEase(Ease.OutQuad));

            // Shake/Vibrate effect
            if (isCritical)
            {
                // Hard shake for crits
                tweenSequence.Join(transform.DOShakePosition(0.4f, new Vector3(0.15f, 0.15f, 0f), 20, 90f, false, false));
            }
            else
            {
                // Mild vibration for normal hits
                tweenSequence.Join(transform.DOPunchPosition(new Vector3(0.05f, 0f, 0f), 0.3f, 5, 0.5f));
            }

            // Scale down slightly and fade out at the end
            float fadeStartTime = duration * 0.5f;
            float fadeDuration = duration - fadeStartTime;
            
            tweenSequence.Insert(fadeStartTime, textMesh.DOFade(0f, fadeDuration));
            tweenSequence.Insert(fadeStartTime, transform.DOScale(targetScale * 0.8f, fadeDuration));

            // Despawn when completed
            tweenSequence.OnComplete(() =>
            {
                Despawn();
            });
        }

        private void OnDestroy()
        {
            if (tweenSequence != null && tweenSequence.IsActive())
            {
                tweenSequence.Kill();
            }
        }
    }
}
