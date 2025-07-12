using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200011A RID: 282
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-cookie-hd/")]
	public class VolumetricCookieHD : MonoBehaviour
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000179CD File Offset: 0x00015BCD
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x000179D5 File Offset: 0x00015BD5
		public float contribution
		{
			get
			{
				return this.m_Contribution;
			}
			set
			{
				if (this.m_Contribution != value)
				{
					this.m_Contribution = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000179ED File Offset: 0x00015BED
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x000179F5 File Offset: 0x00015BF5
		public Texture cookieTexture
		{
			get
			{
				return this.m_CookieTexture;
			}
			set
			{
				if (this.m_CookieTexture != value)
				{
					this.m_CookieTexture = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00017A12 File Offset: 0x00015C12
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x00017A1A File Offset: 0x00015C1A
		public CookieChannel channel
		{
			get
			{
				return this.m_Channel;
			}
			set
			{
				if (this.m_Channel != value)
				{
					this.m_Channel = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00017A32 File Offset: 0x00015C32
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x00017A3A File Offset: 0x00015C3A
		public bool negative
		{
			get
			{
				return this.m_Negative;
			}
			set
			{
				if (this.m_Negative != value)
				{
					this.m_Negative = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00017A52 File Offset: 0x00015C52
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00017A5A File Offset: 0x00015C5A
		public Vector2 translation
		{
			get
			{
				return this.m_Translation;
			}
			set
			{
				if (this.m_Translation != value)
				{
					this.m_Translation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00017A77 File Offset: 0x00015C77
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x00017A7F File Offset: 0x00015C7F
		public float rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				if (this.m_Rotation != value)
				{
					this.m_Rotation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00017A97 File Offset: 0x00015C97
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x00017A9F File Offset: 0x00015C9F
		public Vector2 scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				if (this.m_Scale != value)
				{
					this.m_Scale = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00017ABC File Offset: 0x00015CBC
		private void SetDirty()
		{
			if (this.m_Master)
			{
				this.m_Master.SetPropertyDirty(DirtyProps.CookieProps);
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00017ADC File Offset: 0x00015CDC
		public static void ApplyMaterialProperties(VolumetricCookieHD instance, BeamGeometryHD geom)
		{
			if (instance && instance.enabled && instance.cookieTexture != null)
			{
				geom.SetMaterialProp(ShaderProperties.HD.CookieTexture, instance.cookieTexture);
				geom.SetMaterialProp(ShaderProperties.HD.CookieProperties, new Vector4(instance.negative ? instance.contribution : (-instance.contribution), (float)instance.channel, Mathf.Cos(instance.rotation * 0.017453292f), Mathf.Sin(instance.rotation * 0.017453292f)));
				geom.SetMaterialProp(ShaderProperties.HD.CookiePosAndScale, new Vector4(instance.translation.x, instance.translation.y, instance.scale.x, instance.scale.y));
				return;
			}
			geom.SetMaterialProp(ShaderProperties.HD.CookieTexture, BeamGeometryHD.InvalidTexture.Null);
			geom.SetMaterialProp(ShaderProperties.HD.CookieProperties, Vector4.zero);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00017BCA File Offset: 0x00015DCA
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00017BD8 File Offset: 0x00015DD8
		private void OnEnable()
		{
			this.SetDirty();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00017BD8 File Offset: 0x00015DD8
		private void OnDisable()
		{
			this.SetDirty();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00017BD8 File Offset: 0x00015DD8
		private void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00017BE0 File Offset: 0x00015DE0
		private void Start()
		{
			if (Application.isPlaying)
			{
				this.SetDirty();
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00017BE0 File Offset: 0x00015DE0
		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				this.SetDirty();
			}
		}

		// Token: 0x0400060E RID: 1550
		public const string ClassName = "VolumetricCookieHD";

		// Token: 0x0400060F RID: 1551
		[SerializeField]
		private float m_Contribution = 1f;

		// Token: 0x04000610 RID: 1552
		[SerializeField]
		private Texture m_CookieTexture;

		// Token: 0x04000611 RID: 1553
		[SerializeField]
		private CookieChannel m_Channel = CookieChannel.Alpha;

		// Token: 0x04000612 RID: 1554
		[SerializeField]
		private bool m_Negative;

		// Token: 0x04000613 RID: 1555
		[SerializeField]
		private Vector2 m_Translation = Consts.Cookie.TranslationDefault;

		// Token: 0x04000614 RID: 1556
		[SerializeField]
		private float m_Rotation;

		// Token: 0x04000615 RID: 1557
		[SerializeField]
		private Vector2 m_Scale = Consts.Cookie.ScaleDefault;

		// Token: 0x04000616 RID: 1558
		private VolumetricLightBeamHD m_Master;
	}
}
