using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000160 RID: 352
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamAbstractBase))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dustparticles/")]
	public class VolumetricDustParticles : MonoBehaviour
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001E43B File Offset: 0x0001C63B
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0001E443 File Offset: 0x0001C643
		public bool isCulled { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001E44C File Offset: 0x0001C64C
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0001E454 File Offset: 0x0001C654
		public float alphaAdditionalRuntime
		{
			get
			{
				return this.m_AlphaAdditionalRuntime;
			}
			set
			{
				if (this.m_AlphaAdditionalRuntime != value)
				{
					this.m_AlphaAdditionalRuntime = value;
					this.m_RuntimePropertiesDirty = true;
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001E46D File Offset: 0x0001C66D
		public bool particlesAreInstantiated
		{
			get
			{
				return this.m_Particles;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0001E47A File Offset: 0x0001C67A
		public int particlesCurrentCount
		{
			get
			{
				if (!this.m_Particles)
				{
					return 0;
				}
				return this.m_Particles.particleCount;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x0001E498 File Offset: 0x0001C698
		public int particlesMaxCount
		{
			get
			{
				if (!this.m_Particles)
				{
					return 0;
				}
				return this.m_Particles.main.maxParticles;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
		public Camera mainCamera
		{
			get
			{
				if (!VolumetricDustParticles.ms_MainCamera)
				{
					VolumetricDustParticles.ms_MainCamera = Camera.main;
					if (!VolumetricDustParticles.ms_MainCamera && !VolumetricDustParticles.ms_NoMainCameraLogged)
					{
						Debug.LogErrorFormat(base.gameObject, "In order to use 'VolumetricDustParticles' culling, you must have a MainCamera defined in your scene.", Array.Empty<object>());
						VolumetricDustParticles.ms_NoMainCameraLogged = true;
					}
				}
				return VolumetricDustParticles.ms_MainCamera;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001E51E File Offset: 0x0001C71E
		private void Start()
		{
			this.isCulled = false;
			this.m_Master = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.HandleBackwardCompatibility(this.m_Master._INTERNAL_pluginVersion, 20100);
			this.InstantiateParticleSystem();
			this.SetActiveAndPlay();
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001E558 File Offset: 0x0001C758
		private void InstantiateParticleSystem()
		{
			base.gameObject.ForeachComponentsInDirectChildrenOnly(delegate(ParticleSystem ps)
			{
				UnityEngine.Object.DestroyImmediate(ps.gameObject);
			}, true);
			this.m_Particles = Config.Instance.NewVolumetricDustParticles();
			if (this.m_Particles)
			{
				this.m_Particles.transform.SetParent(base.transform, false);
				this.m_Renderer = this.m_Particles.GetComponent<ParticleSystemRenderer>();
				this.m_Material = new Material(this.m_Renderer.sharedMaterial);
				this.m_Renderer.material = this.m_Material;
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001E5FC File Offset: 0x0001C7FC
		private void OnEnable()
		{
			this.SetActiveAndPlay();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001E604 File Offset: 0x0001C804
		private void SetActive(bool active)
		{
			if (this.m_Particles)
			{
				this.m_Particles.gameObject.SetActive(active);
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001E624 File Offset: 0x0001C824
		private void SetActiveAndPlay()
		{
			this.SetActive(true);
			this.Play();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001E633 File Offset: 0x0001C833
		private void Play()
		{
			if (this.m_Particles)
			{
				this.SetParticleProperties();
				this.m_Particles.Simulate(0f);
				this.m_Particles.Play(true);
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001E664 File Offset: 0x0001C864
		private void OnDisable()
		{
			this.SetActive(false);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001E670 File Offset: 0x0001C870
		private void OnDestroy()
		{
			if (this.m_Particles)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Particles.gameObject);
				this.m_Particles = null;
			}
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
				this.m_Material = null;
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001E6C0 File Offset: 0x0001C8C0
		private void Update()
		{
			this.UpdateCulling();
			if (UtilsBeamProps.CanChangeDuringPlaytime(this.m_Master))
			{
				this.SetParticleProperties();
			}
			if (this.m_RuntimePropertiesDirty && this.m_Material != null)
			{
				this.m_Material.SetColor(ShaderProperties.ParticlesTintColor, new Color(1f, 1f, 1f, this.alphaAdditionalRuntime));
				this.m_RuntimePropertiesDirty = false;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001E730 File Offset: 0x0001C930
		private void SetParticleProperties()
		{
			if (this.m_Particles && this.m_Particles.gameObject.activeSelf)
			{
				this.m_Particles.transform.localRotation = UtilsBeamProps.GetInternalLocalRotation(this.m_Master);
				this.m_Particles.transform.localScale = (this.m_Master.IsScalable() ? Vector3.one : Vector3.one.Divide(this.m_Master.GetLossyScale()));
				float num = UtilsBeamProps.GetFallOffEnd(this.m_Master) * (this.spawnDistanceRange.maxValue - this.spawnDistanceRange.minValue);
				float num2 = num * this.density;
				int maxParticles = (int)(num2 * 4f);
				ParticleSystem.MainModule main = this.m_Particles.main;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				startLifetime.mode = 3;
				startLifetime.constantMin = 4f;
				startLifetime.constantMax = 6f;
				main.startLifetime = startLifetime;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSize.mode = 3;
				startSize.constantMin = this.size * 0.9f;
				startSize.constantMax = this.size * 1.1f;
				main.startSize = startSize;
				ParticleSystem.MinMaxGradient startColor = main.startColor;
				if (UtilsBeamProps.GetColorMode(this.m_Master) == ColorMode.Flat)
				{
					startColor.mode = 0;
					Color colorFlat = UtilsBeamProps.GetColorFlat(this.m_Master);
					colorFlat.a *= this.alpha;
					startColor.color = colorFlat;
				}
				else
				{
					startColor.mode = 1;
					Gradient colorGradient = UtilsBeamProps.GetColorGradient(this.m_Master);
					GradientColorKey[] colorKeys = colorGradient.colorKeys;
					GradientAlphaKey[] alphaKeys = colorGradient.alphaKeys;
					for (int i = 0; i < alphaKeys.Length; i++)
					{
						GradientAlphaKey[] array = alphaKeys;
						int num3 = i;
						array[num3].alpha = array[num3].alpha * this.alpha;
					}
					this.m_GradientCached.SetKeys(colorKeys, alphaKeys);
					startColor.gradient = this.m_GradientCached;
				}
				main.startColor = startColor;
				ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
				startSpeed.constant = ((this.direction == ParticlesDirection.Random) ? Mathf.Abs(this.velocity.z) : 0f);
				main.startSpeed = startSpeed;
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.m_Particles.velocityOverLifetime;
				velocityOverLifetime.enabled = (this.direction > ParticlesDirection.Random);
				velocityOverLifetime.space = ((this.direction == ParticlesDirection.LocalSpace) ? 0 : 1);
				velocityOverLifetime.xMultiplier = this.velocity.x;
				velocityOverLifetime.yMultiplier = this.velocity.y;
				velocityOverLifetime.zMultiplier = this.velocity.z;
				main.maxParticles = maxParticles;
				float thickness = UtilsBeamProps.GetThickness(this.m_Master);
				float fallOffEnd = UtilsBeamProps.GetFallOffEnd(this.m_Master);
				ParticleSystem.ShapeModule shape = this.m_Particles.shape;
				shape.shapeType = 8;
				float num4 = UtilsBeamProps.GetConeAngle(this.m_Master) * Mathf.Lerp(0.7f, 1f, thickness);
				shape.angle = num4 * 0.5f;
				float a = UtilsBeamProps.GetConeRadiusStart(this.m_Master) * Mathf.Lerp(0.3f, 1f, thickness);
				float b = Utils.ComputeConeRadiusEnd(fallOffEnd, num4);
				shape.radius = Mathf.Lerp(a, b, this.spawnDistanceRange.minValue);
				shape.length = num;
				float z = fallOffEnd * this.spawnDistanceRange.minValue;
				shape.position = new Vector3(0f, 0f, z);
				shape.arc = 360f;
				shape.randomDirectionAmount = ((this.direction == ParticlesDirection.Random) ? 1f : 0f);
				ParticleSystem.EmissionModule emission = this.m_Particles.emission;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				rateOverTime.constant = num2;
				emission.rateOverTime = rateOverTime;
				if (this.m_Renderer)
				{
					this.m_Renderer.sortingLayerID = UtilsBeamProps.GetSortingLayerID(this.m_Master);
					this.m_Renderer.sortingOrder = UtilsBeamProps.GetSortingOrder(this.m_Master);
				}
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001EB20 File Offset: 0x0001CD20
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
			if (serializedVersion == -1)
			{
				return;
			}
			if (serializedVersion == newVersion)
			{
				return;
			}
			if (serializedVersion < 1880)
			{
				if (this.direction == ParticlesDirection.Random)
				{
					this.direction = ParticlesDirection.LocalSpace;
				}
				else
				{
					this.direction = ParticlesDirection.Random;
				}
				this.velocity = new Vector3(0f, 0f, this.speed);
			}
			if (serializedVersion < 1940)
			{
				this.spawnDistanceRange = new MinMaxRangeFloat(this.spawnMinDistance, this.spawnMaxDistance);
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001EB98 File Offset: 0x0001CD98
		private void UpdateCulling()
		{
			if (this.m_Particles)
			{
				bool flag = true;
				bool fadeOutEnabled = UtilsBeamProps.GetFadeOutEnabled(this.m_Master);
				if ((this.cullingEnabled || fadeOutEnabled) && this.m_Master.hasGeometry)
				{
					if (this.mainCamera)
					{
						float num = this.cullingMaxDistance;
						if (fadeOutEnabled)
						{
							num = Mathf.Min(num, UtilsBeamProps.GetFadeOutEnd(this.m_Master));
						}
						float num2 = num * num;
						flag = (this.m_Master.bounds.SqrDistance(this.mainCamera.transform.position) <= num2);
					}
					else
					{
						this.cullingEnabled = false;
					}
				}
				if (this.m_Particles.gameObject.activeSelf != flag)
				{
					this.SetActive(flag);
					this.isCulled = !flag;
				}
				if (flag && !this.m_Particles.isPlaying)
				{
					this.m_Particles.Play();
				}
			}
		}

		// Token: 0x0400077A RID: 1914
		public const string ClassName = "VolumetricDustParticles";

		// Token: 0x0400077B RID: 1915
		[Range(0f, 1f)]
		public float alpha = 0.5f;

		// Token: 0x0400077C RID: 1916
		[Range(0.0001f, 0.1f)]
		public float size = 0.01f;

		// Token: 0x0400077D RID: 1917
		public ParticlesDirection direction;

		// Token: 0x0400077E RID: 1918
		public Vector3 velocity = Consts.DustParticles.VelocityDefault;

		// Token: 0x0400077F RID: 1919
		[Obsolete("Use 'velocity' instead")]
		public float speed = 0.03f;

		// Token: 0x04000780 RID: 1920
		public float density = 5f;

		// Token: 0x04000781 RID: 1921
		[MinMaxRange(0f, 1f)]
		public MinMaxRangeFloat spawnDistanceRange = Consts.DustParticles.SpawnDistanceRangeDefault;

		// Token: 0x04000782 RID: 1922
		[Obsolete("Use 'spawnDistanceRange' instead")]
		public float spawnMinDistance;

		// Token: 0x04000783 RID: 1923
		[Obsolete("Use 'spawnDistanceRange' instead")]
		public float spawnMaxDistance = 0.7f;

		// Token: 0x04000784 RID: 1924
		public bool cullingEnabled;

		// Token: 0x04000785 RID: 1925
		public float cullingMaxDistance = 10f;

		// Token: 0x04000787 RID: 1927
		[SerializeField]
		private float m_AlphaAdditionalRuntime = 1f;

		// Token: 0x04000788 RID: 1928
		private ParticleSystem m_Particles;

		// Token: 0x04000789 RID: 1929
		private ParticleSystemRenderer m_Renderer;

		// Token: 0x0400078A RID: 1930
		private Material m_Material;

		// Token: 0x0400078B RID: 1931
		private Gradient m_GradientCached = new Gradient();

		// Token: 0x0400078C RID: 1932
		private bool m_RuntimePropertiesDirty = true;

		// Token: 0x0400078D RID: 1933
		private static bool ms_NoMainCameraLogged;

		// Token: 0x0400078E RID: 1934
		private static Camera ms_MainCamera;

		// Token: 0x0400078F RID: 1935
		private VolumetricLightBeamAbstractBase m_Master;
	}
}
