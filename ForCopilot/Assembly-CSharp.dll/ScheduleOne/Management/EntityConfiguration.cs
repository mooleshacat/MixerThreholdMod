using System;
using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A4 RID: 1444
	public class EntityConfiguration
	{
		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x00092436 File Offset: 0x00090636
		// (set) Token: 0x06002354 RID: 9044 RVA: 0x0009243E File Offset: 0x0009063E
		public ConfigurationReplicator Replicator { get; protected set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x00092447 File Offset: 0x00090647
		// (set) Token: 0x06002356 RID: 9046 RVA: 0x0009244F File Offset: 0x0009064F
		public IConfigurable Configurable { get; protected set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x00092458 File Offset: 0x00090658
		// (set) Token: 0x06002358 RID: 9048 RVA: 0x00092460 File Offset: 0x00090660
		public bool IsSelected { get; protected set; }

		// Token: 0x06002359 RID: 9049 RVA: 0x00092469 File Offset: 0x00090669
		public EntityConfiguration(ConfigurationReplicator replicator, IConfigurable configurable)
		{
			this.Replicator = replicator;
			this.Replicator.Configuration = this;
			this.Configurable = configurable;
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000924A1 File Offset: 0x000906A1
		protected void InvokeChanged()
		{
			if (this.onChanged != null)
			{
				this.onChanged.Invoke();
			}
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000924B6 File Offset: 0x000906B6
		public void ReplicateField(ConfigField field, NetworkConnection conn = null)
		{
			this.Replicator.ReplicateField(field, conn);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000924C8 File Offset: 0x000906C8
		public void ReplicateAllFields(NetworkConnection conn = null, bool replicateDefaults = true)
		{
			foreach (ConfigField configField in this.Fields)
			{
				if (replicateDefaults || !configField.IsValueDefault())
				{
					this.ReplicateField(configField, conn);
				}
			}
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x00092528 File Offset: 0x00090728
		public virtual void Destroy()
		{
			this.Reset();
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Reset()
		{
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x00092530 File Offset: 0x00090730
		public virtual void Selected()
		{
			this.IsSelected = true;
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x00092539 File Offset: 0x00090739
		public virtual void Deselected()
		{
			this.IsSelected = false;
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual bool ShouldSave()
		{
			return false;
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x00092542 File Offset: 0x00090742
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x04001A85 RID: 6789
		public List<ConfigField> Fields = new List<ConfigField>();

		// Token: 0x04001A86 RID: 6790
		public UnityEvent onChanged = new UnityEvent();
	}
}
