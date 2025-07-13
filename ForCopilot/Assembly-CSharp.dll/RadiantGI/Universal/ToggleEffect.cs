using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RadiantGI.Universal
{
	// Token: 0x02000175 RID: 373
	public class ToggleEffect : MonoBehaviour
	{
		// Token: 0x0600072C RID: 1836 RVA: 0x000204DD File Offset: 0x0001E6DD
		private void Start()
		{
			this.profile.TryGet<RadiantGlobalIllumination>(ref this.radiant);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000204F1 File Offset: 0x0001E6F1
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.radiant.active = !this.radiant.active;
			}
		}

		// Token: 0x040007E5 RID: 2021
		public VolumeProfile profile;

		// Token: 0x040007E6 RID: 2022
		private RadiantGlobalIllumination radiant;
	}
}
