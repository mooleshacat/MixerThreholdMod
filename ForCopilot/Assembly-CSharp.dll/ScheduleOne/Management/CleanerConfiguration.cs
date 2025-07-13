using System;
using System.Collections.Generic;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A2 RID: 1442
	public class CleanerConfiguration : EntityConfiguration
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002336 RID: 9014 RVA: 0x00091F42 File Offset: 0x00090142
		// (set) Token: 0x06002337 RID: 9015 RVA: 0x00091F4A File Offset: 0x0009014A
		public Cleaner cleaner { get; protected set; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x00091F53 File Offset: 0x00090153
		// (set) Token: 0x06002339 RID: 9017 RVA: 0x00091F5B File Offset: 0x0009015B
		public List<TrashContainerItem> binItems { get; private set; } = new List<TrashContainerItem>();

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x00091F64 File Offset: 0x00090164
		// (set) Token: 0x0600233B RID: 9019 RVA: 0x00091F6C File Offset: 0x0009016C
		public EmployeeHome assignedHome { get; private set; }

		// Token: 0x0600233C RID: 9020 RVA: 0x00091F78 File Offset: 0x00090178
		public CleanerConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Cleaner _cleaner) : base(replicator, configurable)
		{
			this.cleaner = _cleaner;
			this.Home = new ObjectField(this);
			this.Home.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.HomeChanged));
			this.Home.objectFilter = new ObjectSelector.ObjectFilter(EmployeeHome.IsBuildableEntityAValidEmployeeHome);
			this.Bins = new ObjectListField(this);
			this.Bins.MaxItems = 3;
			this.Bins.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.Bins.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedBinsChanged));
			this.Bins.objectFilter = new ObjectSelector.ObjectFilter(this.IsObjValid);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x00092045 File Offset: 0x00090245
		public override void Reset()
		{
			this.Home.SetObject(null, false);
			base.Reset();
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0009205C File Offset: 0x0009025C
		private bool IsObjValid(BuildableItem obj, out string reason)
		{
			TrashContainerItem trashContainerItem = obj as TrashContainerItem;
			if (trashContainerItem == null)
			{
				reason = string.Empty;
				return false;
			}
			if (!trashContainerItem.UsableByCleaners)
			{
				reason = "This trash can is not usable by cleaners.";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0009209C File Offset: 0x0009029C
		public void AssignedBinsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.binItems.Count; i++)
			{
				if (!objects.Contains(this.binItems[i]))
				{
					this.binItems.RemoveAt(i);
					i--;
				}
			}
			for (int j = 0; j < objects.Count; j++)
			{
				if (!this.binItems.Contains(objects[j] as TrashContainerItem))
				{
					this.binItems.Add(objects[j] as TrashContainerItem);
				}
			}
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x00092124 File Offset: 0x00090324
		public override bool ShouldSave()
		{
			return this.Home.SelectedObject != null || this.Bins.SelectedObjects.Count > 0 || base.ShouldSave();
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x00092156 File Offset: 0x00090356
		public override string GetSaveString()
		{
			return new CleanerConfigurationData(this.Home.GetData(), this.Bins.GetData()).GetJson(true);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0009217C File Offset: 0x0009037C
		private void HomeChanged(BuildableItem newItem)
		{
			EmployeeHome assignedHome = this.assignedHome;
			if (assignedHome != null)
			{
				assignedHome.SetAssignedEmployee(null);
			}
			this.assignedHome = ((newItem != null) ? newItem.GetComponent<EmployeeHome>() : null);
			if (this.assignedHome != null)
			{
				this.assignedHome.SetAssignedEmployee(this.cleaner);
			}
			base.InvokeChanged();
		}

		// Token: 0x04001A79 RID: 6777
		public ObjectField Home;

		// Token: 0x04001A7A RID: 6778
		public ObjectListField Bins;
	}
}
