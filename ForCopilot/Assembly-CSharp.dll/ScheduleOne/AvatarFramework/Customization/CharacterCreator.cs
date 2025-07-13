using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.Clothing;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Networking;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.CharacterCreator;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Customization
{
	// Token: 0x020009E9 RID: 2537
	public class CharacterCreator : Singleton<CharacterCreator>
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06004466 RID: 17510 RVA: 0x0011F739 File Offset: 0x0011D939
		// (set) Token: 0x06004467 RID: 17511 RVA: 0x0011F741 File Offset: 0x0011D941
		public bool IsOpen { get; protected set; }

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06004468 RID: 17512 RVA: 0x0011F74A File Offset: 0x0011D94A
		// (set) Token: 0x06004469 RID: 17513 RVA: 0x0011F752 File Offset: 0x0011D952
		public BasicAvatarSettings ActiveSettings { get; protected set; }

		// Token: 0x0600446A RID: 17514 RVA: 0x0011F75B File Offset: 0x0011D95B
		protected override void Awake()
		{
			if (this.DemoCreator)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.Awake();
			this.Fields = this.Canvas.GetComponentsInChildren<BaseCharacterCreatorField>(true).ToList<BaseCharacterCreatorField>();
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0011F78F File Offset: 0x0011D98F
		protected override void Start()
		{
			base.Start();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0011F7A8 File Offset: 0x0011D9A8
		private void Update()
		{
			this.RigContainer.localEulerAngles = Vector3.Lerp(this.RigContainer.localEulerAngles, new Vector3(0f, this.rigTargetY, 0f), Time.deltaTime * 5f);
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0011F7E8 File Offset: 0x0011D9E8
		public void Open(BasicAvatarSettings initialSettings, bool showUI = true)
		{
			this.IsOpen = true;
			if (showUI)
			{
				this.ShowUI();
			}
			if (!this.DemoCreator)
			{
				PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
				PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0f, false);
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<HUD>.Instance.canvas.enabled = false;
			this.Container.gameObject.SetActive(true);
			if (InstanceFinder.IsServer && !Singleton<Lobby>.Instance.IsInLobby)
			{
				NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = 0f;
			}
			if (initialSettings != null)
			{
				this.ActiveSettings = UnityEngine.Object.Instantiate<BasicAvatarSettings>(initialSettings);
			}
			else
			{
				this.ActiveSettings = ScriptableObject.CreateInstance<BasicAvatarSettings>();
			}
			this.Rig.LoadAvatarSettings(this.ActiveSettings.GetAvatarSettings());
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].ApplyValue();
				this.Fields[i].WriteValue(false);
			}
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0011F911 File Offset: 0x0011DB11
		public void ShowUI()
		{
			this.Canvas.enabled = true;
			this.CanvasAnimation.Play("Character creator fade in");
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0011F94A File Offset: 0x0011DB4A
		public void Close()
		{
			this.IsOpen = false;
			base.StartCoroutine(this.<Close>g__Close|28_0());
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0011F960 File Offset: 0x0011DB60
		public void DisableStuff()
		{
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0011F974 File Offset: 0x0011DB74
		public void Done()
		{
			if (!this.IsOpen)
			{
				return;
			}
			List<ClothingInstance> list = new List<ClothingInstance>();
			if (!string.IsNullOrEmpty(this.ActiveSettings.Shoes))
			{
				EClothingColor clothingColor = ClothingColorExtensions.GetClothingColor(this.ActiveSettings.ShoesColor);
				list.Add(new ClothingInstance(this.lastSelectedClothingDefinitions["Shoes"], 1, clothingColor));
			}
			if (!string.IsNullOrEmpty(this.ActiveSettings.Top))
			{
				EClothingColor clothingColor2 = ClothingColorExtensions.GetClothingColor(this.ActiveSettings.TopColor);
				list.Add(new ClothingInstance(this.lastSelectedClothingDefinitions["Top"], 1, clothingColor2));
			}
			if (!string.IsNullOrEmpty(this.ActiveSettings.Bottom))
			{
				EClothingColor clothingColor3 = ClothingColorExtensions.GetClothingColor(this.ActiveSettings.BottomColor);
				list.Add(new ClothingInstance(this.lastSelectedClothingDefinitions["Bottom"], 1, clothingColor3));
			}
			if (this.onComplete != null)
			{
				this.onComplete.Invoke(this.ActiveSettings);
			}
			if (this.onCompleteWithClothing != null)
			{
				this.onCompleteWithClothing.Invoke(this.ActiveSettings, list);
			}
			this.Close();
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x0011FA89 File Offset: 0x0011DC89
		public void SliderChanged(float newVal)
		{
			this.rigTargetY = newVal * 359f;
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x0011FA98 File Offset: 0x0011DC98
		public T SetValue<T>(string fieldName, T value, ClothingDefinition definition)
		{
			if (!this.lastSelectedClothingDefinitions.ContainsKey(fieldName))
			{
				this.lastSelectedClothingDefinitions.Add(fieldName, definition);
			}
			else
			{
				this.lastSelectedClothingDefinitions[fieldName] = definition;
			}
			if (fieldName == "Preset")
			{
				this.SelectPreset(value as string);
				return default(T);
			}
			this.ActiveSettings.SetValue<T>(fieldName, value);
			return value;
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0011FB08 File Offset: 0x0011DD08
		public void SelectPreset(string presetName)
		{
			BasicAvatarSettings basicAvatarSettings = this.Presets.Find((BasicAvatarSettings p) => p.name == presetName);
			if (basicAvatarSettings == null)
			{
				Debug.LogError("Preset not found: " + presetName);
				return;
			}
			this.ActiveSettings = UnityEngine.Object.Instantiate<BasicAvatarSettings>(basicAvatarSettings);
			this.Rig.LoadAvatarSettings(this.ActiveSettings.GetAvatarSettings());
			for (int i = 0; i < this.Fields.Count; i++)
			{
				this.Fields[i].ApplyValue();
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0011FBA4 File Offset: 0x0011DDA4
		public void RefreshCategory(CharacterCreator.ECategory category)
		{
			AvatarSettings avatarSettings = this.ActiveSettings.GetAvatarSettings();
			switch (category)
			{
			case CharacterCreator.ECategory.Body:
				this.Rig.ApplyBodySettings(avatarSettings);
				this.Rig.ApplyEyeLidColorSettings(avatarSettings);
				this.Rig.ApplyBodyLayerSettings(avatarSettings, -1);
				return;
			case CharacterCreator.ECategory.Hair:
				this.Rig.ApplyHairSettings(avatarSettings);
				this.Rig.ApplyHairColorSettings(avatarSettings);
				this.Rig.ApplyFaceLayerSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Face:
				this.Rig.ApplyFaceLayerSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Eyes:
				this.Rig.ApplyEyeBallSettings(avatarSettings);
				this.Rig.ApplyEyeLidColorSettings(avatarSettings);
				this.Rig.ApplyEyeLidSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Eyebrows:
				this.Rig.ApplyEyebrowSettings(avatarSettings);
				return;
			case CharacterCreator.ECategory.Clothing:
				this.Rig.ApplyBodyLayerSettings(avatarSettings, -1);
				return;
			case CharacterCreator.ECategory.Accessories:
				this.Rig.ApplyAccessorySettings(avatarSettings);
				return;
			default:
				return;
			}
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0011FCA2 File Offset: 0x0011DEA2
		[CompilerGenerated]
		private IEnumerator <Close>g__Close|28_0()
		{
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.rigTargetY = 0f;
			this.Canvas.enabled = false;
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<TimeManager>.Instance.TimeProgressionMultiplier = 1f;
			}
			yield break;
		}

		// Token: 0x0400315B RID: 12635
		public List<BaseCharacterCreatorField> Fields = new List<BaseCharacterCreatorField>();

		// Token: 0x0400315D RID: 12637
		[Header("References")]
		public Transform Container;

		// Token: 0x0400315E RID: 12638
		public Transform CameraPosition;

		// Token: 0x0400315F RID: 12639
		public Transform RigContainer;

		// Token: 0x04003160 RID: 12640
		public Avatar Rig;

		// Token: 0x04003161 RID: 12641
		public Canvas Canvas;

		// Token: 0x04003162 RID: 12642
		public Animation CanvasAnimation;

		// Token: 0x04003163 RID: 12643
		[Header("Settings")]
		public bool DemoCreator;

		// Token: 0x04003164 RID: 12644
		public BasicAvatarSettings DefaultSettings;

		// Token: 0x04003165 RID: 12645
		public List<BasicAvatarSettings> Presets;

		// Token: 0x04003166 RID: 12646
		public UnityEvent<BasicAvatarSettings> onComplete;

		// Token: 0x04003167 RID: 12647
		public UnityEvent<BasicAvatarSettings, List<ClothingInstance>> onCompleteWithClothing;

		// Token: 0x04003168 RID: 12648
		private Dictionary<string, ClothingDefinition> lastSelectedClothingDefinitions = new Dictionary<string, ClothingDefinition>();

		// Token: 0x04003169 RID: 12649
		private float rigTargetY;

		// Token: 0x020009EA RID: 2538
		public enum ECategory
		{
			// Token: 0x0400316B RID: 12651
			Body,
			// Token: 0x0400316C RID: 12652
			Hair,
			// Token: 0x0400316D RID: 12653
			Face,
			// Token: 0x0400316E RID: 12654
			Eyes,
			// Token: 0x0400316F RID: 12655
			Eyebrows,
			// Token: 0x04003170 RID: 12656
			Clothing,
			// Token: 0x04003171 RID: 12657
			Accessories
		}
	}
}
