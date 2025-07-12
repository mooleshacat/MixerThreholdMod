using System;
using System.Collections.Generic;
using System.Linq;
using FluffyUnderware.Curvy;
using UnityEngine;

namespace ScheduleOne.Math
{
	// Token: 0x02000591 RID: 1425
	public class PathSmoothingUtility : MonoBehaviour
	{
		// Token: 0x06002289 RID: 8841 RVA: 0x0008E990 File Offset: 0x0008CB90
		private void Awake()
		{
			PathSmoothingUtility.spline = CurvySpline.Create();
			PathSmoothingUtility.spline.transform.SetParent(base.transform);
			PathSmoothingUtility.spline.Interpolation = 4;
			PathSmoothingUtility.spline.BSplineDegree = 5;
			PathSmoothingUtility.spline.Orientation = 0;
			PathSmoothingUtility.spline.CacheDensity = 30;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0008E9EC File Offset: 0x0008CBEC
		public static PathSmoothingUtility.SmoothedPath CalculateSmoothedPath(List<Vector3> controlPoints, float maxCPDistance = 5f)
		{
			if (controlPoints.Count < 2)
			{
				Debug.LogWarning("Smoothing requires at least 2 control points.");
				return new PathSmoothingUtility.SmoothedPath
				{
					vectorPath = controlPoints
				};
			}
			for (int i = 1; i < controlPoints.Count; i++)
			{
				if (Vector3.Distance(controlPoints[i], controlPoints[i - 1]) < 0.5f)
				{
					controlPoints.RemoveAt(i);
					i--;
				}
			}
			if (controlPoints.Count < 2)
			{
				Debug.LogWarning("Smoothing requires at least 2 control points.");
				return new PathSmoothingUtility.SmoothedPath
				{
					vectorPath = controlPoints
				};
			}
			PathSmoothingUtility.SmoothedPath smoothedPath = new PathSmoothingUtility.SmoothedPath();
			controlPoints = PathSmoothingUtility.InsertIntermediatePoints(controlPoints, maxCPDistance);
			PathSmoothingUtility.spline.Clear(false);
			PathSmoothingUtility.spline.Add(controlPoints.ToArray(), Space.World);
			PathSmoothingUtility.spline.Refresh();
			List<Vector3> collection = PathSmoothingUtility.spline.GetApproximation(Space.Self).ToList<Vector3>();
			smoothedPath.vectorPath.AddRange(collection);
			return smoothedPath;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x0008EAC4 File Offset: 0x0008CCC4
		public static void DrawPath(PathSmoothingUtility.SmoothedPath path, Color col, float duration)
		{
			for (int i = 1; i < path.vectorPath.Count; i++)
			{
				Debug.DrawLine(path.vectorPath[i - 1], path.vectorPath[i], col, duration);
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0008EB08 File Offset: 0x0008CD08
		private static List<Vector3> InsertIntermediatePoints(List<Vector3> points, float maxDistance)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				Vector3 a = points[i];
				Vector3 b = points[i + 1];
				float num = Vector3.Distance(a, b);
				if (num > maxDistance)
				{
					int num2 = Mathf.FloorToInt(num / maxDistance);
					for (int j = 0; j < num2; j++)
					{
						Vector3 item = Vector3.Lerp(a, b, (float)(j + 1) * (1f / (float)(num2 + 1)));
						points.Insert(i + (j + 1), item);
					}
				}
			}
			return points;
		}

		// Token: 0x04001A37 RID: 6711
		public const float MinControlPointDistance = 0.5f;

		// Token: 0x04001A38 RID: 6712
		private static CurvySpline spline;

		// Token: 0x02000592 RID: 1426
		public class SmoothedPath
		{
			// Token: 0x0600228E RID: 8846 RVA: 0x0008EB8C File Offset: 0x0008CD8C
			public void InitializePath()
			{
				this.segmentBounds.Clear();
				for (int i = 0; i < this.vectorPath.Count - 1; i++)
				{
					Vector3 lhs = this.vectorPath[i];
					Vector3 rhs = this.vectorPath[i + 1];
					Vector3 a = Vector3.Min(lhs, rhs);
					Vector3 a2 = Vector3.Max(lhs, rhs);
					Bounds item = default(Bounds);
					item.SetMinMax(a - Vector3.one * 10f, a2 + Vector3.one * 10f);
					this.segmentBounds.Add(item);
				}
			}

			// Token: 0x04001A39 RID: 6713
			public const float MARGIN = 10f;

			// Token: 0x04001A3A RID: 6714
			public List<Vector3> vectorPath = new List<Vector3>();

			// Token: 0x04001A3B RID: 6715
			public List<Bounds> segmentBounds = new List<Bounds>();
		}
	}
}
