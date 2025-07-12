using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000162 RID: 354
	public abstract class VolumetricLightBeamAbstractBase : MonoBehaviour
	{
		// Token: 0x060006DB RID: 1755
		public abstract BeamGeometryAbstractBase GetBeamGeometry();

		// Token: 0x060006DC RID: 1756
		protected abstract void SetBeamGeometryNull();

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001ED10 File Offset: 0x0001CF10
		public bool hasGeometry
		{
			get
			{
				return this.GetBeamGeometry() != null;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0001ED1E File Offset: 0x0001CF1E
		public Bounds bounds
		{
			get
			{
				if (!(this.GetBeamGeometry() != null))
				{
					return new Bounds(Vector3.zero, Vector3.zero);
				}
				return this.GetBeamGeometry().meshRenderer.bounds;
			}
		}

		// Token: 0x060006DF RID: 1759
		public abstract bool IsScalable();

		// Token: 0x060006E0 RID: 1760
		public abstract Vector3 GetLossyScale();

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001ED4E File Offset: 0x0001CF4E
		public int _INTERNAL_pluginVersion
		{
			get
			{
				return this.pluginVersion;
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001ED58 File Offset: 0x0001CF58
		public Light GetLightSpotAttachedSlow(out VolumetricLightBeamAbstractBase.AttachedLightType lightType)
		{
			Light component = base.GetComponent<Light>();
			if (!component)
			{
				lightType = VolumetricLightBeamAbstractBase.AttachedLightType.NoLight;
				return null;
			}
			if (component.type == LightType.Spot)
			{
				lightType = VolumetricLightBeamAbstractBase.AttachedLightType.SpotLight;
				return component;
			}
			lightType = VolumetricLightBeamAbstractBase.AttachedLightType.OtherLight;
			return null;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0001ED8A File Offset: 0x0001CF8A
		public Light lightSpotAttached
		{
			get
			{
				return this.m_CachedLightSpot;
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001ED94 File Offset: 0x0001CF94
		protected void InitLightSpotAttachedCached()
		{
			VolumetricLightBeamAbstractBase.AttachedLightType attachedLightType;
			this.m_CachedLightSpot = this.GetLightSpotAttachedSlow(out attachedLightType);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001EDAF File Offset: 0x0001CFAF
		private void OnDestroy()
		{
			this.DestroyBeam();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001EDB7 File Offset: 0x0001CFB7
		protected void DestroyBeam()
		{
			if (Application.isPlaying)
			{
				BeamGeometryAbstractBase.DestroyBeamGeometryGameObject(this.GetBeamGeometry());
			}
			this.SetBeamGeometryNull();
		}

		// Token: 0x04000792 RID: 1938
		public const string ClassName = "VolumetricLightBeamAbstractBase";

		// Token: 0x04000793 RID: 1939
		[SerializeField]
		protected int pluginVersion = -1;

		// Token: 0x04000794 RID: 1940
		protected Light m_CachedLightSpot;

		// Token: 0x02000163 RID: 355
		public enum AttachedLightType
		{
			// Token: 0x04000796 RID: 1942
			NoLight,
			// Token: 0x04000797 RID: 1943
			OtherLight,
			// Token: 0x04000798 RID: 1944
			SpotLight
		}
	}
}
