using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.NPCs;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Management;
using ScheduleOne.UI.Stations;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A1 RID: 1441
	public class ChemistryStationConfiguration : EntityConfiguration
	{
		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x00091CF5 File Offset: 0x0008FEF5
		// (set) Token: 0x06002328 RID: 9000 RVA: 0x00091CFD File Offset: 0x0008FEFD
		public ChemistryStation Station { get; protected set; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002329 RID: 9001 RVA: 0x00091D06 File Offset: 0x0008FF06
		// (set) Token: 0x0600232A RID: 9002 RVA: 0x00091D0E File Offset: 0x0008FF0E
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x0600232B RID: 9003 RVA: 0x00091D18 File Offset: 0x0008FF18
		public ChemistryStationConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, ChemistryStation station) : base(replicator, configurable)
		{
			this.Station = station;
			this.AssignedChemist = new NPCField(this);
			this.AssignedChemist.onNPCChanged.AddListener(delegate(NPC <p0>)
			{
				base.InvokeChanged();
			});
			this.Recipe = new StationRecipeField(this);
			this.Recipe.Options = Singleton<ChemistryStationCanvas>.Instance.Recipes;
			this.Recipe.onRecipeChanged.AddListener(delegate(StationRecipe <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination = new ObjectField(this);
			this.Destination.objectFilter = new ScheduleOne.UI.Management.ObjectSelector.ObjectFilter(this.DestinationFilter);
			this.Destination.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.Destination.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.DestinationChanged));
			this.Destination.DrawTransitLine = true;
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x00091E00 File Offset: 0x00090000
		public override void Reset()
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x00091E24 File Offset: 0x00090024
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

		// Token: 0x0600232E RID: 9006 RVA: 0x00091E9B File Offset: 0x0009009B
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Station;
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x00091ECA File Offset: 0x000900CA
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x00091EE6 File Offset: 0x000900E6
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x00091F02 File Offset: 0x00090102
		public override bool ShouldSave()
		{
			return this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x00091F1F File Offset: 0x0009011F
		public override string GetSaveString()
		{
			return new ChemistryStationConfigurationData(this.Recipe.GetData(), this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A75 RID: 6773
		public NPCField AssignedChemist;

		// Token: 0x04001A76 RID: 6774
		public StationRecipeField Recipe;

		// Token: 0x04001A77 RID: 6775
		public ObjectField Destination;
	}
}
