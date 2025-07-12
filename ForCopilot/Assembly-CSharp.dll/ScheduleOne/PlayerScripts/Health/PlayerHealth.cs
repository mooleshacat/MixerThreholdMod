using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerScripts.Health
{
	// Token: 0x02000644 RID: 1604
	public class PlayerHealth : NetworkBehaviour
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x000AB6FA File Offset: 0x000A98FA
		// (set) Token: 0x0600297B RID: 10619 RVA: 0x000AB702 File Offset: 0x000A9902
		public bool IsAlive { get; protected set; } = true;

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x0600297C RID: 10620 RVA: 0x000AB70B File Offset: 0x000A990B
		// (set) Token: 0x0600297D RID: 10621 RVA: 0x000AB713 File Offset: 0x000A9913
		public float CurrentHealth { get; protected set; } = 100f;

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x0600297E RID: 10622 RVA: 0x000AB71C File Offset: 0x000A991C
		// (set) Token: 0x0600297F RID: 10623 RVA: 0x000AB724 File Offset: 0x000A9924
		public float TimeSinceLastDamage { get; protected set; }

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x000AB72D File Offset: 0x000A992D
		public bool CanTakeDamage
		{
			get
			{
				return this.IsAlive && !Player.Local.IsArrested && !Player.Local.IsUnconscious;
			}
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000AB752 File Offset: 0x000A9952
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.Health.PlayerHealth_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000AB768 File Offset: 0x000A9968
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000AB7C4 File Offset: 0x000A99C4
		[ObserversRpc]
		public void TakeDamage(float damage, bool flinch = true, bool playBloodMist = true)
		{
			this.RpcWriter___Observers_TakeDamage_3505310624(damage, flinch, playBloodMist);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000AB7E3 File Offset: 0x000A99E3
		private void Update()
		{
			this.TimeSinceLastDamage += Time.deltaTime;
			if (this.IsAlive && this.AfflictedWithLethalEffect)
			{
				this.TakeDamage(15f * Time.deltaTime, false, false);
			}
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000AB81A File Offset: 0x000A9A1A
		private void MinPass()
		{
			if (this.IsAlive && this.CurrentHealth < 100f && this.TimeSinceLastDamage > 30f)
			{
				this.RecoverHealth(0.5f);
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000AB849 File Offset: 0x000A9A49
		public void SetAfflictedWithLethalEffect(bool value)
		{
			this.AfflictedWithLethalEffect = value;
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000AB854 File Offset: 0x000A9A54
		public void RecoverHealth(float recovery)
		{
			if (this.CurrentHealth == 0f)
			{
				Console.LogWarning("RecoverHealth called on dead player. Use Revive() instead.", null);
				return;
			}
			this.CurrentHealth = Mathf.Clamp(this.CurrentHealth + recovery, 0f, 100f);
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x000AB8B0 File Offset: 0x000A9AB0
		public void SetHealth(float health)
		{
			this.CurrentHealth = Mathf.Clamp(health, 0f, 100f);
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
			if (this.CurrentHealth <= 0f)
			{
				this.SendDie();
			}
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000AB8FF File Offset: 0x000A9AFF
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendDie()
		{
			this.RpcWriter___Server_SendDie_2166136261();
			this.RpcLogic___SendDie_2166136261();
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000AB910 File Offset: 0x000A9B10
		[ObserversRpc(RunLocally = true)]
		public void Die()
		{
			this.RpcWriter___Observers_Die_2166136261();
			this.RpcLogic___Die_2166136261();
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000AB929 File Offset: 0x000A9B29
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendRevive(Vector3 position, Quaternion rotation)
		{
			this.RpcWriter___Server_SendRevive_3848837105(position, rotation);
			this.RpcLogic___SendRevive_3848837105(position, rotation);
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000AB948 File Offset: 0x000A9B48
		[ObserversRpc(RunLocally = true, ExcludeOwner = true)]
		public void Revive(Vector3 position, Quaternion rotation)
		{
			this.RpcWriter___Observers_Revive_3848837105(position, rotation);
			this.RpcLogic___Revive_3848837105(position, rotation);
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000AB971 File Offset: 0x000A9B71
		[ObserversRpc]
		public void PlayBloodMist()
		{
			this.RpcWriter___Observers_PlayBloodMist_2166136261();
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000AB9A0 File Offset: 0x000A9BA0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted = true;
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_TakeDamage_3505310624));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SendDie_2166136261));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Die_2166136261));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendRevive_3848837105));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Revive_3848837105));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_PlayBloodMist_2166136261));
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000ABA48 File Offset: 0x000A9C48
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.Health.PlayerHealthAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000ABA5B File Offset: 0x000A9C5B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000ABA6C File Offset: 0x000A9C6C
		private void RpcWriter___Observers_TakeDamage_3505310624(float damage, bool flinch = true, bool playBloodMist = true)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(damage, 0);
			writer.WriteBoolean(flinch);
			writer.WriteBoolean(playBloodMist);
			base.SendObserversRpc(0U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000ABB44 File Offset: 0x000A9D44
		public void RpcLogic___TakeDamage_3505310624(float damage, bool flinch = true, bool playBloodMist = true)
		{
			if (!this.IsAlive)
			{
				return;
			}
			if (!this.CanTakeDamage)
			{
				Console.LogWarning("Player cannot take damage right now.", null);
				return;
			}
			this.CurrentHealth = Mathf.Clamp(this.CurrentHealth - damage, 0f, 100f);
			Console.Log(damage.ToString() + " damange taken. New health: " + this.CurrentHealth.ToString(), null);
			this.TimeSinceLastDamage = 0f;
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
			if (this.Player.IsOwner)
			{
				if (flinch && PlayerSingleton<PlayerCamera>.InstanceExists)
				{
					PlayerSingleton<PlayerCamera>.Instance.JoltCamera();
				}
				if (this.CurrentHealth <= 0f)
				{
					this.SendDie();
				}
			}
			if (playBloodMist)
			{
				this.PlayBloodMist();
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000ABC14 File Offset: 0x000A9E14
		private void RpcReader___Observers_TakeDamage_3505310624(PooledReader PooledReader0, Channel channel)
		{
			float damage = PooledReader0.ReadSingle(0);
			bool flinch = PooledReader0.ReadBoolean();
			bool playBloodMist = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___TakeDamage_3505310624(damage, flinch, playBloodMist);
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000ABC6C File Offset: 0x000A9E6C
		private void RpcWriter___Server_SendDie_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(1U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000ABD06 File Offset: 0x000A9F06
		public void RpcLogic___SendDie_2166136261()
		{
			this.Die();
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000ABD10 File Offset: 0x000A9F10
		private void RpcReader___Server_SendDie_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDie_2166136261();
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000ABD40 File Offset: 0x000A9F40
		private void RpcWriter___Observers_Die_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(2U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000ABDEC File Offset: 0x000A9FEC
		public void RpcLogic___Die_2166136261()
		{
			if (!this.IsAlive)
			{
				Console.LogWarning("Already dead!", null);
				return;
			}
			this.IsAlive = false;
			Player player = this.Player;
			Debug.Log(((player != null) ? player.ToString() : null) + " died.");
			if (this.onDie != null)
			{
				this.onDie.Invoke();
			}
			Debug.Log("Dead!");
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000ABE54 File Offset: 0x000AA054
		private void RpcReader___Observers_Die_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Die_2166136261();
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000ABE80 File Offset: 0x000AA080
		private void RpcWriter___Server_SendRevive_3848837105(Vector3 position, Quaternion rotation)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			base.SendServerRpc(3U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000ABF39 File Offset: 0x000AA139
		public void RpcLogic___SendRevive_3848837105(Vector3 position, Quaternion rotation)
		{
			this.Revive(position, rotation);
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000ABF44 File Offset: 0x000AA144
		private void RpcReader___Server_SendRevive_3848837105(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendRevive_3848837105(position, rotation);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000ABF98 File Offset: 0x000AA198
		private void RpcWriter___Observers_Revive_3848837105(Vector3 position, Quaternion rotation)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, 1);
			base.SendObserversRpc(4U, writer, channel, 0, false, false, true);
			writer.Store();
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000AC060 File Offset: 0x000AA260
		public void RpcLogic___Revive_3848837105(Vector3 position, Quaternion rotation)
		{
			if (this.IsAlive)
			{
				Console.LogWarning("Revive called on living player. Use RecoverHealth() instead.", null);
				return;
			}
			this.CurrentHealth = 100f;
			this.IsAlive = true;
			if (this.onHealthChanged != null)
			{
				this.onHealthChanged.Invoke(this.CurrentHealth);
			}
			if (this.onRevive != null)
			{
				this.onRevive.Invoke();
			}
			if (base.IsOwner)
			{
				Singleton<HUD>.Instance.canvas.enabled = true;
				Player.Local.Energy.RestoreEnergy();
				PlayerSingleton<PlayerMovement>.Instance.Teleport(position);
				Player.Local.transform.rotation = rotation;
				PlayerSingleton<PlayerCamera>.Instance.ResetRotation();
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000AC10C File Offset: 0x000AA30C
		private void RpcReader___Observers_Revive_3848837105(PooledReader PooledReader0, Channel channel)
		{
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Revive_3848837105(position, rotation);
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000AC160 File Offset: 0x000AA360
		private void RpcWriter___Observers_PlayBloodMist_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = 0;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(5U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000AC209 File Offset: 0x000AA409
		public void RpcLogic___PlayBloodMist_2166136261()
		{
			LayerUtility.SetLayerRecursively(this.BloodParticles.gameObject, LayerMask.NameToLayer("Default"));
			this.BloodParticles.Play();
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000AC230 File Offset: 0x000AA430
		private void RpcReader___Observers_PlayBloodMist_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___PlayBloodMist_2166136261();
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000AC250 File Offset: 0x000AA450
		private void dll()
		{
			Singleton<SleepCanvas>.Instance.onSleepFullyFaded.AddListener(delegate()
			{
				this.SetHealth(100f);
			});
		}

		// Token: 0x04001E1F RID: 7711
		public const float MAX_HEALTH = 100f;

		// Token: 0x04001E20 RID: 7712
		public const float HEALTH_RECOVERY_PER_MINUTE = 0.5f;

		// Token: 0x04001E24 RID: 7716
		[Header("References")]
		public Player Player;

		// Token: 0x04001E25 RID: 7717
		public ParticleSystem BloodParticles;

		// Token: 0x04001E26 RID: 7718
		public UnityEvent<float> onHealthChanged;

		// Token: 0x04001E27 RID: 7719
		public UnityEvent onDie;

		// Token: 0x04001E28 RID: 7720
		public UnityEvent onRevive;

		// Token: 0x04001E29 RID: 7721
		private bool AfflictedWithLethalEffect;

		// Token: 0x04001E2A RID: 7722
		private bool dll_Excuted;

		// Token: 0x04001E2B RID: 7723
		private bool dll_Excuted;
	}
}
