using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200017D RID: 381
	public class FishAnimator : MonoBehaviour
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x00020E24 File Offset: 0x0001F024
		private void Update()
		{
			Vector3 position = Camera.main.transform.position;
			base.transform.LookAt(new Vector3(-position.x, base.transform.position.y, -position.z));
		}
	}
}
