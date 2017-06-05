using System;
using System.Drawing;

namespace Microsoft.Bits.TileIO.GeometryEncoderDecoder
{
	// Token: 0x0200002C RID: 44
	public class Quad
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00008CF9 File Offset: 0x00006EF9
		public Quad()
		{
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008D01 File Offset: 0x00006F01
		public Quad(PointF topLeft, PointF topRight, PointF bttomLeft, PointF bottomRight)
		{
			this.TopLeft = topLeft;
			this.TopRight = topRight;
			this.BottomLeft = bttomLeft;
			this.BottomRight = bottomRight;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008D28 File Offset: 0x00006F28
		public bool IntersectsRectangle(RectangleF rect)
		{
			return rect.Contains(this.TopLeft) || rect.Contains(this.TopRight) || rect.Contains(this.BottomLeft) || rect.Contains(this.BottomRight) || Quad.LineSegmentInresectsRectangle(rect, this.TopLeft, this.TopRight) || Quad.LineSegmentInresectsRectangle(rect, this.TopRight, this.BottomRight) || Quad.LineSegmentInresectsRectangle(rect, this.BottomRight, this.BottomLeft) || Quad.LineSegmentInresectsRectangle(rect, this.BottomLeft, this.TopLeft) || (this.PointInQuad(new PointF(rect.Right, rect.Top)) && this.PointInQuad(new PointF(rect.Right, rect.Bottom)) && this.PointInQuad(new PointF(rect.Left, rect.Top)) && this.PointInQuad(new PointF(rect.Left, rect.Bottom)));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008E40 File Offset: 0x00007040
		public bool PointInQuad(PointF point)
		{
			return Quad.IsSameSideOfLine(this.TopLeft, this.TopRight, this.BottomRight, point) && Quad.IsSameSideOfLine(this.TopRight, this.BottomRight, this.BottomLeft, point) && Quad.IsSameSideOfLine(this.BottomRight, this.BottomLeft, this.TopLeft, point) && Quad.IsSameSideOfLine(this.BottomLeft, this.TopLeft, this.TopRight, point);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008EC0 File Offset: 0x000070C0
		public static bool LineSegmentInresectsRectangle(RectangleF rect, PointF seg1, PointF seg2)
		{
			return Quad.LineSegmentCross(seg1, seg2, new PointF(rect.Left, rect.Top), new PointF(rect.Right, rect.Top)) || Quad.LineSegmentCross(seg1, seg2, new PointF(rect.Right, rect.Top), new PointF(rect.Right, rect.Bottom)) || Quad.LineSegmentCross(seg1, seg2, new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom)) || Quad.LineSegmentCross(seg1, seg2, new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Top));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008F94 File Offset: 0x00007194
		public static bool IsSameSideOfLine(PointF seg1, PointF seg2, PointF a, PointF b)
		{
			return ((seg1.X - seg2.X) * (a.Y - seg2.Y) - (seg1.Y - seg2.Y) * (a.X - seg2.X)) * ((seg1.X - seg2.X) * (b.Y - seg2.Y) - (seg1.Y - seg2.Y) * (b.X - seg2.X)) >= 0f;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000902A File Offset: 0x0000722A
		public static bool LineSegmentCross(PointF a1, PointF a2, PointF b1, PointF b2)
		{
			return !Quad.IsSameSideOfLine(a1, a2, b1, b2) && !Quad.IsSameSideOfLine(b1, b2, a1, a2);
		}

		// Token: 0x04000136 RID: 310
		public PointF TopLeft;

		// Token: 0x04000137 RID: 311
		public PointF TopRight;

		// Token: 0x04000138 RID: 312
		public PointF BottomLeft;

		// Token: 0x04000139 RID: 313
		public PointF BottomRight;
	}
}
