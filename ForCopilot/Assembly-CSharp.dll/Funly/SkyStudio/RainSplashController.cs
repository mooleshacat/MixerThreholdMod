using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001EB RID: 491
	public class RainSplashController : MonoBehaviour, ISkyModule
	{
		// Token: 0x06000AE9 RID: 2793 RVA: 0x000300CD File Offset: 0x0002E2CD
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogWarning("Can't render rain splashes since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.ClearSplashRenderers();
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000300EE File Offset: 0x0002E2EE
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00030100 File Offset: 0x0002E300
		private void Update()
		{
			if (this.m_SkyProfile == null || !this.m_SkyProfile.IsFeatureEnabled("RainSplashFeature", true))
			{
				this.ClearSplashRenderers();
				return;
			}
			if (this.m_SkyProfile.rainSplashArtSet == null || this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems == null || this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count == 0)
			{
				this.ClearSplashRenderers();
				return;
			}
			if (this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count != this.m_SplashRenderers.Count)
			{
				this.ClearSplashRenderers();
				this.CreateSplashRenderers();
			}
			for (int i = 0; i < this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count; i++)
			{
				RainSplashArtItem style = this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems[i];
				this.m_SplashRenderers[i].UpdateForTimeOfDay(this.m_SkyProfile, this.m_TimeOfDay, style);
			}
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x000301FC File Offset: 0x0002E3FC
		public void ClearSplashRenderers()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
			}
			this.m_SplashRenderers.Clear();
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00030240 File Offset: 0x0002E440
		public void CreateSplashRenderers()
		{
			for (int i = 0; i < this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count; i++)
			{
				RainSplashRenderer rainSplashRenderer = new GameObject("Rain Splash Renderer").AddComponent<RainSplashRenderer>();
				rainSplashRenderer.transform.parent = base.transform;
				this.m_SplashRenderers.Add(rainSplashRenderer);
			}
		}

		// Token: 0x04000BA7 RID: 2983
		private SkyProfile m_SkyProfile;

		// Token: 0x04000BA8 RID: 2984
		private float m_TimeOfDay;

		// Token: 0x04000BA9 RID: 2985
		private List<RainSplashRenderer> m_SplashRenderers = new List<RainSplashRenderer>();
	}
}
