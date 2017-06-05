using System;
using System.Collections.Generic;
using System.IO;

namespace StreamExtensions
{
	// Token: 0x02000033 RID: 51
	public static class StreamHelpers
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00009ABE File Offset: 0x00007CBE
		public static bool EndOfStream(this Stream stream)
		{
			return stream.Length <= stream.Position + 1L;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00009AD4 File Offset: 0x00007CD4
		public static byte ReadNextByte(this Stream stream)
		{
			byte[] array = new byte[1];
			stream.Read(array, 0, 1);
			return array[0];
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009AF8 File Offset: 0x00007CF8
		public static byte[] ReadNextBytes(this Stream stream, int numberOfBytes)
		{
			byte[] array = new byte[numberOfBytes];
			stream.Read(array, 0, numberOfBytes);
			return array;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00009B18 File Offset: 0x00007D18
		public static void SkipArray(this Stream stream)
		{
			long position = stream.Position;
			int num = stream.ReadArrayDataLength();
			stream.Position += (long)num;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00009B44 File Offset: 0x00007D44
		private static void WriteArrayDataLength(this Stream stream, int dataLength)
		{
			if (dataLength >= 0 && dataLength < 255)
			{
				stream.WriteByte((byte)dataLength);
				return;
			}
			if (dataLength >= -32768 && dataLength < 32767)
			{
				stream.WriteByte(byte.MaxValue);
				stream.Write(BitConverter.GetBytes((short)dataLength), 0, 2);
				return;
			}
			stream.WriteByte(byte.MaxValue);
			stream.Write(BitConverter.GetBytes(short.MaxValue), 0, 2);
			stream.Write(BitConverter.GetBytes(dataLength), 0, 4);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00009BBC File Offset: 0x00007DBC
		private static int ReadArrayDataLength(this Stream stream)
		{
			int num = (int)stream.ReadNextByte();
			if (num != 255)
			{
				return num;
			}
			byte[] array = new byte[2];
			stream.Read(array, 0, 2);
			int num2 = (int)BitConverter.ToInt16(array, 0);
			if (num2 != 32767)
			{
				return num2;
			}
			byte[] array2 = new byte[4];
			stream.Read(array2, 0, 4);
			return BitConverter.ToInt32(array2, 0);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00009C15 File Offset: 0x00007E15
		internal static void WriteArrayBytes(this Stream stream, byte[] buffer)
		{
			stream.WriteArrayDataLength(buffer.Length);
			stream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009C2C File Offset: 0x00007E2C
		internal static byte[] ReadArrayBytes(this Stream stream)
		{
			int numberOfBytes = stream.ReadArrayDataLength();
			return stream.ReadNextBytes(numberOfBytes);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009C48 File Offset: 0x00007E48
		internal static void WriteObject<T>(this Stream stream, T value, Func<T, byte[]> convertToBytes)
		{
			byte[] array = convertToBytes(value);
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00009C68 File Offset: 0x00007E68
		internal static void WriteArray<T>(this Stream stream, IEnumerable<T> values, Action<T> writeBytes)
		{
			long position = stream.Position;
			stream.Position += 7L;
			foreach (T obj in values)
			{
				writeBytes(obj);
			}
			long position2 = stream.Position;
			int value = (int)(position2 - position - 7L);
			stream.Position = position;
			stream.WriteByte(byte.MaxValue);
			stream.Write(BitConverter.GetBytes(short.MaxValue), 0, 2);
			stream.Write(BitConverter.GetBytes(value), 0, 4);
			stream.Position = position2;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009D14 File Offset: 0x00007F14
		internal static void WriteArray<T>(this Stream stream, ICollection<T> values, Func<T, byte[]> convertToBytes, int bytesPerItem)
		{
			int num = values.Count * bytesPerItem;
			byte[] array = new byte[num];
			int num2 = 0;
			foreach (T arg in values)
			{
				byte[] array2 = convertToBytes(arg);
				if (array2.Length != bytesPerItem)
				{
					throw new InvalidDataException(string.Concat(new object[]
					{
						array2.Length,
						" bytes were returned, ",
						bytesPerItem,
						" expected."
					}));
				}
				Array.Copy(array2, 0, array, num2, bytesPerItem);
				num2 += bytesPerItem;
			}
			stream.WriteArrayBytes(array);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009DD0 File Offset: 0x00007FD0
		internal static T ReadObject<T>(this Stream stream, int bytesToRead, Func<byte[], int, T> fromBytes)
		{
			byte[] arg = stream.ReadNextBytes(bytesToRead);
			return fromBytes(arg, 0);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009F20 File Offset: 0x00008120
		internal static IEnumerable<T> EnumeratedReadArray<T>(this Stream stream, Func<Stream, T> readObject)
		{
			int dataLength = stream.ReadArrayDataLength();
			long finalPosition = stream.Position + (long)dataLength;
			while (stream.Position < finalPosition)
			{
				T obj = readObject(stream);
				yield return obj;
			}
			yield break;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009F44 File Offset: 0x00008144
		internal static T[] ReadArray<T>(this Stream stream, Func<byte[], int, T> readObject, int bytesPerItem, int? count = null)
		{
			byte[] array;
			if (count == null)
			{
				array = stream.ReadArrayBytes();
			}
			else
			{
				array = stream.ReadNextBytes(count.Value);
			}
			int num = array.Length / bytesPerItem;
			T[] array2 = new T[num];
			int num2 = 0;
			for (int i = 0; i < array.Length; i += bytesPerItem)
			{
				array2[num2] = readObject(array, i);
				num2++;
			}
			return array2;
		}
	}
}
