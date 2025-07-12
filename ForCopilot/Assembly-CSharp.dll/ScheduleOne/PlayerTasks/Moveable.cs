using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000354 RID: 852
	public class Moveable : Clickable
	{
		// Token: 0x06001317 RID: 4887 RVA: 0x00052BE0 File Offset: 0x00050DE0
		public override void StartClick(RaycastHit hit)
		{
			base.StartClick(hit);
			this.clickDist = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			this.clickOffset = base.transform.position - PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.clickDist));
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00052C60 File Offset: 0x00050E60
		protected virtual void Update()
		{
			if (base.IsHeld)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.clickDist)) + this.clickOffset, Time.deltaTime * 10f);
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Clamp(base.transform.localPosition.y, this.yMin, this.yMax), base.transform.localPosition.z);
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00052D28 File Offset: 0x00050F28
		public override void EndClick()
		{
			base.EndClick();
		}

		// Token: 0x0400123C RID: 4668
		protected Vector3 clickOffset = Vector3.zero;

		// Token: 0x0400123D RID: 4669
		protected float clickDist;

		// Token: 0x0400123E RID: 4670
		[Header("Bounds")]
		[SerializeField]
		protected float yMax = 10f;

		// Token: 0x0400123F RID: 4671
		[SerializeField]
		protected float yMin = -10f;
	}
}
