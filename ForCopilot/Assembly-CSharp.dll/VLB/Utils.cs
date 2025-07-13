using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200015C RID: 348
	public static class Utils
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x0001D8D5 File Offset: 0x0001BAD5
		public static float ComputeConeRadiusEnd(float fallOffEnd, float spotAngle)
		{
			return fallOffEnd * Mathf.Tan(spotAngle * 0.017453292f * 0.5f);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001D8EB File Offset: 0x0001BAEB
		public static float ComputeSpotAngle(float fallOffEnd, float coneRadiusEnd)
		{
			return Mathf.Atan2(coneRadiusEnd, fallOffEnd) * 57.29578f * 2f;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001D900 File Offset: 0x0001BB00
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001D927 File Offset: 0x0001BB27
		public static string GetPath(Transform current)
		{
			if (current.parent == null)
			{
				return "/" + current.name;
			}
			return Utils.GetPath(current.parent) + "/" + current.name;
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001D963 File Offset: 0x0001BB63
		public static T NewWithComponent<T>(string name) where T : Component
		{
			return new GameObject(name, new Type[]
			{
				typeof(T)
			}).GetComponent<T>();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001D984 File Offset: 0x0001BB84
		public static T GetOrAddComponent<T>(this GameObject self) where T : Component
		{
			T t = self.GetComponent<T>();
			if (t == null)
			{
				t = self.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001D9AE File Offset: 0x0001BBAE
		public static T GetOrAddComponent<T>(this MonoBehaviour self) where T : Component
		{
			return self.gameObject.GetOrAddComponent<T>();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001D9BC File Offset: 0x0001BBBC
		public static void ForeachComponentsInAnyChildrenOnly<T>(this GameObject self, Action<T> lambda, bool includeInactive = false) where T : Component
		{
			foreach (T t in self.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.gameObject != self)
				{
					lambda(t);
				}
			}
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001DA04 File Offset: 0x0001BC04
		public static void ForeachComponentsInDirectChildrenOnly<T>(this GameObject self, Action<T> lambda, bool includeInactive = false) where T : Component
		{
			foreach (T t in self.GetComponentsInChildren<T>(includeInactive))
			{
				if (t.transform.parent == self.transform)
				{
					lambda(t);
				}
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001DA54 File Offset: 0x0001BC54
		public static void SetupDepthCamera(Camera depthCamera, float coneApexOffsetZ, float maxGeometryDistance, float coneRadiusStart, float coneRadiusEnd, Vector3 beamLocalForward, Vector3 lossyScale, bool isScalable, Quaternion beamInternalLocalRotation, bool shouldScaleMinNearClipPlane)
		{
			if (!isScalable)
			{
				lossyScale.x = (lossyScale.y = 1f);
			}
			bool flag = coneApexOffsetZ >= 0f;
			float num = Mathf.Max(coneApexOffsetZ, 0f);
			depthCamera.orthographic = !flag;
			depthCamera.transform.localPosition = beamLocalForward * -num;
			Quaternion quaternion = beamInternalLocalRotation;
			if (Mathf.Sign(lossyScale.z) < 0f)
			{
				quaternion *= Quaternion.Euler(0f, 180f, 0f);
			}
			depthCamera.transform.localRotation = quaternion;
			if (!Mathf.Approximately(lossyScale.y * lossyScale.z, 0f))
			{
				float num2 = flag ? 0.1f : 0f;
				float num3 = Mathf.Abs(lossyScale.z);
				depthCamera.nearClipPlane = Mathf.Max(num * num3, num2 * (shouldScaleMinNearClipPlane ? num3 : 1f));
				depthCamera.farClipPlane = (maxGeometryDistance + num * (isScalable ? 1f : num3)) * (isScalable ? num3 : 1f);
				depthCamera.aspect = Mathf.Abs(lossyScale.x / lossyScale.y);
				if (flag)
				{
					float fieldOfView = Mathf.Atan2(coneRadiusEnd * Mathf.Abs(lossyScale.y), depthCamera.farClipPlane) * 57.29578f * 2f;
					depthCamera.fieldOfView = fieldOfView;
					return;
				}
				depthCamera.orthographicSize = coneRadiusStart * lossyScale.y;
			}
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001DBCE File Offset: 0x0001BDCE
		public static bool HasFlag(this Enum mask, Enum flags)
		{
			return ((int)mask & (int)flags) == (int)flags;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
		public static Vector3 Divide(this Vector3 aVector, Vector3 scale)
		{
			if (Mathf.Approximately(scale.x * scale.y * scale.z, 0f))
			{
				return Vector3.zero;
			}
			return new Vector3(aVector.x / scale.x, aVector.y / scale.y, aVector.z / scale.z);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001DC47 File Offset: 0x0001BE47
		public static Vector2 xy(this Vector3 aVector)
		{
			return new Vector2(aVector.x, aVector.y);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001DC5A File Offset: 0x0001BE5A
		public static Vector2 xz(this Vector3 aVector)
		{
			return new Vector2(aVector.x, aVector.z);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001DC6D File Offset: 0x0001BE6D
		public static Vector2 yz(this Vector3 aVector)
		{
			return new Vector2(aVector.y, aVector.z);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001DC80 File Offset: 0x0001BE80
		public static Vector2 yx(this Vector3 aVector)
		{
			return new Vector2(aVector.y, aVector.x);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001DC93 File Offset: 0x0001BE93
		public static Vector2 zx(this Vector3 aVector)
		{
			return new Vector2(aVector.z, aVector.x);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001DCA6 File Offset: 0x0001BEA6
		public static Vector2 zy(this Vector3 aVector)
		{
			return new Vector2(aVector.z, aVector.y);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0001DCB9 File Offset: 0x0001BEB9
		public static bool Approximately(this float a, float b, float epsilon = 1E-05f)
		{
			return Mathf.Abs(a - b) < epsilon;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001DCC6 File Offset: 0x0001BEC6
		public static bool Approximately(this Vector2 a, Vector2 b, float epsilon = 1E-05f)
		{
			return Vector2.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001DCD7 File Offset: 0x0001BED7
		public static bool Approximately(this Vector3 a, Vector3 b, float epsilon = 1E-05f)
		{
			return Vector3.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001DCE8 File Offset: 0x0001BEE8
		public static bool Approximately(this Vector4 a, Vector4 b, float epsilon = 1E-05f)
		{
			return Vector4.SqrMagnitude(a - b) < epsilon;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001DCF9 File Offset: 0x0001BEF9
		public static Vector4 AsVector4(this Vector3 vec3, float w)
		{
			return new Vector4(vec3.x, vec3.y, vec3.z, w);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001DD13 File Offset: 0x0001BF13
		public static Vector4 PlaneEquation(Vector3 normalizedNormal, Vector3 pt)
		{
			return normalizedNormal.AsVector4(-Vector3.Dot(normalizedNormal, pt));
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001DD23 File Offset: 0x0001BF23
		public static float GetVolumeCubic(this Bounds self)
		{
			return self.size.x * self.size.y * self.size.z;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001DD4C File Offset: 0x0001BF4C
		public static float GetMaxArea2D(this Bounds self)
		{
			return Mathf.Max(Mathf.Max(self.size.x * self.size.y, self.size.y * self.size.z), self.size.x * self.size.z);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001DDAE File Offset: 0x0001BFAE
		public static Color Opaque(this Color self)
		{
			return new Color(self.r, self.g, self.b, 1f);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001DDCC File Offset: 0x0001BFCC
		public static Color ComputeComplementaryColor(this Color self, bool blackAndWhite)
		{
			if (!blackAndWhite)
			{
				return new Color(1f - self.r, 1f - self.g, 1f - self.b);
			}
			if ((double)self.r * 0.299 + (double)self.g * 0.587 + (double)self.b * 0.114 <= 0.729411780834198)
			{
				return Color.white;
			}
			return Color.black;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001DE51 File Offset: 0x0001C051
		public static Plane TranslateCustom(this Plane plane, Vector3 translation)
		{
			plane.distance += Vector3.Dot(translation.normalized, plane.normal) * translation.magnitude;
			return plane;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001DE7D File Offset: 0x0001C07D
		public static Vector3 ClosestPointOnPlaneCustom(this Plane plane, Vector3 point)
		{
			return point - plane.GetDistanceToPoint(point) * plane.normal;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001DE99 File Offset: 0x0001C099
		public static bool IsAlmostZero(float f)
		{
			return Mathf.Abs(f) < 0.001f;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001DEA8 File Offset: 0x0001C0A8
		public static bool IsValid(this Plane plane)
		{
			return plane.normal.sqrMagnitude > 0.5f;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001DECB File Offset: 0x0001C0CB
		public static void SetKeywordEnabled(this Material mat, string name, bool enabled)
		{
			if (enabled)
			{
				mat.EnableKeyword(name);
				return;
			}
			mat.DisableKeyword(name);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001DEDF File Offset: 0x0001C0DF
		public static void SetShaderKeywordEnabled(string name, bool enabled)
		{
			if (enabled)
			{
				Shader.EnableKeyword(name);
				return;
			}
			Shader.DisableKeyword(name);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
		public static Matrix4x4 SampleInMatrix(this Gradient self, int floatPackingPrecision)
		{
			Matrix4x4 result = default(Matrix4x4);
			for (int i = 0; i < 16; i++)
			{
				Color color = self.Evaluate(Mathf.Clamp01((float)i / 15f));
				result[i] = color.PackToFloat(floatPackingPrecision);
			}
			return result;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001DF3C File Offset: 0x0001C13C
		public static Color[] SampleInArray(this Gradient self, int samplesCount)
		{
			Color[] array = new Color[samplesCount];
			for (int i = 0; i < samplesCount; i++)
			{
				array[i] = self.Evaluate(Mathf.Clamp01((float)i / (float)(samplesCount - 1)));
			}
			return array;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001DF76 File Offset: 0x0001C176
		private static Vector4 Vector4_Floor(Vector4 vec)
		{
			return new Vector4(Mathf.Floor(vec.x), Mathf.Floor(vec.y), Mathf.Floor(vec.z), Mathf.Floor(vec.w));
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001DFAC File Offset: 0x0001C1AC
		public static float PackToFloat(this Color color, int floatPackingPrecision)
		{
			Vector4 vector = Utils.Vector4_Floor(color * (float)(floatPackingPrecision - 1));
			return 0f + vector.x * (float)floatPackingPrecision * (float)floatPackingPrecision * (float)floatPackingPrecision + vector.y * (float)floatPackingPrecision * (float)floatPackingPrecision + vector.z * (float)floatPackingPrecision + vector.w;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001E001 File Offset: 0x0001C201
		public static Utils.FloatPackingPrecision GetFloatPackingPrecision()
		{
			if (Utils.ms_FloatPackingPrecision == Utils.FloatPackingPrecision.Undef)
			{
				Utils.ms_FloatPackingPrecision = ((SystemInfo.graphicsShaderLevel >= 35) ? Utils.FloatPackingPrecision.High : Utils.FloatPackingPrecision.Low);
			}
			return Utils.ms_FloatPackingPrecision;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001E022 File Offset: 0x0001C222
		public static bool HasAtLeastOneFlag(this Enum mask, Enum flags)
		{
			return ((int)mask & (int)flags) != 0;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000045B1 File Offset: 0x000027B1
		public static void MarkCurrentSceneDirty()
		{
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000045B1 File Offset: 0x000027B1
		public static void MarkObjectDirty(UnityEngine.Object obj)
		{
		}

		// Token: 0x04000772 RID: 1906
		private const float kEpsilon = 1E-05f;

		// Token: 0x04000773 RID: 1907
		private static Utils.FloatPackingPrecision ms_FloatPackingPrecision;

		// Token: 0x04000774 RID: 1908
		private const int kFloatPackingHighMinShaderLevel = 35;

		// Token: 0x0200015D RID: 349
		public enum FloatPackingPrecision
		{
			// Token: 0x04000776 RID: 1910
			High = 64,
			// Token: 0x04000777 RID: 1911
			Low = 8,
			// Token: 0x04000778 RID: 1912
			Undef = 0
		}
	}
}
