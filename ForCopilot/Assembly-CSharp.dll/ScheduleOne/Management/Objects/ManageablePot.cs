using System;
using ScheduleOne.Management.Presets;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.Management.Objects
{
	// Token: 0x020005DB RID: 1499
	[RequireComponent(typeof(Pot))]
	public class ManageablePot : ManageableObject
	{
		// Token: 0x060024CD RID: 9421 RVA: 0x00096093 File Offset: 0x00094293
		protected virtual void Awake()
		{
			this.CurrentPreset = PotPreset.GetDefaultPreset();
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00014B5A File Offset: 0x00012D5A
		public override ManageableObjectType GetObjectType()
		{
			return ManageableObjectType.Pot;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000960A0 File Offset: 0x000942A0
		public override Preset GetCurrentPreset()
		{
			return this.CurrentPreset;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000960A8 File Offset: 0x000942A8
		protected override void SetPreset_Internal(Preset newPreset)
		{
			base.SetPreset_Internal(newPreset);
			PotPreset potPreset = (PotPreset)newPreset;
			if (potPreset == null)
			{
				Console.LogWarning("SetPreset_Internal: preset is not the right type", null);
				return;
			}
			this.CurrentPreset = potPreset;
		}

		// Token: 0x04001B30 RID: 6960
		public PotPreset CurrentPreset;
	}
}
