using System;
using UnityEngine;

namespace ScheduleOne.Decoration
{
	// Token: 0x02000C5D RID: 3165
	public class RockerSwitch : MonoBehaviour
	{
		// Token: 0x06005920 RID: 22816 RVA: 0x0017891E File Offset: 0x00176B1E
		private void Awake()
		{
			this.SetIsOn(this.isOn);
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x0017892C File Offset: 0x00176B2C
		public void SetIsOn(bool on)
		{
			this.isOn = on;
			this.Light.enabled = on;
			this.ButtonTransform.localEulerAngles = new Vector3(on ? 10f : -10f, 0f, 0f);
		}

		// Token: 0x04004145 RID: 16709
		public MeshRenderer ButtonMesh;

		// Token: 0x04004146 RID: 16710
		public Transform ButtonTransform;

		// Token: 0x04004147 RID: 16711
		public Light Light;

		// Token: 0x04004148 RID: 16712
		public bool isOn;
	}
}
