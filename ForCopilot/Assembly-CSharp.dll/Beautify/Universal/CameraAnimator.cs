using System;
using UnityEngine;

namespace Beautify.Universal
{
	// Token: 0x020001FC RID: 508
	public class CameraAnimator : MonoBehaviour
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x00031218 File Offset: 0x0002F418
		private void Update()
		{
			base.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * 10f));
		}
	}
}
