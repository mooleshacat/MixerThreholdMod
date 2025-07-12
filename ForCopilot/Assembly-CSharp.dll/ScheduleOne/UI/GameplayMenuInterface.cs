using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A29 RID: 2601
	public class GameplayMenuInterface : Singleton<GameplayMenuInterface>
	{
		// Token: 0x060045FA RID: 17914 RVA: 0x00125F10 File Offset: 0x00124110
		protected override void Awake()
		{
			base.Awake();
			this.PhoneButton.onClick.AddListener(new UnityAction(this.PhoneClicked));
			this.CharacterButton.onClick.AddListener(new UnityAction(this.CharacterClicked));
			this.Close();
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x00125F61 File Offset: 0x00124161
		public void Open()
		{
			this.Canvas.enabled = true;
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00125F6F File Offset: 0x0012416F
		public void Close()
		{
			this.Canvas.enabled = false;
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x00125F7D File Offset: 0x0012417D
		public void PhoneClicked()
		{
			Singleton<GameplayMenu>.Instance.SetScreen(GameplayMenu.EGameplayScreen.Phone);
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x00125F8A File Offset: 0x0012418A
		public void CharacterClicked()
		{
			Singleton<GameplayMenu>.Instance.SetScreen(GameplayMenu.EGameplayScreen.Character);
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00125F98 File Offset: 0x00124198
		public void SetSelected(GameplayMenu.EGameplayScreen screen)
		{
			GameplayMenuInterface.<>c__DisplayClass11_0 CS$<>8__locals1 = new GameplayMenuInterface.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pos = Vector2.zero;
			this.PhoneButton.interactable = true;
			this.CharacterButton.interactable = true;
			if (screen == GameplayMenu.EGameplayScreen.Character)
			{
				this.CharacterInterface.Open();
			}
			else
			{
				this.CharacterInterface.Close();
			}
			if (screen != GameplayMenu.EGameplayScreen.Phone)
			{
				if (screen == GameplayMenu.EGameplayScreen.Character)
				{
					CS$<>8__locals1.pos = this.CharacterButton.transform.position;
					this.CharacterButton.interactable = false;
				}
			}
			else
			{
				CS$<>8__locals1.pos = this.PhoneButton.transform.position;
				this.PhoneButton.interactable = false;
			}
			if (this.selectionLerp != null)
			{
				base.StopCoroutine(this.selectionLerp);
			}
			this.selectionLerp = base.StartCoroutine(CS$<>8__locals1.<SetSelected>g__Lerp|0());
		}

		// Token: 0x040032B0 RID: 12976
		public Canvas Canvas;

		// Token: 0x040032B1 RID: 12977
		public Button PhoneButton;

		// Token: 0x040032B2 RID: 12978
		public Button CharacterButton;

		// Token: 0x040032B3 RID: 12979
		public RectTransform SelectionIndicator;

		// Token: 0x040032B4 RID: 12980
		public CharacterInterface CharacterInterface;

		// Token: 0x040032B5 RID: 12981
		private Coroutine selectionLerp;
	}
}
