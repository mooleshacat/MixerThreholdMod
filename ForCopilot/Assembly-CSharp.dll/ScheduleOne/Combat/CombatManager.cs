using System;
using System.Collections.Generic;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Combat
{
	// Token: 0x02000772 RID: 1906
	public class CombatManager : NetworkSingleton<CombatManager>
	{
		// Token: 0x06003353 RID: 13139 RVA: 0x000D58B4 File Offset: 0x000D3AB4
		[Button]
		public void CreateTestExplosion()
		{
			Vector3 origin = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * 10f;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(10f, out raycastHit, this.ExplosionLayerMask, true, 0f))
			{
				origin = raycastHit.point;
			}
			this.CreateExplosion(origin, ExplosionData.DefaultSmall);
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x000D5924 File Offset: 0x000D3B24
		public void CreateExplosion(Vector3 origin, ExplosionData data)
		{
			int id = UnityEngine.Random.Range(0, int.MaxValue);
			this.CreateExplosion(origin, data, id);
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000D5946 File Offset: 0x000D3B46
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void CreateExplosion(Vector3 origin, ExplosionData data, int id)
		{
			this.RpcWriter___Server_CreateExplosion_2907189355(origin, data, id);
			this.RpcLogic___CreateExplosion_2907189355(origin, data, id);
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000D596C File Offset: 0x000D3B6C
		[ObserversRpc(RunLocally = true)]
		private void Explosion(Vector3 origin, ExplosionData data, int id)
		{
			this.RpcWriter___Observers_Explosion_2907189355(origin, data, id);
			this.RpcLogic___Explosion_2907189355(origin, data, id);
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000D59A8 File Offset: 0x000D3BA8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_CreateExplosion_2907189355));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_Explosion_2907189355));
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000D59FA File Offset: 0x000D3BFA
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Combat.CombatManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000D5A13 File Offset: 0x000D3C13
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000D5A24 File Offset: 0x000D3C24
		private void RpcWriter___Server_CreateExplosion_2907189355(Vector3 origin, ExplosionData data, int id)
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
			writer.WriteVector3(origin);
			writer.Write___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generated(data);
			writer.WriteInt32(id, 1);
			base.SendServerRpc(0U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000D5AEA File Offset: 0x000D3CEA
		private void RpcLogic___CreateExplosion_2907189355(Vector3 origin, ExplosionData data, int id)
		{
			this.Explosion(origin, data, id);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000D5AF8 File Offset: 0x000D3CF8
		private void RpcReader___Server_CreateExplosion_2907189355(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Vector3 origin = PooledReader0.ReadVector3();
			ExplosionData data = GeneratedReaders___Internal.Read___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generateds(PooledReader0);
			int id = PooledReader0.ReadInt32(1);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___CreateExplosion_2907189355(origin, data, id);
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000D5B60 File Offset: 0x000D3D60
		private void RpcWriter___Observers_Explosion_2907189355(Vector3 origin, ExplosionData data, int id)
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
			writer.WriteVector3(origin);
			writer.Write___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generated(data);
			writer.WriteInt32(id, 1);
			base.SendObserversRpc(1U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000D5C35 File Offset: 0x000D3E35
		private void RpcLogic___Explosion_2907189355(Vector3 origin, ExplosionData data, int id)
		{
			if (this.explosionIDs.Contains(id))
			{
				return;
			}
			this.explosionIDs.Add(id);
			Explosion explosion = UnityEngine.Object.Instantiate<Explosion>(this.ExplosionPrefab);
			explosion.Initialize(origin, data);
			UnityEngine.Object.Destroy(explosion.gameObject, 3f);
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x000D5C74 File Offset: 0x000D3E74
		private void RpcReader___Observers_Explosion_2907189355(PooledReader PooledReader0, Channel channel)
		{
			Vector3 origin = PooledReader0.ReadVector3();
			ExplosionData data = GeneratedReaders___Internal.Read___ScheduleOne.Combat.ExplosionDataFishNet.Serializing.Generateds(PooledReader0);
			int id = PooledReader0.ReadInt32(1);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Explosion_2907189355(origin, data, id);
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x000D5CD6 File Offset: 0x000D3ED6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002431 RID: 9265
		public LayerMask MeleeLayerMask;

		// Token: 0x04002432 RID: 9266
		public LayerMask ExplosionLayerMask;

		// Token: 0x04002433 RID: 9267
		public LayerMask RangedWeaponLayerMask;

		// Token: 0x04002434 RID: 9268
		public Explosion ExplosionPrefab;

		// Token: 0x04002435 RID: 9269
		private List<int> explosionIDs = new List<int>();

		// Token: 0x04002436 RID: 9270
		private bool dll_Excuted;

		// Token: 0x04002437 RID: 9271
		private bool dll_Excuted;
	}
}
