using System;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000493 RID: 1171
	[RequireComponent(typeof(NPCHealth))]
	[DisallowMultipleComponent]
	public class NPCHealth : NetworkBehaviour
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x00069C04 File Offset: 0x00067E04
		// (set) Token: 0x06001800 RID: 6144 RVA: 0x00069C0C File Offset: 0x00067E0C
		public float Health
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Health>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<Health>k__BackingField(value, true);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x00069C16 File Offset: 0x00067E16
		// (set) Token: 0x06001802 RID: 6146 RVA: 0x00069C1E File Offset: 0x00067E1E
		public bool IsDead { get; private set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x00069C27 File Offset: 0x00067E27
		// (set) Token: 0x06001804 RID: 6148 RVA: 0x00069C2F File Offset: 0x00067E2F
		public bool IsKnockedOut { get; private set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x00069C38 File Offset: 0x00067E38
		// (set) Token: 0x06001806 RID: 6150 RVA: 0x00069C40 File Offset: 0x00067E40
		public int DaysPassedSinceDeath { get; private set; }

		// Token: 0x06001807 RID: 6151 RVA: 0x00069C4C File Offset: 0x00067E4C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCHealth_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00069C6B File Offset: 0x00067E6B
		private void OnDestroy()
		{
			TimeManager.onSleepStart = (Action)Delegate.Remove(TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00069C8D File Offset: 0x00067E8D
		public override void OnStartServer()
		{
			base.OnStartServer();
			this.Health = this.MaxHealth;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00069CA1 File Offset: 0x00067EA1
		public void Load(NPCHealthData healthData)
		{
			this.Health = healthData.Health;
			this.DaysPassedSinceDeath = healthData.DaysPassedSinceDeath;
			if (this.IsDead)
			{
				this.Die();
				return;
			}
			if (this.Health == 0f)
			{
				this.KnockOut();
			}
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00069CDD File Offset: 0x00067EDD
		private void Update()
		{
			if (!this.IsDead && this.AfflictedWithLethalEffect)
			{
				this.TakeDamage(15f * Time.deltaTime, true);
			}
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00069D01 File Offset: 0x00067F01
		public void SetAfflictedWithLethalEffect(bool value)
		{
			this.AfflictedWithLethalEffect = value;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00069D0C File Offset: 0x00067F0C
		public void SleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.npc.IsConscious)
			{
				Console.Log(this.npc.fullName + " Dead: " + this.IsDead.ToString(), null);
				if (this.IsDead)
				{
					int daysPassedSinceDeath = this.DaysPassedSinceDeath;
					this.DaysPassedSinceDeath = daysPassedSinceDeath + 1;
					if (this.DaysPassedSinceDeath >= 3 || this.npc.IsImportant)
					{
						this.Revive();
					}
				}
				else
				{
					this.Revive();
				}
			}
			if (this.npc.IsConscious)
			{
				this.Health = this.MaxHealth;
			}
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00069DAC File Offset: 0x00067FAC
		public void TakeDamage(float damage, bool isLethal = true)
		{
			if (this.IsDead)
			{
				return;
			}
			Console.Log(this.npc.fullName + " has taken " + damage.ToString() + " damage.", null);
			this.Health -= damage;
			if (this.Health <= 0f)
			{
				this.Health = 0f;
				if (!this.Invincible)
				{
					if (isLethal)
					{
						if (!this.IsDead)
						{
							this.Die();
							return;
						}
					}
					else if (!this.IsKnockedOut)
					{
						this.KnockOut();
					}
				}
			}
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00069E38 File Offset: 0x00068038
		public virtual void Die()
		{
			if (this.Invincible)
			{
				return;
			}
			Console.Log(this.npc.fullName + " has died.", null);
			this.IsDead = true;
			this.npc.behaviour.DeadBehaviour.Enable_Networked(null);
			if (this.onDie != null)
			{
				this.onDie.Invoke();
			}
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00069E9C File Offset: 0x0006809C
		public virtual void KnockOut()
		{
			if (this.Invincible)
			{
				return;
			}
			Console.Log(this.npc.fullName + " has been knocked out.", null);
			this.IsKnockedOut = true;
			this.npc.behaviour.UnconsciousBehaviour.Enable_Networked(null);
			if (this.onKnockedOut != null)
			{
				this.onKnockedOut.Invoke();
			}
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00069F00 File Offset: 0x00068100
		public virtual void Revive()
		{
			Console.Log(this.npc.fullName + " has been revived.", null);
			this.IsDead = false;
			this.IsKnockedOut = false;
			this.Health = this.MaxHealth;
			this.npc.behaviour.DeadBehaviour.SendDisable();
			this.npc.behaviour.UnconsciousBehaviour.SendDisable();
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00069F80 File Offset: 0x00068180
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Health>k__BackingField = new SyncVar<float>(this, 0U, 1, 0, -1f, 0, this.<Health>k__BackingField);
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.NPCs.NPCHealth));
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00069FDB File Offset: 0x000681DB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCHealthAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Health>k__BackingField.SetRegistered();
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00069FF9 File Offset: 0x000681F9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x0006A007 File Offset: 0x00068207
		// (set) Token: 0x06001817 RID: 6167 RVA: 0x0006A00F File Offset: 0x0006820F
		public float SyncAccessor_<Health>k__BackingField
		{
			get
			{
				return this.<Health>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Health>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Health>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0006A04C File Offset: 0x0006824C
		public override bool NPCHealth(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 0U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<Health>k__BackingField(this.syncVar___<Health>k__BackingField.GetValue(true), true);
				return true;
			}
			float value = PooledReader0.ReadSingle(0);
			this.sync___set_value_<Health>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0006A0A4 File Offset: 0x000682A4
		protected virtual void dll()
		{
			this.npc = base.GetComponent<NPC>();
			TimeManager.onSleepStart = (Action)Delegate.Remove(TimeManager.onSleepStart, new Action(this.SleepStart));
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.SleepStart));
		}

		// Token: 0x0400157D RID: 5501
		public const int REVIVE_DAYS = 3;

		// Token: 0x04001582 RID: 5506
		[Header("Settings")]
		public bool Invincible;

		// Token: 0x04001583 RID: 5507
		public float MaxHealth = 100f;

		// Token: 0x04001584 RID: 5508
		private NPC npc;

		// Token: 0x04001585 RID: 5509
		public UnityEvent onDie;

		// Token: 0x04001586 RID: 5510
		public UnityEvent onKnockedOut;

		// Token: 0x04001587 RID: 5511
		private bool AfflictedWithLethalEffect;

		// Token: 0x04001588 RID: 5512
		public SyncVar<float> syncVar___<Health>k__BackingField;

		// Token: 0x04001589 RID: 5513
		private bool dll_Excuted;

		// Token: 0x0400158A RID: 5514
		private bool dll_Excuted;
	}
}
