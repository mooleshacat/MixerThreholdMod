using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Audio;
using ScheduleOne.EntityFramework;
using ScheduleOne.Interaction;
using ScheduleOne.Tiles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C46 RID: 3142
	public class Sprinkler : GridItem
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06005852 RID: 22610 RVA: 0x00175D6A File Offset: 0x00173F6A
		// (set) Token: 0x06005853 RID: 22611 RVA: 0x00175D72 File Offset: 0x00173F72
		public bool IsSprinkling { get; private set; }

		// Token: 0x06005854 RID: 22612 RVA: 0x00175D7B File Offset: 0x00173F7B
		public void Hovered()
		{
			if (this.isGhost)
			{
				return;
			}
			if (this.CanWater())
			{
				this.IntObj.SetMessage("Activate sprinkler");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x06005855 RID: 22613 RVA: 0x00175DB7 File Offset: 0x00173FB7
		public void Interacted()
		{
			if (this.isGhost)
			{
				return;
			}
			if (!this.CanWater())
			{
				return;
			}
			this.SendWater();
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x00175DD1 File Offset: 0x00173FD1
		private bool CanWater()
		{
			return !this.IsSprinkling;
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x00175DDC File Offset: 0x00173FDC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendWater()
		{
			this.RpcWriter___Server_SendWater_2166136261();
			this.RpcLogic___SendWater_2166136261();
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x00175DEA File Offset: 0x00173FEA
		[ObserversRpc(RunLocally = true)]
		private void Water()
		{
			this.RpcWriter___Observers_Water_2166136261();
			this.RpcLogic___Water_2166136261();
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x00175DF8 File Offset: 0x00173FF8
		public void ApplyWater(float normalizedAmount)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			List<Pot> pots = this.GetPots();
			for (int i = 0; i < pots.Count; i++)
			{
				pots[i].ChangeWaterAmount(pots[i].WaterCapacity * normalizedAmount);
			}
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x00175E40 File Offset: 0x00174040
		protected virtual List<Pot> GetPots()
		{
			List<Pot> list = new List<Pot>();
			Coordinate coord = new Coordinate(this.OriginCoordinate) + Coordinate.RotateCoordinates(new Coordinate(0, 1), (float)this.Rotation);
			Coordinate coord2 = new Coordinate(this.OriginCoordinate) + Coordinate.RotateCoordinates(new Coordinate(1, 1), (float)this.Rotation);
			Tile tile = base.OwnerGrid.GetTile(coord);
			Tile tile2 = base.OwnerGrid.GetTile(coord2);
			if (tile != null && tile2 != null)
			{
				Pot pot = null;
				foreach (GridItem gridItem in tile.BuildableOccupants)
				{
					if (gridItem is Pot)
					{
						pot = (gridItem as Pot);
						break;
					}
				}
				if (pot != null && tile2.BuildableOccupants.Contains(pot))
				{
					list.Add(pot);
				}
			}
			return list;
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x00175F62 File Offset: 0x00174162
		[CompilerGenerated]
		private IEnumerator <Water>g__Routine|15_0()
		{
			if (this.onSprinklerStart != null)
			{
				this.onSprinklerStart.Invoke();
			}
			this.WaterSound.Play();
			for (int j = 0; j < this.WaterParticles.Length; j++)
			{
				this.WaterParticles[j].Play();
			}
			int segments = 5;
			int num;
			for (int i = 0; i < segments; i = num + 1)
			{
				yield return new WaitForSeconds(this.ApplyWaterDelay / (float)segments);
				if (InstanceFinder.IsServer)
				{
					this.ApplyWater(1f / (float)segments);
				}
				num = i;
			}
			yield return new WaitForSeconds(this.ParticleStopDelay);
			for (int k = 0; k < this.WaterParticles.Length; k++)
			{
				this.WaterParticles[k].Stop();
			}
			this.WaterSound.Stop();
			this.IsSprinkling = false;
			yield break;
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x00175F74 File Offset: 0x00174174
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendWater_2166136261));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_Water_2166136261));
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x00175FC6 File Offset: 0x001741C6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.SprinklerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600585F RID: 22623 RVA: 0x00175FDF File Offset: 0x001741DF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x00175FF0 File Offset: 0x001741F0
		private void RpcWriter___Server_SendWater_2166136261()
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
			base.SendServerRpc(8U, writer, channel, 0);
			writer.Store();
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x0017608A File Offset: 0x0017428A
		private void RpcLogic___SendWater_2166136261()
		{
			this.Water();
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x00176094 File Offset: 0x00174294
		private void RpcReader___Server_SendWater_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendWater_2166136261();
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x001760C4 File Offset: 0x001742C4
		private void RpcWriter___Observers_Water_2166136261()
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
			base.SendObserversRpc(9U, writer, channel, 0, false, false, false);
			writer.Store();
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x0017616D File Offset: 0x0017436D
		private void RpcLogic___Water_2166136261()
		{
			if (this.IsSprinkling)
			{
				return;
			}
			this.IsSprinkling = true;
			this.ClickSound.Play();
			base.StartCoroutine(this.<Water>g__Routine|15_0());
		}

		// Token: 0x06005865 RID: 22629 RVA: 0x00176198 File Offset: 0x00174398
		private void RpcReader___Observers_Water_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Water_2166136261();
		}

		// Token: 0x06005866 RID: 22630 RVA: 0x001761C2 File Offset: 0x001743C2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040040CC RID: 16588
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x040040CD RID: 16589
		public ParticleSystem[] WaterParticles;

		// Token: 0x040040CE RID: 16590
		public AudioSourceController ClickSound;

		// Token: 0x040040CF RID: 16591
		public AudioSourceController WaterSound;

		// Token: 0x040040D0 RID: 16592
		[Header("Settings")]
		public float ApplyWaterDelay = 6f;

		// Token: 0x040040D1 RID: 16593
		public float ParticleStopDelay = 2.5f;

		// Token: 0x040040D2 RID: 16594
		public UnityEvent onSprinklerStart;

		// Token: 0x040040D3 RID: 16595
		private bool dll_Excuted;

		// Token: 0x040040D4 RID: 16596
		private bool dll_Excuted;
	}
}
