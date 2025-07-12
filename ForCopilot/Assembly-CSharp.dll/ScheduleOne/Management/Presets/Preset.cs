using System;
using UnityEngine;

namespace ScheduleOne.Management.Presets
{
	// Token: 0x020005D3 RID: 1491
	public abstract class Preset
	{
		// Token: 0x060024A8 RID: 9384 RVA: 0x00095DDC File Offset: 0x00093FDC
		public Preset()
		{
			this.InitializeOptions();
		}

		// Token: 0x060024A9 RID: 9385
		public abstract Preset GetCopy();

		// Token: 0x060024AA RID: 9386 RVA: 0x00095E14 File Offset: 0x00094014
		public virtual void CopyTo(Preset other)
		{
			other.PresetName = this.PresetName;
			other.PresetColor = this.PresetColor;
		}

		// Token: 0x060024AB RID: 9387
		public abstract void InitializeOptions();

		// Token: 0x060024AC RID: 9388 RVA: 0x00095E2E File Offset: 0x0009402E
		public void SetName(string newName)
		{
			if (this.PresetName == newName)
			{
				return;
			}
			this.PresetName = newName;
			if (this.onNameChanged != null)
			{
				this.onNameChanged(newName);
			}
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00095E5A File Offset: 0x0009405A
		public void DeletePreset(Preset replacement)
		{
			if (this.onDeleted != null)
			{
				this.onDeleted(replacement);
			}
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x00095E70 File Offset: 0x00094070
		public static Preset GetDefault(ManageableObjectType type)
		{
			if (type == ManageableObjectType.Pot)
			{
				return PotPreset.GetDefaultPreset();
			}
			Console.LogWarning("GetDefault: type not accounted for", null);
			return null;
		}

		// Token: 0x04001B21 RID: 6945
		public string PresetName = "Default";

		// Token: 0x04001B22 RID: 6946
		public Color32 PresetColor = new Color32(180, 180, 180, byte.MaxValue);

		// Token: 0x04001B23 RID: 6947
		public ManageableObjectType ObjectType;

		// Token: 0x04001B24 RID: 6948
		public Preset.NameChange onNameChanged;

		// Token: 0x04001B25 RID: 6949
		public Preset.PresetDeletion onDeleted;

		// Token: 0x020005D4 RID: 1492
		// (Invoke) Token: 0x060024B0 RID: 9392
		public delegate void NameChange(string name);

		// Token: 0x020005D5 RID: 1493
		// (Invoke) Token: 0x060024B4 RID: 9396
		public delegate void PresetDeletion(Preset replacement);
	}
}
