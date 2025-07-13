using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001EF RID: 495
	public class WeatherController : MonoBehaviour
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00030990 File Offset: 0x0002EB90
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x00030998 File Offset: 0x0002EB98
		public RainDownfallController rainDownfallController { get; protected set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x000309A1 File Offset: 0x0002EBA1
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x000309A9 File Offset: 0x0002EBA9
		public RainSplashController rainSplashController { get; protected set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x000309B2 File Offset: 0x0002EBB2
		// (set) Token: 0x06000B07 RID: 2823 RVA: 0x000309BA File Offset: 0x0002EBBA
		public LightningController lightningController { get; protected set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x000309C3 File Offset: 0x0002EBC3
		// (set) Token: 0x06000B09 RID: 2825 RVA: 0x000309CB File Offset: 0x0002EBCB
		public WeatherDepthCamera weatherDepthCamera { get; protected set; }

		// Token: 0x06000B0A RID: 2826 RVA: 0x000309D4 File Offset: 0x0002EBD4
		private void Awake()
		{
			this.DiscoverWeatherControllers();
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x000309D4 File Offset: 0x0002EBD4
		private void Start()
		{
			this.DiscoverWeatherControllers();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x000309DC File Offset: 0x0002EBDC
		private void OnEnable()
		{
			this.DiscoverWeatherControllers();
			if (this.detector == null)
			{
				Debug.LogError("Can't register for enclosure callbacks since there's no WeatherEnclosureDetector on any children");
				return;
			}
			WeatherEnclosureDetector weatherEnclosureDetector = this.detector;
			weatherEnclosureDetector.enclosureChangedCallback = (Action<WeatherEnclosure>)Delegate.Combine(weatherEnclosureDetector.enclosureChangedCallback, new Action<WeatherEnclosure>(this.OnEnclosureDidChange));
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00030A2F File Offset: 0x0002EC2F
		private void DiscoverWeatherControllers()
		{
			this.rainDownfallController = base.GetComponentInChildren<RainDownfallController>();
			this.rainSplashController = base.GetComponentInChildren<RainSplashController>();
			this.lightningController = base.GetComponentInChildren<LightningController>();
			this.weatherDepthCamera = base.GetComponentInChildren<WeatherDepthCamera>();
			this.detector = base.GetComponentInChildren<WeatherEnclosureDetector>();
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00030A6D File Offset: 0x0002EC6D
		private void OnDisable()
		{
			if (this.detector == null)
			{
				return;
			}
			WeatherEnclosureDetector weatherEnclosureDetector = this.detector;
			weatherEnclosureDetector.enclosureChangedCallback = (Action<WeatherEnclosure>)Delegate.Remove(weatherEnclosureDetector.enclosureChangedCallback, new Action<WeatherEnclosure>(this.OnEnclosureDidChange));
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00030AA8 File Offset: 0x0002ECA8
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			if (!skyProfile)
			{
				return;
			}
			this.m_Profile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			if (this.weatherDepthCamera != null)
			{
				this.weatherDepthCamera.enabled = skyProfile.IsFeatureEnabled("RainSplashFeature", true);
			}
			if (this.rainDownfallController != null)
			{
				this.rainDownfallController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
			if (this.rainSplashController != null)
			{
				this.rainSplashController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
			if (this.lightningController != null)
			{
				this.lightningController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00030B44 File Offset: 0x0002ED44
		private void LateUpdate()
		{
			if (this.m_Profile == null)
			{
				return;
			}
			if (this.m_EnclosureMeshRenderer && this.rainDownfallController && this.m_Profile.IsFeatureEnabled("RainFeature", true))
			{
				this.m_EnclosureMeshRenderer.enabled = true;
				return;
			}
			this.m_EnclosureMeshRenderer.enabled = false;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00030BA8 File Offset: 0x0002EDA8
		private void OnEnclosureDidChange(WeatherEnclosure enclosure)
		{
			this.m_Enclosure = enclosure;
			if (this.m_Enclosure != null)
			{
				this.m_EnclosureMeshRenderer = this.m_Enclosure.GetComponentInChildren<MeshRenderer>();
			}
			this.rainDownfallController.SetWeatherEnclosure(this.m_Enclosure);
			this.UpdateForTimeOfDay(this.m_Profile, this.m_TimeOfDay);
		}

		// Token: 0x04000BC2 RID: 3010
		private WeatherEnclosure m_Enclosure;

		// Token: 0x04000BC3 RID: 3011
		private MeshRenderer m_EnclosureMeshRenderer;

		// Token: 0x04000BC4 RID: 3012
		private WeatherEnclosureDetector detector;

		// Token: 0x04000BC5 RID: 3013
		private SkyProfile m_Profile;

		// Token: 0x04000BC6 RID: 3014
		private float m_TimeOfDay;
	}
}
