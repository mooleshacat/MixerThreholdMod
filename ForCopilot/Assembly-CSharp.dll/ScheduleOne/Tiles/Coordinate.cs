using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002C2 RID: 706
	[Serializable]
	public class Coordinate
	{
		// Token: 0x06000F2E RID: 3886 RVA: 0x00042B5E File Offset: 0x00040D5E
		public static implicit operator Vector2(Coordinate c)
		{
			return new Vector2((float)c.x, (float)c.y);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00042B73 File Offset: 0x00040D73
		public Coordinate()
		{
			this.x = 0;
			this.y = 0;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00042B89 File Offset: 0x00040D89
		public Coordinate(int _x, int _y)
		{
			this.x = _x;
			this.y = _y;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00042B9F File Offset: 0x00040D9F
		public Coordinate(Vector2 vector)
		{
			this.x = (int)vector.x;
			this.y = (int)vector.y;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00042BC1 File Offset: 0x00040DC1
		public override int GetHashCode()
		{
			return this.SignedCantorPair(this.x, this.y);
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00042BD8 File Offset: 0x00040DD8
		public override bool Equals(object obj)
		{
			Coordinate coordinate = obj as Coordinate;
			return coordinate != null && coordinate.x == this.x && coordinate.y == this.y;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00042C0D File Offset: 0x00040E0D
		public static Coordinate operator +(Coordinate a, Coordinate b)
		{
			return new Coordinate(a.x + b.x, a.y + b.y);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00042C2E File Offset: 0x00040E2E
		public static Coordinate operator -(Coordinate a, Coordinate b)
		{
			return new Coordinate(a.x - b.x, a.y - b.y);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00042C4F File Offset: 0x00040E4F
		private int CantorPair(int x, int y)
		{
			return (int)(0.5f * (float)(x + y) * ((float)(x + y) + 1f) + (float)y);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00042C6C File Offset: 0x00040E6C
		private int SignedCantorPair(int x, int y)
		{
			int num = (int)(((float)x >= 0f) ? (2f * (float)x) : (-2f * (float)x - 1f));
			int num2 = (int)(((float)y >= 0f) ? (2f * (float)y) : (-2f * (float)y - 1f));
			return this.CantorPair(num, num2);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00042CC8 File Offset: 0x00040EC8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.x.ToString(),
				",",
				this.y.ToString(),
				"]"
			});
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00042D14 File Offset: 0x00040F14
		public static List<CoordinatePair> BuildCoordinateMatches(Coordinate originCoord, int sizeX, int sizeY, float rot)
		{
			List<CoordinatePair> list = new List<CoordinatePair>();
			rot = (float)Coordinate.MathMod(Mathf.RoundToInt(rot), 360);
			for (int i = 0; i < sizeX; i++)
			{
				for (int j = 0; j < sizeY; j++)
				{
					Coordinate coordinate = new Coordinate(originCoord.x, originCoord.y);
					if ((double)rot == 0.0)
					{
						coordinate.x += i;
						coordinate.y += j;
					}
					else if (rot == 90f)
					{
						coordinate.x += j;
						coordinate.y -= i;
					}
					else if (rot == 180f)
					{
						coordinate.x -= i;
						coordinate.y -= j;
					}
					else if (rot == 270f)
					{
						coordinate.x -= j;
						coordinate.y += i;
					}
					else
					{
						Console.LogWarning("Cock!!!!!! " + rot.ToString(), null);
					}
					list.Add(new CoordinatePair(new Coordinate(i, j), coordinate));
				}
			}
			return list;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00042E3C File Offset: 0x0004103C
		public static Coordinate RotateCoordinates(Coordinate coord, float angle)
		{
			angle = (float)Coordinate.MathMod(Mathf.RoundToInt(angle), 360);
			if (Mathf.Abs(angle - 90f) < 0.01f)
			{
				return new Coordinate(coord.y, -coord.x);
			}
			if (Mathf.Abs(angle - 180f) < 0.01f)
			{
				return new Coordinate(-coord.x, -coord.y);
			}
			if (Mathf.Abs(angle - 270f) < 0.01f)
			{
				return new Coordinate(-coord.y, coord.x);
			}
			return coord;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00042ED0 File Offset: 0x000410D0
		private static int MathMod(int a, int b)
		{
			return (Mathf.Abs(a * b) + a) % b;
		}

		// Token: 0x04000F6C RID: 3948
		public int x;

		// Token: 0x04000F6D RID: 3949
		public int y;
	}
}
