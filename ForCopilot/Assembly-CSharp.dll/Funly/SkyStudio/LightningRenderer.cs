using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E8 RID: 488
	[RequireComponent(typeof(AudioSource))]
	public class LightningRenderer : BaseSpriteInstancedRenderer
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x0002F789 File Offset: 0x0002D989
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError("Can't render lightning since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.m_AudioSource = base.GetComponent<AudioSource>();
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002F7B0 File Offset: 0x0002D9B0
		protected override Bounds CalculateMeshBounds()
		{
			return new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002F7D0 File Offset: 0x0002D9D0
		protected override BaseSpriteItemData CreateSpriteItemData()
		{
			return new BaseSpriteItemData();
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002F7D7 File Offset: 0x0002D9D7
		protected override bool IsRenderingEnabled()
		{
			return !(this.m_SkyProfile == null) && this.m_SkyProfile.IsFeatureEnabled("LightningFeature", true) && LightningRenderer.m_SpawnAreas.Count != 0;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002F80C File Offset: 0x0002DA0C
		protected override void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale)
		{
			LightningSpawnArea randomLightningSpawnArea = this.GetRandomLightningSpawnArea();
			float num = this.CalculateLightningBoltScaleForArea(randomLightningSpawnArea);
			spriteScale = new Vector3(num, num, num);
			spritePosition = this.GetRandomWorldPositionInsideSpawnArea(randomLightningSpawnArea);
			if (Camera.main == null)
			{
				Debug.LogError("Can't billboard lightning to viewer since there is no main camera tagged.");
				spriteRotation = randomLightningSpawnArea.transform.rotation;
				return;
			}
			spriteRotation = Quaternion.LookRotation(spritePosition - Camera.main.transform.position, Vector3.up);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002F897 File Offset: 0x0002DA97
		protected override void ConfigureSpriteItemData(BaseSpriteItemData data)
		{
			if (this.m_SkyProfile.IsFeatureEnabled("ThunderFeature", true))
			{
				base.Invoke("PlayThunderBoltSound", this.m_ThunderSoundDelay);
			}
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000045B1 File Offset: 0x000027B1
		protected override void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data)
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0002F8BD File Offset: 0x0002DABD
		protected override void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock)
		{
			propertyBlock.SetFloat("_Intensity", this.m_LightningIntensity);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0002F8D4 File Offset: 0x0002DAD4
		protected override int GetNextSpawnCount()
		{
			if (this.m_NextSpawnTime > Time.time)
			{
				return 0;
			}
			this.m_NextSpawnTime = Time.time + 0.5f;
			if (UnityEngine.Random.value < this.m_LightningProbability)
			{
				this.m_NextSpawnTime += this.m_SpawnCoolDown;
				return 1;
			}
			return 0;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002F924 File Offset: 0x0002DB24
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay, LightningArtItem artItem)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			this.m_Style = artItem;
			if (this.m_SkyProfile == null)
			{
				Debug.LogError("Assigned null sky profile!");
				return;
			}
			if (this.m_Style == null)
			{
				Debug.LogError("Can't render lightning without an art item");
				return;
			}
			this.SyncDataFromSkyProfile();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002F980 File Offset: 0x0002DB80
		private void SyncDataFromSkyProfile()
		{
			this.m_LightningProbability = this.m_SkyProfile.GetNumberPropertyValue("LightningProbabilityKey", this.m_TimeOfDay);
			this.m_LightningIntensity = this.m_SkyProfile.GetNumberPropertyValue("LightningIntensityKey", this.m_TimeOfDay);
			this.m_SpawnCoolDown = this.m_SkyProfile.GetNumberPropertyValue("LightningStrikeCoolDown", this.m_TimeOfDay);
			this.m_ThunderSoundDelay = this.m_SkyProfile.GetNumberPropertyValue("ThunderSoundDelayKey", this.m_TimeOfDay);
			this.m_LightningProbability *= this.m_Style.strikeProbability;
			this.m_LightningIntensity *= this.m_Style.intensity;
			this.m_SpriteSheetLayout.columns = this.m_Style.columns;
			this.m_SpriteSheetLayout.rows = this.m_Style.rows;
			this.m_SpriteSheetLayout.frameCount = this.m_Style.totalFrames;
			this.m_SpriteSheetLayout.frameRate = this.m_Style.animateSpeed;
			this.m_TintColor = this.m_Style.tintColor * this.m_SkyProfile.GetColorPropertyValue("LightningTintColorKey", this.m_TimeOfDay);
			this.renderMaterial = this.m_Style.material;
			this.modelMesh = this.m_Style.mesh;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002FAD4 File Offset: 0x0002DCD4
		private LightningSpawnArea GetRandomLightningSpawnArea()
		{
			if (LightningRenderer.m_SpawnAreas.Count == 0)
			{
				return null;
			}
			int index = Mathf.RoundToInt((float)UnityEngine.Random.Range(0, LightningRenderer.m_SpawnAreas.Count)) % LightningRenderer.m_SpawnAreas.Count;
			return LightningRenderer.m_SpawnAreas[index];
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0002FB1C File Offset: 0x0002DD1C
		private void PlayThunderBoltSound()
		{
			if (this.m_Style.thunderSound != null)
			{
				this.m_AudioSource.volume = this.m_SkyProfile.GetNumberPropertyValue("ThunderSoundVolumeKey", this.m_TimeOfDay);
				this.m_AudioSource.PlayOneShot(this.m_Style.thunderSound);
			}
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002FB73 File Offset: 0x0002DD73
		public static void AddSpawnArea(LightningSpawnArea area)
		{
			if (!LightningRenderer.m_SpawnAreas.Contains(area))
			{
				LightningRenderer.m_SpawnAreas.Add(area);
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002FB8D File Offset: 0x0002DD8D
		public static void RemoveSpawnArea(LightningSpawnArea area)
		{
			if (LightningRenderer.m_SpawnAreas.Contains(area))
			{
				LightningRenderer.m_SpawnAreas.Remove(area);
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002FBA8 File Offset: 0x0002DDA8
		private Vector3 GetRandomWorldPositionInsideSpawnArea(LightningSpawnArea area)
		{
			float x = UnityEngine.Random.Range(-area.lightningArea.x, area.lightningArea.x) / 2f;
			float z = UnityEngine.Random.Range(-area.lightningArea.z, area.lightningArea.z) / 2f;
			float y = 0f;
			if (this.m_Style.alignment == LightningArtItem.Alignment.TopAlign)
			{
				y = area.lightningArea.y / 2f - this.m_Style.size / 2f;
			}
			return area.transform.TransformPoint(new Vector3(x, y, z));
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002FC46 File Offset: 0x0002DE46
		private float CalculateLightningBoltScaleForArea(LightningSpawnArea area)
		{
			if (this.m_Style.alignment == LightningArtItem.Alignment.ScaleToFit)
			{
				return area.lightningArea.y / 2f;
			}
			return this.m_Style.size;
		}

		// Token: 0x04000B95 RID: 2965
		private static List<LightningSpawnArea> m_SpawnAreas = new List<LightningSpawnArea>();

		// Token: 0x04000B96 RID: 2966
		private float m_LightningProbability;

		// Token: 0x04000B97 RID: 2967
		private float m_NextSpawnTime;

		// Token: 0x04000B98 RID: 2968
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B99 RID: 2969
		private LightningArtItem m_Style;

		// Token: 0x04000B9A RID: 2970
		private float m_TimeOfDay;

		// Token: 0x04000B9B RID: 2971
		private AudioSource m_AudioSource;

		// Token: 0x04000B9C RID: 2972
		private float m_LightningIntensity;

		// Token: 0x04000B9D RID: 2973
		private float m_ThunderSoundDelay;

		// Token: 0x04000B9E RID: 2974
		private float m_SpawnCoolDown;

		// Token: 0x04000B9F RID: 2975
		private const float k_ProbabiltyCheckInterval = 0.5f;
	}
}
