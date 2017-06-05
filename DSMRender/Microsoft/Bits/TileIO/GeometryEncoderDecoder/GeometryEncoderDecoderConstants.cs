using System;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x0200002A RID: 42
	internal class GeometryEncoderDecoderConstants
	{
		// Token: 0x06000121 RID: 289 RVA: 0x000089D4 File Offset: 0x00006BD4
		static GeometryEncoderDecoderConstants()
		{
			// Note: this type is marked as 'beforefieldinit'.
			byte[] @null = new byte[1];
			GeometryEncoderDecoderConstants.Null = @null;
			GeometryEncoderDecoderConstants.WorldTransformationMatrixName = "WORLD";
			GeometryEncoderDecoderConstants.FootprintVectorDataName = "foot";
		}

		// Token: 0x04000109 RID: 265
		public const uint MaximumMeshCount = 65534u;

		// Token: 0x0400010A RID: 266
		public const uint MaximumTextureCount = 65535u;

		// Token: 0x0400010B RID: 267
		public const string MagicString = "geomodel";

		// Token: 0x0400010C RID: 268
		public const byte MajorVersion = 1;

		// Token: 0x0400010D RID: 269
		public const byte MinorVersion = 0;

		// Token: 0x0400010E RID: 270
		public const ushort HeaderLength = 12;

		// Token: 0x0400010F RID: 271
		public const ushort ChunkHeaderLength = 6;

		// Token: 0x04000110 RID: 272
		public const ushort BoundingBoxChunkId = 256;

		// Token: 0x04000111 RID: 273
		public const ushort GeopositionUncertaintyChunkId = 257;

		// Token: 0x04000112 RID: 274
		public const ushort KeyValuePairsChunkId = 512;

		// Token: 0x04000113 RID: 275
		public const ushort MeshIdentifierChunkId = 768;

		// Token: 0x04000114 RID: 276
		public const ushort TransformationMatrixChunkId = 1024;

		// Token: 0x04000115 RID: 277
		public const ushort VerticesChunkId = 1280;

		// Token: 0x04000116 RID: 278
		public const ushort TrianglesChunkId = 1281;

		// Token: 0x04000117 RID: 279
		public const ushort VectorChunkId = 1536;

		// Token: 0x04000118 RID: 280
		public const ushort CamerasChunkId = 1792;

		// Token: 0x04000119 RID: 281
		public const ushort TextureChunkId = 2048;

		// Token: 0x0400011A RID: 282
		public const ushort SegmentChunkId = 2304;

		// Token: 0x0400011B RID: 283
		public const ushort SegmentTextureCoordinates = 2305;

		// Token: 0x0400011C RID: 284
		public const ushort QuadTreePolygons = 4096;

		// Token: 0x0400011D RID: 285
		public const ushort QuadTreeMetadata = 4097;

		// Token: 0x0400011E RID: 286
		public const ushort EndOfDataChunkId = 65535;

		// Token: 0x0400011F RID: 287
		public const ushort VertexMask = 1;

		// Token: 0x04000120 RID: 288
		public const ushort NormalMask = 2;

		// Token: 0x04000121 RID: 289
		public const ushort TextureUvMask = 4;

		// Token: 0x04000122 RID: 290
		public const ushort AppSpecificFloatMask = 16;

		// Token: 0x04000123 RID: 291
		public const ushort AppSpecificVector2FMask = 32;

		// Token: 0x04000124 RID: 292
		public const ushort AppSpecificVector3FMask = 64;

		// Token: 0x04000125 RID: 293
		public const ushort AppSpecificIdMask = 128;

		// Token: 0x04000126 RID: 294
		public const ushort FileIdForGeometry = 65535;

		// Token: 0x04000127 RID: 295
		public const byte VectorDataIs2D = 0;

		// Token: 0x04000128 RID: 296
		public const byte VectorDataIs3D = 1;

		// Token: 0x04000129 RID: 297
		public static readonly byte[] Null;

		// Token: 0x0400012A RID: 298
		public static readonly string WorldTransformationMatrixName;

		// Token: 0x0400012B RID: 299
		public static readonly string FootprintVectorDataName;
	}
}
