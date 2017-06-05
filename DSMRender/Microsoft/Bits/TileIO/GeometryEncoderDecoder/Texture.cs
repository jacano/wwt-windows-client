using System;
using System.Text;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x02000034 RID: 52
	public class Texture
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00009FA7 File Offset: 0x000081A7
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00009FAF File Offset: 0x000081AF
		public Texture.TextureAttributes Attributes { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00009FB8 File Offset: 0x000081B8
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00009FC0 File Offset: 0x000081C0
		public Texture.TextureType Type { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009FC9 File Offset: 0x000081C9
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00009FD1 File Offset: 0x000081D1
		public byte[] Data { get; set; }

		// Token: 0x060001B0 RID: 432 RVA: 0x00009FDA File Offset: 0x000081DA
		public Texture()
		{
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009FE2 File Offset: 0x000081E2
		public Texture(Texture.TextureAttributes attributes, Texture.TextureType type, byte[] data)
		{
			this.Attributes = attributes;
			this.Type = type;
			this.Data = data;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009FFF File Offset: 0x000081FF
		public Texture(Texture.TextureAttributes attributes, string url)
		{
			this.Attributes = attributes;
			this.Type = Texture.TextureType.Indirect;
			this.Data = Encoding.UTF8.GetBytes(url);
		}

		// Token: 0x02000035 RID: 53
		public enum TextureAttributes
		{
			// Token: 0x04000153 RID: 339
			Normal,
			// Token: 0x04000154 RID: 340
			DZI,
			// Token: 0x04000155 RID: 341
			MipMapped
		}

		// Token: 0x02000036 RID: 54
		public enum TextureType
		{
			// Token: 0x04000157 RID: 343
			JPEG = 1,
			// Token: 0x04000158 RID: 344
			JPEGXR,
			// Token: 0x04000159 RID: 345
			DXT1,
			// Token: 0x0400015A RID: 346
			DXT3,
			// Token: 0x0400015B RID: 347
			DXT5,
			// Token: 0x0400015C RID: 348
			Indirect = 255
		}
	}
}
