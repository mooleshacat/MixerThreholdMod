using System;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008F3 RID: 2291
	public class StoredItemRandomRotation : MonoBehaviour
	{
		// Token: 0x06003E22 RID: 15906 RVA: 0x001062A8 File Offset: 0x001044A8
		public void Awake()
		{
			this.ItemContainer.localEulerAngles = new Vector3(this.ItemContainer.localEulerAngles.x, UnityEngine.Random.Range(0f, 360f), this.ItemContainer.localEulerAngles.z);
		}

		// Token: 0x04002C46 RID: 11334
		public Transform ItemContainer;
	}
}
