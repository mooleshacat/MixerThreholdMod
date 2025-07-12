using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Misc
{
	// Token: 0x02000C65 RID: 3173
	public class TreeScaler : MonoBehaviour
	{
		// Token: 0x06005958 RID: 22872 RVA: 0x0017988F File Offset: 0x00177A8F
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.UpdateScale));
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x001798B8 File Offset: 0x00177AB8
		private void UpdateScale()
		{
			float num = Mathf.Clamp(Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position), this.minScaleDistance, this.maxScaleDistance) / (this.maxScaleDistance - this.minScaleDistance);
			float num2 = this.minScale + (this.maxScale - this.minScale) * num;
			foreach (Transform transform in this.branchMeshes)
			{
				transform.localScale = new Vector3(num2, 1f, num2);
			}
		}

		// Token: 0x0400417B RID: 16763
		[Header("References")]
		[SerializeField]
		protected List<Transform> branchMeshes = new List<Transform>();

		// Token: 0x0400417C RID: 16764
		public float minScale = 1f;

		// Token: 0x0400417D RID: 16765
		public float maxScale = 1.3f;

		// Token: 0x0400417E RID: 16766
		public float minScaleDistance = 20f;

		// Token: 0x0400417F RID: 16767
		public float maxScaleDistance = 100f;
	}
}
