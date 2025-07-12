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
	// Token: 0x020005A8 RID: 1448
	public class PackagingStationConfiguration : EntityConfiguration
	{
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x00092FF1 File Offset: 0x000911F1
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x00092FF9 File Offset: 0x000911F9
		public PackagingStation Station { get; protected set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x00093002 File Offset: 0x00091202
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x0009300A File Offset: 0x0009120A
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002392 RID: 9106 RVA: 0x00093014 File Offset: 0x00091214
		public PackagingStationConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, PackagingStation station) : base(replicator, configurable)
		{
			this.Station = station;
			this.AssignedPackager = new NPCField(this);
			this.AssignedPackager.TypeRequirement = typeof(Packager);
			this.AssignedPackager.onNPCChanged.AddListener(delegate(NPC <p0>)
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

		// Token: 0x06002393 RID: 9107 RVA: 0x000930D4 File Offset: 0x000912D4
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000930F8 File Offset: 0x000912F8
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

		// Token: 0x06002395 RID: 9109 RVA: 0x0009316F File Offset: 0x0009136F
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Station;
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0009319E File Offset: 0x0009139E
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000931BA File Offset: 0x000913BA
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000931D6 File Offset: 0x000913D6
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000931F3 File Offset: 0x000913F3
		public override string GetSaveString()
		{
			return new PackagingStationConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A99 RID: 6809
		public NPCField AssignedPackager;

		// Token: 0x04001A9A RID: 6810
		public ObjectField Destination;
	}
}
