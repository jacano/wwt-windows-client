using System;
using System.Collections.Generic;
using SharpDX;
using TerraViewer;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x0200002F RID: 47
	public class Mesh
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000907D File Offset: 0x0000727D
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00009085 File Offset: 0x00007285
		public Vector3d[] Vertices { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000908E File Offset: 0x0000728E
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00009096 File Offset: 0x00007296
		public Vector3[] RawVertices { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000909F File Offset: 0x0000729F
		public int VertexCount
		{
			get
			{
				if (this.Vertices != null)
				{
					return this.Vertices.Length;
				}
				return 0;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000090B3 File Offset: 0x000072B3
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000090BB File Offset: 0x000072BB
		public List<List<int>> TriangleStrips { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000090C4 File Offset: 0x000072C4
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000090CC File Offset: 0x000072CC
		public List<List<int>> TriangleFans { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000090D5 File Offset: 0x000072D5
		// (set) Token: 0x06000153 RID: 339 RVA: 0x000090DD File Offset: 0x000072DD
		public List<int> Triangles { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000090E6 File Offset: 0x000072E6
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000090EE File Offset: 0x000072EE
		public Dictionary<string, string> KeyValuePairs { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000090F7 File Offset: 0x000072F7
		// (set) Token: 0x06000157 RID: 343 RVA: 0x000090FF File Offset: 0x000072FF
		public Vector3[] Normals { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00009108 File Offset: 0x00007308
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00009110 File Offset: 0x00007310
		public Vector2[] TextureUv { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00009119 File Offset: 0x00007319
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00009121 File Offset: 0x00007321
		public float[] ApplicationSpecificFloats { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000912A File Offset: 0x0000732A
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00009132 File Offset: 0x00007332
		public Vector2[] ApplicationSpecificVector2Fs { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000913B File Offset: 0x0000733B
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00009143 File Offset: 0x00007343
		public Vector3[] ApplicationSpecificVector3Fs { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000914C File Offset: 0x0000734C
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00009154 File Offset: 0x00007354
		public ulong[] ApplicationSpecificIDs { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000915D File Offset: 0x0000735D
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00009165 File Offset: 0x00007365
		public Vector2[] DetailTextureUv { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000916E File Offset: 0x0000736E
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00009176 File Offset: 0x00007376
		public Texture DetailTexture { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000917F File Offset: 0x0000737F
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00009187 File Offset: 0x00007387
		public TriangleWindingOrder WindingOrder { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00009190 File Offset: 0x00007390
		public bool IsValid
		{
			get
			{
				bool flag = this.Vertices != null && (this.TriangleStrips.Count != 0 || this.TriangleFans.Count != 0 || this.Triangles.Count != 0);
				if (this.Vertices != null)
				{
					flag = (flag && (this.Normals == null || this.Vertices.Length == this.Normals.Length));
					flag = (flag && (this.TextureUv == null || this.Vertices.Length == this.TextureUv.Length));
					flag = (flag && (this.ApplicationSpecificFloats == null || this.Vertices.Length == this.ApplicationSpecificFloats.Length));
					flag = (flag && (this.ApplicationSpecificVector2Fs == null || this.Vertices.Length == this.ApplicationSpecificVector2Fs.Length));
					flag = (flag && (this.ApplicationSpecificVector3Fs == null || this.Vertices.Length == this.ApplicationSpecificVector3Fs.Length));
					flag = (flag && (this.ApplicationSpecificIDs == null || this.Vertices.Length == this.ApplicationSpecificIDs.Length));
				}
				return flag;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000092BB File Offset: 0x000074BB
		// (set) Token: 0x0600016A RID: 362 RVA: 0x000092C3 File Offset: 0x000074C3
		public Vector3d Centroid { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000092CC File Offset: 0x000074CC
		public Vector3d GeometricCentroid
		{
			get
			{
				if (this.Vertices == null)
				{
					return new Vector3d(0.0, 0.0, 0.0);
				}
				double num = 0.0;
				double num2 = 0.0;
				double num3 = 0.0;
				foreach (Vector3d vector3d in this.Vertices)
				{
					num += vector3d.X;
					num2 += vector3d.Y;
					num3 += vector3d.Z;
				}
				return new Vector3d(num / (double)this.VertexCount, num2 / (double)this.VertexCount, num3 / (double)this.VertexCount);
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00009388 File Offset: 0x00007588
		public Mesh(TriangleWindingOrder windingOrder)
		{
			this.TriangleStrips = new List<List<int>>();
			this.TriangleFans = new List<List<int>>();
			this.Triangles = new List<int>();
			this.KeyValuePairs = new Dictionary<string, string>();
			this.WindingOrder = windingOrder;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000093C4 File Offset: 0x000075C4
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
			int num = 0;
			while (num + 2 < this.Triangles.Count)
			{
				int value = this.Triangles[num];
				this.Triangles[num] = this.Triangles[num + 1];
				this.Triangles[num + 1] = value;
				num += 3;
			}
			List<List<int>> list = new List<List<int>>();
			foreach (List<int> list2 in this.TriangleFans)
			{
				List<int> list3 = new List<int>(list2.Count);
				if (list2.Count >= 1)
				{
					list3[0] = list2[0];
					for (int i = 1; i < list2.Count; i++)
					{
						list3[i] = list2[list2.Count - i];
					}
				}
				list.Add(list3);
			}
			this.TriangleFans = list;
			for (int j = 0; j < this.TriangleStrips.Count; j++)
			{
				this.TriangleStrips[j].Insert(0, this.TriangleStrips[j][0]);
			}
		}
	}
}
