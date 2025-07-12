using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x02000142 RID: 322
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamSD))]
	public abstract class DynamicOcclusionAbstractBase : MonoBehaviour
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B0B6 File Offset: 0x000192B6
		public void ProcessOcclusionManually()
		{
			this.ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource.User);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060005AA RID: 1450 RVA: 0x0001B0C0 File Offset: 0x000192C0
		// (remove) Token: 0x060005AB RID: 1451 RVA: 0x0001B0F8 File Offset: 0x000192F8
		public event Action onOcclusionProcessed;

		// Token: 0x060005AC RID: 1452 RVA: 0x0001B130 File Offset: 0x00019330
		protected void ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			if (!Config.Instance.featureEnabledDynamicOcclusion)
			{
				return;
			}
			if (this.m_LastFrameRendered == Time.frameCount && Application.isPlaying && source == DynamicOcclusionAbstractBase.ProcessOcclusionSource.OnEnable)
			{
				return;
			}
			bool flag = this.OnProcessOcclusion(source);
			if (this.onOcclusionProcessed != null)
			{
				this.onOcclusionProcessed();
			}
			if (this.m_Master)
			{
				this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), flag ? this.m_MaterialModifierCallbackCached : null);
			}
			if (this.updateRate.HasFlag(DynamicOcclusionUpdateRate.OnBeamMove))
			{
				this.m_TransformPacked = base.transform.GetWorldPacked();
			}
			bool flag2 = this.m_LastFrameRendered < 0;
			this.m_LastFrameRendered = Time.frameCount;
			if (flag2 && DynamicOcclusionAbstractBase._INTERNAL_ApplyRandomFrameOffset)
			{
				this.m_LastFrameRendered += UnityEngine.Random.Range(0, this.waitXFrames);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001B207 File Offset: 0x00019407
		public int _INTERNAL_LastFrameRendered
		{
			get
			{
				return this.m_LastFrameRendered;
			}
		}

		// Token: 0x060005AE RID: 1454
		protected abstract string GetShaderKeyword();

		// Token: 0x060005AF RID: 1455
		protected abstract MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode();

		// Token: 0x060005B0 RID: 1456
		protected abstract bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source);

		// Token: 0x060005B1 RID: 1457
		protected abstract void OnModifyMaterialCallback(MaterialModifier.Interface owner);

		// Token: 0x060005B2 RID: 1458
		protected abstract void OnEnablePostValidate();

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001B20F File Offset: 0x0001940F
		protected virtual void OnValidateProperties()
		{
			this.waitXFrames = Mathf.Clamp(this.waitXFrames, 1, 60);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001B225 File Offset: 0x00019425
		protected virtual void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamSD>();
			this.m_Master._INTERNAL_DynamicOcclusionMode = this.GetDynamicOcclusionMode();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001B244 File Offset: 0x00019444
		protected virtual void OnDestroy()
		{
			this.m_Master._INTERNAL_DynamicOcclusionMode = MaterialManager.SD.DynamicOcclusion.Off;
			this.DisableOcclusion();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001B258 File Offset: 0x00019458
		protected virtual void OnEnable()
		{
			this.m_MaterialModifierCallbackCached = new MaterialModifier.Callback(this.OnModifyMaterialCallback);
			this.OnValidateProperties();
			this.OnEnablePostValidate();
			this.m_Master.onWillCameraRenderThisBeam += this.OnWillCameraRender;
			if (!this.updateRate.HasFlag(DynamicOcclusionUpdateRate.Never))
			{
				this.m_Master.RegisterOnBeamGeometryInitializedCallback(delegate
				{
					this.ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource.OnEnable);
				});
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001B2CA File Offset: 0x000194CA
		protected virtual void OnDisable()
		{
			this.m_Master.onWillCameraRenderThisBeam -= this.OnWillCameraRender;
			this.DisableOcclusion();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001B2EC File Offset: 0x000194EC
		private void OnWillCameraRender(Camera cam)
		{
			if (cam != null && cam.enabled && Time.frameCount != this.m_LastFrameRendered)
			{
				bool flag = false;
				if (!flag && this.updateRate.HasFlag(DynamicOcclusionUpdateRate.OnBeamMove) && !this.m_TransformPacked.IsSame(base.transform))
				{
					flag = true;
				}
				if (!flag && this.updateRate.HasFlag(DynamicOcclusionUpdateRate.EveryXFrames) && Time.frameCount >= this.m_LastFrameRendered + this.waitXFrames)
				{
					flag = true;
				}
				if (flag)
				{
					this.ProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource.RenderLoop);
				}
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001B387 File Offset: 0x00019587
		private void DisableOcclusion()
		{
			this.m_Master._INTERNAL_SetDynamicOcclusionCallback(this.GetShaderKeyword(), null);
		}

		// Token: 0x040006AF RID: 1711
		public const string ClassName = "DynamicOcclusionAbstractBase";

		// Token: 0x040006B0 RID: 1712
		public DynamicOcclusionUpdateRate updateRate = DynamicOcclusionUpdateRate.EveryXFrames;

		// Token: 0x040006B1 RID: 1713
		[FormerlySerializedAs("waitFrameCount")]
		public int waitXFrames = 3;

		// Token: 0x040006B3 RID: 1715
		public static bool _INTERNAL_ApplyRandomFrameOffset = true;

		// Token: 0x040006B4 RID: 1716
		private TransformUtils.Packed m_TransformPacked;

		// Token: 0x040006B5 RID: 1717
		private int m_LastFrameRendered = int.MinValue;

		// Token: 0x040006B6 RID: 1718
		protected VolumetricLightBeamSD m_Master;

		// Token: 0x040006B7 RID: 1719
		protected MaterialModifier.Callback m_MaterialModifierCallbackCached;

		// Token: 0x02000143 RID: 323
		protected enum ProcessOcclusionSource
		{
			// Token: 0x040006B9 RID: 1721
			RenderLoop,
			// Token: 0x040006BA RID: 1722
			OnEnable,
			// Token: 0x040006BB RID: 1723
			EditorUpdate,
			// Token: 0x040006BC RID: 1724
			User
		}
	}
}
