using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x020006A3 RID: 1699
	[CreateAssetMenu(fileName = "CustomerData", menuName = "ScriptableObjects/CustomerData", order = 1)]
	[Serializable]
	public class CustomerData : ScriptableObject
	{
		// Token: 0x06002E1B RID: 11803 RVA: 0x000C01EC File Offset: 0x000BE3EC
		public static float GetQualityScalar(EQuality quality)
		{
			switch (quality)
			{
			case EQuality.Trash:
				return 0f;
			case EQuality.Poor:
				return 0.25f;
			case EQuality.Standard:
				return 0.5f;
			case EQuality.Premium:
				return 0.75f;
			case EQuality.Heavenly:
				return 1f;
			default:
				return 0f;
			}
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000C0238 File Offset: 0x000BE438
		public List<EDay> GetOrderDays(float dependence, float normalizedRelationship)
		{
			float t = Mathf.Max(dependence, normalizedRelationship);
			int num = Mathf.RoundToInt(Mathf.Lerp((float)this.MinOrdersPerWeek, (float)this.MaxOrdersPerWeek, t));
			int preferredOrderDay = (int)this.PreferredOrderDay;
			int num2 = Mathf.RoundToInt(7f / (float)num);
			num2 = Mathf.Max(num2, 1);
			List<EDay> list = new List<EDay>();
			for (int i = 0; i < 7; i += num2)
			{
				list.Add((EDay)((i + preferredOrderDay) % 7));
			}
			return list;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000C02AA File Offset: 0x000BE4AA
		public float GetAdjustedWeeklySpend(float normalizedRelationship)
		{
			return Mathf.Lerp(this.MinWeeklySpend, this.MaxWeeklySpend, normalizedRelationship) * LevelManager.GetOrderLimitMultiplier(NetworkSingleton<LevelManager>.Instance.GetFullRank());
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000C02D0 File Offset: 0x000BE4D0
		[Button]
		public void RandomizeAffinities()
		{
			this.DefaultAffinityData = new CustomerAffinityData();
			List<EDrugType> list = Enum.GetValues(typeof(EDrugType)).Cast<EDrugType>().ToList<EDrugType>();
			for (int i = 0; i < list.Count; i++)
			{
				this.DefaultAffinityData.ProductAffinities.Add(new ProductTypeAffinity
				{
					DrugType = list[i],
					Affinity = 0f
				});
			}
			for (int j = 0; j < this.DefaultAffinityData.ProductAffinities.Count; j++)
			{
				this.DefaultAffinityData.ProductAffinities[j].Affinity = UnityEngine.Random.Range(-1f, 1f);
			}
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000C0380 File Offset: 0x000BE580
		[Button]
		public void RandomizeProperties()
		{
			string[] array = new string[]
			{
				"Properties/Tier1",
				"Properties/Tier2",
				"Properties/Tier3",
				"Properties/Tier4",
				"Properties/Tier5"
			};
			List<Property> list = new List<Property>();
			foreach (string path in array)
			{
				list.AddRange(Resources.LoadAll<Property>(path));
			}
			this.PreferredProperties.Clear();
			for (int j = 0; j < 3; j++)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				this.PreferredProperties.Add(list[index]);
				list.RemoveAt(index);
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000C0424 File Offset: 0x000BE624
		[Button]
		public void RandomizeTiming()
		{
			this.PreferredOrderDay = (EDay)UnityEngine.Random.Range(0, 7);
			int num = UnityEngine.Random.Range(420, 1440);
			num = Mathf.RoundToInt((float)num / 15f) * 15;
			this.OrderTime = TimeManager.Get24HourTimeFromMinSum(num);
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000C046B File Offset: 0x000BE66B
		[Button]
		public void ClearInvalid()
		{
			while (this.DefaultAffinityData.ProductAffinities.Count > 3)
			{
				this.DefaultAffinityData.ProductAffinities.RemoveAt(this.DefaultAffinityData.ProductAffinities.Count - 1);
			}
		}

		// Token: 0x0400208B RID: 8331
		public CustomerAffinityData DefaultAffinityData;

		// Token: 0x0400208C RID: 8332
		[Header("Preferred Properties - Properties the customer prefers in a product.")]
		public List<Property> PreferredProperties = new List<Property>();

		// Token: 0x0400208D RID: 8333
		[Header("Spending Behaviour")]
		public float MinWeeklySpend = 200f;

		// Token: 0x0400208E RID: 8334
		public float MaxWeeklySpend = 500f;

		// Token: 0x0400208F RID: 8335
		[Range(0f, 7f)]
		public int MinOrdersPerWeek = 1;

		// Token: 0x04002090 RID: 8336
		[Range(0f, 7f)]
		public int MaxOrdersPerWeek = 5;

		// Token: 0x04002091 RID: 8337
		[Header("Timing Settings")]
		public int OrderTime = 1200;

		// Token: 0x04002092 RID: 8338
		public EDay PreferredOrderDay;

		// Token: 0x04002093 RID: 8339
		[Header("Standards")]
		public ECustomerStandard Standards = ECustomerStandard.Moderate;

		// Token: 0x04002094 RID: 8340
		[Header("Direct approaching")]
		public bool CanBeDirectlyApproached = true;

		// Token: 0x04002095 RID: 8341
		public bool GuaranteeFirstSampleSuccess;

		// Token: 0x04002096 RID: 8342
		[Tooltip("The average relationship of mutual customers to provide a 50% chance of success")]
		[Range(0f, 5f)]
		public float MinMutualRelationRequirement = 3f;

		// Token: 0x04002097 RID: 8343
		[Tooltip("The average relationship of mutual customers to provide a 100% chance of success")]
		[Range(0f, 5f)]
		public float MaxMutualRelationRequirement = 5f;

		// Token: 0x04002098 RID: 8344
		[Tooltip("If direct approach fails, whats the chance the police will be called?")]
		[Range(0f, 1f)]
		public float CallPoliceChance = 0.5f;

		// Token: 0x04002099 RID: 8345
		[Header("Dependence")]
		[Tooltip("How quickly the customer builds dependence")]
		[Range(0f, 2f)]
		public float DependenceMultiplier = 1f;

		// Token: 0x0400209A RID: 8346
		[Tooltip("The customer's starting (and lowest possible) dependence level")]
		[Range(0f, 1f)]
		public float BaseAddiction;

		// Token: 0x0400209B RID: 8347
		public Action onChanged;
	}
}
