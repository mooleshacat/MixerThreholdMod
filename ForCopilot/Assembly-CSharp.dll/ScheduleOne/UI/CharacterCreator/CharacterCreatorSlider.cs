using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCreator
{
	// Token: 0x02000B92 RID: 2962
	public class CharacterCreatorSlider : CharacterCreatorField<float>
	{
		// Token: 0x06004E7C RID: 20092 RVA: 0x0014B9B9 File Offset: 0x00149BB9
		protected override void Awake()
		{
			base.Awake();
			this.Slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderChanged));
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x0014B9DD File Offset: 0x00149BDD
		public override void ApplyValue()
		{
			base.ApplyValue();
			this.Slider.SetValueWithoutNotify(base.value);
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x0014B9F6 File Offset: 0x00149BF6
		public void OnSliderChanged(float newValue)
		{
			base.value = newValue;
			this.WriteValue(false);
		}

		// Token: 0x04003ACD RID: 15053
		[Header("References")]
		public Slider Slider;
	}
}
