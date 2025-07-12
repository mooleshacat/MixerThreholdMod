using System;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A3 RID: 1443
	public class DryingRackConfiguration : EntityConfiguration
	{
		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x000921DD File Offset: 0x000903DD
		// (set) Token: 0x06002345 RID: 9029 RVA: 0x000921E5 File Offset: 0x000903E5
		public DryingRack Rack { get; protected set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x000921EE File Offset: 0x000903EE
		// (set) Token: 0x06002347 RID: 9031 RVA: 0x000921F6 File Offset: 0x000903F6
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002348 RID: 9032 RVA: 0x00092200 File Offset: 0x00090400
		public DryingRackConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, DryingRack rack) : base(replicator, configurable)
		{
			this.Rack = rack;
			this.AssignedBotanist = new NPCField(this);
			this.AssignedBotanist.TypeRequirement = typeof(Botanist);
			this.AssignedBotanist.onNPCChanged.AddListener(delegate(NPC <p0>)
			{
				base.InvokeChanged();
			});
			this.TargetQuality = new QualityField(this);
			this.TargetQuality.onValueChanged.AddListener(delegate(EQuality <p0>)
			{
				base.InvokeChanged();
			});
			this.TargetQuality.SetValue(EQuality.Premium, false);
			this.Destination = new ObjectField(this);
			this.Destination.objectFilter = new ObjectSelector.ObjectFilter(this.DestinationFilter);
			this.Destination.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.DestinationChanged));
			this.Destination.DrawTransitLine = true;
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000922F5 File Offset: 0x000904F5
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x00092318 File Offset: 0x00090518
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Rack, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x0600234B RID: 9035 RVA: 0x0009238F File Offset: 0x0009058F
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Rack;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000923BE File Offset: 0x000905BE
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000923DA File Offset: 0x000905DA
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000923F6 File Offset: 0x000905F6
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x00092413 File Offset: 0x00090613
		public override string GetSaveString()
		{
			return new DryingRackConfigurationData(this.TargetQuality.GetData(), this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A7F RID: 6783
		public NPCField AssignedBotanist;

		// Token: 0x04001A80 RID: 6784
		public QualityField TargetQuality;

		// Token: 0x04001A81 RID: 6785
		public ObjectField Destination;
	}
}
