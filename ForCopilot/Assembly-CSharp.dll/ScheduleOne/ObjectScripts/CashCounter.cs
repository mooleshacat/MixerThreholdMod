using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BE2 RID: 3042
	public class CashCounter : MonoBehaviour
	{
		// Token: 0x060050FE RID: 20734 RVA: 0x00156668 File Offset: 0x00154868
		public virtual void LateUpdate()
		{
			this.UpperNotes.gameObject.SetActive(this.IsOn);
			this.LowerNotes.gameObject.SetActive(this.IsOn);
			if (this.IsOn)
			{
				if (!this.lerping)
				{
					this.lerping = true;
					for (int i = 0; i < this.MovingNotes.Count; i++)
					{
						base.StartCoroutine(this.LerpNote(this.MovingNotes[i]));
					}
				}
				if (!this.Audio.AudioSource.isPlaying)
				{
					this.Audio.Play();
					return;
				}
			}
			else
			{
				this.lerping = false;
				if (this.Audio.AudioSource.isPlaying)
				{
					this.Audio.Stop();
				}
			}
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x00156729 File Offset: 0x00154929
		private IEnumerator LerpNote(Transform note)
		{
			yield return new WaitForSeconds((float)this.MovingNotes.IndexOf(note) / (float)(this.MovingNotes.Count + 1) * 0.18f);
			note.gameObject.SetActive(true);
			while (this.IsOn)
			{
				note.position = this.NoteStartPoint.position;
				note.rotation = this.NoteStartPoint.rotation;
				for (float i = 0f; i < 0.18f; i += Time.deltaTime)
				{
					note.position = Vector3.Lerp(this.NoteStartPoint.position, this.NoteEndPoint.position, i / 0.18f);
					note.rotation = Quaternion.Lerp(this.NoteStartPoint.rotation, this.NoteEndPoint.rotation, i / 0.18f);
					yield return new WaitForEndOfFrame();
				}
			}
			note.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04003CC1 RID: 15553
		public const float NoteLerpTime = 0.18f;

		// Token: 0x04003CC2 RID: 15554
		public bool IsOn;

		// Token: 0x04003CC3 RID: 15555
		[Header("References")]
		public GameObject UpperNotes;

		// Token: 0x04003CC4 RID: 15556
		public GameObject LowerNotes;

		// Token: 0x04003CC5 RID: 15557
		public Transform NoteStartPoint;

		// Token: 0x04003CC6 RID: 15558
		public Transform NoteEndPoint;

		// Token: 0x04003CC7 RID: 15559
		public List<Transform> MovingNotes = new List<Transform>();

		// Token: 0x04003CC8 RID: 15560
		public AudioSourceController Audio;

		// Token: 0x04003CC9 RID: 15561
		private bool lerping;
	}
}
