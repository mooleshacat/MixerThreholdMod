using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A6D RID: 2669
	public class PlayerEnergyUI : Singleton<PlayerEnergyUI>
	{
		// Token: 0x060047C8 RID: 18376 RVA: 0x0012DC69 File Offset: 0x0012BE69
		protected override void Awake()
		{
			base.Awake();
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(delegate()
			{
				this.UpdateDisplayedEnergy();
				Player.Local.Energy.onEnergyChanged.AddListener(new UnityAction(this.UpdateDisplayedEnergy));
			}));
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x0012DC91 File Offset: 0x0012BE91
		private void UpdateDisplayedEnergy()
		{
			this.SetDisplayedEnergy(Player.Local.Energy.CurrentEnergy);
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x0012DCA8 File Offset: 0x0012BEA8
		public void SetDisplayedEnergy(float energy)
		{
			this.displayedValue = energy;
			this.Slider.value = energy / 100f;
			this.FillImage.color = ((energy <= 20f) ? this.SliderColor_Red : this.SliderColor_Green);
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x0012DCE4 File Offset: 0x0012BEE4
		protected virtual void Update()
		{
			if (this.displayedValue < 20f)
			{
				float num = Mathf.Clamp((20f - this.displayedValue) / 20f, 0.25f, 1f);
				float num2 = num * 3f;
				this.SliderRect.anchoredPosition = new Vector2(UnityEngine.Random.Range(-num2, num2), UnityEngine.Random.Range(-num2, num2));
				Color white = Color.white;
				Color b = Color.Lerp(Color.white, Color.red, num);
				white.a = this.Label.color.a;
				b.a = this.Label.color.a;
				this.Label.color = Color.Lerp(white, b, (Mathf.Sin(Time.timeSinceLevelLoad * num * 10f) + 1f) / 2f);
				return;
			}
			this.SliderRect.anchoredPosition = Vector2.zero;
			this.Label.color = new Color(1f, 1f, 1f, this.Label.color.a);
		}

		// Token: 0x04003496 RID: 13462
		public Slider Slider;

		// Token: 0x04003497 RID: 13463
		public RectTransform SliderRect;

		// Token: 0x04003498 RID: 13464
		public Image FillImage;

		// Token: 0x04003499 RID: 13465
		public TextMeshProUGUI Label;

		// Token: 0x0400349A RID: 13466
		[Header("Settings")]
		public Color SliderColor_Green;

		// Token: 0x0400349B RID: 13467
		public Color SliderColor_Red;

		// Token: 0x0400349C RID: 13468
		private float displayedValue = 1f;
	}
}
