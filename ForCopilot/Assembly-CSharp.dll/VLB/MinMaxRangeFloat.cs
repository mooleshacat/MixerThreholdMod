using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200013A RID: 314
	[Serializable]
	public struct MinMaxRangeFloat : IEquatable<MinMaxRangeFloat>
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00019EB7 File Offset: 0x000180B7
		public float minValue
		{
			get
			{
				return this.m_MinValue;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00019EBF File Offset: 0x000180BF
		public float maxValue
		{
			get
			{
				return this.m_MaxValue;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00019EC7 File Offset: 0x000180C7
		public float randomValue
		{
			get
			{
				return UnityEngine.Random.Range(this.minValue, this.maxValue);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00019EDA File Offset: 0x000180DA
		public Vector2 asVector2
		{
			get
			{
				return new Vector2(this.minValue, this.maxValue);
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00019EED File Offset: 0x000180ED
		public float GetLerpedValue(float lerp01)
		{
			return Mathf.Lerp(this.minValue, this.maxValue, lerp01);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00019F01 File Offset: 0x00018101
		public MinMaxRangeFloat(float min, float max)
		{
			this.m_MinValue = min;
			this.m_MaxValue = max;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00019F14 File Offset: 0x00018114
		public override bool Equals(object obj)
		{
			if (obj is MinMaxRangeFloat)
			{
				MinMaxRangeFloat other = (MinMaxRangeFloat)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00019F39 File Offset: 0x00018139
		public bool Equals(MinMaxRangeFloat other)
		{
			return this.m_MinValue == other.m_MinValue && this.m_MaxValue == other.m_MaxValue;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00019F5C File Offset: 0x0001815C
		public override int GetHashCode()
		{
			return new ValueTuple<float, float>(this.m_MinValue, this.m_MaxValue).GetHashCode();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00019F88 File Offset: 0x00018188
		public static bool operator ==(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00019F92 File Offset: 0x00018192
		public static bool operator !=(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400069D RID: 1693
		[SerializeField]
		private float m_MinValue;

		// Token: 0x0400069E RID: 1694
		[SerializeField]
		private float m_MaxValue;
	}
}
