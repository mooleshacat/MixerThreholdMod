using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001F2 RID: 498
	public class TransitionBetweenProfiles : MonoBehaviour
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x00030D51 File Offset: 0x0002EF51
		private void Start()
		{
			this.m_CurrentSkyProfile = this.daySkyProfile;
			if (this.timeOfDayController == null)
			{
				this.timeOfDayController = TimeOfDayController.instance;
			}
			this.timeOfDayController.skyProfile = this.m_CurrentSkyProfile;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00030D8C File Offset: 0x0002EF8C
		public void ToggleSkyProfiles()
		{
			this.m_CurrentSkyProfile = ((this.m_CurrentSkyProfile == this.daySkyProfile) ? this.nightSkyProfile : this.daySkyProfile);
			this.timeOfDayController.StartSkyProfileTransition(this.m_CurrentSkyProfile, this.transitionDuration);
		}

		// Token: 0x04000BCD RID: 3021
		public SkyProfile daySkyProfile;

		// Token: 0x04000BCE RID: 3022
		public SkyProfile nightSkyProfile;

		// Token: 0x04000BCF RID: 3023
		[Tooltip("How long the transition animation will last.")]
		[Range(0.1f, 30f)]
		public float transitionDuration = 2f;

		// Token: 0x04000BD0 RID: 3024
		public TimeOfDayController timeOfDayController;

		// Token: 0x04000BD1 RID: 3025
		private SkyProfile m_CurrentSkyProfile;
	}
}
