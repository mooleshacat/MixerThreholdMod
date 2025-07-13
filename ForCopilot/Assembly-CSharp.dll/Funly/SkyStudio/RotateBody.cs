using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000193 RID: 403
	public class RotateBody : MonoBehaviour
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x00026885 File Offset: 0x00024A85
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0002688D File Offset: 0x00024A8D
		public float SpinSpeed
		{
			get
			{
				return this.m_SpinSpeed;
			}
			set
			{
				this.m_SpinSpeed = value;
				this.UpdateOrbitBodyRotation();
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000850 RID: 2128 RVA: 0x0002689C File Offset: 0x00024A9C
		// (set) Token: 0x06000851 RID: 2129 RVA: 0x000268A4 File Offset: 0x00024AA4
		public bool AllowSpinning
		{
			get
			{
				return this.m_AllowSpinning;
			}
			set
			{
				this.m_AllowSpinning = value;
				this.UpdateOrbitBodyRotation();
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000268B4 File Offset: 0x00024AB4
		public void UpdateOrbitBodyRotation()
		{
			float num = (float)(this.m_AllowSpinning ? 1 : 0);
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 euler = new Vector3(0f, -180f, (eulerAngles.z + -10f * this.SpinSpeed * Time.deltaTime) * num);
			base.transform.localRotation = Quaternion.Euler(euler);
		}

		// Token: 0x0400093C RID: 2364
		private float m_SpinSpeed;

		// Token: 0x0400093D RID: 2365
		private bool m_AllowSpinning;
	}
}
