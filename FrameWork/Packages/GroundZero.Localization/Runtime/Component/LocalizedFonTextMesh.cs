using Knight.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GroundZero.Localization
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMesh))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LocalizedFonTextMesh : MonoBehaviour
    {
        public TextMesh TextMesh;
        public MeshRenderer MeshRenderer;
        public string LocalizedAsset = "msyhbd";
        private void Awake()
        {
            if (!this.TextMesh)
                this.TextMesh = this.GetComponent<TextMesh>();
            if (!this.MeshRenderer)
                this.MeshRenderer = this.GetComponent<MeshRenderer>();
        }
        public void Initialize()
        {
            if (this.TextMesh != null && !string.IsNullOrEmpty(this.LocalizedAsset))
            {
                this.TextMesh.font = LocalizationManager.Instance.GetMultiLanFont(this.LocalizedAsset);
                this.MeshRenderer.material = this.TextMesh.font.material;
            }
        }
    }
}
