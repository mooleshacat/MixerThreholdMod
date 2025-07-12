using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000119 RID: 281
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Light), typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-trackrealtimechanges-hd/")]
	public class TrackRealtimeChangesOnLightHD : MonoBehaviour
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x000179A5 File Offset: 0x00015BA5
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000179B3 File Offset: 0x00015BB3
		private void Update()
		{
			if (this.m_Master.enabled)
			{
				this.m_Master.AssignPropertiesFromAttachedSpotLight();
			}
		}

		// Token: 0x0400060C RID: 1548
		public const string ClassName = "TrackRealtimeChangesOnLightHD";

		// Token: 0x0400060D RID: 1549
		private VolumetricLightBeamHD m_Master;
	}
}
