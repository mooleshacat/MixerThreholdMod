using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005B1 RID: 1457
	public class ObjectListField : ConfigField
	{
		// Token: 0x060023D6 RID: 9174 RVA: 0x00093B48 File Offset: 0x00091D48
		public ObjectListField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x00093B7C File Offset: 0x00091D7C
		public void SetList(List<BuildableItem> list, bool network)
		{
			if (this.SelectedObjects.SequenceEqual(list))
			{
				return;
			}
			for (int i = 0; i < this.SelectedObjects.Count; i++)
			{
				if (!(this.SelectedObjects[i] == null))
				{
					BuildableItem buildableItem = this.SelectedObjects[i];
					buildableItem.onDestroyedWithParameter = (Action<BuildableItem>)Delegate.Remove(buildableItem.onDestroyedWithParameter, new Action<BuildableItem>(this.SelectedObjectDestroyed));
				}
			}
			this.SelectedObjects = new List<BuildableItem>();
			this.SelectedObjects.AddRange(list);
			for (int j = 0; j < this.SelectedObjects.Count; j++)
			{
				if (!(this.SelectedObjects[j] == null))
				{
					BuildableItem buildableItem2 = this.SelectedObjects[j];
					buildableItem2.onDestroyedWithParameter = (Action<BuildableItem>)Delegate.Combine(buildableItem2.onDestroyedWithParameter, new Action<BuildableItem>(this.SelectedObjectDestroyed));
				}
			}
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onListChanged != null)
			{
				this.onListChanged.Invoke(list);
			}
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00093C84 File Offset: 0x00091E84
		public void AddItem(BuildableItem item)
		{
			if (this.SelectedObjects.Contains(item))
			{
				return;
			}
			if (this.SelectedObjects.Count >= this.MaxItems)
			{
				Console.LogWarning(item.ItemInstance.Name + " cannot be added to " + base.ParentConfig.GetType().Name + " because the maximum number of items has been reached", null);
				return;
			}
			this.SetList(new List<BuildableItem>(this.SelectedObjects)
			{
				item
			}, true);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x00093D00 File Offset: 0x00091F00
		public void RemoveItem(BuildableItem item)
		{
			if (!this.SelectedObjects.Contains(item))
			{
				return;
			}
			List<BuildableItem> list = new List<BuildableItem>(this.SelectedObjects);
			list.Remove(item);
			this.SetList(list, true);
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x00093D38 File Offset: 0x00091F38
		private void SelectedObjectDestroyed(BuildableItem item)
		{
			if (item == null)
			{
				return;
			}
			Console.Log("Removing destroyed object from " + base.ParentConfig.GetType().Name, null);
			this.RemoveItem(item);
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x00093D6B File Offset: 0x00091F6B
		public override bool IsValueDefault()
		{
			return this.SelectedObjects.Count == 0;
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x00093D7C File Offset: 0x00091F7C
		public ObjectListFieldData GetData()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.SelectedObjects.Count; i++)
			{
				list.Add(this.SelectedObjects[i].GUID.ToString());
			}
			return new ObjectListFieldData(list);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x00093DD0 File Offset: 0x00091FD0
		public void Load(ObjectListFieldData data)
		{
			if (data != null)
			{
				List<BuildableItem> list = new List<BuildableItem>();
				for (int i = 0; i < data.ObjectGUIDs.Count; i++)
				{
					if (!string.IsNullOrEmpty(data.ObjectGUIDs[i]))
					{
						BuildableItem @object = GUIDManager.GetObject<BuildableItem>(new Guid(data.ObjectGUIDs[i]));
						if (@object != null)
						{
							list.Add(@object);
						}
					}
				}
				this.SetList(list, true);
			}
		}

		// Token: 0x04001AC3 RID: 6851
		public List<BuildableItem> SelectedObjects = new List<BuildableItem>();

		// Token: 0x04001AC4 RID: 6852
		public int MaxItems = 1;

		// Token: 0x04001AC5 RID: 6853
		public ObjectSelector.ObjectFilter objectFilter;

		// Token: 0x04001AC6 RID: 6854
		public List<Type> TypeRequirements = new List<Type>();

		// Token: 0x04001AC7 RID: 6855
		public UnityEvent<List<BuildableItem>> onListChanged = new UnityEvent<List<BuildableItem>>();
	}
}
