using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000563 RID: 1379
public class CheatUpdate : MonoBehaviour
{
	// Token: 0x0600219E RID: 8606 RVA: 0x000B71E5 File Offset: 0x000B53E5
	private void Start()
	{
		base.StartCoroutine(this.UpdateNumberOfPlayers());
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000B71F4 File Offset: 0x000B53F4
	public IEnumerator UpdateNumberOfPlayers()
	{
		for (;;)
		{
			base.StartCoroutine(this.UpdatePlayerCount());
			yield return new WaitForSeconds(10f);
		}
		yield break;
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000B7203 File Offset: 0x000B5403
	private IEnumerator UpdatePlayerCount()
	{
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("player_count", PhotonNetwork.CountOfPlayers - 1);
		wwwform.AddField("game_version", "live");
		wwwform.AddField("game_name", Application.productName);
		Debug.Log(PhotonNetwork.CountOfPlayers - 1);
		using (UnityWebRequest www = UnityWebRequest.Post("http://ntsfranz.crabdance.com/update_monke_count", wwwform))
		{
			yield return www.SendWebRequest();
			if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
			{
				Debug.Log(www.error);
			}
		}
		UnityWebRequest www = null;
		yield break;
		yield break;
	}

	// Token: 0x060021A1 RID: 8609 RVA: 0x000026E9 File Offset: 0x000008E9
	public CheatUpdate()
	{
	}

	// Token: 0x02000564 RID: 1380
	[CompilerGenerated]
	private sealed class <UpdateNumberOfPlayers>d__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060021A2 RID: 8610 RVA: 0x000B720B File Offset: 0x000B540B
		[DebuggerHidden]
		public <UpdateNumberOfPlayers>d__1(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000B721C File Offset: 0x000B541C
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			CheatUpdate cheatUpdate = this;
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
			}
			cheatUpdate.StartCoroutine(cheatUpdate.UpdatePlayerCount());
			this.<>2__current = new WaitForSeconds(10f);
			this.<>1__state = 1;
			return true;
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000B7275 File Offset: 0x000B5475
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x000B7275 File Offset: 0x000B5475
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04002B19 RID: 11033
		private int <>1__state;

		// Token: 0x04002B1A RID: 11034
		private object <>2__current;

		// Token: 0x04002B1B RID: 11035
		public CheatUpdate <>4__this;
	}

	// Token: 0x02000565 RID: 1381
	[CompilerGenerated]
	private sealed class <UpdatePlayerCount>d__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060021A8 RID: 8616 RVA: 0x000B727D File Offset: 0x000B547D
		[DebuggerHidden]
		public <UpdatePlayerCount>d__2(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000B728C File Offset: 0x000B548C
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = this.<>1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					this.<>m__Finally1();
				}
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000B72C4 File Offset: 0x000B54C4
		bool IEnumerator.MoveNext()
		{
			bool flag;
			try
			{
				int num = this.<>1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						flag = false;
					}
					else
					{
						this.<>1__state = -3;
						if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
						{
							Debug.Log(www.error);
						}
						this.<>m__Finally1();
						www = null;
						flag = false;
					}
				}
				else
				{
					this.<>1__state = -1;
					WWWForm wwwform = new WWWForm();
					wwwform.AddField("player_count", PhotonNetwork.CountOfPlayers - 1);
					wwwform.AddField("game_version", "live");
					wwwform.AddField("game_name", Application.productName);
					Debug.Log(PhotonNetwork.CountOfPlayers - 1);
					www = UnityWebRequest.Post("http://ntsfranz.crabdance.com/update_monke_count", wwwform);
					this.<>1__state = -3;
					this.<>2__current = www.SendWebRequest();
					this.<>1__state = 1;
					flag = true;
				}
			}
			catch
			{
				this.System.IDisposable.Dispose();
				throw;
			}
			return flag;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000B73CC File Offset: 0x000B55CC
		private void <>m__Finally1()
		{
			this.<>1__state = -1;
			if (www != null)
			{
				((IDisposable)www).Dispose();
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x000B73E8 File Offset: 0x000B55E8
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000B73E8 File Offset: 0x000B55E8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x04002B1C RID: 11036
		private int <>1__state;

		// Token: 0x04002B1D RID: 11037
		private object <>2__current;

		// Token: 0x04002B1E RID: 11038
		private UnityWebRequest <www>5__2;
	}
}
