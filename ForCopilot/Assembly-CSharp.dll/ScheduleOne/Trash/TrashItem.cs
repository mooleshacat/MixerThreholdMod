using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.Audio;
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dragging;
using ScheduleOne.Equipping;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000871 RID: 2161
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Draggable))]
	[RequireComponent(typeof(PhysicsDamageable))]
	public class TrashItem : MonoBehaviour, IGUIDRegisterable, ISaveable
	{
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003AF7 RID: 15095 RVA: 0x000F9721 File Offset: 0x000F7921
		// (set) Token: 0x06003AF8 RID: 15096 RVA: 0x000F9729 File Offset: 0x000F7929
		public Guid GUID { get; protected set; }

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x000F9732 File Offset: 0x000F7932
		// (set) Token: 0x06003AFA RID: 15098 RVA: 0x000F973A File Offset: 0x000F793A
		public Property CurrentProperty { get; protected set; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x000F9744 File Offset: 0x000F7944
		public string SaveFolderName
		{
			get
			{
				return "Trash_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06003AFC RID: 15100 RVA: 0x000F9778 File Offset: 0x000F7978
		public string SaveFileName
		{
			get
			{
				return "Trash_" + this.GUID.ToString().Substring(0, 6);
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06003AFE RID: 15102 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06003AFF RID: 15103 RVA: 0x000F97AA File Offset: 0x000F79AA
		// (set) Token: 0x06003B00 RID: 15104 RVA: 0x000F97B2 File Offset: 0x000F79B2
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06003B01 RID: 15105 RVA: 0x000F97BB File Offset: 0x000F79BB
		// (set) Token: 0x06003B02 RID: 15106 RVA: 0x000F97C3 File Offset: 0x000F79C3
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06003B03 RID: 15107 RVA: 0x000F97CC File Offset: 0x000F79CC
		// (set) Token: 0x06003B04 RID: 15108 RVA: 0x000F97D4 File Offset: 0x000F79D4
		public bool HasChanged { get; set; }

		// Token: 0x06003B05 RID: 15109 RVA: 0x000F97E0 File Offset: 0x000F79E0
		protected void Awake()
		{
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Trash"));
			this.RecheckPosition();
			base.InvokeRepeating("RecheckPosition", UnityEngine.Random.Range(0f, 1f), 1f);
			this.SetPhysicsActive(false);
			this.Rigidbody.drag = 0.1f;
			this.Rigidbody.angularDrag = 0.1f;
			this.Rigidbody.interpolation = 1;
			this.Rigidbody.collisionDetectionMode = 0;
			this.Rigidbody.sleepThreshold = 0.01f;
			this.Draggable.onDragStart.AddListener(delegate()
			{
				this.SetContinuousCollisionDetection();
			});
			PhysicsDamageable physicsDamageable = base.GetComponent<PhysicsDamageable>();
			if (physicsDamageable == null)
			{
				physicsDamageable = base.gameObject.AddComponent<PhysicsDamageable>();
			}
			PhysicsDamageable physicsDamageable2 = physicsDamageable;
			physicsDamageable2.onImpacted = (Action<Impact>)Delegate.Combine(physicsDamageable2.onImpacted, new Action<Impact>(delegate(Impact impact)
			{
				if (impact.ImpactForce > 0f)
				{
					this.SetContinuousCollisionDetection();
				}
			}));
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000F98D0 File Offset: 0x000F7AD0
		protected void Start()
		{
			this.InitializeSaveable();
			TimeManager.onSleepEnd = (Action<int>)Delegate.Combine(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			this.Draggable.onHovered.AddListener(new UnityAction(this.Hovered));
			this.Draggable.onInteracted.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x000F9964 File Offset: 0x000F7B64
		protected void OnValidate()
		{
			if (this.Rigidbody == null)
			{
				this.Rigidbody = base.GetComponent<Rigidbody>();
			}
			if (this.Draggable == null)
			{
				this.Draggable = base.GetComponent<Draggable>();
			}
			if (this.colliders == null || this.colliders.Length == 0)
			{
				this.colliders = base.GetComponentsInChildren<Collider>();
			}
			if (base.GetComponent<ImpactSoundEntity>() == null)
			{
				base.gameObject.AddComponent<ImpactSoundEntity>();
			}
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x000F99DC File Offset: 0x000F7BDC
		protected void MinPass()
		{
			if (this == null || base.transform == null)
			{
				return;
			}
			if (Time.time - this.timeOnPhysicsEnabled > 30f)
			{
				float num = Vector3.SqrMagnitude(PlayerSingleton<PlayerMovement>.Instance.transform.position - base.transform.position);
				this.SetCollidersEnabled(num < 900f);
			}
			if (base.transform.position.y < -100f && InstanceFinder.IsServer)
			{
				Console.LogWarning("Trash item fell below the world. Destroying.", null);
				this.DestroyTrash();
			}
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x000045B1 File Offset: 0x000027B1
		protected void SleepEnd(int mins)
		{
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x000F9A78 File Offset: 0x000F7C78
		protected void Hovered()
		{
			if (Equippable_TrashGrabber.IsEquipped && this.CanGoInContainer)
			{
				if (Equippable_TrashGrabber.Instance.GetCapacity() > 0)
				{
					this.Draggable.IntObj.SetMessage("Pick up");
					this.Draggable.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
					return;
				}
				this.Draggable.IntObj.SetMessage("Bin is full");
				this.Draggable.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
			}
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x000F9AEE File Offset: 0x000F7CEE
		protected void Interacted()
		{
			if (Equippable_TrashGrabber.IsEquipped && this.CanGoInContainer && Equippable_TrashGrabber.Instance.GetCapacity() > 0)
			{
				Equippable_TrashGrabber.Instance.PickupTrash(this);
			}
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x000F9B18 File Offset: 0x000F7D18
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
			string text = this.GUID.ToString();
			if (text[text.Length - 1] != '1')
			{
				text = text.Substring(0, text.Length - 1) + "1";
			}
			else
			{
				text = text.Substring(0, text.Length - 1) + "2";
			}
			this.Draggable.SetGUID(new Guid(text));
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x000F9BA0 File Offset: 0x000F7DA0
		public void SetVelocity(Vector3 velocity)
		{
			this.Rigidbody.velocity = velocity;
			this.HasChanged = true;
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x000F9BB5 File Offset: 0x000F7DB5
		public void DestroyTrash()
		{
			NetworkSingleton<TrashManager>.Instance.DestroyTrash(this);
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x000F9BC4 File Offset: 0x000F7DC4
		public virtual void Deinitialize()
		{
			TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x000F9C18 File Offset: 0x000F7E18
		private void OnDestroy()
		{
			TimeManager.onSleepEnd = (Action<int>)Delegate.Remove(TimeManager.onSleepEnd, new Action<int>(this.SleepEnd));
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x000F9C72 File Offset: 0x000F7E72
		private void RecheckPosition()
		{
			if (Vector3.Distance(this.lastPosition, base.transform.position) > 1f)
			{
				this.lastPosition = base.transform.position;
				this.HasChanged = true;
				this.RecheckProperty();
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x000F9CB0 File Offset: 0x000F7EB0
		public virtual TrashItemData GetData()
		{
			return new TrashItemData(this.ID, this.GUID.ToString(), base.transform.position, base.transform.rotation);
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x00092542 File Offset: 0x00090742
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool ShouldSave()
		{
			return true;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x000F9CF4 File Offset: 0x000F7EF4
		private void RecheckProperty()
		{
			if (this.CurrentProperty != null && this.CurrentProperty.DoBoundsContainPoint(base.transform.position))
			{
				return;
			}
			this.CurrentProperty = null;
			for (int i = 0; i < Property.OwnedProperties.Count; i++)
			{
				if (Vector3.Distance(base.transform.position, Property.OwnedProperties[i].BoundingBox.transform.position) <= 25f && Property.OwnedProperties[i].DoBoundsContainPoint(base.transform.position))
				{
					this.CurrentProperty = Property.OwnedProperties[i];
					return;
				}
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x000F9DA4 File Offset: 0x000F7FA4
		public void SetContinuousCollisionDetection()
		{
			this.Rigidbody.collisionDetectionMode = 1;
			this.SetPhysicsActive(true);
			base.CancelInvoke("SetDiscreteCollisionDetection");
			base.Invoke("SetDiscreteCollisionDetection", 60f);
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x000F9DD4 File Offset: 0x000F7FD4
		public void SetDiscreteCollisionDetection()
		{
			if (this.Rigidbody == null)
			{
				return;
			}
			this.SetPhysicsActive(false);
			this.Rigidbody.collisionDetectionMode = 0;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x000F9DF8 File Offset: 0x000F7FF8
		public void SetPhysicsActive(bool active)
		{
			this.Rigidbody.isKinematic = !active;
			this.SetCollidersEnabled(active);
			if (active)
			{
				this.timeOnPhysicsEnabled = Time.time;
			}
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x000F9E20 File Offset: 0x000F8020
		public void SetCollidersEnabled(bool enabled)
		{
			if (this.collidersEnabled == enabled)
			{
				return;
			}
			this.collidersEnabled = enabled;
			for (int i = 0; i < this.colliders.Length; i++)
			{
				this.colliders[i].enabled = true;
			}
			if (!this.collidersEnabled)
			{
				this.Rigidbody.isKinematic = true;
			}
		}

		// Token: 0x04002A3C RID: 10812
		public const float POSITION_CHANGE_THRESHOLD = 1f;

		// Token: 0x04002A3D RID: 10813
		public const float LINEAR_DRAG = 0.1f;

		// Token: 0x04002A3E RID: 10814
		public const float ANGULAR_DRAG = 0.1f;

		// Token: 0x04002A3F RID: 10815
		public const float MIN_Y = -100f;

		// Token: 0x04002A40 RID: 10816
		public const int INTERACTION_PRIORITY = 5;

		// Token: 0x04002A41 RID: 10817
		public Rigidbody Rigidbody;

		// Token: 0x04002A42 RID: 10818
		public Draggable Draggable;

		// Token: 0x04002A43 RID: 10819
		[Header("Settings")]
		public string ID = "trashid";

		// Token: 0x04002A44 RID: 10820
		[Range(0f, 5f)]
		public int Size = 2;

		// Token: 0x04002A45 RID: 10821
		[Range(0f, 10f)]
		public int SellValue = 1;

		// Token: 0x04002A46 RID: 10822
		public bool CanGoInContainer = true;

		// Token: 0x04002A47 RID: 10823
		public Collider[] colliders;

		// Token: 0x04002A4A RID: 10826
		private Vector3 lastPosition = Vector3.zero;

		// Token: 0x04002A4B RID: 10827
		public Action<TrashItem> onDestroyed;

		// Token: 0x04002A4C RID: 10828
		private bool collidersEnabled = true;

		// Token: 0x04002A4D RID: 10829
		private float timeOnPhysicsEnabled;
	}
}
