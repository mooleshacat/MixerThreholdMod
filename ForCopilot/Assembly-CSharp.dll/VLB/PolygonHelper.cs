using System;
using System.Collections.Generic;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200013E RID: 318
	public class PolygonHelper : MonoBehaviour
	{
		// Token: 0x0200013F RID: 319
		public struct Plane2D
		{
			// Token: 0x06000570 RID: 1392 RVA: 0x0001A0AA File Offset: 0x000182AA
			public float Distance(Vector2 point)
			{
				return Vector2.Dot(this.normal, point) + this.distance;
			}

			// Token: 0x06000571 RID: 1393 RVA: 0x0001A0BF File Offset: 0x000182BF
			public Vector2 ClosestPoint(Vector2 pt)
			{
				return pt - this.normal * this.Distance(pt);
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x0001A0DC File Offset: 0x000182DC
			public Vector2 Intersect(Vector2 p1, Vector2 p2)
			{
				float num = Vector2.Dot(this.normal, p1 - p2);
				if (Utils.IsAlmostZero(num))
				{
					return (p1 + p2) * 0.5f;
				}
				float d = (this.normal.x * p1.x + this.normal.y * p1.y + this.distance) / num;
				return p1 + d * (p2 - p1);
			}

			// Token: 0x06000573 RID: 1395 RVA: 0x0001A158 File Offset: 0x00018358
			public bool GetSide(Vector2 point)
			{
				return this.Distance(point) > 0f;
			}

			// Token: 0x06000574 RID: 1396 RVA: 0x0001A168 File Offset: 0x00018368
			public static PolygonHelper.Plane2D FromPoints(Vector3 p1, Vector3 p2)
			{
				Vector3 normalized = (p2 - p1).normalized;
				return new PolygonHelper.Plane2D
				{
					normal = new Vector2(normalized.y, -normalized.x),
					distance = -normalized.y * p1.x + normalized.x * p1.y
				};
			}

			// Token: 0x06000575 RID: 1397 RVA: 0x0001A1CC File Offset: 0x000183CC
			public static PolygonHelper.Plane2D FromNormalAndPoint(Vector3 normalizedNormal, Vector3 p1)
			{
				return new PolygonHelper.Plane2D
				{
					normal = normalizedNormal,
					distance = -normalizedNormal.x * p1.x - normalizedNormal.y * p1.y
				};
			}

			// Token: 0x06000576 RID: 1398 RVA: 0x0001A212 File Offset: 0x00018412
			public void Flip()
			{
				this.normal = -this.normal;
				this.distance = -this.distance;
			}

			// Token: 0x06000577 RID: 1399 RVA: 0x0001A234 File Offset: 0x00018434
			public Vector2[] CutConvex(Vector2[] poly)
			{
				List<Vector2> list = new List<Vector2>(poly.Length);
				Vector2 vector = poly[poly.Length - 1];
				foreach (Vector2 vector2 in poly)
				{
					bool side = this.GetSide(vector);
					bool side2 = this.GetSide(vector2);
					if (side && side2)
					{
						list.Add(vector2);
					}
					else if (side && !side2)
					{
						list.Add(this.Intersect(vector, vector2));
					}
					else if (!side && side2)
					{
						list.Add(this.Intersect(vector, vector2));
						list.Add(vector2);
					}
					vector = vector2;
				}
				return list.ToArray();
			}

			// Token: 0x06000578 RID: 1400 RVA: 0x0001A2D6 File Offset: 0x000184D6
			public override string ToString()
			{
				return string.Format("{0} x {1} + {2}", this.normal.x, this.normal.y, this.distance);
			}

			// Token: 0x040006A5 RID: 1701
			public Vector2 normal;

			// Token: 0x040006A6 RID: 1702
			public float distance;
		}
	}
}
