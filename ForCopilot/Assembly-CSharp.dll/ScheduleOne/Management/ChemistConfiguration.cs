using System;
using System.Collections.Generic;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A0 RID: 1440
	public class ChemistConfiguration : EntityConfiguration
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x0009130B File Offset: 0x0008F50B
		public int TotalStations
		{
			get
			{
				return this.ChemStations.Count + this.LabOvens.Count + this.Cauldrons.Count + this.MixStations.Count;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x0009133C File Offset: 0x0008F53C
		// (set) Token: 0x0600231C RID: 8988 RVA: 0x00091344 File Offset: 0x0008F544
		public Chemist chemist { get; protected set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x0009134D File Offset: 0x0008F54D
		// (set) Token: 0x0600231E RID: 8990 RVA: 0x00091355 File Offset: 0x0008F555
		public EmployeeHome assignedHome { get; private set; }

		// Token: 0x0600231F RID: 8991 RVA: 0x00091360 File Offset: 0x0008F560
		public ChemistConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Chemist _chemist) : base(replicator, configurable)
		{
			this.chemist = _chemist;
			this.Home = new ObjectField(this);
			this.Home.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.HomeChanged));
			this.Home.objectFilter = new ObjectSelector.ObjectFilter(EmployeeHome.IsBuildableEntityAValidEmployeeHome);
			this.Stations = new ObjectListField(this);
			this.Stations.MaxItems = 4;
			this.Stations.TypeRequirements = new List<Type>
			{
				typeof(ChemistryStation),
				typeof(LabOven),
				typeof(Cauldron),
				typeof(MixingStation),
				typeof(MixingStationMk2)
			};
			this.Stations.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.Stations.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedStationsChanged));
			this.Stations.objectFilter = new ObjectSelector.ObjectFilter(this.IsStationValid);
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000914B0 File Offset: 0x0008F6B0
		public override void Reset()
		{
			this.Home.SetObject(null, false);
			foreach (ChemistryStation chemistryStation in this.ChemStations)
			{
				(chemistryStation.Configuration as ChemistryStationConfiguration).AssignedChemist.SetNPC(null, false);
			}
			foreach (LabOven labOven in this.LabOvens)
			{
				(labOven.Configuration as LabOvenConfiguration).AssignedChemist.SetNPC(null, false);
			}
			foreach (Cauldron cauldron in this.Cauldrons)
			{
				(cauldron.Configuration as CauldronConfiguration).AssignedChemist.SetNPC(null, false);
			}
			foreach (MixingStation mixingStation in this.MixStations)
			{
				(mixingStation.Configuration as MixingStationConfiguration).AssignedChemist.SetNPC(null, false);
			}
			base.Reset();
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x00091614 File Offset: 0x0008F814
		private bool IsStationValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (obj is ChemistryStation)
			{
				ChemistryStationConfiguration chemistryStationConfiguration = (obj as ChemistryStation).Configuration as ChemistryStationConfiguration;
				if (chemistryStationConfiguration.AssignedChemist.SelectedNPC != null && chemistryStationConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + chemistryStationConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else if (obj is LabOven)
			{
				LabOvenConfiguration labOvenConfiguration = (obj as LabOven).Configuration as LabOvenConfiguration;
				if (labOvenConfiguration.AssignedChemist.SelectedNPC != null && labOvenConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + labOvenConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else if (obj is Cauldron)
			{
				CauldronConfiguration cauldronConfiguration = (obj as Cauldron).Configuration as CauldronConfiguration;
				if (cauldronConfiguration.AssignedChemist.SelectedNPC != null && cauldronConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + cauldronConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else
			{
				if (!(obj is MixingStation))
				{
					return false;
				}
				MixingStationConfiguration mixingStationConfiguration = (obj as MixingStation).Configuration as MixingStationConfiguration;
				if (mixingStationConfiguration.AssignedChemist.SelectedNPC != null && mixingStationConfiguration.AssignedChemist.SelectedNPC != this.chemist)
				{
					reason = "Already assigned to " + mixingStationConfiguration.AssignedChemist.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x000917BC File Offset: 0x0008F9BC
		public void AssignedStationsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.ChemStations.Count; i++)
			{
				if (!objects.Contains(this.ChemStations[i]))
				{
					ChemistryStation chemistryStation = this.ChemStations[i];
					this.ChemStations.RemoveAt(i);
					i--;
					if ((chemistryStation.Configuration as ChemistryStationConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(chemistryStation.Configuration as ChemistryStationConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int j = 0; j < this.LabOvens.Count; j++)
			{
				if (!objects.Contains(this.LabOvens[j]))
				{
					LabOven labOven = this.LabOvens[j];
					this.LabOvens.RemoveAt(j);
					j--;
					if ((labOven.Configuration as LabOvenConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(labOven.Configuration as LabOvenConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int k = 0; k < this.Cauldrons.Count; k++)
			{
				if (!objects.Contains(this.Cauldrons[k]))
				{
					Cauldron cauldron = this.Cauldrons[k];
					this.Cauldrons.RemoveAt(k);
					k--;
					if ((cauldron.Configuration as CauldronConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(cauldron.Configuration as CauldronConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int l = 0; l < this.MixStations.Count; l++)
			{
				if (!objects.Contains(this.MixStations[l]))
				{
					MixingStation mixingStation = this.MixStations[l];
					this.MixStations.RemoveAt(l);
					l--;
					if ((mixingStation.Configuration as MixingStationConfiguration).AssignedChemist.SelectedNPC == this.chemist)
					{
						(mixingStation.Configuration as MixingStationConfiguration).AssignedChemist.SetNPC(null, false);
					}
				}
			}
			for (int m = 0; m < objects.Count; m++)
			{
				if (objects[m] is ChemistryStation && !this.ChemStations.Contains(objects[m] as ChemistryStation))
				{
					ChemistryStation chemistryStation2 = objects[m] as ChemistryStation;
					this.ChemStations.Add(chemistryStation2);
					if ((chemistryStation2.Configuration as ChemistryStationConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(chemistryStation2.Configuration as ChemistryStationConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
				if (objects[m] is LabOven && !this.LabOvens.Contains(objects[m] as LabOven))
				{
					LabOven labOven2 = objects[m] as LabOven;
					this.LabOvens.Add(labOven2);
					if ((labOven2.Configuration as LabOvenConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(labOven2.Configuration as LabOvenConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
				if (objects[m] is Cauldron && !this.Cauldrons.Contains(objects[m] as Cauldron))
				{
					Cauldron cauldron2 = objects[m] as Cauldron;
					this.Cauldrons.Add(cauldron2);
					if ((cauldron2.Configuration as CauldronConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(cauldron2.Configuration as CauldronConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
				if (objects[m] is MixingStation && !this.MixStations.Contains(objects[m] as MixingStation))
				{
					MixingStation mixingStation2 = objects[m] as MixingStation;
					this.MixStations.Add(mixingStation2);
					if ((mixingStation2.Configuration as MixingStationConfiguration).AssignedChemist.SelectedNPC != this.chemist)
					{
						(mixingStation2.Configuration as MixingStationConfiguration).AssignedChemist.SetNPC(this.chemist, false);
					}
				}
			}
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x00091C18 File Offset: 0x0008FE18
		public override bool ShouldSave()
		{
			return this.Home.SelectedObject != null || this.ChemStations.Count > 0 || this.LabOvens.Count > 0 || this.Cauldrons.Count > 0 || base.ShouldSave();
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x00091C70 File Offset: 0x0008FE70
		public override string GetSaveString()
		{
			return new ChemistConfigurationData(this.Home.GetData(), this.Stations.GetData()).GetJson(true);
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x00091C94 File Offset: 0x0008FE94
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
				this.assignedHome.SetAssignedEmployee(this.chemist);
			}
			base.InvokeChanged();
		}

		// Token: 0x04001A6C RID: 6764
		public ObjectField Home;

		// Token: 0x04001A6D RID: 6765
		public ObjectListField Stations;

		// Token: 0x04001A6E RID: 6766
		public List<ChemistryStation> ChemStations = new List<ChemistryStation>();

		// Token: 0x04001A6F RID: 6767
		public List<LabOven> LabOvens = new List<LabOven>();

		// Token: 0x04001A70 RID: 6768
		public List<Cauldron> Cauldrons = new List<Cauldron>();

		// Token: 0x04001A71 RID: 6769
		public List<MixingStation> MixStations = new List<MixingStation>();
	}
}
