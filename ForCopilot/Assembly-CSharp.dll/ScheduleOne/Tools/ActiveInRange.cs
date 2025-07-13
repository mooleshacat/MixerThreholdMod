using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200087F RID: 2175
	public class ActiveInRange : MonoBehaviour
	{
		// Token: 0x06003B91 RID: 15249 RVA: 0x000FC458 File Offset: 0x000FA658
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			bool flag = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position) < this.Distance * (this.ScaleByLODBias ? QualitySettings.lodBias : 1f);
			if (flag && !this.isVisible)
			{
				this.isVisible = true;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(!this.Reverse);
				}
				return;
			}
			if (!flag && this.isVisible)
			{
				this.isVisible = false;
				GameObject[] objectsToActivate = this.ObjectsToActivate;
				for (int i = 0; i < objectsToActivate.Length; i++)
				{
					objectsToActivate[i].SetActive(this.Reverse);
				}
			}
		}

		// Token: 0x04002A89 RID: 10889
		public float Distance = 10f;

		// Token: 0x04002A8A RID: 10890
		public bool ScaleByLODBias = true;

		// Token: 0x04002A8B RID: 10891
		public GameObject[] ObjectsToActivate;

		// Token: 0x04002A8C RID: 10892
		public bool Reverse;

		// Token: 0x04002A8D RID: 10893
		private bool isVisible = true;
	}
}
