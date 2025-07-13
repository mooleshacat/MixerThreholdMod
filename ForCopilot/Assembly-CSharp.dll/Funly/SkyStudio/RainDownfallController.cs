using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001EA RID: 490
	[RequireComponent(typeof(AudioSource))]
	public class RainDownfallController : MonoBehaviour, ISkyModule
	{
		// Token: 0x06000AE5 RID: 2789 RVA: 0x0002FD18 File Offset: 0x0002DF18
		public void SetWeatherEnclosure(WeatherEnclosure enclosure)
		{
			if (this.rainMeshRenderer != null)
			{
				this.rainMeshRenderer.enabled = false;
				this.rainMeshRenderer = null;
			}
			if (!enclosure)
			{
				return;
			}
			this.rainMeshRenderer = enclosure.GetComponentInChildren<MeshRenderer>();
			if (!this.rainMeshRenderer)
			{
				Debug.LogError("Can't render rain since there's no MeshRenderer on the WeatherEnclosure");
				return;
			}
			this.m_PropertyBlock = new MaterialPropertyBlock();
			if (!this.rainMaterial)
			{
				return;
			}
			this.rainMeshRenderer.material = this.rainMaterial;
			this.rainMeshRenderer.enabled = true;
			this.UpdateForTimeOfDay(this.m_SkyProfile, this.m_TimeOfDay);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0002FDBB File Offset: 0x0002DFBB
		private void Update()
		{
			if (this.m_SkyProfile == null)
			{
				return;
			}
			this.UpdateForTimeOfDay(this.m_SkyProfile, this.m_TimeOfDay);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002FDE0 File Offset: 0x0002DFE0
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			if (!skyProfile)
			{
				return;
			}
			if (this.m_RainAudioSource == null)
			{
				this.m_RainAudioSource = base.GetComponent<AudioSource>();
			}
			if (skyProfile == null || !this.m_SkyProfile.IsFeatureEnabled("RainFeature", true))
			{
				if (this.m_RainAudioSource != null)
				{
					this.m_RainAudioSource.enabled = false;
				}
				return;
			}
			if (!this.rainMaterial)
			{
				Debug.LogError("Can't render rain without a rain material");
				return;
			}
			if (!this.rainMeshRenderer)
			{
				Debug.LogError("Can't show rain without an enclosure mesh renderer.");
				return;
			}
			if (this.m_PropertyBlock == null)
			{
				this.m_PropertyBlock = new MaterialPropertyBlock();
			}
			this.rainMeshRenderer.enabled = true;
			this.rainMeshRenderer.material = this.rainMaterial;
			this.rainMeshRenderer.GetPropertyBlock(this.m_PropertyBlock);
			float numberPropertyValue = skyProfile.GetNumberPropertyValue("RainNearIntensityKey", timeOfDay);
			float numberPropertyValue2 = skyProfile.GetNumberPropertyValue("RainFarIntensityKey", timeOfDay);
			Texture texturePropertyValue = skyProfile.GetTexturePropertyValue("RainNearTextureKey", timeOfDay);
			Texture texturePropertyValue2 = skyProfile.GetTexturePropertyValue("RainFarTextureKey", timeOfDay);
			float numberPropertyValue3 = skyProfile.GetNumberPropertyValue("RainNearSpeedKey", timeOfDay);
			float numberPropertyValue4 = skyProfile.GetNumberPropertyValue("RainFarSpeedKey", timeOfDay);
			Color colorPropertyValue = this.m_SkyProfile.GetColorPropertyValue("RainTintColorKey", this.m_TimeOfDay);
			float numberPropertyValue5 = this.m_SkyProfile.GetNumberPropertyValue("RainWindTurbulenceKey", this.m_TimeOfDay);
			float numberPropertyValue6 = this.m_SkyProfile.GetNumberPropertyValue("RainWindTurbulenceSpeedKey", this.m_TimeOfDay);
			float numberPropertyValue7 = this.m_SkyProfile.GetNumberPropertyValue("RainNearTextureTiling", this.m_TimeOfDay);
			float numberPropertyValue8 = this.m_SkyProfile.GetNumberPropertyValue("RainFarTextureTiling", this.m_TimeOfDay);
			if (texturePropertyValue != null)
			{
				this.m_PropertyBlock.SetTexture("_NearTex", texturePropertyValue);
				this.m_PropertyBlock.SetVector("_NearTex_ST", new Vector4(numberPropertyValue7, numberPropertyValue7, numberPropertyValue7, 1f));
			}
			this.m_PropertyBlock.SetFloat("_NearDensity", numberPropertyValue);
			this.m_PropertyBlock.SetFloat("_NearRainSpeed", numberPropertyValue3);
			if (texturePropertyValue2 != null)
			{
				this.m_PropertyBlock.SetTexture("_FarTex", texturePropertyValue2);
				this.m_PropertyBlock.SetVector("_FarTex_ST", new Vector4(numberPropertyValue8, numberPropertyValue8, numberPropertyValue8, 1f));
			}
			this.m_PropertyBlock.SetFloat("_FarDensity", numberPropertyValue2);
			this.m_PropertyBlock.SetFloat("_FarRainSpeed", numberPropertyValue4);
			this.m_PropertyBlock.SetColor("_TintColor", colorPropertyValue);
			this.m_PropertyBlock.SetFloat("_Turbulence", numberPropertyValue5);
			this.m_PropertyBlock.SetFloat("_TurbulenceSpeed", numberPropertyValue6);
			this.rainMeshRenderer.SetPropertyBlock(this.m_PropertyBlock);
			if (skyProfile.IsFeatureEnabled("RainSoundFeature", true))
			{
				this.m_RainAudioSource.enabled = true;
				this.m_RainAudioSource.volume = skyProfile.GetNumberPropertyValue("RainSoundVolume", timeOfDay);
				return;
			}
			this.m_RainAudioSource.enabled = false;
		}

		// Token: 0x04000BA1 RID: 2977
		public MeshRenderer rainMeshRenderer;

		// Token: 0x04000BA2 RID: 2978
		public Material rainMaterial;

		// Token: 0x04000BA3 RID: 2979
		private MaterialPropertyBlock m_PropertyBlock;

		// Token: 0x04000BA4 RID: 2980
		private AudioSource m_RainAudioSource;

		// Token: 0x04000BA5 RID: 2981
		private float m_TimeOfDay;

		// Token: 0x04000BA6 RID: 2982
		private SkyProfile m_SkyProfile;
	}
}
