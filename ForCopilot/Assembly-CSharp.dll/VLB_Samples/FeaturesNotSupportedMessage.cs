using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x02000166 RID: 358
	public class FeaturesNotSupportedMessage : MonoBehaviour
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x0001EEF5 File Offset: 0x0001D0F5
		private void Start()
		{
			if (!Noise3D.isSupported)
			{
				Debug.LogWarning(Noise3D.isNotSupportedString);
			}
		}
	}
}
