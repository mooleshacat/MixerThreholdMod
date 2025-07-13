using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200001B RID: 27
public class FirebaseManager : MonoBehaviour
{
	// Token: 0x0600007E RID: 126 RVA: 0x00005024 File Offset: 0x00003224
	private IEnumerator FetchActiveVote()
	{
		string text = "https://your-backend-url.com/active_vote";
		using (UnityWebRequest request = UnityWebRequest.Get(text))
		{
			yield return request.SendWebRequest();
			if (request.result == 1)
			{
				JObject jobject = JObject.Parse(request.downloadHandler.text);
				Debug.Log("Active Vote: " + jobject.ToString());
			}
			else
			{
				Debug.LogError("Failed to fetch vote: " + request.downloadHandler.text);
			}
		}
		UnityWebRequest request = null;
		yield break;
		yield break;
	}
}
