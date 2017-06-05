using System;
using System.Collections.Generic;
using SharpDX;
using TerraViewer;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x0200002B RID: 43
	public class GeometryModel
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00008A0C File Offset: 0x00006C0C
		public Tuple<Vector3d, Vector3d> BoundingBox
		{
			get
			{
				if (this.Meshes == null)
				{
					return new Tuple<Vector3d, Vector3d>(new Vector3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 0.0));
				}
				List<Vector3d> list = new List<Vector3d>();
				foreach (Mesh mesh in this.Meshes)
				{
					foreach (Vector3d vector3d in mesh.Vertices)
					{
						list.Add((this.TransformationMatrix != null) ? this.TransformationMatrix.Value.Transform(new Vector3d(vector3d)) : new Vector3d(vector3d));
					}
				}
				Vector3d minCoordinate = Vector3d.GetMinCoordinate(list);
				Vector3d maxCoordinate = Vector3d.GetMaxCoordinate(list);
				return new Tuple<Vector3d, Vector3d>(new Vector3d(minCoordinate), new Vector3d(maxCoordinate));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00008B38 File Offset: 0x00006D38
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00008B40 File Offset: 0x00006D40
		public Vector3? GeopositionUncertainty { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00008B49 File Offset: 0x00006D49
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00008B51 File Offset: 0x00006D51
		public Dictionary<string, string> KeyValuePairs { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00008B5A File Offset: 0x00006D5A
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00008B62 File Offset: 0x00006D62
		public string Id { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00008B6B File Offset: 0x00006D6B
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00008B73 File Offset: 0x00006D73
		public Matrix3d? TransformationMatrix { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00008B7C File Offset: 0x00006D7C
		// (set) Token: 0x0600012C RID: 300 RVA: 0x00008B84 File Offset: 0x00006D84
		public List<Mesh> Meshes { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00008B8D File Offset: 0x00006D8D
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00008B95 File Offset: 0x00006D95
		public List<Matrix3d> CameraMatrices { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00008B9E File Offset: 0x00006D9E
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00008BA6 File Offset: 0x00006DA6
		public List<Texture> Textures { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00008BAF File Offset: 0x00006DAF
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00008BB7 File Offset: 0x00006DB7
		public byte[] EncodedExistenceMap { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00008BC0 File Offset: 0x00006DC0
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00008BC8 File Offset: 0x00006DC8
		public byte[] EncodedTileMetadata { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00008BD1 File Offset: 0x00006DD1
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00008BD9 File Offset: 0x00006DD9
		public TriangleWindingOrder WindingOrder { get; private set; }

		// Token: 0x06000137 RID: 311 RVA: 0x00008BE2 File Offset: 0x00006DE2
		public GeometryModel(TriangleWindingOrder windingOrder)
		{
			this.WindingOrder = windingOrder;
			this.Meshes = new List<Mesh>();
			this.CameraMatrices = new List<Matrix3d>();
			this.Textures = new List<Texture>();
			this.KeyValuePairs = new Dictionary<string, string>();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00008C20 File Offset: 0x00006E20
		public void AddMeshes(ICollection<Mesh> meshes)
		{
			foreach (Mesh mesh in meshes)
			{
				if (mesh.WindingOrder != this.WindingOrder)
				{
					mesh.InvertTriangleWindingOrder();
				}
				this.Meshes.Add(mesh);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00008C84 File Offset: 0x00006E84
		public void ClearMeshes()
		{
			this.Meshes = new List<Mesh>();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008C91 File Offset: 0x00006E91
		public void ClearCameraMatrices()
		{
			this.CameraMatrices = new List<Matrix3d>();
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008C9E File Offset: 0x00006E9E
		public void ClearTextures()
		{
			this.Textures = new List<Texture>();
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008CAC File Offset: 0x00006EAC
		public void InvertTriangleWindingOrder()
		{
			if (this.WindingOrder == TriangleWindingOrder.CCW)
			{
				this.WindingOrder = TriangleWindingOrder.CW;
			}
			else
			{
				this.WindingOrder = TriangleWindingOrder.CCW;
			}
			for (int i = 0; i < this.Meshes.Count; i++)
			{
				this.Meshes[i].InvertTriangleWindingOrder();
			}
		}
	}
}
