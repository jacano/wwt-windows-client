using System;
using System.IO;
using System.Text;

namespace StreamExtensions
{
	// Token: 0x02000031 RID: 49
	public static class ObjectWriteHelpers
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00009610 File Offset: 0x00007810
		public static void WriteObject(this Stream stream, string value)
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
			byte[] bytes = utf8Encoding.GetBytes(value);
			stream.WriteArrayBytes(bytes);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00009634 File Offset: 0x00007834
		public static void WriteObject(this Stream stream, byte value)
		{
			stream.WriteByte(value);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000963D File Offset: 0x0000783D
		public static void WriteObject(this Stream stream, short value)
		{
			stream.WriteObject(value, new Func<short, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00009652 File Offset: 0x00007852
		public static void WriteObject(this Stream stream, ushort value)
		{
			stream.WriteObject(value, new Func<ushort, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009667 File Offset: 0x00007867
		public static void WriteObject(this Stream stream, int value)
		{
			stream.WriteObject(value, new Func<int, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000967C File Offset: 0x0000787C
		public static void WriteObject(this Stream stream, uint value)
		{
			stream.WriteObject(value, new Func<uint, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00009691 File Offset: 0x00007891
		public static void WriteObject(this Stream stream, long value)
		{
			stream.WriteObject(value, new Func<long, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000096A6 File Offset: 0x000078A6
		public static void WriteObject(this Stream stream, ulong value)
		{
			stream.WriteObject(value, new Func<ulong, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000096BB File Offset: 0x000078BB
		public static void WriteObject(this Stream stream, float value)
		{
			stream.WriteObject(value, new Func<float, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000096D0 File Offset: 0x000078D0
		public static void WriteObject(this Stream stream, double value)
		{
			stream.WriteObject(value, new Func<double, byte[]>(BitConverter.GetBytes));
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000096E5 File Offset: 0x000078E5
		public static void WriteObject(this Stream stream, DateTime value)
		{
			stream.WriteObject(value.ToBinary());
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000096F4 File Offset: 0x000078F4
		public static void WriteObject(this Stream stream, Guid value)
		{
			byte[] array = value.ToByteArray();
			stream.Write(array, 0, array.Length);
		}
	}
}
