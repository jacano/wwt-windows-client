using System;
using System.IO;

namespace StreamExtensions
{
	// Token: 0x02000002 RID: 2
	public static class ArrayReadHelpers
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static byte[] ReadByteArray(this Stream stream, int? count = null)
		{
			if (count == null)
			{
				return stream.ReadArrayBytes();
			}
			return stream.ReadNextBytes(count.Value);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		public static short[] ReadShortArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, short>(BitConverter.ToInt16), 2, count);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002085 File Offset: 0x00000285
		public static ushort[] ReadUShortArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, ushort>(BitConverter.ToUInt16), 2, count);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000209B File Offset: 0x0000029B
		public static int[] ReadIntArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, int>(BitConverter.ToInt32), 4, count);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020B1 File Offset: 0x000002B1
		public static uint[] ReadUIntArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, uint>(BitConverter.ToUInt32), 4, count);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020C7 File Offset: 0x000002C7
		public static long[] ReadLongArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, long>(BitConverter.ToInt64), 8, count);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020DD File Offset: 0x000002DD
		public static ulong[] ReadULongArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, ulong>(BitConverter.ToUInt64), 8, count);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020F3 File Offset: 0x000002F3
		public static float[] ReadFloatArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, float>(BitConverter.ToSingle), 4, count);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002109 File Offset: 0x00000309
		public static double[] ReadDoubleArray(this Stream stream, int? count = null)
		{
			return stream.ReadArray(new Func<byte[], int, double>(BitConverter.ToDouble), 8, count);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002120 File Offset: 0x00000320
		public static bool[] ReadBoolArray(this Stream stream)
		{
			int count = stream.ReadInt();
			return stream.ReadBoolArray(count);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000213C File Offset: 0x0000033C
		public static bool[] ReadBoolArray(this Stream stream, int count)
		{
			bool[] array = new bool[count];
			int i = 0;
			int num = 0;
			byte b = 0;
			while (i < count)
			{
				if (num == 0)
				{
					b = stream.ReadNextByte();
				}
				array[i] = ((b & ArrayWriteHelpers.bitMask[num]) != 0);
				i++;
				num = (num + 1) % 8;
			}
			return array;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002194 File Offset: 0x00000394
		public static DateTime[] ReadDateTimeArray(this Stream stream, int? count = null)
		{
			Func<byte[], int, DateTime> readObject = (byte[] buffer, int start) => DateTime.FromBinary(BitConverter.ToInt64(buffer, start));
			return stream.ReadArray(readObject, 8, count);
		}
	}
}
