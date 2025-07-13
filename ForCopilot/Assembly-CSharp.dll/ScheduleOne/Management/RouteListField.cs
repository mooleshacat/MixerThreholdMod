using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005B3 RID: 1459
	public class RouteListField : ConfigField
	{
		// Token: 0x060023E5 RID: 9189 RVA: 0x00093EC6 File Offset: 0x000920C6
		public RouteListField(EntityConfiguration parentConfig) : base(parentConfig)
		{
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x00093EEC File Offset: 0x000920EC
		public void SetList(List<AdvancedTransitRoute> list, bool network, bool bypassSequenceCheck = false)
		{
			if (this.Routes.SequenceEqual(list) && !bypassSequenceCheck)
			{
				return;
			}
			this.Routes = new List<AdvancedTransitRoute>();
			this.Routes.AddRange(list);
			if (network)
			{
				base.ParentConfig.ReplicateField(this, null);
			}
			if (this.onListChanged != null)
			{
				this.onListChanged.Invoke(list);
			}
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x00093F46 File Offset: 0x00092146
		public void Replicate()
		{
			Console.Log("Replicating route list field", null);
			this.SetList(this.Routes, true, true);
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00093F64 File Offset: 0x00092164
		public void AddItem(AdvancedTransitRoute item)
		{
			if (this.Routes.Contains(item))
			{
				return;
			}
			if (this.Routes.Count >= this.MaxRoutes)
			{
				Console.LogWarning("Route cannot be added to " + base.ParentConfig.GetType().Name + " because the maximum number of routes has been reached", null);
				return;
			}
			this.SetList(new List<AdvancedTransitRoute>(this.Routes)
			{
				item
			}, true, false);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x00093FD8 File Offset: 0x000921D8
		public void RemoveItem(AdvancedTransitRoute item)
		{
			if (!this.Routes.Contains(item))
			{
				return;
			}
			List<AdvancedTransitRoute> list = new List<AdvancedTransitRoute>(this.Routes);
			list.Remove(item);
			this.SetList(list, true, false);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x00094011 File Offset: 0x00092211
		public override bool IsValueDefault()
		{
			return this.Routes.Count == 0;
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00094024 File Offset: 0x00092224
		public RouteListData GetData()
		{
			List<AdvancedTransitRouteData> list = new List<AdvancedTransitRouteData>();
			for (int i = 0; i < this.Routes.Count; i++)
			{
				list.Add(this.Routes[i].GetData());
			}
			return new RouteListData(list);
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x0009406C File Offset: 0x0009226C
		public void Load(RouteListData data)
		{
			if (data != null)
			{
				List<AdvancedTransitRoute> list = new List<AdvancedTransitRoute>();
				for (int i = 0; i < data.Routes.Count; i++)
				{
					if (string.IsNullOrEmpty(data.Routes[i].SourceGUID) || string.IsNullOrEmpty(data.Routes[i].DestinationGUID))
					{
						Console.LogWarning("Route data is missing source or destination GUID", null);
					}
					else
					{
						ITransitEntity source = null;
						ITransitEntity destination = null;
						try
						{
							source = GUIDManager.GetObject<ITransitEntity>(new Guid(data.Routes[i].SourceGUID));
							destination = GUIDManager.GetObject<ITransitEntity>(new Guid(data.Routes[i].DestinationGUID));
						}
						catch (Exception ex)
						{
							Console.LogError("Error loading route: " + ex.Message, null);
							goto IL_175;
						}
						AdvancedTransitRoute advancedTransitRoute = new AdvancedTransitRoute(source, destination);
						advancedTransitRoute.Filter.SetMode(data.Routes[i].FilterMode);
						for (int j = 0; j < data.Routes[i].FilterItemIDs.Count; j++)
						{
							ItemDefinition @object = GUIDManager.GetObject<ItemDefinition>(new Guid(data.Routes[i].FilterItemIDs[j]));
							if (@object == null)
							{
								Console.LogWarning("Could not find item definition with GUID " + data.Routes[i].FilterItemIDs[j], null);
							}
							else if (@object != null)
							{
								advancedTransitRoute.Filter.AddItem(@object);
							}
						}
						list.Add(advancedTransitRoute);
					}
					IL_175:;
				}
				this.SetList(list, true, false);
			}
		}

		// Token: 0x04001ACA RID: 6858
		public List<AdvancedTransitRoute> Routes = new List<AdvancedTransitRoute>();

		// Token: 0x04001ACB RID: 6859
		public int MaxRoutes = 1;

		// Token: 0x04001ACC RID: 6860
		public UnityEvent<List<AdvancedTransitRoute>> onListChanged = new UnityEvent<List<AdvancedTransitRoute>>();
	}
}
