using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Microsoft.Bits.TileIO.GeometryEncoderDecoder;
using SevenZip.Sdk.Compression.Lzma;
using TerraViewer;

namespace DSMRender
{
	// Token: 0x02000037 RID: 55
	public class DSMTile
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x0000A02C File Offset: 0x0000822C
		public Texture11 LoadMeshFile(string filename, Vector3d localCenter, out Vector3d center, out double radius)
		{
			string path = filename + ".mesh";
			center = default(Vector3d);
			radius = 0.0;
			if (!File.Exists(path))
			{
				return null;
			}
			byte[] array = File.ReadAllBytes(path);
			if (array == null || array.Length < 50000)
			{
				return null;
			}
			if (array[0] == 31 && array[1] == 139)
			{
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
					{
						return this.LoadData(gzipStream, localCenter, out center, out radius);
					}
				}
			}
			if (array[0] == 93 && array[1] == 0)
			{
				using (MemoryStream memoryStream2 = new MemoryStream(array))
				{
					using (MemoryStream memoryStream3 = new MemoryStream())
					{
						DSMTile.DecodeLZMA(memoryStream2, memoryStream3);
						memoryStream3.Seek(0L, SeekOrigin.Begin);
						return this.LoadData(memoryStream3, localCenter, out center, out radius);
					}
				}
			}
			Texture11 result;
			using (MemoryStream memoryStream4 = new MemoryStream(array))
			{
				result = this.LoadData(memoryStream4, localCenter, out center, out radius);
			}
			return result;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000A17C File Offset: 0x0000837C
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000A184 File Offset: 0x00008384
		public ushort[] Subsets
		{
			get
			{
				return this._subsets;
			}
			set
			{
				this._subsets = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000A18D File Offset: 0x0000838D
		public int VertexFloatSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000A190 File Offset: 0x00008390
		public int TexCoordFloatSize
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000A193 File Offset: 0x00008393
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000A19B File Offset: 0x0000839B
		public IndexBuffer11 DsmIndex
		{
			get
			{
				return this.dsmIndex;
			}
			set
			{
				this.dsmIndex = value;
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000A1A4 File Offset: 0x000083A4
		private Texture11 LoadData(Stream stream, Vector3d localCenter, out Vector3d center, out double radius)
		{
			GeometryDecoder geometryDecoder = new GeometryDecoder();
			GeometryModel geometryModel = geometryDecoder.Decode(stream, TriangleWindingOrder.CCW);
			int vertexCount = geometryModel.Meshes[0].VertexCount;
			int num = geometryModel.Meshes[0].Triangles.Count;
			foreach (List<int> list in geometryModel.Meshes[0].TriangleStrips)
			{
				num += (list.Count - 2) * 3;
			}
			foreach (List<int> list2 in geometryModel.Meshes[0].TriangleFans)
			{
				num += (list2.Count - 2) * 3;
			}
			this._indices = new short[num];
			this.VertexBuffer = BufferPool11.GetPNTX2VertexBuffer(vertexCount);
			this.VertexBuffer.ComputeSphereOnUnlock = true;
			PositionNormalTexturedX2[] array = (PositionNormalTexturedX2[])this.VertexBuffer.Lock(0, 0);
			for (int i = 0; i < vertexCount; i++)
			{
				Vector3d position = Coordinates.XyzToGeo(new Vector3d(geometryModel.Meshes[0].Vertices[i].Y, geometryModel.Meshes[0].Vertices[i].X, geometryModel.Meshes[0].Vertices[i].Z));
				position.Subtract(localCenter);
				array[i].Position = position;
				array[i].Normal = localCenter;
				array[i].Normal.Normalize();
				array[i].Tu = geometryModel.Meshes[0].TextureUv[i].X;
				array[i].Tv = geometryModel.Meshes[0].TextureUv[i].Y;
			}
			this.VertexBuffer.Unlock();
			int num2 = this._indices.Length;
			int num3 = 0;
			for (int j = 0; j < geometryModel.Meshes[0].Triangles.Count; j++)
			{
				this._indices[num3++] = (short)geometryModel.Meshes[0].Triangles[j];
			}
			foreach (List<int> list3 in geometryModel.Meshes[0].TriangleStrips)
			{
				for (int k = 2; k < list3.Count; k++)
				{
					this._indices[num3++] = (short)list3[k - 2];
					this._indices[num3++] = (short)list3[k - 1];
					this._indices[num3++] = (short)list3[k];
				}
			}
			foreach (List<int> list4 in geometryModel.Meshes[0].TriangleFans)
			{
				for (int l = 2; l < list4.Count; l++)
				{
					this._indices[num3++] = (short)list4[0];
					this._indices[num3++] = (short)list4[l - 1];
					this._indices[num3++] = (short)list4[l];
				}
			}
			this.dsmIndex = new IndexBuffer11(RenderContext11.PrepDevice, this._indices);
			string text = geometryModel.Meshes[0].KeyValuePairs.ContainsKey("subsets") ? geometryModel.Meshes[0].KeyValuePairs["subsets"] : "";
			string[] array2 = new string[0];
			if (!string.IsNullOrEmpty(text))
			{
				array2 = text.Split(new char[]
				{
					','
				});
			}
			this._subsets = new ushort[array2.Length + 1];
			for (int m = 0; m < array2.Length; m++)
			{
				this._subsets[m] = (ushort)(3 * ushort.Parse(array2[m]));
			}
			this._subsets[array2.Length] = (ushort)num2;
			if (geometryModel.Textures.Count > 0)
			{
				Texture11 result = this.LoadBitsTexture(geometryModel.Textures[0]);
				radius = this.VertexBuffer.SphereRadius;
				center = this.VertexBuffer.SphereCenter;
				center.Add(localCenter);
				return result;
			}
			throw new Exception("Packet did not include texture data");
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000A6B8 File Offset: 0x000088B8
		private Texture11 LoadBitsTexture(Texture tex)
		{
			MemoryStream memoryStream = new MemoryStream(tex.Data);
			Texture11 result = Texture11.FromStream(memoryStream);
			memoryStream.Close();
			return result;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000A6E0 File Offset: 0x000088E0
		private static void DecodeLZMA(Stream inStream, Stream outStream)
		{
			byte[] array = new byte[5];
			if (inStream.Read(array, 0, 5) != 5)
			{
				throw new Exception("input .lzma is too short");
			}
			Decoder decoder = new Decoder();
			decoder.SetDecoderProperties(array);
			long num = 0L;
			for (int i = 0; i < 8; i++)
			{
				int num2 = inStream.ReadByte();
				if (num2 < 0)
				{
					throw new Exception("Can't Read 1");
				}
				num |= (long)((long)((ulong)((byte)num2)) << 8 * i);
			}
			long inSize = inStream.Length - inStream.Position;
			decoder.Code(inStream, outStream, inSize, num, null);
		}

		// Token: 0x0400015D RID: 349
		private const string SubsetKey = "subsets";

		// Token: 0x0400015E RID: 350
		private short[] _indices;

		// Token: 0x0400015F RID: 351
		private ushort[] _subsets;

		// Token: 0x04000160 RID: 352
		private IndexBuffer11 dsmIndex;

		// Token: 0x04000161 RID: 353
		public VertexBuffer11 VertexBuffer;
	}
}
