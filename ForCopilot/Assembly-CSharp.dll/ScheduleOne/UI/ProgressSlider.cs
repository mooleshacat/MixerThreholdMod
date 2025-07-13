using System;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A76 RID: 2678
	public class ProgressSlider : Singleton<ProgressSlider>
	{
		// Token: 0x06004806 RID: 18438 RVA: 0x0012ED13 File Offset: 0x0012CF13
		private void LateUpdate()
		{
			if (this.progressSetThisFrame)
			{
				this.Container.SetActive(true);
				this.progressSetThisFrame = false;
				return;
			}
			this.Container.SetActive(false);
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0012ED3D File Offset: 0x0012CF3D
		public void ShowProgress(float progress)
		{
			this.progressSetThisFrame = true;
			this.Slider.value = progress;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0012ED52 File Offset: 0x0012CF52
		public void Configure(string label, Color sliderFillColor)
		{
			this.Label.text = label;
			this.Label.color = sliderFillColor;
			this.SliderFill.color = sliderFillColor;
		}

		// Token: 0x040034D2 RID: 13522
		[Header("References")]
		public GameObject Container;

		// Token: 0x040034D3 RID: 13523
		public TextMeshProUGUI Label;

		// Token: 0x040034D4 RID: 13524
		public Slider Slider;

		// Token: 0x040034D5 RID: 13525
		public Image SliderFill;

		// Token: 0x040034D6 RID: 13526
		private bool progressSetThisFrame;
	}
}
