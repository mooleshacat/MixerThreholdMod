using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A60 RID: 2656
	public class CrimeStatusUI : MonoBehaviour
	{
		// Token: 0x06004778 RID: 18296 RVA: 0x0012C6AC File Offset: 0x0012A8AC
		public void UpdateStatus()
		{
			float b = 0f;
			this.animateText = false;
			PlayerCrimeData.EPursuitLevel currentPursuitLevel = Player.Local.CrimeData.CurrentPursuitLevel;
			this.InvestigatingMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.Investigating);
			this.UnderArrestMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.Arresting);
			this.WantedMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.NonLethal);
			this.WantedDeadMask.gameObject.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.Lethal);
			this.BodysearchLabel.SetActive(currentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && Player.Local.CrimeData.BodySearchPending);
			if (currentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				b = 0.6f;
				if (Player.Local.CrimeData.TimeSinceSighted < 3f)
				{
					b = 1f;
					this.animateText = true;
					if (this.routine == null)
					{
						this.routine = base.StartCoroutine(this.Routine());
					}
				}
			}
			else if (Player.Local.CrimeData.BodySearchPending)
			{
				b = 1f;
			}
			float fillAmount = 1f - Mathf.Clamp01((Player.Local.CrimeData.TimeSinceSighted - 3f) / Player.Local.CrimeData.GetSearchTime());
			this.InvestigatingMask.fillAmount = fillAmount;
			this.UnderArrestMask.fillAmount = fillAmount;
			this.WantedMask.fillAmount = fillAmount;
			this.WantedDeadMask.fillAmount = fillAmount;
			this.CrimeStatusGroup.alpha = Mathf.Lerp(this.CrimeStatusGroup.alpha, b, Time.deltaTime);
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x0012C827 File Offset: 0x0012AA27
		private void OnDestroy()
		{
			if (this.routine != null && Singleton<CoroutineService>.InstanceExists)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.Routine());
			}
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x0012C848 File Offset: 0x0012AA48
		private IEnumerator Routine()
		{
			this.CrimeStatusContainer.localScale = Vector3.one * 0.75f;
			for (;;)
			{
				if (!this.animateText)
				{
					yield return new WaitForEndOfFrame();
				}
				else
				{
					float lerpTime = 1.5f;
					float t = 0f;
					while (t < lerpTime)
					{
						t += Time.deltaTime;
						this.CrimeStatusContainer.localScale = Vector3.one * Mathf.Lerp(0.75f, 1f, (Mathf.Sin(t / lerpTime * 2f * 3.1415927f) + 1f) / 2f);
						yield return new WaitForEndOfFrame();
					}
				}
			}
			yield break;
		}

		// Token: 0x04003442 RID: 13378
		public const float SmallTextSize = 0.75f;

		// Token: 0x04003443 RID: 13379
		public const float LargeTextSize = 1f;

		// Token: 0x04003444 RID: 13380
		[Header("References")]
		public RectTransform CrimeStatusContainer;

		// Token: 0x04003445 RID: 13381
		public CanvasGroup CrimeStatusGroup;

		// Token: 0x04003446 RID: 13382
		public GameObject BodysearchLabel;

		// Token: 0x04003447 RID: 13383
		public Image InvestigatingMask;

		// Token: 0x04003448 RID: 13384
		public Image UnderArrestMask;

		// Token: 0x04003449 RID: 13385
		public Image WantedMask;

		// Token: 0x0400344A RID: 13386
		public Image WantedDeadMask;

		// Token: 0x0400344B RID: 13387
		public GameObject ArrestProgressContainer;

		// Token: 0x0400344C RID: 13388
		private bool animateText;

		// Token: 0x0400344D RID: 13389
		private Coroutine routine;
	}
}
