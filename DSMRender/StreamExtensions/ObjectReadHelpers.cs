using System;
using System.IO;
using System.Text;

namespace StreamExtensions
{
	// Token: 0x02000030 RID: 48
	public static class ObjectReadHelpers
	{
		// Token: 0x0600016E RID: 366 RVA: 0x00009520 File Offset: 0x00007720
		public static string ReadString(this Stream stream)
		{
			byte[] bytes = stream.ReadArrayBytes();
			UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
			return utf8Encoding.GetString(bytes);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009543 File Offset: 0x00007743
		public static byte ReadByte(this Stream stream)
		{
			return stream.ReadNextByte();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000954B File Offset: 0x0000774B
		public static short ReadShort(this Stream stream)
		{
			return stream.ReadObject(2, new Func<byte[], int, short>(BitConverter.ToInt16));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00009560 File Offset: 0x00007760
		public static ushort ReadUShort(this Stream stream)
		{
			return stream.ReadObject(2, new Func<byte[], int, ushort>(BitConverter.ToUInt16));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009575 File Offset: 0x00007775
		public static int ReadInt(this Stream stream)
		{
			return stream.ReadObject(4, new Func<byte[], int, int>(BitConverter.ToInt32));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000958A File Offset: 0x0000778A
		public static uint ReadUInt(this Stream stream)
		{
			return stream.ReadObject(4, new Func<byte[], int, uint>(BitConverter.ToUInt32));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000959F File Offset: 0x0000779F
		public static long ReadLong(this Stream stream)
		{
			return stream.ReadObject(8, new Func<byte[], int, long>(BitConverter.ToInt64));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000095B4 File Offset: 0x000077B4
		public static ulong ReadULong(this Stream stream)
		{
			return stream.ReadObject(8, new Func<byte[], int, ulong>(BitConverter.ToUInt64));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000095C9 File Offset: 0x000077C9
		public static float ReadFloat(this Stream stream)
		{
			return stream.ReadObject(4, new Func<byte[], int, float>(BitConverter.ToSingle));
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000095DE File Offset: 0x000077DE
		public static double ReadDouble(this Stream stream)
		{
			return stream.ReadObject(8, new Func<byte[], int, double>(BitConverter.ToDouble));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000095F3 File Offset: 0x000077F3
		public static DateTime ReadDateTime(this Stream stream)
		{
			return DateTime.FromBinary(stream.ReadLong());
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00009600 File Offset: 0x00007800
		public static Guid ReadGuid(this Stream stream)
		{
			return new Guid(stream.ReadNextBytes(16));
		}
	}
}
