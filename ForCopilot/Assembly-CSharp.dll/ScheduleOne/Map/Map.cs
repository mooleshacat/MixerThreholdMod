using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using ScheduleOne.Persistence;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Map
{
	// Token: 0x02000C76 RID: 3190
	public class Map : Singleton<Map>
	{
		// Token: 0x060059AD RID: 22957 RVA: 0x0017AB90 File Offset: 0x00178D90
		protected override void Awake()
		{
			base.Awake();
			if (!GameManager.IS_TUTORIAL)
			{
				using (IEnumerator enumerator = Enum.GetValues(typeof(EMapRegion)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EMapRegion region = (EMapRegion)enumerator.Current;
						if (this.Regions == null || Array.Find<MapRegionData>(this.Regions, (MapRegionData x) => x.Region == region) == null)
						{
							Console.LogError(string.Format("No region data found for {0}", region), null);
						}
					}
				}
			}
			if (this.TreeBounds != null)
			{
				this.TreeBounds.gameObject.SetActive(false);
			}
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x0017AC64 File Offset: 0x00178E64
		protected override void Start()
		{
			base.Start();
			LevelManager instance = NetworkSingleton<LevelManager>.Instance;
			instance.onRankUp = (Action<FullRank, FullRank>)Delegate.Combine(instance.onRankUp, new Action<FullRank, FullRank>(this.OnRankUp));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.GameLoaded));
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x0017ACB8 File Offset: 0x00178EB8
		protected override void OnDestroy()
		{
			if (Singleton<LoadManager>.InstanceExists)
			{
				Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.GameLoaded));
			}
			base.OnDestroy();
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x0017ACE4 File Offset: 0x00178EE4
		public MapRegionData GetRegionData(EMapRegion region)
		{
			return Array.Find<MapRegionData>(this.Regions, (MapRegionData x) => x.Region == region);
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x0017AD18 File Offset: 0x00178F18
		private void GameLoaded()
		{
			foreach (MapRegionData mapRegionData in this.Regions)
			{
				if (mapRegionData.IsUnlocked)
				{
					mapRegionData.SetUnlocked();
				}
			}
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x0017AD4C File Offset: 0x00178F4C
		private void OnRankUp(FullRank oldRank, FullRank newRank)
		{
			foreach (MapRegionData mapRegionData in this.Regions)
			{
				if (oldRank < mapRegionData.RankRequirement && newRank >= mapRegionData.RankRequirement)
				{
					mapRegionData.SetUnlocked();
					if (!Singleton<LoadManager>.Instance.IsLoading)
					{
						Singleton<RegionUnlockedCanvas>.Instance.QueueUnlocked(mapRegionData.Region);
					}
				}
			}
		}

		// Token: 0x040041D2 RID: 16850
		public MapRegionData[] Regions;

		// Token: 0x040041D3 RID: 16851
		[Header("References")]
		public PoliceStation PoliceStation;

		// Token: 0x040041D4 RID: 16852
		public MedicalCentre MedicalCentre;

		// Token: 0x040041D5 RID: 16853
		public Transform TreeBounds;
	}
}
