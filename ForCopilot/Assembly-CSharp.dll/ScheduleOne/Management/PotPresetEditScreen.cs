using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.Presets;
using ScheduleOne.Management.Presets.Options;
using ScheduleOne.Management.SetterScreens;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005BE RID: 1470
	public class PotPresetEditScreen : PresetEditScreen
	{
		// Token: 0x0600243A RID: 9274 RVA: 0x00094BC0 File Offset: 0x00092DC0
		protected override void Awake()
		{
			base.Awake();
			this.SeedsUI.Button.onClick.AddListener(new UnityAction(this.SeedsUIClicked));
			this.AdditivesUI.Button.onClick.AddListener(new UnityAction(this.AdditivesUIClicked));
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x00094C15 File Offset: 0x00092E15
		protected virtual void Update()
		{
			if (base.isOpen)
			{
				this.UpdateUI();
			}
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x00094C25 File Offset: 0x00092E25
		public override void Open(Preset preset)
		{
			base.Open(preset);
			this.castedPreset = (PotPreset)this.EditedPreset;
			this.UpdateUI();
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x00094C48 File Offset: 0x00092E48
		private void UpdateUI()
		{
			this.SeedsUI.ValueLabel.text = this.castedPreset.Seeds.GetDisplayString();
			this.AdditivesUI.ValueLabel.text = this.castedPreset.Additives.GetDisplayString();
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x00094C95 File Offset: 0x00092E95
		public void SeedsUIClicked()
		{
			Singleton<ItemSetterScreen>.Instance.Open((this.EditedPreset as PotPreset).Seeds);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x00094CB1 File Offset: 0x00092EB1
		public void AdditivesUIClicked()
		{
			Singleton<ItemSetterScreen>.Instance.Open((this.EditedPreset as PotPreset).Additives);
		}

		// Token: 0x04001AE0 RID: 6880
		public GenericOptionUI SeedsUI;

		// Token: 0x04001AE1 RID: 6881
		public GenericOptionUI AdditivesUI;

		// Token: 0x04001AE2 RID: 6882
		private PotPreset castedPreset;
	}
}
