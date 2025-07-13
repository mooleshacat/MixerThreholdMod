using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Levelling;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C71 RID: 3185
	[Serializable]
	public class MapRegionData
	{
		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06005994 RID: 22932 RVA: 0x0017A5D7 File Offset: 0x001787D7
		public bool IsUnlocked
		{
			get
			{
				return NetworkSingleton<LevelManager>.InstanceExists && NetworkSingleton<LevelManager>.Instance.GetFullRank() >= this.RankRequirement;
			}
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x0017A5F8 File Offset: 0x001787F8
		public DeliveryLocation GetRandomUnscheduledDeliveryLocation()
		{
			List<DeliveryLocation> list = (from x in this.RegionDeliveryLocations
			where x.ScheduledContracts.Count == 0
			select x).ToList<DeliveryLocation>();
			if (list.Count == 0)
			{
				Console.LogWarning("No unscheduled delivery locations found for " + this.Region.ToString(), null);
				return null;
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x06005996 RID: 22934 RVA: 0x0017A674 File Offset: 0x00178874
		public void SetUnlocked()
		{
			foreach (NPC npc in this.StartingNPCs)
			{
				if (!npc.RelationData.Unlocked)
				{
					npc.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, false);
				}
			}
		}

		// Token: 0x040041B0 RID: 16816
		public EMapRegion Region;

		// Token: 0x040041B1 RID: 16817
		public string Name;

		// Token: 0x040041B2 RID: 16818
		public FullRank RankRequirement;

		// Token: 0x040041B3 RID: 16819
		public NPC[] StartingNPCs;

		// Token: 0x040041B4 RID: 16820
		public Sprite RegionSprite;

		// Token: 0x040041B5 RID: 16821
		public DeliveryLocation[] RegionDeliveryLocations;
	}
}
