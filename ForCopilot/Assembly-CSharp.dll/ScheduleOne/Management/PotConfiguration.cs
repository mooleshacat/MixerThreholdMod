using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
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
	// Token: 0x020005A9 RID: 1449
	public class PotConfiguration : EntityConfiguration
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x0600239C RID: 9116 RVA: 0x0009320B File Offset: 0x0009140B
		// (set) Token: 0x0600239D RID: 9117 RVA: 0x00093213 File Offset: 0x00091413
		public Pot Pot { get; protected set; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x0009321C File Offset: 0x0009141C
		// (set) Token: 0x0600239F RID: 9119 RVA: 0x00093224 File Offset: 0x00091424
		public TransitRoute DestinationRoute { get; protected set; }

		// Token: 0x060023A0 RID: 9120 RVA: 0x00093230 File Offset: 0x00091430
		public PotConfiguration(ConfigurationReplicator replicator, IConfigurable configurable, Pot pot) : base(replicator, configurable)
		{
			this.Pot = pot;
			this.Seed = new ItemField(this);
			this.Seed.CanSelectNone = true;
			List<ItemDefinition> options = Singleton<Registry>.Instance.Seeds.Cast<ItemDefinition>().ToList<ItemDefinition>();
			this.Seed.Options = options;
			this.Seed.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			List<ItemDefinition> options2 = Singleton<ManagementUtilities>.Instance.AdditiveDefinitions.Cast<ItemDefinition>().ToList<ItemDefinition>();
			this.Additive1 = new ItemField(this);
			this.Additive1.CanSelectNone = true;
			this.Additive1.Options = options2;
			this.Additive1.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			this.Additive2 = new ItemField(this);
			this.Additive2.CanSelectNone = true;
			this.Additive2.Options = options2;
			this.Additive2.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			this.Additive3 = new ItemField(this);
			this.Additive3.CanSelectNone = true;
			this.Additive3.Options = options2;
			this.Additive3.onItemChanged.AddListener(delegate(ItemDefinition <p0>)
			{
				base.InvokeChanged();
			});
			this.AssignedBotanist = new NPCField(this);
			this.AssignedBotanist.TypeRequirement = typeof(Botanist);
			this.AssignedBotanist.onNPCChanged.AddListener(delegate(NPC <p0>)
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

		// Token: 0x060023A1 RID: 9121 RVA: 0x0009341C File Offset: 0x0009161C
		public override void Reset()
		{
			if (this.AssignedBotanist.SelectedNPC != null)
			{
				((this.AssignedBotanist.SelectedNPC as Botanist).Configuration as BotanistConfiguration).AssignedStations.RemoveItem(this.Pot);
			}
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			base.Reset();
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x00093488 File Offset: 0x00091688
		private void DestinationChanged(BuildableItem item)
		{
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.Destroy();
				this.DestinationRoute = null;
			}
			if (this.Destination.SelectedObject != null)
			{
				this.DestinationRoute = new TransitRoute(this.Pot, this.Destination.SelectedObject as ITransitEntity);
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

		// Token: 0x060023A3 RID: 9123 RVA: 0x000934FF File Offset: 0x000916FF
		public bool DestinationFilter(BuildableItem obj, out string reason)
		{
			reason = "";
			return obj is ITransitEntity && (obj as ITransitEntity).Selectable && obj != this.Pot;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0009352E File Offset: 0x0009172E
		public override void Selected()
		{
			base.Selected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(true);
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0009354A File Offset: 0x0009174A
		public override void Deselected()
		{
			base.Deselected();
			if (this.DestinationRoute != null)
			{
				this.DestinationRoute.SetVisualsActive(false);
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x00093568 File Offset: 0x00091768
		public override bool ShouldSave()
		{
			return this.Seed.SelectedItem != null || this.Additive1.SelectedItem != null || this.Additive2.SelectedItem != null || this.Additive3.SelectedItem != null || this.AssignedBotanist.SelectedNPC != null || this.Destination.SelectedObject != null || base.ShouldSave();
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000935FC File Offset: 0x000917FC
		public override string GetSaveString()
		{
			return new PotConfigurationData(this.Seed.GetData(), this.Additive1.GetData(), this.Additive2.GetData(), this.Additive3.GetData(), this.Destination.GetData()).GetJson(true);
		}

		// Token: 0x04001A9D RID: 6813
		public ItemField Seed;

		// Token: 0x04001A9E RID: 6814
		public ItemField Additive1;

		// Token: 0x04001A9F RID: 6815
		public ItemField Additive2;

		// Token: 0x04001AA0 RID: 6816
		public ItemField Additive3;

		// Token: 0x04001AA1 RID: 6817
		public NPCField AssignedBotanist;

		// Token: 0x04001AA2 RID: 6818
		public ObjectField Destination;
	}
}
