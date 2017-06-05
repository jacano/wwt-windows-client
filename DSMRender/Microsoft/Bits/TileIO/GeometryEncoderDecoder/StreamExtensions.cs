using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpDX;
using TerraViewer;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x02000032 RID: 50
	public static class StreamExtensions
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00009714 File Offset: 0x00007914
		public static void CopyTo(this Stream input, Stream output)
		{
			byte[] array = new byte[32768];
			int count;
			while ((count = input.Read(array, 0, array.Length)) > 0)
			{
				output.Write(array, 0, count);
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00009747 File Offset: 0x00007947
		public static void Write(this Stream stream, byte[] bytes)
		{
			stream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00009754 File Offset: 0x00007954
		public static void Write(this Stream stream, string s)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			stream.Write(bytes);
			stream.Write(GeometryEncoderDecoderConstants.Null);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00009780 File Offset: 0x00007980
		public static void Write(this Stream stream, ushort value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000979C File Offset: 0x0000799C
		public static void Write(this Stream stream, uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000097B8 File Offset: 0x000079B8
		public static void Write(this Stream stream, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000097D4 File Offset: 0x000079D4
		public static void Write(this Stream stream, ulong value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000097F0 File Offset: 0x000079F0
		public static void Write(this Stream stream, float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000980C File Offset: 0x00007A0C
		public static void Write(this Stream stream, double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			stream.Write(bytes);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00009827 File Offset: 0x00007A27
		public static void Write(this Stream stream, byte value)
		{
			stream.WriteByte(value);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009830 File Offset: 0x00007A30
		public static void Write(this Stream stream, Vector2 value)
		{
			stream.Write(value.X);
			stream.Write(value.Y);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000984C File Offset: 0x00007A4C
		public static void Write(this Stream stream, Vector2d value)
		{
			stream.Write(value.X);
			stream.Write(value.Y);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009868 File Offset: 0x00007A68
		public static void Write(this Stream stream, Vector3 value)
		{
			stream.Write(value.X);
			stream.Write(value.Y);
			stream.Write(value.Z);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009891 File Offset: 0x00007A91
		public static void Write(this Stream stream, Vector3d value)
		{
			stream.Write(value.X);
			stream.Write(value.Y);
			stream.Write(value.Z);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000098BC File Offset: 0x00007ABC
		public static int ReadArray(this Stream stream, int length, byte[] buffer)
		{
			int num = 0;
			int i = length;
			while (i > 0)
			{
				int num2 = stream.Read(buffer, num, i);
				if (num2 <= 0)
				{
					return num;
				}
				i -= num2;
				num += num2;
			}
			return num;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000098EC File Offset: 0x00007AEC
		public static byte ReadSingleByte(this Stream stream)
		{
			byte[] array = new byte[1];
			int num = stream.ReadArray(1, array);
			if (num != array.Length)
			{
				throw new EndOfStreamException(string.Format("End of stream reached with {0} bytes left to read", num));
			}
			return array[0];
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000992C File Offset: 0x00007B2C
		public static string ReadString(this Stream stream)
		{
			List<byte> list = new List<byte>();
			byte b;
			do
			{
				b = stream.ReadSingleByte();
				list.Add(b);
			}
			while (b != 0);
			return Encoding.UTF8.GetString(list.ToArray(), 0, list.Count - 1);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000996C File Offset: 0x00007B6C
		public static ushort ReadShort(this Stream stream)
		{
			byte[] array = new byte[2];
			int num = stream.ReadArray(2, array);
			if (num != array.Length)
			{
				throw new EndOfStreamException(string.Format("End of stream reached with {0} bytes left to read", num));
			}
			return BitConverter.ToUInt16(array, 0);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000099B0 File Offset: 0x00007BB0
		public static uint ReadInt(this Stream stream)
		{
			byte[] array = new byte[4];
			int num = stream.ReadArray(4, array);
			if (num != array.Length)
			{
				throw new EndOfStreamException(string.Format("End of stream reached with {0} bytes left to read", num));
			}
			return BitConverter.ToUInt32(array, 0);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000099F4 File Offset: 0x00007BF4
		public static ulong ReadLong(this Stream stream)
		{
			byte[] array = new byte[8];
			int num = stream.ReadArray(8, array);
			if (num != array.Length)
			{
				throw new EndOfStreamException(string.Format("End of stream reached with {0} bytes left to read", num));
			}
			return BitConverter.ToUInt64(array, 0);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00009A38 File Offset: 0x00007C38
		public static double ReadDouble(this Stream stream)
		{
			byte[] array = new byte[8];
			int num = stream.ReadArray(8, array);
			if (num != array.Length)
			{
				throw new EndOfStreamException(string.Format("End of stream reached with {0} bytes left to read", num));
			}
			return BitConverter.ToDouble(array, 0);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009A7C File Offset: 0x00007C7C
		public static float ReadFloat(this Stream stream)
		{
			byte[] array = new byte[4];
			int num = stream.ReadArray(4, array);
			if (num != array.Length)
			{
				throw new EndOfStreamException(string.Format("End of stream reached with {0} bytes left to read", num));
			}
			return BitConverter.ToSingle(array, 0);
		}
	}
}
