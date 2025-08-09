using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000526 RID: 1318
public class BuilderDropZone : MonoBehaviour
{
	// Token: 0x0600201E RID: 8222 RVA: 0x000A9CFA File Offset: 0x000A7EFA
	private void Awake()
	{
		this.repelDirectionWorld = base.transform.TransformDirection(this.repelDirectionLocal.normalized);
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000A9D18 File Offset: 0x000A7F18
	private void OnTriggerEnter(Collider other)
	{
		if (!this.onEnter)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		BuilderPieceCollider component = other.GetComponent<BuilderPieceCollider>();
		if (component != null)
		{
			BuilderPiece piece = component.piece;
			if (this.table != null && this.table.builderNetworking != null)
			{
				if (piece == null)
				{
					return;
				}
				if (this.dropType == BuilderDropZone.DropType.Recycle)
				{
					bool flag = piece.state != BuilderPiece.State.Displayed && piece.state != BuilderPiece.State.OnShelf && piece.state > BuilderPiece.State.AttachedAndPlaced;
					if (!piece.isBuiltIntoTable && flag)
					{
						this.table.builderNetworking.RequestRecyclePiece(piece.pieceId, piece.transform.position, piece.transform.rotation, true, -1);
						return;
					}
				}
				else
				{
					this.table.builderNetworking.PieceEnteredDropZone(piece, this.dropType, this.dropZoneID);
				}
			}
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000A9E02 File Offset: 0x000A8002
	public Vector3 GetRepelDirectionWorld()
	{
		return this.repelDirectionWorld;
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000A9E0C File Offset: 0x000A800C
	public void PlayEffect()
	{
		if (this.vfxRoot != null && !this.playingEffect)
		{
			this.vfxRoot.SetActive(true);
			this.playingEffect = true;
			if (this.sfxPrefab != null)
			{
				ObjectPools.instance.Instantiate(this.sfxPrefab, base.transform.position, base.transform.rotation, true);
			}
			base.StartCoroutine(this.DelayedStopEffect());
		}
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000A9E85 File Offset: 0x000A8085
	private IEnumerator DelayedStopEffect()
	{
		yield return new WaitForSeconds(this.effectDuration);
		this.vfxRoot.SetActive(false);
		this.playingEffect = false;
		yield break;
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000A9E94 File Offset: 0x000A8094
	private void OnTriggerExit(Collider other)
	{
		if (this.onEnter)
		{
			return;
		}
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		BuilderPieceCollider component = other.GetComponent<BuilderPieceCollider>();
		if (component != null)
		{
			BuilderPiece piece = component.piece;
			if (this.table != null && this.table.builderNetworking != null)
			{
				if (piece == null)
				{
					return;
				}
				if (this.dropType == BuilderDropZone.DropType.Recycle)
				{
					bool flag = piece.state != BuilderPiece.State.Displayed && piece.state != BuilderPiece.State.OnShelf && piece.state > BuilderPiece.State.AttachedAndPlaced;
					if (!piece.isBuiltIntoTable && flag)
					{
						this.table.builderNetworking.RequestRecyclePiece(piece.pieceId, piece.transform.position, piece.transform.rotation, true, -1);
						return;
					}
				}
				else
				{
					this.table.builderNetworking.PieceEnteredDropZone(piece, this.dropType, this.dropZoneID);
				}
			}
		}
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000A9F7E File Offset: 0x000A817E
	public BuilderDropZone()
	{
	}

	// Token: 0x040028E3 RID: 10467
	[SerializeField]
	private BuilderDropZone.DropType dropType;

	// Token: 0x040028E4 RID: 10468
	[SerializeField]
	private bool onEnter = true;

	// Token: 0x040028E5 RID: 10469
	[SerializeField]
	private GameObject vfxRoot;

	// Token: 0x040028E6 RID: 10470
	[SerializeField]
	private GameObject sfxPrefab;

	// Token: 0x040028E7 RID: 10471
	public float effectDuration = 1f;

	// Token: 0x040028E8 RID: 10472
	private bool playingEffect;

	// Token: 0x040028E9 RID: 10473
	public bool overrideDirection;

	// Token: 0x040028EA RID: 10474
	[SerializeField]
	private Vector3 repelDirectionLocal = Vector3.up;

	// Token: 0x040028EB RID: 10475
	private Vector3 repelDirectionWorld = Vector3.up;

	// Token: 0x040028EC RID: 10476
	[HideInInspector]
	public int dropZoneID = -1;

	// Token: 0x040028ED RID: 10477
	internal BuilderTable table;

	// Token: 0x02000527 RID: 1319
	public enum DropType
	{
		// Token: 0x040028EF RID: 10479
		Invalid = -1,
		// Token: 0x040028F0 RID: 10480
		Repel,
		// Token: 0x040028F1 RID: 10481
		ReturnToShelf,
		// Token: 0x040028F2 RID: 10482
		BreakApart,
		// Token: 0x040028F3 RID: 10483
		Recycle
	}

	// Token: 0x02000528 RID: 1320
	[CompilerGenerated]
	private sealed class <DelayedStopEffect>d__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06002025 RID: 8229 RVA: 0x000A9FB5 File Offset: 0x000A81B5
		[DebuggerHidden]
		public <DelayedStopEffect>d__16(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000A9FC4 File Offset: 0x000A81C4
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			BuilderDropZone builderDropZone = this;
			if (num == 0)
			{
				this.<>1__state = -1;
				this.<>2__current = new WaitForSeconds(builderDropZone.effectDuration);
				this.<>1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.<>1__state = -1;
			builderDropZone.vfxRoot.SetActive(false);
			builderDropZone.playingEffect = false;
			return false;
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x000AA024 File Offset: 0x000A8224
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x000AA024 File Offset: 0x000A8224
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040028F4 RID: 10484
		private int <>1__state;

		// Token: 0x040028F5 RID: 10485
		private object <>2__current;

		// Token: 0x040028F6 RID: 10486
		public BuilderDropZone <>4__this;
	}
}
