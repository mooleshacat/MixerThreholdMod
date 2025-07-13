using System;
using FishNet.Connection;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Variables
{
	// Token: 0x02000297 RID: 663
	public abstract class BaseVariable
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0003D47A File Offset: 0x0003B67A
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x0003D482 File Offset: 0x0003B682
		public Player Owner { get; private set; }

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0003D48C File Offset: 0x0003B68C
		public BaseVariable(string name, EVariableReplicationMode replicationMode, bool persistent, EVariableMode mode, Player owner)
		{
			this.Name = name;
			this.ReplicationMode = replicationMode;
			if (mode == EVariableMode.Global)
			{
				NetworkSingleton<VariableDatabase>.Instance.AddVariable(this);
			}
			else
			{
				if (owner == null)
				{
					Console.LogError("Player variable created without owner", null);
					return;
				}
				owner.AddVariable(this);
			}
			this.Persistent = persistent;
			this.VariableMode = mode;
			this.Owner = owner;
		}

		// Token: 0x06000DD5 RID: 3541
		public abstract object GetValue();

		// Token: 0x06000DD6 RID: 3542
		public abstract void SetValue(object value, bool replicate = true);

		// Token: 0x06000DD7 RID: 3543
		public abstract void ReplicateValue(NetworkConnection conn);

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual bool EvaluateCondition(Condition.EConditionType operation, string value)
		{
			return false;
		}

		// Token: 0x04000E3F RID: 3647
		public EVariableReplicationMode ReplicationMode;

		// Token: 0x04000E40 RID: 3648
		public string Name;

		// Token: 0x04000E41 RID: 3649
		public bool Persistent;

		// Token: 0x04000E42 RID: 3650
		public EVariableMode VariableMode;
	}
}
