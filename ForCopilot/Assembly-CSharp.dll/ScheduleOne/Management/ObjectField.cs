using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005B0 RID: 1456
	public class ObjectField : ConfigField
	{
		// Token: 0x060023D0 RID: 9168 RVA: 0x000939F5 File Offset: 0x00091BF5
		public ObjectField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x00093A14 File Offset: 0x00091C14
		public void SetObject(BuildableItem obj, bool network)
		{
			if (this.SelectedObject == obj)
			{
				return;
			}
			if (this.SelectedObject != null)
			{
				this.SelectedObject.onDestroyed.RemoveListener(new UnityAction(this.SelectedObjectDestroyed));
			}
			this.SelectedObject = obj;
			if (this.SelectedObject != null)
			{
				this.SelectedObject.onDestroyed.AddListener(new UnityAction(this.SelectedObjectDestroyed));
			}
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onObjectChanged != null)
			{
				this.onObjectChanged.Invoke(obj);
			}
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x00093AAF File Offset: 0x00091CAF
		public override bool IsValueDefault()
		{
			return this.SelectedObject == null;
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x00093ABD File Offset: 0x00091CBD
		private void SelectedObjectDestroyed()
		{
			this.SetObject(null, false);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x00093AC8 File Offset: 0x00091CC8
		public void Load(ObjectFieldData data)
		{
			if (data != null && !string.IsNullOrEmpty(data.ObjectGUID))
			{
				BuildableItem @object = GUIDManager.GetObject<BuildableItem>(new Guid(data.ObjectGUID));
				if (@object != null)
				{
					this.SetObject(@object, true);
				}
			}
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x00093B08 File Offset: 0x00091D08
		public ObjectFieldData GetData()
		{
			return new ObjectFieldData((this.SelectedObject != null) ? this.SelectedObject.GUID.ToString() : "");
		}

		// Token: 0x04001ABE RID: 6846
		public BuildableItem SelectedObject;

		// Token: 0x04001ABF RID: 6847
		public UnityEvent<BuildableItem> onObjectChanged = new UnityEvent<BuildableItem>();

		// Token: 0x04001AC0 RID: 6848
		public ObjectSelector.ObjectFilter objectFilter;

		// Token: 0x04001AC1 RID: 6849
		public List<Type> TypeRequirements = new List<Type>();

		// Token: 0x04001AC2 RID: 6850
		public bool DrawTransitLine;
	}
}
