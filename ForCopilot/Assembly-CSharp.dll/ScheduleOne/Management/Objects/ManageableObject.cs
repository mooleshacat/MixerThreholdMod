using System;
using ScheduleOne.Management.Presets;
using UnityEngine;

namespace ScheduleOne.Management.Objects
{
	// Token: 0x020005DA RID: 1498
	public abstract class ManageableObject : MonoBehaviour
	{
		// Token: 0x060024C7 RID: 9415
		public abstract ManageableObjectType GetObjectType();

		// Token: 0x060024C8 RID: 9416
		public abstract Preset GetCurrentPreset();

		// Token: 0x060024C9 RID: 9417 RVA: 0x0009602E File Offset: 0x0009422E
		public void SetPreset(Preset newPreset)
		{
			if (this.GetCurrentPreset() != null)
			{
				Preset currentPreset = this.GetCurrentPreset();
				currentPreset.onDeleted = (Preset.PresetDeletion)Delegate.Remove(currentPreset.onDeleted, new Preset.PresetDeletion(this.ExistingPresetDeleted));
			}
			this.SetPreset_Internal(newPreset);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00096066 File Offset: 0x00094266
		protected virtual void SetPreset_Internal(Preset preset)
		{
			preset.onDeleted = (Preset.PresetDeletion)Delegate.Combine(preset.onDeleted, new Preset.PresetDeletion(this.ExistingPresetDeleted));
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x0009608A File Offset: 0x0009428A
		public void ExistingPresetDeleted(Preset replacement)
		{
			this.SetPreset(replacement);
		}
	}
}
