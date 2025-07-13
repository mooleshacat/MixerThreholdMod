using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using VLB;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005E6 RID: 1510
	[ExecuteInEditMode]
	[RequireComponent(typeof(Light))]
	[RequireComponent(typeof(VolumetricLightBeamSD))]
	public class VolumetricLightTracker : MonoBehaviour
	{
		// Token: 0x06002502 RID: 9474 RVA: 0x000969B8 File Offset: 0x00094BB8
		private void OnValidate()
		{
			if (this.light == null)
			{
				this.light = base.GetComponent<Light>();
			}
			if (this.optimizedLight == null)
			{
				this.optimizedLight = base.GetComponent<OptimizedLight>();
			}
			if (this.beam == null)
			{
				this.beam = base.GetComponent<VolumetricLightBeamSD>();
			}
			if (this.dust == null)
			{
				this.dust = base.GetComponent<VolumetricDustParticles>();
			}
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00096A30 File Offset: 0x00094C30
		private void LateUpdate()
		{
			if (this.Override)
			{
				this.beam.enabled = this.Enabled;
			}
			else if (this.optimizedLight != null)
			{
				this.beam.enabled = this.optimizedLight.Enabled;
			}
			else if (this.light != null)
			{
				this.beam.enabled = this.light.enabled;
			}
			if (this.dust != null)
			{
				this.dust.enabled = this.beam.enabled;
			}
		}

		// Token: 0x04001B64 RID: 7012
		public bool Override;

		// Token: 0x04001B65 RID: 7013
		public bool Enabled;

		// Token: 0x04001B66 RID: 7014
		public Light light;

		// Token: 0x04001B67 RID: 7015
		public OptimizedLight optimizedLight;

		// Token: 0x04001B68 RID: 7016
		public VolumetricLightBeamSD beam;

		// Token: 0x04001B69 RID: 7017
		public VolumetricDustParticles dust;
	}
}
