using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E7 RID: 487
	public class LightningController : MonoBehaviour, ISkyModule
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002F5AE File Offset: 0x0002D7AE
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogWarning("Can't render lightning since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.ClearLightningRenderers();
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0002F5CF File Offset: 0x0002D7CF
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0002F5E0 File Offset: 0x0002D7E0
		public void Update()
		{
			if (this.m_SkyProfile == null || !this.m_SkyProfile.IsFeatureEnabled("LightningFeature", true))
			{
				this.ClearLightningRenderers();
				return;
			}
			if (this.m_SkyProfile.lightningArtSet == null || this.m_SkyProfile.lightningArtSet.lightingStyleItems == null || this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count == 0)
			{
				return;
			}
			if (this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count != this.m_LightningRenderers.Count)
			{
				this.ClearLightningRenderers();
				this.CreateLightningRenderers();
			}
			for (int i = 0; i < this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count; i++)
			{
				LightningArtItem artItem = this.m_SkyProfile.lightningArtSet.lightingStyleItems[i];
				this.m_LightningRenderers[i].UpdateForTimeOfDay(this.m_SkyProfile, this.m_TimeOfDay, artItem);
			}
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002F6D8 File Offset: 0x0002D8D8
		public void ClearLightningRenderers()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
			}
			this.m_LightningRenderers.Clear();
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002F71C File Offset: 0x0002D91C
		public void CreateLightningRenderers()
		{
			for (int i = 0; i < this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count; i++)
			{
				LightningRenderer lightningRenderer = new GameObject("Lightning Renderer").AddComponent<LightningRenderer>();
				lightningRenderer.transform.parent = base.transform;
				this.m_LightningRenderers.Add(lightningRenderer);
			}
		}

		// Token: 0x04000B92 RID: 2962
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B93 RID: 2963
		private float m_TimeOfDay;

		// Token: 0x04000B94 RID: 2964
		private List<LightningRenderer> m_LightningRenderers = new List<LightningRenderer>();
	}
}
