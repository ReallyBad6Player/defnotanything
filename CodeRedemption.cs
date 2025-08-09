using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000427 RID: 1063
public class CodeRedemption : MonoBehaviour
{
	// Token: 0x060019BB RID: 6587 RVA: 0x0008A356 File Offset: 0x00088556
	public void Awake()
	{
		if (CodeRedemption.Instance == null)
		{
			CodeRedemption.Instance = this;
			return;
		}
		if (CodeRedemption.Instance != this)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x0008A388 File Offset: 0x00088588
	public void HandleCodeRedemption(string code)
	{
		string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
		string playFabSessionTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket();
		string text = string.Concat(new string[] { "{ \"itemGUID\": \"", code, "\", \"playFabID\": \"", playFabPlayerId, "\", \"playFabSessionTicket\": \"", playFabSessionTicket, "\" }" });
		Debug.Log("[CodeRedemption] Web Request body: \n" + text);
		base.StartCoroutine(CodeRedemption.ProcessWebRequest(PlayFabAuthenticatorSettings.HpPromoApiBaseUrl + "/api/ConsumeCodeItem", text, "application/json", new Action<UnityWebRequest>(this.OnCodeRedemptionResponse)));
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x0008A428 File Offset: 0x00088628
	private void OnCodeRedemptionResponse(UnityWebRequest completedRequest)
	{
		if (completedRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("[CodeRedemption] Web Request failed: " + completedRequest.error + "\nDetails: " + completedRequest.downloadHandler.text);
			GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.Invalid;
			return;
		}
		string text = string.Empty;
		try
		{
			CodeRedemption.CodeRedemptionRequest codeRedemptionRequest = JsonUtility.FromJson<CodeRedemption.CodeRedemptionRequest>(completedRequest.downloadHandler.text);
			if (codeRedemptionRequest.result.Contains("AlreadyRedeemed", StringComparison.OrdinalIgnoreCase))
			{
				Debug.Log("[CodeRedemption] Item has already been redeemed!");
				GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.AlreadyUsed;
				return;
			}
			text = codeRedemptionRequest.playFabItemName;
		}
		catch (Exception ex)
		{
			string text2 = "[CodeRedemption] Error parsing JSON response: ";
			Exception ex2 = ex;
			Debug.LogError(text2 + ((ex2 != null) ? ex2.ToString() : null));
			GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.Invalid;
			return;
		}
		Debug.Log("[CodeRedemption] Item successfully granted, processing external unlock...");
		GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.Success;
		GorillaComputer.instance.RedemptionCode = "";
		base.StartCoroutine(this.CheckProcessExternalUnlock(new string[] { text }, true, true, true));
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x0008A53C File Offset: 0x0008873C
	private IEnumerator CheckProcessExternalUnlock(string[] itemIDs, bool autoEquip, bool isLeftHand, bool destroyOnFinish)
	{
		Debug.Log("[CodeRedemption] Checking if we can process external cosmetic unlock...");
		while (!CosmeticsController.instance.allCosmeticsDict_isInitialized || !CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			yield return null;
		}
		Debug.Log("[CodeRedemption] Cosmetics initialized, proceeding to process external unlock...");
		foreach (string text in itemIDs)
		{
			CosmeticsController.instance.ProcessExternalUnlock(text, autoEquip, isLeftHand);
		}
		yield break;
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x0008A559 File Offset: 0x00088759
	private static IEnumerator ProcessWebRequest(string url, string data, string contentType, Action<UnityWebRequest> callback)
	{
		UnityWebRequest request = UnityWebRequest.Post(url, data, contentType);
		yield return request.SendWebRequest();
		callback(request);
		yield break;
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000026E9 File Offset: 0x000008E9
	public CodeRedemption()
	{
	}

	// Token: 0x04002216 RID: 8726
	public static volatile CodeRedemption Instance;

	// Token: 0x04002217 RID: 8727
	private const string HiddenPathCollabEndpoint = "/api/ConsumeCodeItem";

	// Token: 0x02000428 RID: 1064
	[Serializable]
	public class CodeRedemptionRequest
	{
		// Token: 0x060019C1 RID: 6593 RVA: 0x00002050 File Offset: 0x00000250
		public CodeRedemptionRequest()
		{
		}

		// Token: 0x04002218 RID: 8728
		public string result;

		// Token: 0x04002219 RID: 8729
		public string itemID;

		// Token: 0x0400221A RID: 8730
		public string playFabItemName;
	}

	// Token: 0x02000429 RID: 1065
	[CompilerGenerated]
	private sealed class <CheckProcessExternalUnlock>d__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060019C2 RID: 6594 RVA: 0x0008A57D File Offset: 0x0008877D
		[DebuggerHidden]
		public <CheckProcessExternalUnlock>d__6(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0008A58C File Offset: 0x0008878C
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				this.<>1__state = -1;
			}
			else
			{
				this.<>1__state = -1;
				Debug.Log("[CodeRedemption] Checking if we can process external cosmetic unlock...");
			}
			if (CosmeticsController.instance.allCosmeticsDict_isInitialized && CosmeticsV2Spawner_Dirty.allPartsInstantiated)
			{
				Debug.Log("[CodeRedemption] Cosmetics initialized, proceeding to process external unlock...");
				foreach (string text in itemIDs)
				{
					CosmeticsController.instance.ProcessExternalUnlock(text, autoEquip, isLeftHand);
				}
				return false;
			}
			this.<>2__current = null;
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060019C5 RID: 6597 RVA: 0x0008A625 File Offset: 0x00088825
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060019C7 RID: 6599 RVA: 0x0008A625 File Offset: 0x00088825
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x0400221B RID: 8731
		private int <>1__state;

		// Token: 0x0400221C RID: 8732
		private object <>2__current;

		// Token: 0x0400221D RID: 8733
		public string[] itemIDs;

		// Token: 0x0400221E RID: 8734
		public bool autoEquip;

		// Token: 0x0400221F RID: 8735
		public bool isLeftHand;
	}

	// Token: 0x0200042A RID: 1066
	[CompilerGenerated]
	private sealed class <ProcessWebRequest>d__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060019C8 RID: 6600 RVA: 0x0008A62D File Offset: 0x0008882D
		[DebuggerHidden]
		public <ProcessWebRequest>d__7(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x0008A63C File Offset: 0x0008883C
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			if (num == 0)
			{
				this.<>1__state = -1;
				request = UnityWebRequest.Post(url, data, contentType);
				this.<>2__current = request.SendWebRequest();
				this.<>1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.<>1__state = -1;
			callback(request);
			return false;
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0008A6B0 File Offset: 0x000888B0
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0008A6B0 File Offset: 0x000888B0
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04002220 RID: 8736
		private int <>1__state;

		// Token: 0x04002221 RID: 8737
		private object <>2__current;

		// Token: 0x04002222 RID: 8738
		public string url;

		// Token: 0x04002223 RID: 8739
		public string data;

		// Token: 0x04002224 RID: 8740
		public string contentType;

		// Token: 0x04002225 RID: 8741
		public Action<UnityWebRequest> callback;

		// Token: 0x04002226 RID: 8742
		private UnityWebRequest <request>5__2;
	}
}
