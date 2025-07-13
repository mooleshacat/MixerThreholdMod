using System;
using ScheduleOne.Math;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x02000838 RID: 2104
	public static class PathUtility
	{
		// Token: 0x060038E3 RID: 14563 RVA: 0x000EFECC File Offset: 0x000EE0CC
		public static Vector3 GetAverageAheadPoint(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, int sampleCount, float stepSize)
		{
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(path, referencePoint, out startPointIndex, out num, out pointLerp);
			Vector3 a = Vector3.zero;
			for (int i = 1; i <= sampleCount; i++)
			{
				a += PathUtility.GetPointAheadOfPathPoint(path, startPointIndex, pointLerp, (float)i * stepSize);
			}
			return a / (float)sampleCount;
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x000EFF1C File Offset: 0x000EE11C
		public static Vector3 GetAheadPoint(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, float distance)
		{
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(path, referencePoint, out startPointIndex, out num, out pointLerp);
			return PathUtility.GetPointAheadOfPathPoint(path, startPointIndex, pointLerp, distance);
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x000EFF40 File Offset: 0x000EE140
		public static Vector3 GetAheadPoint(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, float distance, int startPointIndex, float pointLerp)
		{
			return PathUtility.GetPointAheadOfPathPoint(path, startPointIndex, pointLerp, distance);
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x000EFF4C File Offset: 0x000EE14C
		public static Vector3 GetPointAheadOfPathPoint(PathSmoothingUtility.SmoothedPath path, int startPointIndex, float pointLerp, float distanceAhead)
		{
			if (path == null || path.vectorPath.Count < 2)
			{
				return Vector3.zero;
			}
			if (path.vectorPath.Count == startPointIndex + 1)
			{
				return path.vectorPath[startPointIndex];
			}
			float num = distanceAhead;
			Vector3 zero = Vector3.zero;
			int num2 = startPointIndex;
			while (num > 0f)
			{
				Vector3 vector = path.vectorPath[num2] + (path.vectorPath[num2 + 1] - path.vectorPath[num2]) * pointLerp;
				pointLerp = 0f;
				Vector3 vector2 = path.vectorPath[num2 + 1];
				if (Vector3.Distance(vector, vector2) > num)
				{
					return vector + (vector2 - vector).normalized * num;
				}
				num -= Vector3.Distance(vector, vector2);
				num2++;
				if (path.vectorPath.Count <= num2 + 1)
				{
					return vector2;
				}
			}
			return zero;
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x000F0044 File Offset: 0x000EE244
		public static float CalculateAngleChangeOverPath(PathSmoothingUtility.SmoothedPath path, int startPointIndex, float pointLerp, float distanceAhead)
		{
			if (path.vectorPath.Count == startPointIndex + 1)
			{
				return 0f;
			}
			float num = distanceAhead;
			int num2 = startPointIndex;
			float num3 = 0f;
			while (num > 0f)
			{
				Vector3 vector = path.vectorPath[num2] + (path.vectorPath[num2 + 1] - path.vectorPath[num2]) * pointLerp;
				pointLerp = 0f;
				if (path.vectorPath.Count <= num2 + 2)
				{
					break;
				}
				Vector3 vector2 = path.vectorPath[num2 + 1];
				Vector3 a = path.vectorPath[num2 + 2];
				if (Vector3.Distance(vector, vector2) > num)
				{
					break;
				}
				num -= Vector3.Distance(vector, vector2);
				num2++;
				num3 += Vector3.Angle((a - vector2).normalized, (vector2 - vector).normalized);
				if (path.vectorPath.Count <= num2 + 2)
				{
					break;
				}
			}
			return num3;
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x000F0148 File Offset: 0x000EE348
		public static float CalculateCTE(Vector3 flatCarPos, Transform vehicleTransform, Vector3 wp_from, Vector3 wp_to, PathSmoothingUtility.SmoothedPath path)
		{
			new Vector3(wp_from.x, flatCarPos.y, wp_from.z);
			new Vector3(wp_to.x, flatCarPos.y, wp_to.z);
			int num;
			int num2;
			float num3;
			Vector3 closestPointOnPath = PathUtility.GetClosestPointOnPath(path, flatCarPos, out num, out num2, out num3);
			Debug.DrawLine(flatCarPos, closestPointOnPath, Color.red);
			Vector3 vector = closestPointOnPath - flatCarPos;
			return -vehicleTransform.InverseTransformVector(Vector3.Project(vector, vehicleTransform.right)).x;
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x000F01C4 File Offset: 0x000EE3C4
		public static Vector3 GetClosestPointOnPath(PathSmoothingUtility.SmoothedPath path, Vector3 point, out int startPointIndex, out int endPointIndex, out float pointLerp)
		{
			startPointIndex = 0;
			endPointIndex = 0;
			pointLerp = 0f;
			if (path == null || path.vectorPath == null || path.vectorPath.Count < 2)
			{
				return Vector3.zero;
			}
			float num = float.MaxValue;
			Vector3 result = Vector3.zero;
			for (int i = 0; i < path.vectorPath.Count - 1; i++)
			{
				if (path.segmentBounds[i].Contains(point))
				{
					Vector3 vector = path.vectorPath[i];
					Vector3 vector2 = path.vectorPath[i + 1];
					Vector3 closestPointOnLine = PathUtility.GetClosestPointOnLine(point, vector, vector2, true);
					float sqrMagnitude = (closestPointOnLine - point).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						result = closestPointOnLine;
						num = sqrMagnitude;
						startPointIndex = i;
						Vector3 vector3 = vector2 - vector;
						pointLerp = Vector3.Dot(closestPointOnLine - vector, vector3.normalized) / vector3.magnitude;
					}
				}
			}
			endPointIndex = startPointIndex + 1;
			return result;
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x000F02BC File Offset: 0x000EE4BC
		public static Vector3 GetAheadPointDirection(PathSmoothingUtility.SmoothedPath path, Vector3 referencePoint, float distanceAhead)
		{
			int startPointIndex;
			int num;
			float pointLerp;
			PathUtility.GetClosestPointOnPath(path, referencePoint, out startPointIndex, out num, out pointLerp);
			Vector3 pointAheadOfPathPoint = PathUtility.GetPointAheadOfPathPoint(path, startPointIndex, pointLerp, distanceAhead);
			return (PathUtility.GetPointAheadOfPathPoint(path, startPointIndex, pointLerp, distanceAhead + 0.01f) - pointAheadOfPathPoint).normalized;
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x000F0300 File Offset: 0x000EE500
		private static Vector3 GetClosestPointOnLine(Vector3 point, Vector3 line_start, Vector3 line_end, bool clamp = true)
		{
			Vector3 vector = line_end - line_start;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < Mathf.Epsilon)
			{
				return line_start;
			}
			float num = Vector3.Dot(point - line_start, vector) / sqrMagnitude;
			if (clamp)
			{
				num = Mathf.Clamp01(num);
			}
			return line_start + num * vector;
		}
	}
}
