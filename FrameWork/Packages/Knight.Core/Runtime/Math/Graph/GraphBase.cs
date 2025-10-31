using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    [ExecuteInEditMode]
    public class GraphBase : MonoBehaviour
    {
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public Color MainColor = Color.red;
        public float MainAlpha = 0.1f;

        protected Mesh Mesh;
        protected Material Material;

        protected List<Vector3> mVertexes;
        protected List<int> mIndices;

        private void Awake()
        {
            this.MeshFilter = this.gameObject.ReceiveComponent<MeshFilter>();
            this.MeshRenderer = this.gameObject.ReceiveComponent<MeshRenderer>();

            this.CreateMaterail();
            this.CreateMesh();
        }

        private void OnEnable()
        {
            if (!this.Mesh)
                this.CreateMesh();
            if (!this.Material)
                this.CreateMaterail();

            this.UpdateMesh();
        }

        private void OnDestroy()
        {
            UtilTool.SafeDestroy(this.Mesh);
            UtilTool.SafeDestroy(this.Material);
        }

        private void Update()
        {
            this.UpdateMesh();
        }

        protected virtual void CreateMesh()
        {
            UtilTool.SafeDestroy(this.Mesh);

            this.mVertexes = new List<Vector3>();
            this.mIndices = new List<int>();

            this.Mesh = new Mesh();
            this.Mesh.MarkDynamic();
            this.MeshFilter.mesh = this.Mesh;

            this.CreateVertices();

            this.Mesh.SetVertices(this.mVertexes);
            this.Mesh.SetIndices(this.mIndices, MeshTopology.Triangles, 0);
        }

        protected virtual void UpdateMesh()
        {
            this.CreateVertices();
            if (this.Mesh)
            {
                this.Mesh.Clear();
                this.Mesh.SetVertices(this.mVertexes);
                this.Mesh.SetIndices(this.mIndices, MeshTopology.Triangles, 0);
            }
            if (this.Material)
            {
                var rMainColor = this.MainColor;
                rMainColor.a = this.MainAlpha;
                this.Material.SetColor("_TintColor", rMainColor);
            }
        }

        protected virtual void CreateMaterail()
        {
            UtilTool.SafeDestroy(this.Material);

            this.Material = new Material(Shader.Find("LMD/Effect/Base-AlphaBlend"));
            var rMainColor = this.MainColor;
            rMainColor.a = this.MainAlpha;
            this.Material.SetColor("_TintColor", rMainColor);
            this.MeshRenderer.material = this.Material;
        }

        protected virtual void CreateVertices()
        {

        }
    }
}
