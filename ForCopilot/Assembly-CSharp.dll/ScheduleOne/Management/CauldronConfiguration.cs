using System;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200059F RID: 1439
	public class CauldronConfiguration : EntityConfiguration
	{
		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x000910EF File Offset: 0x0008F2EF
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x000910F7 File Offset: 0x0008F2F7
		public Cauldron Station { get; protected set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x00091100 File Offset: 0x0008F300
		// (set) Token: 0x0600230F RID: 8975 RVA: 0x00091108 File Offset: 0x0008F308
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002310 RID: 8976 RVA: 0x00091114 File Offset: 0x0008F314
		public CauldronConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Cauldron cauldron) : base(replicator, configurable)
		{
			this.Station = cauldron;
			this.AssignedChemist = new NPCField(this);
			this.AssignedChemist.TypeRequirement = typeof(Chemist);
			this.AssignedChemist.onNPCChanged.AddListener(delegate(NPC <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination = new ObjectField(this);
			this.Destination.objectFilter = new ObjectSelector.ObjectFilter(this.DestinationFilter);
			this.Destination.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.DestinationChanged));
			this.Destination.DrawTransitLine = true;
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000911D4 File Offset: 0x0008F3D4
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x000911F8 File Offset: 0x0008F3F8
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Station, this.Destination.SelectedObject as ITransitEntity);
				if (base.IsSelected)
				{
					this.DestinationRoute.SetVisualsActive(true);
					return;
				}
			}
			else
			{
				this.DestinationRoute = null;
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0009126F File Offset: 0x0008F46F
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Station;
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0009129E File Offset: 0x0008F49E
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x000912BA File Offset: 0x0008F4BA
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x000912D6 File Offset: 0x0008F4D6
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x000912F3 File Offset: 0x0008F4F3
		public override string GetSaveString()
		{
			return new CauldronConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A69 RID: 6761
		public NPCField AssignedChemist;

		// Token: 0x04001A6A RID: 6762
		public ObjectField Destination;
	}
}
