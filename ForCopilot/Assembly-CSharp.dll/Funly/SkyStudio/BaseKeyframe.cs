using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B6 RID: 438
	[Serializable]
	public class BaseKeyframe : IComparable, IBaseKeyframe
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00028291 File Offset: 0x00026491
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x00028299 File Offset: 0x00026499
		public string id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x000282A2 File Offset: 0x000264A2
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x000282AA File Offset: 0x000264AA
		public float time
		{
			get
			{
				return this.m_Time;
			}
			set
			{
				this.m_Time = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x000282B3 File Offset: 0x000264B3
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x000282BB File Offset: 0x000264BB
		public InterpolationCurve interpolationCurve
		{
			get
			{
				return this.m_InterpolationCurve;
			}
			set
			{
				this.m_InterpolationCurve = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x000282C4 File Offset: 0x000264C4
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x000282CC File Offset: 0x000264CC
		public InterpolationDirection interpolationDirection
		{
			get
			{
				return this.m_InterpolationDirection;
			}
			set
			{
				this.m_InterpolationDirection = value;
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000282D8 File Offset: 0x000264D8
		public BaseKeyframe(float time)
		{
			this.id = Guid.NewGuid().ToString();
			this.time = time;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002830C File Offset: 0x0002650C
		public int CompareTo(object other)
		{
			BaseKeyframe baseKeyframe = other as BaseKeyframe;
			return this.time.CompareTo(baseKeyframe.time);
		}

		// Token: 0x04000983 RID: 2435
		[SerializeField]
		public string m_Id;

		// Token: 0x04000984 RID: 2436
		[SerializeField]
		private float m_Time;

		// Token: 0x04000985 RID: 2437
		[SerializeField]
		private InterpolationCurve m_InterpolationCurve;

		// Token: 0x04000986 RID: 2438
		[SerializeField]
		private InterpolationDirection m_InterpolationDirection;
	}
}
