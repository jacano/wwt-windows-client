using System;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x0200002E RID: 46
	public class Tuple<T1, T2>
	{
		// Token: 0x06000144 RID: 324 RVA: 0x00009045 File Offset: 0x00007245
		public Tuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000905B File Offset: 0x0000725B
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00009063 File Offset: 0x00007263
		public T1 Item1 { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000906C File Offset: 0x0000726C
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00009074 File Offset: 0x00007274
		public T2 Item2 { get; set; }
	}
}
