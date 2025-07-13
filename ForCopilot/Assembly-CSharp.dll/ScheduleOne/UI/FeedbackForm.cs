using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using AeLa.EasyFeedback;
using AeLa.EasyFeedback.FormElements;
using AeLa.EasyFeedback.Utility;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A1F RID: 2591
	public class FeedbackForm : FeedbackForm
	{
		// Token: 0x060045BD RID: 17853 RVA: 0x00124F24 File Offset: 0x00123124
		public override void Awake()
		{
			base.Awake();
			this.ScreenshotToggle.SetIsOnWithoutNotify(this.IncludeScreenshot);
			this.ScreenshotToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnScreenshotToggle));
			this.SaveFileToggle.SetIsOnWithoutNotify(this.IncludeSaveFile);
			this.SaveFileToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnSaveFileToggle));
			this.OnSubmissionSucceeded.AddListener(new UnityAction(this.Clear));
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x00124FA8 File Offset: 0x001231A8
		private void Update()
		{
			this.Cog.localEulerAngles += new Vector3(0f, 0f, -180f * Time.unscaledDeltaTime);
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x00124FDA File Offset: 0x001231DA
		public void PrepScreenshot()
		{
			this.CurrentReport = new Report();
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x00124FE7 File Offset: 0x001231E7
		private void OnScreenshotToggle(bool value)
		{
			this.IncludeScreenshot = value;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x00124FF0 File Offset: 0x001231F0
		private void OnSaveFileToggle(bool value)
		{
			this.IncludeSaveFile = value;
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x00124FF9 File Offset: 0x001231F9
		public void SetFormData(string title)
		{
			if (this.CurrentReport == null)
			{
				this.CurrentReport = new Report();
			}
			this.CurrentReport.Title = title;
			base.GetComponentInChildren<ReportTitle>().GetComponent<TMP_InputField>().SetTextWithoutNotify(title);
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x0012502C File Offset: 0x0012322C
		public void SetCategory(string categoryName)
		{
			for (int i = 0; i < this.Config.Board.CategoryNames.Length; i++)
			{
				if (this.Config.Board.CategoryNames[i].Contains(categoryName))
				{
					this.CategoryDropdown.SetValueWithoutNotify(i + 1);
					return;
				}
			}
			Console.LogWarning("Category not found: " + categoryName, null);
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x00125090 File Offset: 0x00123290
		public override void Submit()
		{
			if (this.IncludeScreenshot)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0f);
				this.CanvasGroup.alpha = 0f;
				this.ssCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.ScreenshotAndOpenForm());
				Singleton<CoroutineService>.Instance.StartCoroutine(this.<Submit>g__Wait|15_0());
			}
			if (File.Exists(Application.persistentDataPath + "/Player-prev.log"))
			{
				try
				{
					byte[] array = File.ReadAllBytes(Application.persistentDataPath + "/Player-prev.log");
					this.CurrentReport.AttachFile("Player-prev.txt", array);
				}
				catch (Exception ex)
				{
					Console.LogError("Failed to attach Player-prev.txt: " + ex.Message, null);
				}
			}
			if (this.IncludeSaveFile)
			{
				string loadedGameFolderPath = Singleton<LoadManager>.Instance.LoadedGameFolderPath;
				string text = loadedGameFolderPath + ".zip";
				try
				{
					if (File.Exists(text))
					{
						Console.Log("Deleting prior zip file: " + text, null);
						File.Delete(text);
					}
					ZipFile.CreateFromDirectory(loadedGameFolderPath, text, System.IO.Compression.CompressionLevel.Optimal, true);
					byte[] array2 = File.ReadAllBytes(text);
					this.CurrentReport.AttachFile("SaveGame.zip", array2);
				}
				catch (Exception ex2)
				{
					Console.LogError("Failed to attach save file: " + ex2.Message, null);
				}
				finally
				{
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
			}
			if (Player.Local != null)
			{
				Report currentReport = this.CurrentReport;
				currentReport.Title = currentReport.Title + " (" + Player.Local.PlayerName + ")";
			}
			this.CurrentReport.AddSection("Game Info", 2);
			string text2 = "Singleplayer";
			if (Singleton<Lobby>.InstanceExists && Singleton<Lobby>.Instance.IsInLobby)
			{
				text2 = "Multiplayer";
				if (Singleton<Lobby>.Instance.IsHost)
				{
					text2 += " (Host)";
				}
				else
				{
					text2 += " (Client)";
				}
			}
			this.CurrentReport["Game Info"].AppendLine("Network Mode: " + text2);
			this.CurrentReport["Game Info"].AppendLine("Player Count: " + Player.PlayerList.Count.ToString());
			this.CurrentReport["Game Info"].AppendLine("Beta Branch: " + GameManager.IS_BETA.ToString());
			this.CurrentReport["Game Info"].AppendLine("Is Demo: " + false.ToString());
			this.CurrentReport["Game Info"].AppendLine("Load History: " + string.Join(", ", LoadManager.LoadHistory));
			Singleton<CoroutineService>.Instance.StartCoroutine(base.SubmitAsync());
			base.Submit();
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x0012537C File Offset: 0x0012357C
		protected override string GetTextToAppendToTitle()
		{
			string text = base.GetTextToAppendToTitle();
			text = text + " (" + Application.version + ")";
			if (Player.Local != null)
			{
				text = text + " (" + Player.Local.PlayerName + ")";
			}
			return text;
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x001253CF File Offset: 0x001235CF
		private void Clear()
		{
			this.SummaryField.SetTextWithoutNotify(string.Empty);
			this.DescriptionField.SetTextWithoutNotify(string.Empty);
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x001253F1 File Offset: 0x001235F1
		private IEnumerator ScreenshotAndOpenForm()
		{
			if (this.IncludeScreenshot)
			{
				yield return ScreenshotUtil.CaptureScreenshot(this.ScreenshotCaptureMode, this.ResizeLargeScreenshots, delegate(byte[] ss)
				{
					this.CurrentReport.AttachFile("screenshot.png", ss);
				}, delegate(string err)
				{
					this.OnSubmissionError.Invoke(err);
				});
			}
			base.EnableForm();
			this.Form.gameObject.SetActive(true);
			this.OnFormOpened.Invoke();
			this.ssCoroutine = null;
			yield break;
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00125408 File Offset: 0x00123608
		[CompilerGenerated]
		private IEnumerator <Submit>g__Wait|15_0()
		{
			yield return new WaitForEndOfFrame();
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0f);
			this.CanvasGroup.alpha = 1f;
			yield break;
		}

		// Token: 0x0400327A RID: 12922
		private Coroutine ssCoroutine;

		// Token: 0x0400327B RID: 12923
		public CanvasGroup CanvasGroup;

		// Token: 0x0400327C RID: 12924
		public Toggle ScreenshotToggle;

		// Token: 0x0400327D RID: 12925
		public Toggle SaveFileToggle;

		// Token: 0x0400327E RID: 12926
		public TMP_InputField SummaryField;

		// Token: 0x0400327F RID: 12927
		public TMP_InputField DescriptionField;

		// Token: 0x04003280 RID: 12928
		public RectTransform Cog;

		// Token: 0x04003281 RID: 12929
		public TMP_Dropdown CategoryDropdown;
	}
}
