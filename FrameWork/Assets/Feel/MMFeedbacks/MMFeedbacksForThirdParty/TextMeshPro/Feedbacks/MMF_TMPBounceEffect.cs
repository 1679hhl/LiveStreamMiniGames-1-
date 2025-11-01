using MoreMountains.Tools;
using UnityEngine;
using System.Collections;

#if MM_UGUI2
using TMPro;
#endif
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
    /// <summary>
    /// This feedback lets you apply a bounce animation effect to a target TMP_Text over time.
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback lets you apply a bounce animation effect to a target TMP_Text over time.")]
    #if MM_UGUI2
    [FeedbackPath("TextMesh Pro/TMP Bounce Effect")]
    #endif
    [MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro")]
    public class MMF_TMPBounceEffect : MMF_Feedback
    {
        /// a static bool used to disable all feedbacks of this type at once
        public static bool FeedbackTypeAuthorized = true;

        #if UNITY_EDITOR
        public override Color FeedbackColor => MMFeedbacksInspectorColors.TMPColor;

        public override string RequiresSetupText => "This feedback requires that a TargetTMPText be set to work properly. You can set one below.";
        #endif

        #if UNITY_EDITOR && MM_UGUI2
        public override bool EvaluateRequiresSetup() => TargetTMPText == null;

        public override string RequiredTargetText => TargetTMPText != null ? TargetTMPText.name : "";
        #endif

        #if MM_UGUI2
        public override bool HasAutomatedTargetAcquisition => true;
        protected override void AutomateTargetAcquisition() => TargetTMPText = FindAutomatedTarget<TMP_Text>();

        [MMFInspectorGroup("Target", true, 12, true)]
        [Tooltip("The TMP_Text component to control")]
        public TMP_Text TargetTMPText;
        #endif

        [MMFInspectorGroup("Bounce Settings", true, 16)]
        [Tooltip("The vertical offset for the bounce effect (in Unity units)")]
        public float BounceOffset = 10f;

        [Tooltip("The scale factor for the bounce effect")]
        public float BounceScale = 1.5f;

        [Tooltip("The rotation angle for the bounce effect (in degrees)")]
        public float BounceRotation = 15f;

        [Tooltip("The interval between each character's bounce (in seconds)")]
        public float BounceInterval = 0.1f;

        [Tooltip("The duration of each character's bounce (in seconds)")]
        public float BounceDuration = 0.05f;

        protected Coroutine _coroutine;

        /// <summary>
        /// On play we trigger the bounce animation effect
        /// </summary>
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            #if MM_UGUI2
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            if (TargetTMPText == null)
            {
                return;
            }

            _coroutine = Owner.StartCoroutine(AnimateBounceEffect());
            #endif
        }

        /// <summary>
        /// On stop we interrupt the animation if it's still running
        /// </summary>
        protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            #if MM_UGUI2
            if (!Active || !FeedbackTypeAuthorized)
            {
                return;
            }
            if (_coroutine != null)
            {
                Owner.StopCoroutine(_coroutine);
                _coroutine = null;
            }
            base.CustomStopFeedback(position, feedbacksIntensity);
            #endif
        }

        #if MM_UGUI2
        private IEnumerator AnimateBounceEffect()
        {
            if (TargetTMPText == null)
            {
                yield break;
            }

            string originalText = TargetTMPText.text;
            TMP_TextInfo textInfo = TargetTMPText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TargetTMPText.ForceMeshUpdate();
                if (i >= textInfo.characterCount || !textInfo.characterInfo[i].isVisible)
                {
                    continue;
                }

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Vector3[] sourceVertices = textInfo.meshInfo[materialIndex].vertices;

                Vector3[] originalVertices = new Vector3[4];
                for (int j = 0; j < 4; j++)
                {
                    originalVertices[j] = sourceVertices[vertexIndex + j];
                }

                Vector3 center = (originalVertices[0] + originalVertices[2]) / 2f;
                Vector3 offset = new Vector3(0, BounceOffset, 0);
                float rotationRad = BounceRotation * Mathf.Deg2Rad;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 localPos = originalVertices[j] - center;
                    localPos *= BounceScale;

                    float newX = localPos.x * Mathf.Cos(rotationRad) - localPos.y * Mathf.Sin(rotationRad);
                    float newY = localPos.x * Mathf.Sin(rotationRad) + localPos.y * Mathf.Cos(rotationRad);
                    localPos = new Vector3(newX, newY, localPos.z);

                    sourceVertices[vertexIndex + j] = center + localPos + offset;
                }

                TargetTMPText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                yield return new WaitForSeconds(BounceDuration);

                TargetTMPText.ForceMeshUpdate();
                textInfo = TargetTMPText.textInfo;
                if (i < textInfo.characterCount && textInfo.characterInfo[i].isVisible)
                {
                    sourceVertices = textInfo.meshInfo[materialIndex].vertices;
                    for (int j = 0; j < 4; j++)
                    {
                        sourceVertices[vertexIndex + j] = originalVertices[j];
                    }
                    TargetTMPText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                }

                yield return new WaitForSeconds(BounceInterval - BounceDuration);
            }
        }
        #endif
    }
}
