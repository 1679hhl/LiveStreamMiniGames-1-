
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// 软裁切，被裁切组件
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MaskableGraphic))]
    public class SoftMaskable : MonoBehaviour, IMaterialModifier, IMeshModifier
    {
        private SoftMask mMask;
        private MaskableGraphic mMaskableGraphic;
        private Image mMaskImage;
        private Material mMaterial;
        private int mStencilID;
        private RectTransform mRectTransform;

        public int RenderQueue = 3000;

        private void LateUpdate()
        {
            if (this.mMaterial != null && this.mRectTransform != null)
            {
                this.RefreshValue();
            }
        }

        private void OnEnable()
        {
            this.OnRefresh();
        }

        private void OnDestroy()
        {
            this.mMaterial = null;
        }

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            if (this.mMaterial == null)
                this.RefreshMaterial();
            return this.mMaterial;
        }

        public void ModifyMesh(VertexHelper mesh)
        {
            this.RefreshMaterial();
        }

        public void ModifyMesh(Mesh mesh)
        {
        }

        public void RefreshMaterial()
        {
            if (this.mMaskableGraphic == null)
                return;

            if (this.mMask == null)
                this.CheckMaskImageInFather(this.transform.parent);
            if (this.mMaskImage == null && this.mMask != null)
                this.mMaskImage = this.mMask.GetComponent<Image>();

            if (this.mMaskableGraphic == null && this.mMask != null)
                this.mMaskableGraphic = this.mMask.GetComponent<MaskableGraphic>();

            if (this.mMaskableGraphic == null || this.mMaskImage == null || this.mMaskImage.sprite == null)
                return;

            if (this.mMaterial == null)
            {
                this.mMaterial = new Material(Shader.Find("UI/SoftMaskImage"));
                this.mMaterial.renderQueue = this.RenderQueue;
            }

            this.RefreshStencil();
            this.mMaterial.SetInt("_Stencil", this.mStencilID);
            this.mMaterial.SetTexture("_MaskTex", this.mMaskImage.sprite.texture);
            this.mRectTransform = this.GetComponent<RectTransform>();
            this.RefreshValue();

        }

        private void RefreshValue()
        {
            var rSize = new Vector4(this.mRectTransform.rect.width, this.mRectTransform.rect.height, this.mMaskImage.rectTransform.rect.width, this.mMaskImage.rectTransform.rect.height);

            this.mMaterial.SetVector("_Size", rSize);
            var rLocalScale = this.GetLocalScale(this.transform);
            var rLocalPosition = this.GetLocalPosition(this.transform);
            Vector4 rPositionAndScale = new Vector4(rLocalPosition.x, rLocalPosition.y, rLocalScale.x, rLocalScale.y);
            this.mMaterial.SetVector("_LocalPosition", rPositionAndScale);

            Image rImage = this.mMaskableGraphic as Image;
            if (rImage != null && rImage.sprite != null)
            {
                if (Application.isPlaying)
                {
                    this.mMaterial.SetVector("_SpriteRect", this.GetSpriteRect(rImage.sprite));
                    this.mMaterial.SetVector("_MaskSpriteRect", this.GetSpriteRect(this.mMaskImage.sprite));
                }
                else
                {
                    this.mMaterial.SetVector("_SpriteRect", new Vector4(0, 0, 1, 1));
                    this.mMaterial.SetVector("_MaskSpriteRect", new Vector4(0, 0, 1, 1));
                }
            }

            RawImage rRawImage = this.mMaskableGraphic as RawImage;
            if (rRawImage != null && rRawImage.texture != null)
            {
                if (Application.isPlaying)
                {
                    this.mMaterial.SetVector("_SpriteRect", new Vector4(0, 0, 1, 1));
                    this.mMaterial.SetVector("_MaskSpriteRect", this.GetSpriteRect(this.mMaskImage.sprite));
                }
                else
                {
                    this.mMaterial.SetVector("_SpriteRect", new Vector4(0, 0, 1, 1));
                    this.mMaterial.SetVector("_MaskSpriteRect", new Vector4(0, 0, 1, 1));
                }
            }
            this.mMaterial.SetMatrix("_RotationMatrix", Matrix4x4.Rotate(Quaternion.Euler(0, 0, this.GetLocalEulerZ(this.transform))));
            var rMaskPivot = this.mMaskImage.GetComponent<RectTransform>().pivot;
            this.mMaterial.SetVector("_Pivot", new Vector4(this.mRectTransform.pivot.x, this.mRectTransform.pivot.y, rMaskPivot.x, rMaskPivot.y));
        }

        private float GetLocalEulerZ(Transform rTransform)
        {
            float rAngle = rTransform.localEulerAngles.z;
            while (rTransform.parent != null && rTransform.parent != this.mMask.transform)
            {
                rAngle += rTransform.parent.localEulerAngles.z;
                rTransform = rTransform.parent;
            }
            return rAngle;
        }

        private Vector2 GetLocalScale(Transform rTransform)
        {
            Vector3 rScale = rTransform.localScale;
            while (rTransform.parent != null && rTransform.parent != this.mMask.transform)
            {
                rScale.x *= rTransform.parent.localScale.x;
                rScale.y *= rTransform.parent.localScale.y;
                rTransform = rTransform.parent;
            }
            return rScale;
        }
        private Vector3 GetLocalPosition(Transform rTransform)
        {
            var rPosition = rTransform.position - this.mMaskImage.transform.position;
            rPosition = Quaternion.Inverse(this.mMaskImage.transform.rotation) * rPosition;
            rPosition.x /= this.mMaskImage.transform.lossyScale.x;
            rPosition.y /= this.mMaskImage.transform.lossyScale.y;
            return rPosition;
        }

        private Vector4 GetSpriteRect(Sprite rSprite)
        {
            float fOffsetX = rSprite.textureRect.x / rSprite.texture.width;
            float fOffsetY = rSprite.textureRect.y / rSprite.texture.height;
            float fSizeX = rSprite.textureRect.width / rSprite.texture.width;
            float fSizeY = rSprite.textureRect.height / rSprite.texture.height;
            return new Vector4(fOffsetX, fOffsetY, fSizeX, fSizeY);
        }

        public void Refresh()
        {
            this.mMaterial = null;
            if (this.mMaskableGraphic == null)
                this.mMaskableGraphic = this.GetComponent<MaskableGraphic>();
            if (this.mMaskableGraphic != null)
            {
                this.mMaskableGraphic.enabled = false;
                this.mMaskableGraphic.enabled = true;
            }
        }

        private void OnRefresh()
        {
            // 获取父节点SoftMask组件
            if (this.mMask == null)
                this.CheckMaskImageInFather(this.transform);

            if (this.mMaskableGraphic == null)
                this.mMaskableGraphic = this.GetComponent<MaskableGraphic>();
            this.RefreshMaterial();
        }

        private void CheckMaskImageInFather(Transform rTrans)
        {
            this.mMask = rTrans.GetComponent<SoftMask>();
            if (this.mMask == null && rTrans.parent != null)
                this.CheckMaskImageInFather(rTrans.parent);
        }

        private void RefreshStencil()
        {
            this.mStencilID = MaskUtilities.GetStencilDepth(this.transform, null);
        }
    }
}