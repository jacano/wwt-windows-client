using System;
using System.Collections.Generic;
using System.IO;

namespace StreamExtensions
{
	// Token: 0x02000003 RID: 3
	public static class ArrayWriteHelpers
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000021C8 File Offset: 0x000003C8
		public static void WriteArray(this Stream stream, byte[] values)
		{
			stream.WriteArrayBytes(values);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021D1 File Offset: 0x000003D1
		public static void WriteArray(this Stream stream, short[] values)
		{
			stream.WriteArray(values, new Func<short, byte[]>(BitConverter.GetBytes), 2);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021E7 File Offset: 0x000003E7
		public static void WriteArray(this Stream stream, ushort[] values)
		{
			stream.WriteArray(values, new Func<ushort, byte[]>(BitConverter.GetBytes), 2);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021FD File Offset: 0x000003FD
		public static void WriteArray(this Stream stream, int[] values)
		{
			stream.WriteArray(values, new Func<int, byte[]>(BitConverter.GetBytes), 4);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002213 File Offset: 0x00000413
		public static void WriteArray(this Stream stream, uint[] values)
		{
			stream.WriteArray(values, new Func<uint, byte[]>(BitConverter.GetBytes), 4);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002229 File Offset: 0x00000429
		public static void WriteArray(this Stream stream, long[] values)
		{
			stream.WriteArray(values, new Func<long, byte[]>(BitConverter.GetBytes), 8);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000223F File Offset: 0x0000043F
		public static void WriteArray(this Stream stream, ulong[] values)
		{
			stream.WriteArray(values, new Func<ulong, byte[]>(BitConverter.GetBytes), 8);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002255 File Offset: 0x00000455
		public static void WriteArray(this Stream stream, float[] values)
		{
			stream.WriteArray(values, new Func<float, byte[]>(BitConverter.GetBytes), 4);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000226B File Offset: 0x0000046B
		public static void WriteArray(this Stream stream, double[] values)
		{
			stream.WriteArray(values, new Func<double, byte[]>(BitConverter.GetBytes), 8);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023E4 File Offset: 0x000005E4
		public static void WriteArray(this Stream stream, DateTime[] values)
		{
			Func<DateTime, byte[]> convertToBytes = (DateTime value) => BitConverter.GetBytes(value.ToBinary());
			stream.WriteArray(values, convertToBytes, 8);
		}

		// Token: 0x04000002 RID: 2
		private const byte b10000000 = 128;

		// Token: 0x04000003 RID: 3
		private const byte b01000000 = 64;

		// Token: 0x04000004 RID: 4
		private const byte b00100000 = 32;

		// Token: 0x04000005 RID: 5
		private const byte b00010000 = 16;

		// Token: 0x04000006 RID: 6
		private const byte b00001000 = 8;

		// Token: 0x04000007 RID: 7
		private const byte b00000100 = 4;

		// Token: 0x04000008 RID: 8
		private const byte b00000010 = 2;

		// Token: 0x04000009 RID: 9
		private const byte b00000001 = 1;

		// Token: 0x0400000A RID: 10
		internal static byte[] bitMask = new byte[]
		{
			128,
			64,
			32,
			16,
			8,
			4,
			2,
			1
		};
	}
}
