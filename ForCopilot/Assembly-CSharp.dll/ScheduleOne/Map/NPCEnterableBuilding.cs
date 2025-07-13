using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Doors;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000C7B RID: 3195
	[DisallowMultipleComponent]
	public class NPCEnterableBuilding : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060059BF RID: 22975 RVA: 0x0017AE82 File Offset: 0x00179082
		// (set) Token: 0x060059C0 RID: 22976 RVA: 0x0017AE8A File Offset: 0x0017908A
		public Guid GUID { get; protected set; }

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x060059C1 RID: 22977 RVA: 0x0017AE93 File Offset: 0x00179093
		public int OccupantCount
		{
			get
			{
				return this.Occupants.Count;
			}
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x0017AEA0 File Offset: 0x001790A0
		protected virtual void Awake()
		{
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
			if (this.Doors.Length == 0)
			{
				this.GetDoors();
				if (this.Doors.Length == 0)
				{
					Console.LogError(this.BuildingName + " has no doors! NPCs won't be able to enter the building.", null);
				}
			}
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x0017AF1A File Offset: 0x0017911A
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x0017AF2C File Offset: 0x0017912C
		public virtual void NPCEnteredBuilding(NPC npc)
		{
			if (!this.Occupants.Contains(npc))
			{
				this.Occupants.Add(npc);
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, npc.Movement.FootPosition) > 15f)
			{
				return;
			}
			AudioSourceController audioSourceController = UnityEngine.Object.Instantiate<AudioSourceController>(Singleton<AudioManager>.Instance.DoorOpen, NetworkSingleton<GameManager>.Instance.Temp.transform);
			audioSourceController.transform.position = npc.Avatar.transform.position;
			audioSourceController.Play();
			UnityEngine.Object.Destroy(audioSourceController.gameObject, audioSourceController.AudioSource.clip.length);
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x0017AFE0 File Offset: 0x001791E0
		public virtual void NPCExitedBuilding(NPC npc)
		{
			this.Occupants.Remove(npc);
			if (!PlayerSingleton<PlayerCamera>.InstanceExists || Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, npc.Avatar.transform.position) > 15f)
			{
				return;
			}
			if (!Singleton<AudioManager>.InstanceExists)
			{
				return;
			}
			if (!NetworkSingleton<GameManager>.InstanceExists)
			{
				return;
			}
			AudioSourceController audioSourceController = UnityEngine.Object.Instantiate<AudioSourceController>(Singleton<AudioManager>.Instance.DoorClose, NetworkSingleton<GameManager>.Instance.Temp.transform);
			audioSourceController.Play();
			UnityEngine.Object.Destroy(audioSourceController.gameObject, audioSourceController.AudioSource.clip.length);
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x0017B07D File Offset: 0x0017927D
		[Button]
		public void GetDoors()
		{
			this.Doors = base.GetComponentsInChildren<StaticDoor>();
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x0017B08B File Offset: 0x0017928B
		public List<NPC> GetSummonableNPCs()
		{
			return (from npc in this.Occupants
			where npc.CanBeSummoned
			select npc).ToList<NPC>();
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x0017B0BC File Offset: 0x001792BC
		public StaticDoor GetClosestDoor(Vector3 pos, bool useableOnly)
		{
			return (from door in this.Doors
			where !useableOnly || door.Usable
			orderby Vector3.Distance(door.transform.position, pos)
			select door).FirstOrDefault<StaticDoor>();
		}

		// Token: 0x040041DD RID: 16861
		public const float DOOR_SOUND_DISTANCE_LIMIT = 15f;

		// Token: 0x040041DF RID: 16863
		[Header("Settings")]
		public string BuildingName;

		// Token: 0x040041E0 RID: 16864
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x040041E1 RID: 16865
		[Header("References")]
		public StaticDoor[] Doors;

		// Token: 0x040041E2 RID: 16866
		[Header("Readonly")]
		[SerializeField]
		private List<NPC> Occupants = new List<NPC>();
	}
}
