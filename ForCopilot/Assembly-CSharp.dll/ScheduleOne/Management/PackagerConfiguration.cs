using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Employees;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Management;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005A7 RID: 1447
	public class PackagerConfiguration : EntityConfiguration
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000929C6 File Offset: 0x00090BC6
		public int AssignedStationCount
		{
			get
			{
				return this.AssignedStations.Count + this.AssignedBrickPresses.Count;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x000929DF File Offset: 0x00090BDF
		// (set) Token: 0x06002382 RID: 9090 RVA: 0x000929E7 File Offset: 0x00090BE7
		public Packager packager { get; protected set; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002383 RID: 9091 RVA: 0x000929F0 File Offset: 0x00090BF0
		// (set) Token: 0x06002384 RID: 9092 RVA: 0x000929F8 File Offset: 0x00090BF8
		public EmployeeHome assignedHome { get; private set; }

		// Token: 0x06002385 RID: 9093 RVA: 0x00092A04 File Offset: 0x00090C04
		public PackagerConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Packager _botanist) : base(replicator, configurable)
		{
			this.packager = _botanist;
			this.Home = new ObjectField(this);
			this.Home.onObjectChanged.AddListener(new UnityAction<BuildableItem>(this.HomeChanged));
			this.Home.objectFilter = new ObjectSelector.ObjectFilter(EmployeeHome.IsBuildableEntityAValidEmployeeHome);
			this.Stations = new ObjectListField(this);
			this.Stations.MaxItems = this.packager.MaxAssignedStations;
			this.Stations.TypeRequirements = new List<Type>
			{
				typeof(PackagingStation),
				typeof(PackagingStationMk2),
				typeof(BrickPress)
			};
			this.Stations.onListChanged.AddListener(delegate(List<BuildableItem> <p0>)
			{
				base.InvokeChanged();
			});
			this.Stations.onListChanged.AddListener(new UnityAction<List<BuildableItem>>(this.AssignedStationsChanged));
			this.Stations.objectFilter = new ObjectSelector.ObjectFilter(this.IsStationValid);
			this.Routes = new RouteListField(this);
			this.Routes.MaxRoutes = 5;
			this.Routes.onListChanged.AddListener(delegate(List<AdvancedTransitRoute> <p0>)
			{
				base.InvokeChanged();
			});
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x00092B5C File Offset: 0x00090D5C
		public override void Reset()
		{
			this.Home.SetObject(null, false);
			foreach (PackagingStation packagingStation in this.AssignedStations)
			{
				(packagingStation.Configuration as PackagingStationConfiguration).AssignedPackager.SetNPC(null, false);
			}
			foreach (BrickPress brickPress in this.AssignedBrickPresses)
			{
				(brickPress.Configuration as BrickPressConfiguration).AssignedPackager.SetNPC(null, false);
			}
			base.Reset();
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x00092C20 File Offset: 0x00090E20
		private bool IsStationValid(BuildableItem obj, out string reason)
		{
			reason = string.Empty;
			if (obj is PackagingStation)
			{
				PackagingStationConfiguration packagingStationConfiguration = (obj as PackagingStation).Configuration as PackagingStationConfiguration;
				if (packagingStationConfiguration.AssignedPackager.SelectedNPC != null && packagingStationConfiguration.AssignedPackager.SelectedNPC != this.packager)
				{
					reason = "Already assigned to " + packagingStationConfiguration.AssignedPackager.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
			else
			{
				if (!(obj is BrickPress))
				{
					return false;
				}
				BrickPressConfiguration brickPressConfiguration = (obj as BrickPress).Configuration as BrickPressConfiguration;
				if (brickPressConfiguration.AssignedPackager.SelectedNPC != null && brickPressConfiguration.AssignedPackager.SelectedNPC != this.packager)
				{
					reason = "Already assigned to " + brickPressConfiguration.AssignedPackager.SelectedNPC.fullName;
					return false;
				}
				return true;
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x00092D00 File Offset: 0x00090F00
		public void AssignedStationsChanged(List<BuildableItem> objects)
		{
			for (int i = 0; i < this.AssignedStations.Count; i++)
			{
				if (!objects.Contains(this.AssignedStations[i]))
				{
					PackagingStation packagingStation = this.AssignedStations[i];
					this.AssignedStations.RemoveAt(i);
					i--;
					if ((packagingStation.Configuration as PackagingStationConfiguration).AssignedPackager.SelectedNPC == this.packager)
					{
						(packagingStation.Configuration as PackagingStationConfiguration).AssignedPackager.SetNPC(null, false);
					}
				}
			}
			for (int j = 0; j < this.AssignedBrickPresses.Count; j++)
			{
				if (!objects.Contains(this.AssignedBrickPresses[j]))
				{
					BrickPress brickPress = this.AssignedBrickPresses[j];
					this.AssignedBrickPresses.RemoveAt(j);
					j--;
					if ((brickPress.Configuration as BrickPressConfiguration).AssignedPackager.SelectedNPC == this.packager)
					{
						(brickPress.Configuration as BrickPressConfiguration).AssignedPackager.SetNPC(null, false);
					}
				}
			}
			for (int k = 0; k < objects.Count; k++)
			{
				if (objects[k] is PackagingStation)
				{
					if (!this.AssignedStations.Contains(objects[k]))
					{
						PackagingStation packagingStation2 = objects[k] as PackagingStation;
						this.AssignedStations.Add(packagingStation2);
						if ((packagingStation2.Configuration as PackagingStationConfiguration).AssignedPackager.SelectedNPC != this.packager)
						{
							(packagingStation2.Configuration as PackagingStationConfiguration).AssignedPackager.SetNPC(this.packager, false);
						}
					}
				}
				else if (objects[k] is BrickPress && !this.AssignedBrickPresses.Contains(objects[k]))
				{
					BrickPress brickPress2 = objects[k] as BrickPress;
					this.AssignedBrickPresses.Add(brickPress2);
					if ((brickPress2.Configuration as BrickPressConfiguration).AssignedPackager.SelectedNPC != this.packager)
					{
						(brickPress2.Configuration as BrickPressConfiguration).AssignedPackager.SetNPC(this.packager, false);
					}
				}
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x00092F33 File Offset: 0x00091133
		public override bool ShouldSave()
		{
			return this.Home.SelectedObject != null || this.AssignedStations.Count > 0 || base.ShouldSave();
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x00092F60 File Offset: 0x00091160
		public override string GetSaveString()
		{
			return new PackagerConfigurationData(this.Home.GetData(), this.Stations.GetData(), this.Routes.GetData()).GetJson(true);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x00092F90 File Offset: 0x00091190
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
				this.assignedHome.SetAssignedEmployee(this.packager);
			}
			base.InvokeChanged();
		}

		// Token: 0x04001A91 RID: 6801
		public ObjectField Home;

		// Token: 0x04001A92 RID: 6802
		public ObjectListField Stations;

		// Token: 0x04001A93 RID: 6803
		public RouteListField Routes;

		// Token: 0x04001A94 RID: 6804
		public List<PackagingStation> AssignedStations = new List<PackagingStation>();

		// Token: 0x04001A95 RID: 6805
		public List<BrickPress> AssignedBrickPresses = new List<BrickPress>();
	}
}
