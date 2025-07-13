using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000192 RID: 402
	[ExecuteInEditMode]
	public class OrbitingBody : MonoBehaviour
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x000266EE File Offset: 0x000248EE
		public Transform positionTransform
		{
			get
			{
				if (this.m_PositionTransform == null)
				{
					this.m_PositionTransform = base.transform.Find("Position");
				}
				return this.m_PositionTransform;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0002671C File Offset: 0x0002491C
		public RotateBody rotateBody
		{
			get
			{
				if (this.m_RotateBody == null)
				{
					Transform positionTransform = this.positionTransform;
					if (!positionTransform)
					{
						Debug.LogError("Can't return rotation body without a position transform game object");
						return null;
					}
					this.m_RotateBody = positionTransform.GetComponent<RotateBody>();
				}
				return this.m_RotateBody;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x00026764 File Offset: 0x00024964
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x0002676C File Offset: 0x0002496C
		public SpherePoint Point
		{
			get
			{
				return this.m_SpherePoint;
			}
			set
			{
				if (this.m_SpherePoint == null)
				{
					this.m_SpherePoint = new SpherePoint(0f, 0f);
				}
				else
				{
					this.m_SpherePoint = value;
				}
				this.m_CachedWorldDirection = this.m_SpherePoint.GetWorldDirection();
				this.LayoutOribit();
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x000267AB File Offset: 0x000249AB
		public Vector3 BodyGlobalDirection
		{
			get
			{
				return this.m_CachedWorldDirection;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x000267B4 File Offset: 0x000249B4
		public Light BodyLight
		{
			get
			{
				if (this.m_BodyLight == null)
				{
					this.m_BodyLight = base.transform.GetComponentInChildren<Light>();
					if (this.m_BodyLight != null)
					{
						this.m_BodyLight.transform.localRotation = Quaternion.identity;
					}
				}
				return this.m_BodyLight;
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00026809 File Offset: 0x00024A09
		public void ResetOrbit()
		{
			this.LayoutOribit();
			this.m_PositionTransform = null;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00026818 File Offset: 0x00024A18
		public void LayoutOribit()
		{
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
			base.transform.forward = this.BodyGlobalDirection * -1f;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00026855 File Offset: 0x00024A55
		private void OnValidate()
		{
			this.LayoutOribit();
		}

		// Token: 0x04000937 RID: 2359
		private Transform m_PositionTransform;

		// Token: 0x04000938 RID: 2360
		private RotateBody m_RotateBody;

		// Token: 0x04000939 RID: 2361
		private SpherePoint m_SpherePoint = new SpherePoint(0f, 0f);

		// Token: 0x0400093A RID: 2362
		private Vector3 m_CachedWorldDirection = Vector3.right;

		// Token: 0x0400093B RID: 2363
		private Light m_BodyLight;
	}
}
