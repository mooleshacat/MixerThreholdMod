using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

namespace ScheduleOne.Polling
{
	// Token: 0x02000341 RID: 833
	public class PollManager : MonoBehaviour
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001242 RID: 4674 RVA: 0x0004F097 File Offset: 0x0004D297
		// (set) Token: 0x06001243 RID: 4675 RVA: 0x0004F09F File Offset: 0x0004D29F
		public PollData ActivePoll { get; private set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x0004F0A8 File Offset: 0x0004D2A8
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x0004F0B0 File Offset: 0x0004D2B0
		public PollData ConfirmedPoll { get; private set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x0004F0B9 File Offset: 0x0004D2B9
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x0004F0C1 File Offset: 0x0004D2C1
		public PollManager.EPollSubmissionResult SubmissionResult { get; private set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x0004F0CA File Offset: 0x0004D2CA
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x0004F0D2 File Offset: 0x0004D2D2
		public string SubmisssionFailedMesssage { get; private set; } = string.Empty;

		// Token: 0x0600124A RID: 4682 RVA: 0x0004F0DB File Offset: 0x0004D2DB
		private void Start()
		{
			if (SteamManager.Initialized)
			{
				base.StartCoroutine(this.RequestPoll("https://us-central1-s1-polling-987345.cloudfunctions.net/poll", new Action<string>(this.ResponseCallback)));
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0004F102 File Offset: 0x0004D302
		public void GenerateAppTicket()
		{
			if (this.appTicketRequested)
			{
				return;
			}
			if (!SteamManager.Initialized)
			{
				Console.LogError("Steam not initialized, cannot generate app ticket.", null);
				return;
			}
			this.appTicketRequested = true;
			this.InitAppTicket();
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0004F130 File Offset: 0x0004D330
		public void SelectPollResponse(int responseIndex)
		{
			if (string.IsNullOrEmpty(this.appTicket))
			{
				this.SubmisssionFailedMesssage = "Failed to generate session ticket.";
				this.SubmissionResult = PollManager.EPollSubmissionResult.Failed;
				return;
			}
			Console.Log("Sending poll response: " + responseIndex.ToString(), null);
			this.sentResponse = responseIndex;
			this.SubmissionResult = PollManager.EPollSubmissionResult.InProgress;
			PollAnswer answer = new PollAnswer(this.receivedPollResponse.active, responseIndex, this.appTicket);
			base.StartCoroutine(this.SubmitAnswerToServer(answer));
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0004F1A8 File Offset: 0x0004D3A8
		private Task InitAppTicket()
		{
			PollManager.<InitAppTicket>d__30 <InitAppTicket>d__;
			<InitAppTicket>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InitAppTicket>d__.<>4__this = this;
			<InitAppTicket>d__.<>1__state = -1;
			<InitAppTicket>d__.<>t__builder.Start<PollManager.<InitAppTicket>d__30>(ref <InitAppTicket>d__);
			return <InitAppTicket>d__.<>t__builder.Task;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0004F1EB File Offset: 0x0004D3EB
		private IEnumerator SubmitAnswerToServer(PollAnswer answer)
		{
			string text = JsonUtility.ToJson(answer);
			Console.Log("Submitting poll response: " + text, null);
			using (UnityWebRequest req = UnityWebRequest.Post("https://us-central1-s1-polling-987345.cloudfunctions.net/poll", text, "application/json"))
			{
				yield return req.SendWebRequest();
				Console.Log("Result: " + req.result.ToString(), null);
				Console.Log("Response data: " + req.downloadHandler.text, null);
				if (req.result != 1)
				{
					Console.LogError("Failed to send poll response!", null);
					this.SubmisssionFailedMesssage = req.downloadHandler.text;
					this.SubmissionResult = PollManager.EPollSubmissionResult.Failed;
					yield break;
				}
				Console.Log("Successfully submitted poll response!", null);
				this.SubmissionResult = PollManager.EPollSubmissionResult.Success;
				PollManager.RecordSubmission(answer.pollId, answer.answer);
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0004F201 File Offset: 0x0004D401
		private IEnumerator RequestPoll(string url, Action<string> callback = null)
		{
			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			string text = request.downloadHandler.text;
			if (callback != null)
			{
				callback(text);
			}
			yield break;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0004F218 File Offset: 0x0004D418
		private void ResponseCallback(string data)
		{
			PollResponseWrapper pollResponseWrapper = JsonUtility.FromJson<PollResponseWrapper>(data);
			if (pollResponseWrapper == null)
			{
				Console.LogError("Failed to parse poll response wrapper: " + data, null);
				return;
			}
			if (!pollResponseWrapper.success)
			{
				Console.LogError("Failed to get poll response: " + data, null);
				return;
			}
			this.receivedPollResponse = pollResponseWrapper.data;
			Console.Log("Received " + this.receivedPollResponse.polls.Length.ToString() + " polls.", null);
			this.ActivePoll = this.receivedPollResponse.GetActive();
			this.ConfirmedPoll = this.receivedPollResponse.GetConfirmed();
			if (this.ActivePoll != null)
			{
				string str = "Active poll: ";
				PollData active = this.receivedPollResponse.GetActive();
				Console.Log(str + ((active != null) ? active.question : null), null);
				if (this.onActivePollReceived != null)
				{
					this.onActivePollReceived(this.ActivePoll);
				}
			}
			else if (this.ConfirmedPoll != null)
			{
				string str2 = "Confirmed poll: ";
				PollData confirmed = this.receivedPollResponse.GetConfirmed();
				Console.Log(str2 + ((confirmed != null) ? confirmed.question : null), null);
				if (this.onConfirmedPollReceived != null)
				{
					this.onConfirmedPollReceived(this.ConfirmedPoll);
				}
			}
			int num;
			if (PollManager.TryGetExistingPollResponse(this.receivedPollResponse.active, out num))
			{
				Console.Log("Found existing poll response: " + num.ToString(), null);
				this.sentResponse = num;
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0004F378 File Offset: 0x0004D578
		private void OnEncryptedAppTicketResponse(EncryptedAppTicketResponse_t response, bool ioFailure)
		{
			if (response.m_eResult != 1 || ioFailure)
			{
				Console.LogError("Failed to get valid ticket response", null);
				return;
			}
			Console.Log("Received ticket response", null);
			byte[] array = new byte[1024];
			uint newSize;
			if (!SteamUser.GetEncryptedAppTicket(array, 1024, ref newSize))
			{
				Console.LogError("GetEncryptedAppTicket fail", null);
				return;
			}
			Array.Resize<byte>(ref array, (int)newSize);
			string result = BitConverter.ToString(array);
			this.tokenCompletion.SetResult(result);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0004F3F0 File Offset: 0x0004D5F0
		private Task<string> GetAppTicket()
		{
			if (!SteamManager.Initialized)
			{
				Console.LogError("Steam not init", null);
				return null;
			}
			this.tokenCompletion = new TaskCompletionSource<string>();
			SteamAPICall_t steamAPICall_t = SteamUser.RequestEncryptedAppTicket(null, 0);
			this.appTicketCallbackResponse.Set(steamAPICall_t, null);
			return this.tokenCompletion.Task;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0004F43C File Offset: 0x0004D63C
		private static string CleanTicket(string ticket)
		{
			return ticket.Replace("-", "");
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0004F450 File Offset: 0x0004D650
		public static bool TryGetExistingPollResponse(int pollId, out int response)
		{
			string str = SteamUser.GetSteamID().ToString();
			response = PlayerPrefs.GetInt("poll_response_" + str + pollId.ToString(), -1);
			if (response == -1)
			{
				response = PlayerPrefs.GetInt("poll_response_" + pollId.ToString(), -1);
			}
			return response != -1;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0004F4B4 File Offset: 0x0004D6B4
		private static void RecordSubmission(int pollId, int response)
		{
			string str = SteamUser.GetSteamID().ToString();
			PlayerPrefs.SetInt("poll_response_" + str + pollId.ToString(), response);
		}

		// Token: 0x0400119F RID: 4511
		public const string SERVER_URL = "https://us-central1-s1-polling-987345.cloudfunctions.net/poll";

		// Token: 0x040011A4 RID: 4516
		private CallResult<EncryptedAppTicketResponse_t> appTicketCallbackResponse;

		// Token: 0x040011A5 RID: 4517
		private TaskCompletionSource<string> tokenCompletion;

		// Token: 0x040011A6 RID: 4518
		private PollResponse receivedPollResponse;

		// Token: 0x040011A7 RID: 4519
		private int sentResponse = -1;

		// Token: 0x040011A8 RID: 4520
		private string appTicket = string.Empty;

		// Token: 0x040011A9 RID: 4521
		public Action<PollData> onActivePollReceived;

		// Token: 0x040011AA RID: 4522
		public Action<PollData> onConfirmedPollReceived;

		// Token: 0x040011AB RID: 4523
		private bool appTicketRequested;

		// Token: 0x02000342 RID: 834
		public enum EPollSubmissionResult
		{
			// Token: 0x040011AD RID: 4525
			InProgress,
			// Token: 0x040011AE RID: 4526
			Success,
			// Token: 0x040011AF RID: 4527
			Failed
		}
	}
}
