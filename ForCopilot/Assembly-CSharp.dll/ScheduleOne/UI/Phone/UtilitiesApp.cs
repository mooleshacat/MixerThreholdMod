using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Property;
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.Property.Utilities.Water;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000ADD RID: 2781
	public class UtilitiesApp : App<UtilitiesApp>
	{
		// Token: 0x06004A91 RID: 19089 RVA: 0x001396CC File Offset: 0x001378CC
		protected override void Awake()
		{
			base.Awake();
			this.water_Cost.text = "Cost per litre: $" + WaterManager.pricePerL.ToString();
			this.electricity_Cost.text = "Cost per kWh $" + PowerManager.pricePerkWh.ToString();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.RefreshShownValues));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Combine(instance2.onDayPass, new Action(this.OnDayPass));
			PropertyDropdown propertyDropdown = this.propertySelector;
			propertyDropdown.onSelectionChanged = (Action)Delegate.Combine(propertyDropdown.onSelectionChanged, new Action(this.RefreshShownValues));
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x00139794 File Offset: 0x00137994
		protected override void OnDestroy()
		{
			base.OnDestroy();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.RefreshShownValues));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onDayPass = (Action)Delegate.Remove(instance2.onDayPass, new Action(this.OnDayPass));
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x001397F5 File Offset: 0x001379F5
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				this.selectedProperty = this.propertySelector.selectedProperty;
			}
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x00139818 File Offset: 0x00137A18
		protected virtual void RefreshShownValues()
		{
			if (!base.isOpen)
			{
				return;
			}
			this.selectedProperty = this.propertySelector.selectedProperty;
			this.water_Usage.text = "Water usage: " + this.Round(Singleton<WaterManager>.Instance.GetTotalUsage(), 1f).ToString() + " litres";
			this.water_Total.text = "Total cost: " + MoneyManager.FormatAmount(this.Round(Singleton<WaterManager>.Instance.GetTotalUsage() * WaterManager.pricePerL, 2f), true, false);
			this.electricity_Usage.text = "Electricity usage: " + this.Round(Singleton<PowerManager>.Instance.GetTotalUsage(), 2f).ToString() + " kWh";
			this.electricity_Total.text = "Total cost: " + MoneyManager.FormatAmount(this.Round(Singleton<PowerManager>.Instance.GetTotalUsage() * PowerManager.pricePerkWh, 2f), true, false);
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void OnDayPass()
		{
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0013991B File Offset: 0x00137B1B
		private float Round(float n, float decimals)
		{
			return Mathf.Round(n * Mathf.Pow(10f, decimals)) / Mathf.Pow(10f, decimals);
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0013993B File Offset: 0x00137B3B
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (open)
			{
				this.RefreshShownValues();
			}
		}

		// Token: 0x040036EE RID: 14062
		[Header("References")]
		[SerializeField]
		protected Text water_Usage;

		// Token: 0x040036EF RID: 14063
		[SerializeField]
		protected Text water_Cost;

		// Token: 0x040036F0 RID: 14064
		[SerializeField]
		protected Text water_Total;

		// Token: 0x040036F1 RID: 14065
		[SerializeField]
		protected Text electricity_Usage;

		// Token: 0x040036F2 RID: 14066
		[SerializeField]
		protected Text electricity_Cost;

		// Token: 0x040036F3 RID: 14067
		[SerializeField]
		protected Text electricity_Total;

		// Token: 0x040036F4 RID: 14068
		[SerializeField]
		protected Text dumpster_Count;

		// Token: 0x040036F5 RID: 14069
		[SerializeField]
		protected Text dumpster_EmptyCost;

		// Token: 0x040036F6 RID: 14070
		[SerializeField]
		protected Text dumpster_Total;

		// Token: 0x040036F7 RID: 14071
		[SerializeField]
		protected Button dumpsterButton;

		// Token: 0x040036F8 RID: 14072
		[SerializeField]
		protected PropertyDropdown propertySelector;

		// Token: 0x040036F9 RID: 14073
		private Property selectedProperty;
	}
}
