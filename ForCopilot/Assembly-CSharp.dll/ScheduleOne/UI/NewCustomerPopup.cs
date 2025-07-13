using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A48 RID: 2632
	public class NewCustomerPopup : Singleton<NewCustomerPopup>
	{
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x00128B32 File Offset: 0x00126D32
		// (set) Token: 0x060046BA RID: 18106 RVA: 0x00128B3A File Offset: 0x00126D3A
		public bool IsPlaying { get; protected set; }

		// Token: 0x060046BB RID: 18107 RVA: 0x00128B43 File Offset: 0x00126D43
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.DisableEntries();
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x00128B70 File Offset: 0x00126D70
		public void PlayPopup(Customer customer)
		{
			this.IsPlaying = true;
			RectTransform rectTransform = null;
			int num = 0;
			for (int i = 0; i < this.Entries.Length; i++)
			{
				num++;
				if (!this.Entries[i].gameObject.activeSelf)
				{
					rectTransform = this.Entries[i];
					break;
				}
			}
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.Find("Mask/Icon").GetComponent<Image>().sprite = customer.NPC.MugshotSprite;
			rectTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = customer.NPC.FirstName + "\n" + customer.NPC.LastName;
			rectTransform.gameObject.SetActive(true);
			if (num == 1)
			{
				this.Title.text = "New Customer Unlocked!";
			}
			else
			{
				this.Title.text = "New Customers Unlocked!";
			}
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
				this.Anim.Stop();
				this.routine = null;
			}
			this.routine = base.StartCoroutine(this.<PlayPopup>g__Routine|13_0());
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00128C88 File Offset: 0x00126E88
		private void DisableEntries()
		{
			for (int i = 0; i < this.Entries.Length; i++)
			{
				this.Entries[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x00128CC3 File Offset: 0x00126EC3
		[CompilerGenerated]
		private IEnumerator <PlayPopup>g__Routine|13_0()
		{
			yield return new WaitUntil(() => !Singleton<DealCompletionPopup>.Instance.IsPlaying);
			this.Group.alpha = 0.01f;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.SoundEffect.Play();
			this.Anim.Play();
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => this.Group.alpha == 0f);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.routine = null;
			this.IsPlaying = false;
			this.DisableEntries();
			yield break;
		}

		// Token: 0x0400337C RID: 13180
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400337D RID: 13181
		public RectTransform Container;

		// Token: 0x0400337E RID: 13182
		public CanvasGroup Group;

		// Token: 0x0400337F RID: 13183
		public Animation Anim;

		// Token: 0x04003380 RID: 13184
		public TextMeshProUGUI Title;

		// Token: 0x04003381 RID: 13185
		public RectTransform[] Entries;

		// Token: 0x04003382 RID: 13186
		public AudioSourceController SoundEffect;

		// Token: 0x04003383 RID: 13187
		private Coroutine routine;
	}
}
