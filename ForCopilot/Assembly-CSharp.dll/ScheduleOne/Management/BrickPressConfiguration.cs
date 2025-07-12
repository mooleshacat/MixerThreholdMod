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
	// Token: 0x0200059E RID: 1438
	public class BrickPressConfiguration : EntityConfiguration
	{
		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x00090ED5 File Offset: 0x0008F0D5
		// (set) Token: 0x060022FF RID: 8959 RVA: 0x00090EDD File Offset: 0x0008F0DD
		public BrickPress BrickPress { get; protected set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x00090EE6 File Offset: 0x0008F0E6
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x00090EEE File Offset: 0x0008F0EE
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002302 RID: 8962 RVA: 0x00090EF8 File Offset: 0x0008F0F8
		public BrickPressConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, BrickPress station) : base(replicator, configurable)
		{
			this.BrickPress = station;
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

		// Token: 0x06002303 RID: 8963 RVA: 0x00090FB8 File Offset: 0x0008F1B8
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x00090FDC File Offset: 0x0008F1DC
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.BrickPress, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x06002305 RID: 8965 RVA: 0x00091053 File Offset: 0x0008F253
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.BrickPress;
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x00091082 File Offset: 0x0008F282
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0009109E File Offset: 0x0008F29E
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000910BA File Offset: 0x0008F2BA
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000910D7 File Offset: 0x0008F2D7
		public override string GetSaveString()
		{
			return new BrickPressConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A65 RID: 6757
		public NPCField AssignedPackager;

		// Token: 0x04001A66 RID: 6758
		public ObjectField Destination;
	}
}
