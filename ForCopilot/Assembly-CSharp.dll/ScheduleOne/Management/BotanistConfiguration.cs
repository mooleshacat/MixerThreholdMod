using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x0200059D RID: 1437
	public class BotanistConfiguration : EntityConfiguration
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x0009090B File Offset: 0x0008EB0B
		// (set) Token: 0x060022F2 RID: 8946 RVA: 0x00090913 File Offset: 0x0008EB13
		public Botanist botanist { get; protected set; }

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x0009091C File Offset: 0x0008EB1C
		// (set) Token: 0x060022F4 RID: 8948 RVA: 0x00090924 File Offset: 0x0008EB24
		public EmployeeHome assignedHome { get; private set; }

		// Token: 0x060022F5 RID: 8949 RVA: 0x00090930 File Offset: 0x0008EB30
		public BotanistConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Botanist _botanist) : base(replicator, configurable)
		{
			this.botanist = _botanist;
			this.Home = new ObjectField(this);
			this.Home.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.HomeChanged));
			this.Home.objectFilter = new ObjectSelector.ObjectFilter(EmployeeHome.IsBuildableEntityAValidEmployeeHome);
			this.Supplies = new ObjectField(this);
			this.Supplies.TypeRequirements = new List<Type>
			{
				typeof(PlaceableStorageEntity)
			};
			this.Supplies.onObjectChanged.AddListener(delegate(BuildableItem <p0>)
			{
				base.InvokeChanged();
			});
			this.AssignedStations = new ObjectListField(this);
			this.AssignedStations.MaxItems = this.botanist.MaxAssignedPots;
			this.AssignedStations.TypeRequirements = new List<Type>
			{
				typeof(Pot),
				typeof(DryingRack)
			};
			this.AssignedStations.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.AssignedStations.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedPotsChanged));
			this.AssignedStations.objectFilter = new ObjectSelector.ObjectFilter(this.IsStationValid);
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x00090A8C File Offset: 0x0008EC8C
		public override void Reset()
		{
			this.Home.SetObject(null, false);
			foreach (Pot pot in this.AssignedPots)
			{
				(pot.Configuration as PotConfiguration).AssignedBotanist.SetNPC(null, false);
			}
			foreach (DryingRack dryingRack in this.AssignedRacks)
			{
				(dryingRack.Configuration as DryingRackConfiguration).AssignedBotanist.SetNPC(null, false);
			}
			base.Reset();
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x00090B50 File Offset: 0x0008ED50
		private bool IsStationValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			Pot pot = obj as Pot;
			DryingRack dryingRack = obj as DryingRack;
			if (pot != null)
			{
				PotConfiguration potConfiguration = pot.Configuration as PotConfiguration;
				if (potConfiguration.AssignedBotanist.SelectedNPC != null && potConfiguration.AssignedBotanist.SelectedNPC != this.botanist)
				{
					reason = "Already assigned to " + potConfiguration.AssignedBotanist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else
			{
				if (!(dryingRack != null))
				{
					reason = "Not a pot or drying rack";
					return false;
				}
				DryingRackConfiguration dryingRackConfiguration = dryingRack.Configuration as DryingRackConfiguration;
				if (dryingRackConfiguration.AssignedBotanist.SelectedNPC != null && dryingRackConfiguration.AssignedBotanist.SelectedNPC != this.botanist)
				{
					reason = "Already assigned to " + dryingRackConfiguration.AssignedBotanist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x00090C3C File Offset: 0x0008EE3C
		public void AssignedPotsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.AssignedPots.Count; i++)
			{
				if (!objects.Contains(this.AssignedPots[i]))
				{
					Pot pot = this.AssignedPots[i];
					this.AssignedPots.RemoveAt(i);
					i--;
					if ((pot.Configuration as PotConfiguration).AssignedBotanist.SelectedNPC == this.botanist)
					{
						(pot.Configuration as PotConfiguration).AssignedBotanist.SetNPC(null, false);
					}
				}
			}
			for (int j = 0; j < objects.Count; j++)
			{
				if (objects[j] is Pot)
				{
					if (!this.AssignedPots.Contains(objects[j]))
					{
						Pot pot2 = objects[j] as Pot;
						this.AssignedPots.Add(pot2);
						if ((pot2.Configuration as PotConfiguration).AssignedBotanist.SelectedNPC != this.botanist)
						{
							(pot2.Configuration as PotConfiguration).AssignedBotanist.SetNPC(this.botanist, false);
						}
					}
				}
				else if (objects[j] is DryingRack && !this.AssignedRacks.Contains(objects[j]))
				{
					DryingRack dryingRack = objects[j] as DryingRack;
					this.AssignedRacks.Add(dryingRack);
					if ((dryingRack.Configuration as DryingRackConfiguration).AssignedBotanist.SelectedNPC != this.botanist)
					{
						(dryingRack.Configuration as DryingRackConfiguration).AssignedBotanist.SetNPC(this.botanist, false);
					}
				}
			}
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x00090DE0 File Offset: 0x0008EFE0
		public override bool ShouldSave()
		{
			return this.AssignedPots.Count > 0 || this.AssignedRacks.Count > 0 || this.Supplies.SelectedObject != null || this.Home.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x00090E3D File Offset: 0x0008F03D
		public override string GetSaveString()
		{
			return new BotanistConfigurationData(this.Home.GetData(), this.Supplies.GetData(), this.AssignedStations.GetData()).GetJson(true);
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x00090E6C File Offset: 0x0008F06C
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
				this.assignedHome.SetAssignedEmployee(this.botanist);
			}
			base.InvokeChanged();
		}

		// Token: 0x04001A5D RID: 6749
		public ObjectField Home;

		// Token: 0x04001A5E RID: 6750
		public ObjectField Supplies;

		// Token: 0x04001A5F RID: 6751
		public ObjectListField AssignedStations;

		// Token: 0x04001A60 RID: 6752
		public List<Pot> AssignedPots = new List<Pot>();

		// Token: 0x04001A61 RID: 6753
		public List<DryingRack> AssignedRacks = new List<DryingRack>();
	}
}
