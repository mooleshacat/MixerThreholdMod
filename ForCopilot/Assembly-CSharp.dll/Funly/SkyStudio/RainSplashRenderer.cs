using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001ED RID: 493
	public class RainSplashRenderer : BaseSpriteInstancedRenderer
	{
		// Token: 0x06000AF0 RID: 2800 RVA: 0x000302B8 File Offset: 0x0002E4B8
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError("Can't render rain splashes since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			WeatherDepthCamera weatherDepthCamera = UnityEngine.Object.FindObjectOfType<WeatherDepthCamera>();
			if (weatherDepthCamera == null)
			{
				Debug.LogError("Can't generate splashes without a RainDepthCamera in the scene");
				base.enabled = false;
				return;
			}
			this.m_DepthCamera = weatherDepthCamera.GetComponent<Camera>();
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003030B File Offset: 0x0002E50B
		protected override Bounds CalculateMeshBounds()
		{
			return this.m_Bounds;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00030313 File Offset: 0x0002E513
		protected override BaseSpriteItemData CreateSpriteItemData()
		{
			return new RainSplashData();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003031C File Offset: 0x0002E51C
		protected override bool IsRenderingEnabled()
		{
			if (this.m_SkyProfile == null)
			{
				return false;
			}
			if (!this.m_SkyProfile.IsFeatureEnabled("RainSplashFeature", true))
			{
				return false;
			}
			if (base.m_ViewerCamera == null)
			{
				Debug.LogError("Can't render ground raindrops since no active camera has the MainCamera tag applied.");
				return false;
			}
			return true;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003036C File Offset: 0x0002E56C
		protected override int GetNextSpawnCount()
		{
			int num = base.maxSprites - this.m_Active.Count;
			if (num <= 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00030394 File Offset: 0x0002E594
		protected override void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale)
		{
			float num = UnityEngine.Random.Range(this.m_SplashScale * (1f - this.m_SplashScaleVarience), this.m_SplashScale);
			spritePosition = data.spritePosition;
			spriteRotation = Quaternion.identity;
			spriteScale = new Vector3(num, num, num);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000303E6 File Offset: 0x0002E5E6
		protected override void ConfigureSpriteItemData(BaseSpriteItemData data)
		{
			data.spritePosition = this.CreateWorldSplashPoint();
			data.delay = UnityEngine.Random.Range(0f, 0.5f);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003040C File Offset: 0x0002E60C
		protected override void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data)
		{
			RainSplashData rainSplashData = data as RainSplashData;
			Vector3 vector = this.m_DepthCamera.WorldToScreenPoint(rainSplashData.spritePosition);
			Vector2 depthTextureUV = new Vector2(vector.x / (float)this.m_DepthCamera.pixelWidth, vector.y / (float)this.m_DepthCamera.pixelHeight);
			rainSplashData.depthTextureUV = depthTextureUV;
			this.m_StartSplashYPositions[instanceId] = rainSplashData.spritePosition.y;
			this.m_DepthUs[instanceId] = rainSplashData.depthTextureUV.x;
			this.m_DepthVs[instanceId] = rainSplashData.depthTextureUV.y;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000304A0 File Offset: 0x0002E6A0
		protected override void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock)
		{
			propertyBlock.SetFloat("_Intensity", this.m_SplashItensity);
			propertyBlock.SetFloatArray("_OverheadDepthU", this.m_DepthUs);
			propertyBlock.SetFloatArray("_OverheadDepthV", this.m_DepthVs);
			propertyBlock.SetFloatArray("_SplashStartYPosition", this.m_StartSplashYPositions);
			propertyBlock.SetFloat("_SplashGroundOffset", this.m_SplashSurfaceOffset);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00030507 File Offset: 0x0002E707
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay, RainSplashArtItem style)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			this.m_Style = style;
			if (this.m_SkyProfile == null)
			{
				return;
			}
			this.SyncDataFromSkyProfile();
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00030534 File Offset: 0x0002E734
		private void SyncDataFromSkyProfile()
		{
			base.maxSprites = (int)this.m_SkyProfile.GetNumberPropertyValue("RainSplashMaxConcurrentKey", this.m_TimeOfDay);
			this.m_SplashAreaStart = this.m_SkyProfile.GetNumberPropertyValue("RainSplashAreaStartKey", this.m_TimeOfDay);
			this.m_SplashAreaLength = this.m_SkyProfile.GetNumberPropertyValue("RainSplashAreaLengthKey", this.m_TimeOfDay);
			this.m_SplashScale = this.m_SkyProfile.GetNumberPropertyValue("RainSplashScaleKey", this.m_TimeOfDay);
			this.m_SplashScaleVarience = this.m_SkyProfile.GetNumberPropertyValue("RainSplashScaleVarienceKey", this.m_TimeOfDay);
			this.m_SplashItensity = this.m_SkyProfile.GetNumberPropertyValue("RainSplashIntensityKey", this.m_TimeOfDay);
			this.m_SplashSurfaceOffset = this.m_SkyProfile.GetNumberPropertyValue("RainSplashSurfaceOffsetKey", this.m_TimeOfDay);
			this.m_SplashScale *= this.m_Style.scaleMultiplier;
			this.m_SplashItensity *= this.m_Style.intensityMultiplier;
			this.m_SpriteSheetLayout.columns = this.m_Style.columns;
			this.m_SpriteSheetLayout.rows = this.m_Style.rows;
			this.m_SpriteSheetLayout.frameCount = this.m_Style.totalFrames;
			this.m_SpriteSheetLayout.frameRate = this.m_Style.animateSpeed;
			this.m_TintColor = this.m_Style.tintColor * this.m_SkyProfile.GetColorPropertyValue("RainSplashTintColorKey", this.m_TimeOfDay);
			this.modelMesh = this.m_Style.mesh;
			this.renderMaterial = this.m_Style.material;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000306DC File Offset: 0x0002E8DC
		private Vector3 CreateWorldSplashPoint()
		{
			float y = UnityEngine.Random.Range(0f, -170f);
			Vector3 vector = Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.right;
			float d = UnityEngine.Random.Range(this.m_SplashAreaStart, this.m_SplashAreaStart + this.m_SplashAreaLength);
			Vector3 position = vector.normalized * d;
			return base.m_ViewerCamera.transform.TransformPoint(position);
		}

		// Token: 0x04000BAB RID: 2987
		private Camera m_DepthCamera;

		// Token: 0x04000BAC RID: 2988
		private float[] m_StartSplashYPositions = new float[1000];

		// Token: 0x04000BAD RID: 2989
		private float[] m_DepthUs = new float[1000];

		// Token: 0x04000BAE RID: 2990
		private float[] m_DepthVs = new float[1000];

		// Token: 0x04000BAF RID: 2991
		private float m_SplashAreaStart;

		// Token: 0x04000BB0 RID: 2992
		private float m_SplashAreaLength;

		// Token: 0x04000BB1 RID: 2993
		private float m_SplashScale;

		// Token: 0x04000BB2 RID: 2994
		private float m_SplashScaleVarience;

		// Token: 0x04000BB3 RID: 2995
		private float m_SplashItensity;

		// Token: 0x04000BB4 RID: 2996
		private float m_SplashSurfaceOffset;

		// Token: 0x04000BB5 RID: 2997
		private SkyProfile m_SkyProfile;

		// Token: 0x04000BB6 RID: 2998
		private float m_TimeOfDay;

		// Token: 0x04000BB7 RID: 2999
		private RainSplashArtItem m_Style;

		// Token: 0x04000BB8 RID: 3000
		private Bounds m_Bounds = new Bounds(Vector3.zero, new Vector3(100f, 100f, 100f));
	}
}
