using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.AvatarFramework;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B83 RID: 2947
	public class CharacterCustomizationUI : MonoBehaviour
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x0014AC68 File Offset: 0x00148E68
		// (set) Token: 0x06004E35 RID: 20021 RVA: 0x0014AC70 File Offset: 0x00148E70
		public bool IsOpen { get; private set; }

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06004E36 RID: 20022 RVA: 0x0014AC79 File Offset: 0x00148E79
		// (set) Token: 0x06004E37 RID: 20023 RVA: 0x0014AC81 File Offset: 0x00148E81
		public CharacterCustomizationCategory ActiveCategory { get; private set; }

		// Token: 0x06004E38 RID: 20024 RVA: 0x0014AC8A File Offset: 0x00148E8A
		private void OnValidate()
		{
			this.Categories = base.GetComponentsInChildren<CharacterCustomizationCategory>(true);
			this.TitleText.text = this.Title;
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0014ACAC File Offset: 0x00148EAC
		private void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			this.RigRotationSlider.onValueChanged.AddListener(delegate(float value)
			{
				this.rigTargetY = value * 359f;
			});
			this.Categories = base.GetComponentsInChildren<CharacterCustomizationCategory>(true);
			this.TitleText.text = this.Title;
			this.ExitButton.onClick.AddListener(new UnityAction(this.Close));
			for (int i = 0; i < this.Categories.Length; i++)
			{
				Button button = UnityEngine.Object.Instantiate<Button>(this.CategoryButtonPrefab, this.ButtonContainer);
				button.GetComponentInChildren<TextMeshProUGUI>().text = this.Categories[i].CategoryName;
				CharacterCustomizationCategory category = this.Categories[i];
				button.onClick.AddListener(delegate()
				{
					this.SetActiveCategory(category);
				});
			}
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.MainContainer.gameObject.SetActive(false);
			this.AvatarRig.gameObject.SetActive(false);
			this.SetActiveCategory(null);
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0014ADCC File Offset: 0x00148FCC
		protected virtual void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.RigContainer.localEulerAngles = Vector3.Lerp(this.RigContainer.localEulerAngles, new Vector3(0f, this.rigTargetY, 0f), Time.deltaTime * 5f);
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x0014AE20 File Offset: 0x00149020
		public void SetActiveCategory(CharacterCustomizationCategory category)
		{
			this.ActiveCategory = category;
			for (int i = 0; i < this.Categories.Length; i++)
			{
				this.Categories[i].gameObject.SetActive(this.Categories[i] == category);
				if (this.Categories[i] == category)
				{
					this.Categories[i].Open();
				}
			}
			this.MenuContainer.gameObject.SetActive(category == null);
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x00014B5A File Offset: 0x00012D5A
		public virtual bool IsOptionCurrentlyApplied(CharacterCustomizationOption option)
		{
			return false;
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x0014AE9B File Offset: 0x0014909B
		public virtual void OptionSelected(CharacterCustomizationOption option)
		{
			this.PreviewIndicator.gameObject.SetActive(!option.purchased);
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x0014AEB6 File Offset: 0x001490B6
		public virtual void OptionDeselected(CharacterCustomizationOption option)
		{
			Console.Log("Deselected option: " + option.Label, null);
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x0014AECE File Offset: 0x001490CE
		public virtual void OptionPurchased(CharacterCustomizationOption option)
		{
			this.PreviewIndicator.gameObject.SetActive(false);
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x0014AEE4 File Offset: 0x001490E4
		public virtual void Open()
		{
			if (this.openCloseRoutine != null)
			{
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			this.currentSettings = UnityEngine.Object.Instantiate<BasicAvatarSettings>(Player.Local.CurrentAvatarSettings);
			this.openCloseRoutine = base.StartCoroutine(this.<Open>g__Close|34_0());
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x0014AF54 File Offset: 0x00149154
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				if (this.ActiveCategory != null)
				{
					this.ActiveCategory.Back();
					return;
				}
				this.Close();
			}
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x0014AFA4 File Offset: 0x001491A4
		protected virtual void Close()
		{
			if (this.openCloseRoutine != null)
			{
				return;
			}
			this.SetActiveCategory(null);
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.MainContainer.gameObject.SetActive(false);
			Player.Local.SendAppearance(this.currentSettings);
			this.openCloseRoutine = base.StartCoroutine(this.<Close>g__Close|36_0());
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x0014B029 File Offset: 0x00149229
		[CompilerGenerated]
		private IEnumerator <Open>g__Close|34_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			this.IsOpen = true;
			this.Canvas.enabled = true;
			this.MainContainer.gameObject.SetActive(true);
			this.AvatarRig.gameObject.SetActive(true);
			if (this.LoadAvatarSettingsNaked)
			{
				this.AvatarRig.LoadNakedSettings(Player.Local.Avatar.CurrentSettings, 19);
			}
			else
			{
				this.AvatarRig.LoadAvatarSettings(Player.Local.Avatar.CurrentSettings);
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0f);
			this.SetActiveCategory(null);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x0014B038 File Offset: 0x00149238
		[CompilerGenerated]
		private IEnumerator <Close>g__Close|36_0()
		{
			Singleton<BlackOverlay>.Instance.Open(0.5f);
			yield return new WaitForSeconds(0.6f);
			this.AvatarRig.gameObject.SetActive(false);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, true);
				PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0f);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<BlackOverlay>.Instance.Close(0.5f);
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x04003A91 RID: 14993
		[Header("Settings")]
		public string Title = "Customize";

		// Token: 0x04003A92 RID: 14994
		public CharacterCustomizationCategory[] Categories;

		// Token: 0x04003A93 RID: 14995
		public bool LoadAvatarSettingsNaked;

		// Token: 0x04003A94 RID: 14996
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003A95 RID: 14997
		public RectTransform MainContainer;

		// Token: 0x04003A96 RID: 14998
		public RectTransform MenuContainer;

		// Token: 0x04003A97 RID: 14999
		public TextMeshProUGUI TitleText;

		// Token: 0x04003A98 RID: 15000
		public RectTransform ButtonContainer;

		// Token: 0x04003A99 RID: 15001
		public Button ExitButton;

		// Token: 0x04003A9A RID: 15002
		public Slider RigRotationSlider;

		// Token: 0x04003A9B RID: 15003
		public Transform CameraPosition;

		// Token: 0x04003A9C RID: 15004
		public Transform RigContainer;

		// Token: 0x04003A9D RID: 15005
		public Avatar AvatarRig;

		// Token: 0x04003A9E RID: 15006
		public RectTransform PreviewIndicator;

		// Token: 0x04003A9F RID: 15007
		[Header("Prefab")]
		public Button CategoryButtonPrefab;

		// Token: 0x04003AA0 RID: 15008
		private float rigTargetY;

		// Token: 0x04003AA1 RID: 15009
		private Coroutine openCloseRoutine;

		// Token: 0x04003AA2 RID: 15010
		protected BasicAvatarSettings currentSettings;
	}
}
