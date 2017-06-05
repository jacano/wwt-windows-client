using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using SharpDX;
using TerraViewer;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x02000025 RID: 37
	public class GeometryDecoder
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00007514 File Offset: 0x00005714
		// (set) Token: 0x060000DC RID: 220 RVA: 0x0000751C File Offset: 0x0000571C
		public GeometryModel Model { get; private set; }

		// Token: 0x060000DD RID: 221 RVA: 0x00007525 File Offset: 0x00005725
		public GeometryDecoder()
		{
			this._meshesByInternalId = new Dictionary<ushort, Mesh>();
			this._segmentTexturesById = new Dictionary<ushort, GeometryDecoder.SegmentTextures>();
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00007568 File Offset: 0x00005768
		public GeometryModel Decode(Stream stream, TriangleWindingOrder windingOrder)
		{
			string text = null;
			ushort num = 0;
			byte b = 0;
			byte b2 = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			double num6 = 0.0;
			double num7 = 0.0;
			short num8 = 0;
			this.ReadHeader(stream, out text, out num, out b, out b2);
			this.Model = new GeometryModel(windingOrder);
			bool flag = false;
			while (!flag)
			{
				ushort num9 = 0;
				int num10 = 0;
				bool flag2 = this.ReadChunkHeader(stream, ref num9, ref num10);
				if (!flag2)
				{
					break;
				}
				byte[] array = new byte[num10];
				int num11 = stream.ReadArray(num10, array);
				if (num11 != array.Length)
				{
					throw new EndOfStreamException(string.Format("End of stream reading a chunk reached with {0} bytes left to read", num11));
				}
				MemoryStream stream2 = new MemoryStream(array);
				try
				{
					ushort num12 = num9;
					if (num12 <= 1024)
					{
						if (num12 <= 512)
						{
							switch (num12)
							{
							case 256:
								flag2 = this.DecodeTerrestrialBoundingBox(stream2, num10, ref num2, ref num3, ref num4, ref num5, ref num6, ref num7);
								break;
							case 257:
								flag2 = this.DecodeGeopositionUncertainty(stream2, num10);
								break;
							default:
								if (num12 == 512)
								{
									flag2 = this.DecodeKeyValuePairs(stream2, num10, windingOrder);
								}
								break;
							}
						}
						else if (num12 != 768)
						{
							if (num12 == 1024)
							{
								flag2 = this.DecodeTransformationMatrix(stream2, num10);
							}
						}
						else
						{
							flag2 = this.DecodeMeshIdentifier(stream2, num10);
						}
					}
					else if (num12 <= 1792)
					{
						switch (num12)
						{
						case 1280:
							flag2 = this.DecodeVertices(stream2, num10);
							break;
						case 1281:
							flag2 = this.DecodeTriangle(stream2, num10);
							break;
						default:
							if (num12 == 1792)
							{
								flag2 = this.DecodeCameras(stream2, num10);
							}
							break;
						}
					}
					else if (num12 != 2048)
					{
						switch (num12)
						{
						case 2304:
							flag2 = this.DecodeSegments(stream2, num10, ref num8);
							break;
						case 2305:
							flag2 = this.DecodeSegmentTextureCoordinates(stream2, num10);
							break;
						default:
							if (num12 == 65535)
							{
								flag = true;
							}
							break;
						}
					}
					else
					{
						flag2 = this.DecodeTexture(stream2, num10);
					}
				}
				catch (Exception ex)
				{
					this.Model = null;
					throw ex;
				}
			}
			for (int i = 0; i < (int)num8; i++)
			{
				ushort key = this._segmentsBuffer.SegmentId[i];
				Mesh mesh = null;
				if (this._meshesByInternalId.ContainsKey(key))
				{
					mesh = this._meshesByInternalId[key];
				}
				else
				{
					mesh = new Mesh(windingOrder);
					this._meshesByInternalId.Add(key, mesh);
				}
				int num13 = this._segmentsBuffer.OffsetFirstVertex[i];
				int num14 = this._segmentsBuffer.OffsetLastVertex[i];
				mesh.Vertices = new Vector3d[num14 - num13 + 1];
				mesh.RawVertices = new Vector3[num14 - num13 + 1];
				mesh.Normals = ((this._vertexBuffer.Normals == null) ? null : new Vector3[num14 - num13 + 1]);
				mesh.TextureUv = ((this._vertexBuffer.TextureUv == null) ? null : new Vector2[num14 - num13 + 1]);
				mesh.ApplicationSpecificFloats = ((this._vertexBuffer.ApplicationSpecificFloats == null) ? null : new float[num14 - num13 + 1]);
				mesh.ApplicationSpecificVector2Fs = ((this._vertexBuffer.ApplicationSpecificVector2Fs == null) ? null : new Vector2[num14 - num13 + 1]);
				mesh.ApplicationSpecificVector3Fs = ((this._vertexBuffer.ApplicationSpecificVector3Fs == null) ? null : new Vector3[num14 - num13 + 1]);
				mesh.ApplicationSpecificIDs = ((this._vertexBuffer.ApplicationSpecificIDs == null) ? null : new ulong[num14 - num13 + 1]);
				mesh.Centroid = this._vertexBuffer.Offset;
				for (int j = num13; j <= num14; j++)
				{
					int num15 = j - num13;
					mesh.Vertices[num15] = this._vertexBuffer.Vertices[j];
					mesh.RawVertices[num15] = this._vertexBuffer.RawVertices[j];
					if (mesh.Normals != null)
					{
						mesh.Normals[num15] = this._vertexBuffer.Normals[j];
					}
					if (mesh.TextureUv != null)
					{
						mesh.TextureUv[num15] = this._vertexBuffer.TextureUv[j];
					}
					if (mesh.ApplicationSpecificFloats != null)
					{
						mesh.ApplicationSpecificFloats[num15] = this._vertexBuffer.ApplicationSpecificFloats[j];
					}
					if (mesh.ApplicationSpecificVector2Fs != null)
					{
						mesh.ApplicationSpecificVector2Fs[num15] = this._vertexBuffer.ApplicationSpecificVector2Fs[j];
					}
					if (mesh.ApplicationSpecificVector3Fs != null)
					{
						mesh.ApplicationSpecificVector3Fs[num15] = this._vertexBuffer.ApplicationSpecificVector3Fs[j];
					}
					if (mesh.ApplicationSpecificIDs != null)
					{
						mesh.ApplicationSpecificIDs[num15] = this._vertexBuffer.ApplicationSpecificIDs[j];
					}
				}
				int num16 = this._segmentsBuffer.OffsetFirstTriangleStrip[i];
				int num17 = this._segmentsBuffer.OffsetLastTriangleStrip[i];
				mesh.TriangleStrips = this._triangleBuffer.TriangleStrips.GetRange(num16, num17 - num16 + 1);
				int num18 = this._segmentsBuffer.OffsetFirstTriangleFan[i];
				int num19 = this._segmentsBuffer.OffsetLastTriangleFan[i];
				mesh.TriangleFans = this._triangleBuffer.TriangleFans.GetRange(num18, num19 - num18 + 1);
				int num20 = this._segmentsBuffer.OffsetFirstTriangle[i];
				int num21 = this._segmentsBuffer.OffsetLastTriangle[i];
				mesh.Triangles = this._triangleBuffer.Triangles.GetRange(num20, num21 - num20 + 1);
				if (this._segmentTexturesById.ContainsKey(key))
				{
					short textureIndex = this._segmentTexturesById[key].TextureIndex;
					mesh.DetailTexture = this.Model.Textures[(int)textureIndex];
					mesh.DetailTextureUv = this._segmentTexturesById[key].DetailTextureUv;
				}
				foreach (List<int> list in mesh.TriangleStrips)
				{
					for (int k = 0; k < list.Count; k++)
					{
						List<int> list2;
						int index;
						(list2 = list)[index = k] = list2[index] - num13;
					}
				}
				foreach (List<int> list3 in mesh.TriangleFans)
				{
					for (int l = 0; l < list3.Count; l++)
					{
						List<int> list4;
						int index2;
						(list4 = list3)[index2 = l] = list4[index2] - num13;
					}
				}
				for (int m = 0; m < mesh.Triangles.Count; m++)
				{
					List<int> triangles;
					int index3;
					(triangles = mesh.Triangles)[index3 = m] = triangles[index3] - num13;
				}
			}
			ICollection<Mesh> meshes = this._meshesByInternalId.Values.ToList<Mesh>();
			this.Model.AddMeshes(meshes);
			return this.Model;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00007D84 File Offset: 0x00005F84
		public static bool IsDataGzipped(byte[] data)
		{
			if (data == null || data.Length < 3)
			{
				return false;
			}
			for (int i = 0; i < GeometryDecoder.GzipSignature.Length; i++)
			{
				if (data[i] != GeometryDecoder.GzipSignature[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00007DC0 File Offset: 0x00005FC0
		public GeometryModel DecodeGzipped(byte[] bytes, TriangleWindingOrder windingOrder)
		{
			using (GZipStream gzipStream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
			{
				MemoryStream memoryStream = new MemoryStream();
				gzipStream.CopyTo(memoryStream);
				bytes = memoryStream.ToArray();
			}
			return this.Decode(bytes, windingOrder);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00007E14 File Offset: 0x00006014
		public GeometryModel Decode(byte[] bytes, TriangleWindingOrder windingOrder)
		{
			MemoryStream stream = new MemoryStream(bytes);
			return this.Decode(stream, windingOrder);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007E30 File Offset: 0x00006030
		private void ReadHeader(Stream stream, out string stringId, out ushort headerLength, out byte majorVersion, out byte minorVersion)
		{
			byte[] array = new byte["geomodel".Length];
			if ("geomodel".Length != stream.ReadArray("geomodel".Length, array))
			{
				throw new EndOfStreamException(string.Format("End of stream reached reading header", new object[0]));
			}
			stringId = Encoding.UTF8.GetString(array);
			if (stringId != "geomodel")
			{
				throw new InvalidDataException(string.Format("Wrong magic number in header.", new object[0]));
			}
			headerLength = stream.ReadShort();
			majorVersion = stream.ReadSingleByte();
			minorVersion = stream.ReadSingleByte();
			if (headerLength - 12 > 0)
			{
				byte[] buffer = new byte[(int)(headerLength - 12)];
				stream.ReadArray((int)(headerLength - 12), buffer);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007EEB File Offset: 0x000060EB
		private bool ReadChunkHeader(Stream stream, ref ushort chunkId, ref int chunkLength)
		{
			chunkId = stream.ReadShort();
			chunkLength = (int)stream.ReadInt();
			return true;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007EFE File Offset: 0x000060FE
		private bool DecodeTerrestrialBoundingBox(Stream stream, int length, ref double x1, ref double y1, ref double z1, ref double x2, ref double y2, ref double z2)
		{
			x1 = stream.ReadDouble();
			y1 = stream.ReadDouble();
			z1 = stream.ReadDouble();
			x2 = stream.ReadDouble();
			y2 = stream.ReadDouble();
			z2 = stream.ReadDouble();
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007F38 File Offset: 0x00006138
		private bool DecodeGeopositionUncertainty(Stream stream, int length)
		{
			float num = stream.ReadFloat();
			float num2 = stream.ReadFloat();
			float num3 = stream.ReadFloat();
			Vector3 value = new Vector3(num, num2, num3);
			this.Model.GeopositionUncertainty = new Vector3?(value);
			return true;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007F78 File Offset: 0x00006178
		private bool DecodeKeyValuePairs(Stream stream, int length, TriangleWindingOrder windingOrder)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			ushort num = stream.ReadShort();
			for (ushort num2 = stream.ReadShort(); num2 > 0; num2 -= 1)
			{
				string text = null;
				string key = stream.ReadString();
				ushort num3 = stream.ReadShort();
				for (int i = 0; i < (int)num3; i++)
				{
					string text2 = stream.ReadString();
					text = ((i == 0) ? text2 : text);
				}
				dictionary[key] = text;
			}
			if (num == 65535)
			{
				using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string key2 = enumerator.Current;
						this.Model.KeyValuePairs.Add(key2, dictionary[key2]);
					}
					return true;
				}
			}
			Mesh mesh;
			if (this._meshesByInternalId.ContainsKey(num))
			{
				mesh = this._meshesByInternalId[num];
			}
			else
			{
				mesh = new Mesh(windingOrder);
				this._meshesByInternalId.Add(num, mesh);
			}
			foreach (string key3 in dictionary.Keys)
			{
				mesh.KeyValuePairs.Add(key3, dictionary[key3]);
			}
			return true;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000080D4 File Offset: 0x000062D4
		private bool DecodeMeshIdentifier(Stream stream, int length)
		{
			this.Model.Id = stream.ReadString();
			return true;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000080E8 File Offset: 0x000062E8
		private bool DecodeTransformationMatrix(Stream stream, int length)
		{
			for (byte b = stream.ReadSingleByte(); b > 0; b -= 1)
			{
				stream.ReadString();
				Matrix3d value = default(Matrix3d);
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						value[i, j] = stream.ReadDouble();
					}
				}
				this.Model.TransformationMatrix = new Matrix3d?(value);
			}
			return true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008150 File Offset: 0x00006350
		private bool DecodeVertices(Stream stream, int length)
		{
			double num = stream.ReadDouble();
			double num2 = stream.ReadDouble();
			double num3 = stream.ReadDouble();
			uint num4 = stream.ReadInt();
			ushort num5 = stream.ReadShort();
			Vector3d[] array = new Vector3d[num4];
			Vector3[] array2 = new Vector3[num4];
			Vector3[] array3 = null;
			if ((num5 & 2) != 0)
			{
				array3 = new Vector3[num4];
			}
			Vector2[] array4 = null;
			if ((num5 & 4) != 0)
			{
				array4 = new Vector2[num4];
			}
			float[] array5 = null;
			if ((num5 & 16) != 0)
			{
				array5 = new float[num4];
			}
			Vector2[] array6 = null;
			if ((num5 & 32) != 0)
			{
				array6 = new Vector2[num4];
			}
			Vector3[] array7 = null;
			if ((num5 & 64) != 0)
			{
				array7 = new Vector3[num4];
			}
			ulong[] array8 = null;
			if ((num5 & 128) != 0)
			{
				array8 = new ulong[num4];
			}
			int num6 = 0;
			while ((long)num6 < (long)((ulong)num4))
			{
				if ((num5 & 1) == 0)
				{
					throw new InvalidDataException("Vertex data must contain vertex positions");
				}
				float num7 = stream.ReadFloat();
				float num8 = stream.ReadFloat();
				float num9 = stream.ReadFloat();
				array2[num6] = new Vector3(num7, num8, num9);
				array[num6] = new Vector3d((double)num7 + num, (double)num8 + num2, (double)num9 + num3);
				if ((num5 & 2) != 0)
				{
					float num10 = stream.ReadFloat();
					float num11 = stream.ReadFloat();
					float num12 = stream.ReadFloat();
					array3[num6] = new Vector3(num10, num11, num12);
				}
				if ((num5 & 4) != 0)
				{
					float num13 = stream.ReadFloat();
					float num14 = stream.ReadFloat();
					array4[num6] = new Vector2(num13, num14);
				}
				if ((num5 & 16) != 0)
				{
					array5[num6] = stream.ReadFloat();
				}
				if ((num5 & 32) != 0)
				{
					float[] array9 = new float[]
					{
						stream.ReadFloat(),
						stream.ReadFloat()
					};
					array6[num6] = new Vector2(array9[0], array9[1]);
				}
				if ((num5 & 64) != 0)
				{
					float[] array10 = new float[]
					{
						stream.ReadFloat(),
						stream.ReadFloat(),
						stream.ReadFloat()
					};
					array7[num6] = new Vector3(array10[0], array10[1], array10[2]);
				}
				if ((num5 & 128) != 0)
				{
					array8[num6] = stream.ReadLong();
				}
				num6++;
			}
			this._vertexBuffer.Offset = new Vector3d(num, num2, num3);
			this._vertexBuffer.Vertices = array;
			this._vertexBuffer.RawVertices = array2;
			this._vertexBuffer.Normals = array3;
			this._vertexBuffer.TextureUv = array4;
			this._vertexBuffer.ApplicationSpecificFloats = array5;
			this._vertexBuffer.ApplicationSpecificVector2Fs = array6;
			this._vertexBuffer.ApplicationSpecificVector3Fs = array7;
			this._vertexBuffer.ApplicationSpecificIDs = array8;
			return true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008418 File Offset: 0x00006618
		private bool DecodeTriangle(Stream stream, int length)
		{
			uint num = stream.ReadInt();
			uint num2 = stream.ReadInt();
			uint num3 = stream.ReadInt();
			while (num > 0u)
			{
				uint num4 = stream.ReadInt();
				List<int> list = new List<int>();
				int num5 = 0;
				while ((long)num5 < (long)((ulong)num4))
				{
					int num6 = (int)stream.ReadInt();
					if (num5 == 0)
					{
						list.Add(num6);
					}
					else
					{
						list.Add(list[num5 - 1] + num6);
					}
					num5++;
				}
				this._triangleBuffer.TriangleStrips.Add(list);
				num -= 1u;
			}
			while (num2 > 0u)
			{
				uint num7 = stream.ReadInt();
				List<int> list2 = new List<int>();
				int num8 = 0;
				while ((long)num8 < (long)((ulong)num7))
				{
					int num9 = (int)stream.ReadInt();
					if (num8 == 0)
					{
						list2.Add(num9);
					}
					else
					{
						list2.Add(list2[num8 - 1] + num9);
					}
					num8++;
				}
				this._triangleBuffer.TriangleFans.Add(list2);
				num2 -= 1u;
			}
			while (num3 > 0u)
			{
				for (int i = 0; i < 3; i++)
				{
					this._triangleBuffer.Triangles.Add((int)stream.ReadInt());
				}
				num3 -= 1u;
			}
			return true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008540 File Offset: 0x00006740
		private bool DecodeCameras(Stream stream, int length)
		{
			for (byte b = stream.ReadSingleByte(); b > 0; b -= 1)
			{
				Matrix3d item = default(Matrix3d);
				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						item[i, j] = stream.ReadDouble();
					}
				}
				this.Model.CameraMatrices.Add(item);
			}
			return true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000085A0 File Offset: 0x000067A0
		private bool DecodeTexture(Stream stream, int length)
		{
			for (ushort num = stream.ReadShort(); num > 0; num -= 1)
			{
				Texture texture = new Texture();
				texture.Attributes = (Texture.TextureAttributes)stream.ReadSingleByte();
				texture.Type = (Texture.TextureType)stream.ReadSingleByte();
				uint num2 = stream.ReadInt();
				byte[] array = new byte[num2];
				stream.ReadArray((int)num2, array);
				texture.Data = array;
				this.Model.Textures.Add(texture);
			}
			return true;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00008610 File Offset: 0x00006810
		private bool DecodeSegments(Stream stream, int length, ref short numSegments)
		{
			short num = (short)stream.ReadShort();
			numSegments = num;
			while (num > 0)
			{
				this._segmentsBuffer.SegmentId.Add(stream.ReadShort());
				this._segmentsBuffer.OffsetFirstVertex.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetLastVertex.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetFirstTriangleStrip.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetLastTriangleStrip.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetFirstTriangleFan.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetLastTriangleFan.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetFirstTriangle.Add((int)stream.ReadInt());
				this._segmentsBuffer.OffsetLastTriangle.Add((int)stream.ReadInt());
				num -= 1;
			}
			return true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00008700 File Offset: 0x00006900
		private bool DecodeSegmentTextureCoordinates(Stream stream, int length)
		{
			ushort key = stream.ReadShort();
			short textureIndex = (short)stream.ReadShort();
			uint num = stream.ReadInt();
			Vector2[] array = new Vector2[num];
			int num2 = 0;
			while (num > 0u)
			{
				float num3 = stream.ReadFloat();
				float num4 = stream.ReadFloat();
				array[num2] = new Vector2(num3, num4);
				num -= 1u;
				num2++;
			}
			GeometryDecoder.SegmentTextures value = default(GeometryDecoder.SegmentTextures);
			value.TextureIndex = textureIndex;
			value.DetailTextureUv = array;
			this._segmentTexturesById[key] = value;
			return true;
		}

		// Token: 0x040000EB RID: 235
		private Dictionary<ushort, Mesh> _meshesByInternalId;

		// Token: 0x040000EC RID: 236
		private GeometryDecoder.VertexData _vertexBuffer = default(GeometryDecoder.VertexData);

		// Token: 0x040000ED RID: 237
		private GeometryDecoder.TriangleBuffer _triangleBuffer = new GeometryDecoder.TriangleBuffer();

		// Token: 0x040000EE RID: 238
		private GeometryDecoder.SegmentsBuffer _segmentsBuffer = new GeometryDecoder.SegmentsBuffer();

		// Token: 0x040000EF RID: 239
		private Dictionary<ushort, GeometryDecoder.SegmentTextures> _segmentTexturesById;

		// Token: 0x040000F0 RID: 240
		private static byte[] GzipSignature = new byte[]
		{
			31,
			139,
			8
		};

		// Token: 0x02000026 RID: 38
		private struct VertexData
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x060000F0 RID: 240 RVA: 0x000087AB File Offset: 0x000069AB
			// (set) Token: 0x060000F1 RID: 241 RVA: 0x000087B3 File Offset: 0x000069B3
			public Vector3d[] Vertices { get; set; }

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x060000F2 RID: 242 RVA: 0x000087BC File Offset: 0x000069BC
			// (set) Token: 0x060000F3 RID: 243 RVA: 0x000087C4 File Offset: 0x000069C4
			public Vector3[] RawVertices { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x060000F4 RID: 244 RVA: 0x000087CD File Offset: 0x000069CD
			// (set) Token: 0x060000F5 RID: 245 RVA: 0x000087D5 File Offset: 0x000069D5
			public Vector3[] Normals { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x060000F6 RID: 246 RVA: 0x000087DE File Offset: 0x000069DE
			// (set) Token: 0x060000F7 RID: 247 RVA: 0x000087E6 File Offset: 0x000069E6
			public Vector2[] TextureUv { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x060000F8 RID: 248 RVA: 0x000087EF File Offset: 0x000069EF
			// (set) Token: 0x060000F9 RID: 249 RVA: 0x000087F7 File Offset: 0x000069F7
			public float[] ApplicationSpecificFloats { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x060000FA RID: 250 RVA: 0x00008800 File Offset: 0x00006A00
			// (set) Token: 0x060000FB RID: 251 RVA: 0x00008808 File Offset: 0x00006A08
			public Vector2[] ApplicationSpecificVector2Fs { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x060000FC RID: 252 RVA: 0x00008811 File Offset: 0x00006A11
			// (set) Token: 0x060000FD RID: 253 RVA: 0x00008819 File Offset: 0x00006A19
			public Vector3[] ApplicationSpecificVector3Fs { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x060000FE RID: 254 RVA: 0x00008822 File Offset: 0x00006A22
			// (set) Token: 0x060000FF RID: 255 RVA: 0x0000882A File Offset: 0x00006A2A
			public ulong[] ApplicationSpecificIDs { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000100 RID: 256 RVA: 0x00008833 File Offset: 0x00006A33
			// (set) Token: 0x06000101 RID: 257 RVA: 0x0000883B File Offset: 0x00006A3B
			public Vector3d Offset { get; set; }
		}

		// Token: 0x02000027 RID: 39
		private class TriangleBuffer
		{
			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000102 RID: 258 RVA: 0x00008844 File Offset: 0x00006A44
			// (set) Token: 0x06000103 RID: 259 RVA: 0x0000884C File Offset: 0x00006A4C
			public List<List<int>> TriangleStrips { get; set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000104 RID: 260 RVA: 0x00008855 File Offset: 0x00006A55
			// (set) Token: 0x06000105 RID: 261 RVA: 0x0000885D File Offset: 0x00006A5D
			public List<List<int>> TriangleFans { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000106 RID: 262 RVA: 0x00008866 File Offset: 0x00006A66
			// (set) Token: 0x06000107 RID: 263 RVA: 0x0000886E File Offset: 0x00006A6E
			public List<int> Triangles { get; set; }

			// Token: 0x06000108 RID: 264 RVA: 0x00008877 File Offset: 0x00006A77
			public TriangleBuffer()
			{
				this.TriangleStrips = new List<List<int>>();
				this.TriangleFans = new List<List<int>>();
				this.Triangles = new List<int>();
			}
		}

		// Token: 0x02000028 RID: 40
		private class SegmentsBuffer
		{
			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000109 RID: 265 RVA: 0x000088A0 File Offset: 0x00006AA0
			// (set) Token: 0x0600010A RID: 266 RVA: 0x000088A8 File Offset: 0x00006AA8
			public List<ushort> SegmentId { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600010B RID: 267 RVA: 0x000088B1 File Offset: 0x00006AB1
			// (set) Token: 0x0600010C RID: 268 RVA: 0x000088B9 File Offset: 0x00006AB9
			public List<int> OffsetFirstVertex { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600010D RID: 269 RVA: 0x000088C2 File Offset: 0x00006AC2
			// (set) Token: 0x0600010E RID: 270 RVA: 0x000088CA File Offset: 0x00006ACA
			public List<int> OffsetLastVertex { get; set; }

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600010F RID: 271 RVA: 0x000088D3 File Offset: 0x00006AD3
			// (set) Token: 0x06000110 RID: 272 RVA: 0x000088DB File Offset: 0x00006ADB
			public List<int> OffsetFirstTriangleStrip { get; set; }

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000111 RID: 273 RVA: 0x000088E4 File Offset: 0x00006AE4
			// (set) Token: 0x06000112 RID: 274 RVA: 0x000088EC File Offset: 0x00006AEC
			public List<int> OffsetLastTriangleStrip { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000113 RID: 275 RVA: 0x000088F5 File Offset: 0x00006AF5
			// (set) Token: 0x06000114 RID: 276 RVA: 0x000088FD File Offset: 0x00006AFD
			public List<int> OffsetFirstTriangleFan { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000115 RID: 277 RVA: 0x00008906 File Offset: 0x00006B06
			// (set) Token: 0x06000116 RID: 278 RVA: 0x0000890E File Offset: 0x00006B0E
			public List<int> OffsetLastTriangleFan { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000117 RID: 279 RVA: 0x00008917 File Offset: 0x00006B17
			// (set) Token: 0x06000118 RID: 280 RVA: 0x0000891F File Offset: 0x00006B1F
			public List<int> OffsetFirstTriangle { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000119 RID: 281 RVA: 0x00008928 File Offset: 0x00006B28
			// (set) Token: 0x0600011A RID: 282 RVA: 0x00008930 File Offset: 0x00006B30
			public List<int> OffsetLastTriangle { get; set; }

			// Token: 0x0600011B RID: 283 RVA: 0x0000893C File Offset: 0x00006B3C
			public SegmentsBuffer()
			{
				this.SegmentId = new List<ushort>();
				this.OffsetFirstVertex = new List<int>();
				this.OffsetLastVertex = new List<int>();
				this.OffsetFirstTriangleStrip = new List<int>();
				this.OffsetLastTriangleStrip = new List<int>();
				this.OffsetFirstTriangleFan = new List<int>();
				this.OffsetLastTriangleFan = new List<int>();
				this.OffsetFirstTriangle = new List<int>();
				this.OffsetLastTriangle = new List<int>();
			}
		}

		// Token: 0x02000029 RID: 41
		private struct SegmentTextures
		{
			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600011C RID: 284 RVA: 0x000089B2 File Offset: 0x00006BB2
			// (set) Token: 0x0600011D RID: 285 RVA: 0x000089BA File Offset: 0x00006BBA
			public short TextureIndex { get; set; }

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600011E RID: 286 RVA: 0x000089C3 File Offset: 0x00006BC3
			// (set) Token: 0x0600011F RID: 287 RVA: 0x000089CB File Offset: 0x00006BCB
			public Vector2[] DetailTextureUv { get; set; }
		}
	}
}
