using System;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005AE RID: 1454
	public class NPCField : ConfigField
	{
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x0009380A File Offset: 0x00091A0A
		// (set) Token: 0x060023BB RID: 9147 RVA: 0x00093812 File Offset: 0x00091A12
		public NPC SelectedNPC { get; protected set; }

		// Token: 0x060023BC RID: 9148 RVA: 0x0009381B File Offset: 0x00091A1B
		public NPCField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0009382F File Offset: 0x00091A2F
		public void SetNPC(NPC npc, bool network)
		{
			if (this.SelectedNPC == npc)
			{
				return;
			}
			this.SelectedNPC = npc;
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onNPCChanged != null)
			{
				this.onNPCChanged.Invoke(npc);
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0009386B File Offset: 0x00091A6B
		public bool DoesNPCMatchRequirement(NPC npc)
		{
			return this.TypeRequirement == null || npc.GetType() == this.TypeRequirement;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0009388E File Offset: 0x00091A8E
		public override bool IsValueDefault()
		{
			return this.SelectedNPC == null;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0009389C File Offset: 0x00091A9C
		public NPCFieldData GetData()
		{
			return new NPCFieldData((this.SelectedNPC != null) ? this.SelectedNPC.GUID.ToString() : "");
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000938DC File Offset: 0x00091ADC
		public void Load(NPCFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.NPCGuid))
			{
				NPC @object = GUIDManager.GetObject<NPC>(new Guid(data.NPCGuid));
				if (@object != null)
				{
					this.SetNPC(@object, true);
				}
			}
		}

		// Token: 0x04001AB7 RID: 6839
		public Type TypeRequirement;

		// Token: 0x04001AB8 RID: 6840
		public UnityEvent<NPC> onNPCChanged = new UnityEvent<NPC>();
	}
}
