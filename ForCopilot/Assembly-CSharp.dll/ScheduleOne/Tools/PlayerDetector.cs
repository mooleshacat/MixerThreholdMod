using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x020008A9 RID: 2217
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerDetector : MonoBehaviour
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x000FD79D File Offset: 0x000FB99D
		// (set) Token: 0x06003C28 RID: 15400 RVA: 0x000FD7A5 File Offset: 0x000FB9A5
		public bool IgnoreNewDetections { get; protected set; }

		// Token: 0x06003C29 RID: 15401 RVA: 0x000FD7B0 File Offset: 0x000FB9B0
		private void Awake()
		{
			Rigidbody rigidbody = base.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = base.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
			this.detectionColliders = base.GetComponentsInChildren<Collider>();
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x000FD7EC File Offset: 0x000FB9EC
		private void Start()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onTick = (Action)Delegate.Combine(instance.onTick, new Action(this.MinPass));
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x000FD814 File Offset: 0x000FBA14
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onTick = (Action)Delegate.Remove(instance.onTick, new Action(this.MinPass));
			}
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x000FD844 File Offset: 0x000FBA44
		private void MinPass()
		{
			bool flag = false;
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				if (Vector3.SqrMagnitude(Player.PlayerList[i].Avatar.CenterPoint - base.transform.position) < 400f)
				{
					flag = true;
					break;
				}
			}
			if (flag != this.collidersEnabled)
			{
				this.collidersEnabled = flag;
				for (int j = 0; j < this.detectionColliders.Length; j++)
				{
					this.detectionColliders[j].enabled = this.collidersEnabled;
				}
			}
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x000FD8D4 File Offset: 0x000FBAD4
		private void OnTriggerEnter(Collider other)
		{
			if (this.IgnoreNewDetections)
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && !this.DetectedPlayers.Contains(componentInParent) && other == componentInParent.CapCol)
			{
				this.DetectedPlayers.Add(componentInParent);
				if (this.onPlayerEnter != null)
				{
					this.onPlayerEnter.Invoke(componentInParent);
				}
				if (componentInParent.IsOwner && this.onLocalPlayerEnter != null)
				{
					this.onLocalPlayerEnter.Invoke();
				}
			}
			if (this.DetectPlayerInVehicle)
			{
				LandVehicle componentInParent2 = other.GetComponentInParent<LandVehicle>();
				if (componentInParent2 != null)
				{
					foreach (Player player in componentInParent2.OccupantPlayers)
					{
						if (player != null && !this.DetectedPlayers.Contains(player))
						{
							this.DetectedPlayers.Add(player);
							if (this.onPlayerEnter != null)
							{
								this.onPlayerEnter.Invoke(player);
							}
							if (player.IsOwner && this.onLocalPlayerEnter != null)
							{
								this.onLocalPlayerEnter.Invoke();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x000FDA00 File Offset: 0x000FBC00
		private void FixedUpdate()
		{
			for (int i = 0; i < this.DetectedPlayers.Count; i++)
			{
				if (this.DetectedPlayers[i].CurrentVehicle != null)
				{
					this.OnTriggerExit(this.DetectedPlayers[i].CapCol);
				}
			}
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x000FDA54 File Offset: 0x000FBC54
		private void OnTriggerExit(Collider other)
		{
			if (this.ignoreExit)
			{
				return;
			}
			Player componentInParent = other.GetComponentInParent<Player>();
			if (componentInParent != null && this.DetectedPlayers.Contains(componentInParent) && other == componentInParent.CapCol)
			{
				this.DetectedPlayers.Remove(componentInParent);
				if (this.onPlayerExit != null)
				{
					this.onPlayerExit.Invoke(componentInParent);
				}
				if (componentInParent.IsOwner && this.onLocalPlayerExit != null)
				{
					this.onLocalPlayerExit.Invoke();
				}
			}
			if (this.DetectPlayerInVehicle)
			{
				LandVehicle componentInParent2 = other.GetComponentInParent<LandVehicle>();
				if (componentInParent2 != null)
				{
					foreach (Player player in componentInParent2.OccupantPlayers)
					{
						if (player != null && this.DetectedPlayers.Contains(player))
						{
							this.DetectedPlayers.Remove(player);
							if (this.onPlayerExit != null)
							{
								this.onPlayerExit.Invoke(player);
							}
							if (player.IsOwner && this.onLocalPlayerExit != null)
							{
								this.onLocalPlayerExit.Invoke();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x000FDB84 File Offset: 0x000FBD84
		public void SetIgnoreNewCollisions(bool ignore)
		{
			this.IgnoreNewDetections = ignore;
			if (!ignore)
			{
				this.ignoreExit = true;
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].isTrigger)
					{
						componentsInChildren[i].enabled = false;
						componentsInChildren[i].enabled = true;
					}
				}
				this.ignoreExit = false;
			}
		}

		// Token: 0x04002AED RID: 10989
		public const float ACTIVATION_DISTANCE_SQ = 400f;

		// Token: 0x04002AEE RID: 10990
		public bool DetectPlayerInVehicle;

		// Token: 0x04002AEF RID: 10991
		public UnityEvent<Player> onPlayerEnter;

		// Token: 0x04002AF0 RID: 10992
		public UnityEvent<Player> onPlayerExit;

		// Token: 0x04002AF1 RID: 10993
		public UnityEvent onLocalPlayerEnter;

		// Token: 0x04002AF2 RID: 10994
		public UnityEvent onLocalPlayerExit;

		// Token: 0x04002AF3 RID: 10995
		public List<Player> DetectedPlayers = new List<Player>();

		// Token: 0x04002AF5 RID: 10997
		private bool ignoreExit;

		// Token: 0x04002AF6 RID: 10998
		private bool collidersEnabled = true;

		// Token: 0x04002AF7 RID: 10999
		private Collider[] detectionColliders;
	}
}
