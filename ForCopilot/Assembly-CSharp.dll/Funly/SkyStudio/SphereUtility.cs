using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E2 RID: 482
	public abstract class SphereUtility
	{
		// Token: 0x06000A8D RID: 2701 RVA: 0x0002EBBC File Offset: 0x0002CDBC
		public static Vector2 DirectionToSphericalCoordinate(Vector3 direction)
		{
			Vector3 normalized = direction.normalized;
			float x = SphereUtility.Atan2Positive(normalized.z, normalized.x);
			float num = Vector3.Angle(direction, Vector3.up) * 0.017453292f;
			float y;
			if (num <= 1.5707964f)
			{
				y = 1.5707964f - num;
			}
			else
			{
				y = -1f * (num - 1.5707964f);
			}
			return new Vector2(x, y);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0002EC20 File Offset: 0x0002CE20
		public static Vector3 SphericalCoordinateToDirection(Vector2 coord)
		{
			float num = Mathf.Cos(coord.y);
			float y = Mathf.Sin(coord.y);
			float num2 = num;
			num = num2 * Mathf.Cos(coord.x);
			float z = num2 * Mathf.Sin(coord.x);
			return new Vector3(num, y, z);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002EC6F File Offset: 0x0002CE6F
		public static float RadiusAtHeight(float yPos)
		{
			return Mathf.Abs(Mathf.Cos(Mathf.Asin(yPos)));
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0002EC84 File Offset: 0x0002CE84
		public static Vector3 SphericalToPoint(float yPosition, float radAngle)
		{
			float num = SphereUtility.RadiusAtHeight(yPosition);
			return new Vector3(num * Mathf.Cos(radAngle), yPosition, num * Mathf.Sin(radAngle));
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002ECAE File Offset: 0x0002CEAE
		public static float RadAngleToPercent(float radAngle)
		{
			return radAngle / 6.2831855f;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002ECB7 File Offset: 0x0002CEB7
		public static float PercentToRadAngle(float percent)
		{
			return percent * 6.2831855f;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0002ECC0 File Offset: 0x0002CEC0
		public static float HeightToPercent(float yValue)
		{
			return yValue / 2f + 0.5f;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0002ECCF File Offset: 0x0002CECF
		public static float PercentToHeight(float hPercent)
		{
			return Mathf.Lerp(-1f, 1f, hPercent);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0002ECE4 File Offset: 0x0002CEE4
		public static float AngleToReachTarget(Vector2 point, float targetAngle)
		{
			float num = SphereUtility.Atan2Positive(point.y, point.x);
			return 6.2831855f - num + targetAngle;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0002ED0C File Offset: 0x0002CF0C
		public static float Atan2Positive(float y, float x)
		{
			float num = Mathf.Atan2(y, x);
			if (num < 0f)
			{
				num = 3.1415927f + (3.1415927f + num);
			}
			return num;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0002ED38 File Offset: 0x0002CF38
		public static Vector3 RotateAroundXAxis(Vector3 point, float angle)
		{
			Vector2 vector = SphereUtility.Rotate2d(new Vector2(point.z, point.y), angle);
			return new Vector3(point.x, vector.y, vector.x);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0002ED74 File Offset: 0x0002CF74
		public static Vector3 RotateAroundYAxis(Vector3 point, float angle)
		{
			Vector2 vector = SphereUtility.Rotate2d(new Vector2(point.x, point.z), angle);
			return new Vector3(vector.x, point.y, vector.y);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002EDB0 File Offset: 0x0002CFB0
		public static Vector3 RotatePoint(Vector3 point, float xAxisRotation, float yAxisRotation)
		{
			return SphereUtility.RotateAroundXAxis(SphereUtility.RotateAroundYAxis(point, yAxisRotation), xAxisRotation);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0002EDBF File Offset: 0x0002CFBF
		public static Vector2 Rotate2d(Vector2 pos, float angle)
		{
			return SphereUtility.Matrix2x2Mult(new Vector4(Mathf.Cos(angle), -Mathf.Sin(angle), Mathf.Sin(angle), Mathf.Cos(angle)), pos);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002EDE8 File Offset: 0x0002CFE8
		public static Vector2 Matrix2x2Mult(Vector4 matrix, Vector2 pos)
		{
			return new Vector2(matrix[0] * pos[0] + matrix[1] * pos[1], matrix[2] * pos[0] + matrix[3] * pos[1]);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002EE40 File Offset: 0x0002D040
		public static void CalculateStarRotation(Vector3 star, out float xRotationAngle, out float yRotationAngle)
		{
			Vector3 vector = new Vector3(star.x, star.y, star.z);
			yRotationAngle = SphereUtility.AngleToReachTarget(new Vector2(vector.x, vector.z), 1.5707964f);
			vector = SphereUtility.RotateAroundYAxis(vector, yRotationAngle);
			xRotationAngle = SphereUtility.AngleToReachTarget(new Vector3(vector.z, vector.y), 0f);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002EEAE File Offset: 0x0002D0AE
		public static Vector2 ConvertUVToSphericalCoordinate(Vector2 uv)
		{
			return new Vector2(Mathf.Lerp(0f, 6.2831855f, uv.x), Mathf.Lerp(-1.5707964f, 1.5707964f, uv.y));
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002EEDF File Offset: 0x0002D0DF
		public static Vector2 ConvertSphericalCoordateToUV(Vector2 sphereCoord)
		{
			return new Vector2(sphereCoord.x / 6.2831855f, (sphereCoord.y + 1.5707964f) / 3.1415927f);
		}

		// Token: 0x04000B72 RID: 2930
		private const float k_HalfPI = 1.5707964f;
	}
}
