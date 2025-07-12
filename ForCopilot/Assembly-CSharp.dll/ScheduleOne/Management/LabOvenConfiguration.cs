using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A5 RID: 1445
	public class LabOvenConfiguration : EntityConfiguration
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x00092549 File Offset: 0x00090749
		// (set) Token: 0x06002364 RID: 9060 RVA: 0x00092551 File Offset: 0x00090751
		public LabOven Oven { get; protected set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x0009255A File Offset: 0x0009075A
		// (set) Token: 0x06002366 RID: 9062 RVA: 0x00092562 File Offset: 0x00090762
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x06002367 RID: 9063 RVA: 0x0009256C File Offset: 0x0009076C
		public LabOvenConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, LabOven oven) : base(replicator, configurable)
		{
			this.Oven = oven;
			this.AssignedChemist = new NPCField(this);
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

		// Token: 0x06002368 RID: 9064 RVA: 0x00092617 File Offset: 0x00090817
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0009263C File Offset: 0x0009083C
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Oven, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x0600236A RID: 9066 RVA: 0x000926B3 File Offset: 0x000908B3
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Oven;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000926E2 File Offset: 0x000908E2
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000926FE File Offset: 0x000908FE
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0009271A File Offset: 0x0009091A
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x00092737 File Offset: 0x00090937
		public override string GetSaveString()
		{
			return new LabOvenConfigurationData(this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A89 RID: 6793
		public NPCField AssignedChemist;

		// Token: 0x04001A8A RID: 6794
		public ObjectField Destination;
	}
}
