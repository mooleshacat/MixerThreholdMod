using System;
using EasyButtons;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x020008F6 RID: 2294
	public class StorageDoorAnimation : MonoBehaviour
	{
		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x00106620 File Offset: 0x00104820
		// (set) Token: 0x06003E43 RID: 15939 RVA: 0x00106628 File Offset: 0x00104828
		public bool IsOpen { get; protected set; }

		// Token: 0x06003E44 RID: 15940 RVA: 0x00106631 File Offset: 0x00104831
		private void Start()
		{
			if (this.ItemContainer != null)
			{
				this.ItemContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x00106652 File Offset: 0x00104852
		[Button]
		public void Open()
		{
			this.SetIsOpen(true);
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x0010665B File Offset: 0x0010485B
		[Button]
		public void Close()
		{
			this.SetIsOpen(false);
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00106664 File Offset: 0x00104864
		public void SetIsOpen(bool open)
		{
			if (this.overriddeIsOpen)
			{
				open = this.overrideState;
			}
			if (this.IsOpen == open)
			{
				return;
			}
			if (open && this.ItemContainer != null)
			{
				this.ItemContainer.gameObject.SetActive(true);
			}
			this.IsOpen = open;
			for (int i = 0; i < this.Anims.Length; i++)
			{
				this.Anims[i].Play(this.IsOpen ? this.OpenAnim.name : this.CloseAnim.name);
			}
			if (this.IsOpen)
			{
				if (this.OpenSound != null)
				{
					this.OpenSound.Play();
				}
			}
			else if (this.CloseSound != null)
			{
				this.CloseSound.Play();
			}
			if (!open)
			{
				base.Invoke("DisableItems", this.CloseAnim.length);
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x0010674A File Offset: 0x0010494A
		private void DisableItems()
		{
			if (this.IsOpen)
			{
				return;
			}
			if (this.ItemContainer != null)
			{
				this.ItemContainer.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00106774 File Offset: 0x00104974
		public void OverrideState(bool open)
		{
			this.overriddeIsOpen = true;
			this.overrideState = open;
			this.SetIsOpen(open);
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x0010678B File Offset: 0x0010498B
		public void ResetOverride()
		{
			this.overriddeIsOpen = false;
		}

		// Token: 0x04002C54 RID: 11348
		private bool overriddeIsOpen;

		// Token: 0x04002C55 RID: 11349
		private bool overrideState;

		// Token: 0x04002C56 RID: 11350
		public Transform ItemContainer;

		// Token: 0x04002C57 RID: 11351
		[Header("Animations")]
		public Animation[] Anims;

		// Token: 0x04002C58 RID: 11352
		public AnimationClip OpenAnim;

		// Token: 0x04002C59 RID: 11353
		public AnimationClip CloseAnim;

		// Token: 0x04002C5A RID: 11354
		public AudioSourceController OpenSound;

		// Token: 0x04002C5B RID: 11355
		public AudioSourceController CloseSound;
	}
}
