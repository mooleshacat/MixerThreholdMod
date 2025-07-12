using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001F1 RID: 497
	public class WeatherEnclosureDetector : MonoBehaviour
	{
		// Token: 0x06000B14 RID: 2836 RVA: 0x00030C35 File Offset: 0x0002EE35
		private void Start()
		{
			this.ApplyEnclosure();
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00030C35 File Offset: 0x0002EE35
		private void OnEnable()
		{
			this.ApplyEnclosure();
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00030C40 File Offset: 0x0002EE40
		private void OnTriggerEnter(Collider other)
		{
			WeatherEnclosure componentInChildren = other.gameObject.GetComponentInChildren<WeatherEnclosure>();
			if (!componentInChildren)
			{
				return;
			}
			if (this.triggeredEnclosures.Contains(componentInChildren))
			{
				this.triggeredEnclosures.Remove(componentInChildren);
			}
			this.triggeredEnclosures.Add(componentInChildren);
			this.ApplyEnclosure();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00030C90 File Offset: 0x0002EE90
		private void OnTriggerExit(Collider other)
		{
			WeatherEnclosure componentInChildren = other.gameObject.GetComponentInChildren<WeatherEnclosure>();
			if (!componentInChildren)
			{
				return;
			}
			if (!this.triggeredEnclosures.Contains(componentInChildren))
			{
				return;
			}
			this.triggeredEnclosures.Remove(componentInChildren);
			this.ApplyEnclosure();
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00030CD4 File Offset: 0x0002EED4
		public void ApplyEnclosure()
		{
			WeatherEnclosure weatherEnclosure;
			if (this.triggeredEnclosures.Count > 0)
			{
				weatherEnclosure = this.triggeredEnclosures[this.triggeredEnclosures.Count - 1];
				if (!weatherEnclosure)
				{
					Debug.LogError("Failed to find mesh renderer on weather enclosure, using main enclosure instead.");
					weatherEnclosure = this.mainEnclosure;
				}
			}
			else
			{
				weatherEnclosure = this.mainEnclosure;
			}
			if (this.enclosureChangedCallback != null)
			{
				this.enclosureChangedCallback(weatherEnclosure);
			}
		}

		// Token: 0x04000BC9 RID: 3017
		[Tooltip("Default enclosure used when not inside the trigger of another enclosure area.")]
		public WeatherEnclosure mainEnclosure;

		// Token: 0x04000BCA RID: 3018
		private List<WeatherEnclosure> triggeredEnclosures = new List<WeatherEnclosure>();

		// Token: 0x04000BCB RID: 3019
		public RainDownfallController rainController;

		// Token: 0x04000BCC RID: 3020
		public Action<WeatherEnclosure> enclosureChangedCallback;
	}
}
