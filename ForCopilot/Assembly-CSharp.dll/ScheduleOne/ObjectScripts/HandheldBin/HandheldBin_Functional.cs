using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.HandheldBin
{
	// Token: 0x02000C53 RID: 3155
	public class HandheldBin_Functional : MonoBehaviour
	{
		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x060058F1 RID: 22769 RVA: 0x00177F4E File Offset: 0x0017614E
		// (set) Token: 0x060058F2 RID: 22770 RVA: 0x00177F56 File Offset: 0x00176156
		public float fillLevel { get; protected set; }

		// Token: 0x060058F3 RID: 22771 RVA: 0x00177F5F File Offset: 0x0017615F
		protected virtual void Awake()
		{
			this.UpdateTrashVisuals();
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x00177F5F File Offset: 0x0017615F
		public void SetAmount(float amount)
		{
			this.UpdateTrashVisuals();
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x00177F67 File Offset: 0x00176167
		protected virtual void UpdateTrashVisuals()
		{
			this.trash.gameObject.SetActive(this.fillLevel > 0f);
		}

		// Token: 0x04004118 RID: 16664
		[Header("References")]
		public Transform trash;

		// Token: 0x04004119 RID: 16665
		[Header("Settings")]
		public float trash_MinY;

		// Token: 0x0400411A RID: 16666
		public float trash_MaxY;
	}
}
