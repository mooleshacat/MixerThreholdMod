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
	// Token: 0x020005A6 RID: 1446
	public class MixingStationConfiguration : EntityConfiguration
	{
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x0009274F File Offset: 0x0009094F
		// (set) Token: 0x06002372 RID: 9074 RVA: 0x00092757 File Offset: 0x00090957
		public MixingStation station { get; protected set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x00092760 File Offset: 0x00090960
		// (set) Token: 0x06002374 RID: 9076 RVA: 0x00092768 File Offset: 0x00090968
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002375 RID: 9077 RVA: 0x00092774 File Offset: 0x00090974
		public MixingStationConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, MixingStation station) : base(replicator, configurable)
		{
			this.station = station;
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
			this.StartThrehold = new NumberField(this);
			this.StartThrehold.Configure(1f, 10f, true);
			this.StartThrehold.SetValue(1f, false);
			this.StartThrehold.onItemChanged.AddListener(delegate(float <p0>)
			{
				base.InvokeChanged();
			});
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x00092883 File Offset: 0x00090A83
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000928A8 File Offset: 0x00090AA8
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.station, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x06002378 RID: 9080 RVA: 0x0009291F File Offset: 0x00090B1F
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.station;
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0009294E File Offset: 0x00090B4E
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0009296A File Offset: 0x00090B6A
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x00092986 File Offset: 0x00090B86
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x000929A3 File Offset: 0x00090BA3
		public override string GetSaveString()
		{
			return new MixingStationConfigurationData(this.Destination.GetData(), this.StartThrehold.GetData()).GetJson(true);
		}

		// Token: 0x04001A8D RID: 6797
		public NPCField AssignedChemist;

		// Token: 0x04001A8E RID: 6798
		public ObjectField Destination;

		// Token: 0x04001A8F RID: 6799
		public NumberField StartThrehold;
	}
}
