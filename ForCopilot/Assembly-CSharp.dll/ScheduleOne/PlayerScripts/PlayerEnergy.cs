using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts.Health;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200062D RID: 1581
	public class PlayerEnergy : MonoBehaviour
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000A6E79 File Offset: 0x000A5079
		// (set) Token: 0x060028A8 RID: 10408 RVA: 0x000A6E81 File Offset: 0x000A5081
		public float CurrentEnergy { get; protected set; } = 100f;

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000A6E8A File Offset: 0x000A508A
		// (set) Token: 0x060028AA RID: 10410 RVA: 0x000A6E92 File Offset: 0x000A5092
		public int EnergyDrinksConsumed { get; protected set; }

		// Token: 0x060028AB RID: 10411 RVA: 0x000A6E9C File Offset: 0x000A509C
		protected virtual void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			Singleton<SleepCanvas>.Instance.onSleepFullyFaded.AddListener(new UnityAction(this.SleepEnd));
			base.GetComponent<PlayerHealth>().onRevive.AddListener(new UnityAction(this.ResetEnergyDrinks));
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000A6F08 File Offset: 0x000A5108
		private void MinPass()
		{
			if (this.DEBUG_DISABLE_ENERGY && (Debug.isDebugBuild || Application.isEditor))
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
			{
				return;
			}
			float num = -(1f / (this.EnergyDuration_Hours * 60f)) * 100f;
			if (PlayerSingleton<PlayerMovement>.Instance.isSprinting)
			{
				num *= 1.3f;
			}
			this.ChangeEnergy(num);
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000A6F6E File Offset: 0x000A516E
		private void ChangeEnergy(float change)
		{
			this.SetEnergy(this.CurrentEnergy + change);
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000A6F80 File Offset: 0x000A5180
		public void SetEnergy(float newEnergy)
		{
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000A6F8D File Offset: 0x000A518D
		public void RestoreEnergy()
		{
			this.SetEnergy(100f);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000A6F9A File Offset: 0x000A519A
		private void SleepEnd()
		{
			this.ResetEnergyDrinks();
			this.RestoreEnergy();
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000A6FA8 File Offset: 0x000A51A8
		public void IncrementEnergyDrinks()
		{
			int energyDrinksConsumed = this.EnergyDrinksConsumed;
			this.EnergyDrinksConsumed = energyDrinksConsumed + 1;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000A6FC5 File Offset: 0x000A51C5
		private void ResetEnergyDrinks()
		{
			this.EnergyDrinksConsumed = 0;
		}

		// Token: 0x04001D41 RID: 7489
		public const float CRITICAL_THRESHOLD = 20f;

		// Token: 0x04001D42 RID: 7490
		public const float MAX_ENERGY = 100f;

		// Token: 0x04001D43 RID: 7491
		public const float SPRINT_DRAIN_MULTIPLIER = 1.3f;

		// Token: 0x04001D46 RID: 7494
		public bool DEBUG_DISABLE_ENERGY;

		// Token: 0x04001D47 RID: 7495
		[Header("Settings")]
		public float EnergyDuration_Hours = 22f;

		// Token: 0x04001D48 RID: 7496
		public float EnergyRechargeTime_Hours = 6f;

		// Token: 0x04001D49 RID: 7497
		public UnityEvent onEnergyChanged;

		// Token: 0x04001D4A RID: 7498
		public UnityEvent onEnergyDepleted;
	}
}
