using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class BuilderDispenser : MonoBehaviour
{
	// Token: 0x06001FF9 RID: 8185 RVA: 0x000A8CB4 File Offset: 0x000A6EB4
	private void Awake()
	{
		this.nullPiece = new BuilderPieceSet.PieceInfo
		{
			piecePrefab = null,
			overrideSetMaterial = false
		};
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000A8CE0 File Offset: 0x000A6EE0
	public void UpdateDispenser()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (!this.hasPiece && Time.timeAsDouble > this.nextSpawnTime && this.pieceToSpawn.piecePrefab != null)
		{
			this.TrySpawnPiece();
			this.nextSpawnTime = Time.timeAsDouble + (double)this.spawnRetryDelay;
			return;
		}
		if (this.hasPiece && (this.spawnedPieceInstance == null || (this.spawnedPieceInstance.state != BuilderPiece.State.OnShelf && this.spawnedPieceInstance.state != BuilderPiece.State.Displayed)))
		{
			base.StopAllCoroutines();
			if (this.spawnedPieceInstance != null)
			{
				this.spawnedPieceInstance.shelfOwner = -1;
			}
			this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
			this.spawnedPieceInstance = null;
			this.hasPiece = false;
		}
	}

	// Token: 0x06001FFB RID: 8187 RVA: 0x000A8DAC File Offset: 0x000A6FAC
	public bool DoesPieceMatchSpawnInfo(BuilderPiece piece)
	{
		if (piece == null || this.pieceToSpawn.piecePrefab == null)
		{
			return false;
		}
		if (piece.pieceType != this.pieceToSpawn.piecePrefab.name.GetStaticHash())
		{
			return false;
		}
		if (!(piece.materialOptions != null))
		{
			return true;
		}
		int num = piece.materialType;
		int num2;
		Material material;
		int num3;
		piece.materialOptions.GetDefaultMaterial(out num2, out material, out num3);
		if (this.pieceToSpawn.overrideSetMaterial)
		{
			for (int i = 0; i < this.pieceToSpawn.pieceMaterialTypes.Length; i++)
			{
				string text = this.pieceToSpawn.pieceMaterialTypes[i];
				if (!string.IsNullOrEmpty(text))
				{
					int hashCode = text.GetHashCode();
					if (hashCode == num)
					{
						return true;
					}
					if (hashCode == num2 && num == -1)
					{
						return true;
					}
				}
				else if (num == -1 || num == num2)
				{
					return true;
				}
			}
		}
		else if (num == this.materialType || (this.materialType == num2 && num == -1) || (num == num2 && this.materialType == -1))
		{
			return true;
		}
		return false;
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x000A8EB0 File Offset: 0x000A70B0
	public void ShelfPieceCreated(BuilderPiece piece, bool playAnimation)
	{
		if (this.DoesPieceMatchSpawnInfo(piece))
		{
			if (this.hasPiece && this.spawnedPieceInstance != null)
			{
				this.spawnedPieceInstance.shelfOwner = -1;
			}
			this.spawnedPieceInstance = piece;
			this.hasPiece = true;
			this.spawnCount++;
			this.spawnCount = Mathf.Max(0, this.spawnCount);
			if (this.table.GetTableState() == BuilderTable.TableState.Ready && playAnimation)
			{
				base.StartCoroutine(this.PlayAnimation());
				if (this.playFX)
				{
					ObjectPools.instance.Instantiate(this.dispenserFX, this.spawnTransform.position, this.spawnTransform.rotation, true);
					return;
				}
				this.playFX = true;
				return;
			}
			else
			{
				Vector3 desiredShelfOffset = this.pieceToSpawn.piecePrefab.desiredShelfOffset;
				Vector3 vector = this.displayTransform.position + this.displayTransform.rotation * desiredShelfOffset;
				Quaternion quaternion = this.displayTransform.rotation * Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
				this.spawnedPieceInstance.transform.SetPositionAndRotation(vector, quaternion);
				this.spawnedPieceInstance.SetState(BuilderPiece.State.OnShelf, false);
				this.playFX = true;
			}
		}
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x000A8FF0 File Offset: 0x000A71F0
	private IEnumerator PlayAnimation()
	{
		this.spawnedPieceInstance.SetState(BuilderPiece.State.Displayed, false);
		this.animateParent.Rewind();
		this.spawnedPieceInstance.transform.SetParent(this.animateParent.transform);
		this.spawnedPieceInstance.transform.SetLocalPositionAndRotation(this.pieceToSpawn.piecePrefab.desiredShelfOffset, Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset));
		this.animateParent.Play();
		yield return new WaitForSeconds(this.animateParent.clip.length);
		if (this.spawnedPieceInstance != null && this.spawnedPieceInstance.state == BuilderPiece.State.Displayed)
		{
			this.spawnedPieceInstance.transform.SetParent(null);
			Vector3 desiredShelfOffset = this.pieceToSpawn.piecePrefab.desiredShelfOffset;
			Vector3 vector = this.displayTransform.position + this.displayTransform.rotation * desiredShelfOffset;
			Quaternion quaternion = this.displayTransform.rotation * Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
			this.spawnedPieceInstance.transform.SetPositionAndRotation(vector, quaternion);
			this.spawnedPieceInstance.SetState(BuilderPiece.State.OnShelf, false);
		}
		yield break;
	}

	// Token: 0x06001FFE RID: 8190 RVA: 0x000A9000 File Offset: 0x000A7200
	public void ShelfPieceRecycled(BuilderPiece piece)
	{
		if (piece != null && this.spawnedPieceInstance != null && piece.Equals(this.spawnedPieceInstance))
		{
			piece.shelfOwner = -1;
			this.spawnedPieceInstance = null;
			this.hasPiece = false;
			this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
		}
	}

	// Token: 0x06001FFF RID: 8191 RVA: 0x000A905C File Offset: 0x000A725C
	public void AssignPieceType(BuilderPieceSet.PieceInfo piece, int inMaterialType)
	{
		this.playFX = false;
		this.pieceToSpawn = piece;
		this.materialType = inMaterialType;
		this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
		this.currentAnimation = this.dispenseDefaultAnimation;
		this.animateParent.clip = this.currentAnimation;
		this.spawnCount = 0;
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000A90B8 File Offset: 0x000A72B8
	private void TrySpawnPiece()
	{
		if (this.spawnedPieceInstance != null && this.spawnedPieceInstance.state == BuilderPiece.State.OnShelf)
		{
			return;
		}
		if (this.pieceToSpawn.piecePrefab == null)
		{
			return;
		}
		if (this.table.HasEnoughResources(this.pieceToSpawn.piecePrefab))
		{
			Vector3 desiredShelfOffset = this.pieceToSpawn.piecePrefab.desiredShelfOffset;
			Vector3 vector = this.spawnTransform.position + this.spawnTransform.rotation * desiredShelfOffset;
			Quaternion quaternion = this.spawnTransform.rotation * Quaternion.Euler(this.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
			int num = this.materialType;
			if (this.pieceToSpawn.overrideSetMaterial && this.pieceToSpawn.pieceMaterialTypes.Length != 0)
			{
				int num2 = this.spawnCount % this.pieceToSpawn.pieceMaterialTypes.Length;
				string text = this.pieceToSpawn.pieceMaterialTypes[num2];
				if (string.IsNullOrEmpty(text))
				{
					num = -1;
				}
				else
				{
					num = text.GetHashCode();
				}
			}
			this.table.RequestCreateDispenserShelfPiece(this.pieceToSpawn.piecePrefab.name.GetStaticHash(), vector, quaternion, num, this.shelfID);
		}
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000A91F0 File Offset: 0x000A73F0
	public void ParentPieceToShelf(Transform shelfTransform)
	{
		if (this.spawnedPieceInstance != null)
		{
			if (this.spawnedPieceInstance.state != BuilderPiece.State.OnShelf && this.spawnedPieceInstance.state != BuilderPiece.State.Displayed)
			{
				base.StopAllCoroutines();
				if (this.spawnedPieceInstance != null)
				{
					this.spawnedPieceInstance.shelfOwner = -1;
				}
				this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
				this.spawnedPieceInstance = null;
				this.hasPiece = false;
				return;
			}
			this.spawnedPieceInstance.SetState(BuilderPiece.State.Displayed, false);
			this.spawnedPieceInstance.transform.SetParent(shelfTransform);
		}
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000A9288 File Offset: 0x000A7488
	public void ClearDispenser()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		this.pieceToSpawn = this.nullPiece;
		this.hasPiece = false;
		if (this.spawnedPieceInstance != null)
		{
			if (this.spawnedPieceInstance.state != BuilderPiece.State.OnShelf && this.spawnedPieceInstance.state != BuilderPiece.State.Displayed)
			{
				this.spawnedPieceInstance.shelfOwner = -1;
				this.nextSpawnTime = Time.timeAsDouble + (double)this.OnGrabSpawnDelay;
				this.spawnedPieceInstance = null;
				return;
			}
			this.table.RequestRecyclePiece(this.spawnedPieceInstance, false, -1);
		}
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000A9314 File Offset: 0x000A7514
	public void OnClearTable()
	{
		this.playFX = false;
		this.nextSpawnTime = 0.0;
		this.hasPiece = false;
		this.spawnedPieceInstance = null;
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000A933A File Offset: 0x000A753A
	public BuilderDispenser()
	{
	}

	// Token: 0x040028B8 RID: 10424
	public Transform displayTransform;

	// Token: 0x040028B9 RID: 10425
	public Transform spawnTransform;

	// Token: 0x040028BA RID: 10426
	public Animation animateParent;

	// Token: 0x040028BB RID: 10427
	public AnimationClip dispenseDefaultAnimation;

	// Token: 0x040028BC RID: 10428
	public GameObject dispenserFX;

	// Token: 0x040028BD RID: 10429
	private AnimationClip currentAnimation;

	// Token: 0x040028BE RID: 10430
	[HideInInspector]
	public BuilderTable table;

	// Token: 0x040028BF RID: 10431
	[HideInInspector]
	public int shelfID;

	// Token: 0x040028C0 RID: 10432
	private BuilderPieceSet.PieceInfo pieceToSpawn;

	// Token: 0x040028C1 RID: 10433
	private BuilderPiece spawnedPieceInstance;

	// Token: 0x040028C2 RID: 10434
	private int materialType = -1;

	// Token: 0x040028C3 RID: 10435
	private BuilderPieceSet.PieceInfo nullPiece;

	// Token: 0x040028C4 RID: 10436
	private int spawnCount;

	// Token: 0x040028C5 RID: 10437
	private double nextSpawnTime;

	// Token: 0x040028C6 RID: 10438
	private bool hasPiece;

	// Token: 0x040028C7 RID: 10439
	private float OnGrabSpawnDelay = 0.5f;

	// Token: 0x040028C8 RID: 10440
	private float spawnRetryDelay = 2f;

	// Token: 0x040028C9 RID: 10441
	private bool playFX;

	// Token: 0x02000524 RID: 1316
	[CompilerGenerated]
	private sealed class <PlayAnimation>d__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x06002005 RID: 8197 RVA: 0x000A935F File Offset: 0x000A755F
		[DebuggerHidden]
		public <PlayAnimation>d__22(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000023F5 File Offset: 0x000005F5
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000A9370 File Offset: 0x000A7570
		bool IEnumerator.MoveNext()
		{
			int num = this.<>1__state;
			BuilderDispenser builderDispenser = this;
			if (num == 0)
			{
				this.<>1__state = -1;
				builderDispenser.spawnedPieceInstance.SetState(BuilderPiece.State.Displayed, false);
				builderDispenser.animateParent.Rewind();
				builderDispenser.spawnedPieceInstance.transform.SetParent(builderDispenser.animateParent.transform);
				builderDispenser.spawnedPieceInstance.transform.SetLocalPositionAndRotation(builderDispenser.pieceToSpawn.piecePrefab.desiredShelfOffset, Quaternion.Euler(builderDispenser.pieceToSpawn.piecePrefab.desiredShelfRotationOffset));
				builderDispenser.animateParent.Play();
				this.<>2__current = new WaitForSeconds(builderDispenser.animateParent.clip.length);
				this.<>1__state = 1;
				return true;
			}
			if (num != 1)
			{
				return false;
			}
			this.<>1__state = -1;
			if (builderDispenser.spawnedPieceInstance != null && builderDispenser.spawnedPieceInstance.state == BuilderPiece.State.Displayed)
			{
				builderDispenser.spawnedPieceInstance.transform.SetParent(null);
				Vector3 desiredShelfOffset = builderDispenser.pieceToSpawn.piecePrefab.desiredShelfOffset;
				Vector3 vector = builderDispenser.displayTransform.position + builderDispenser.displayTransform.rotation * desiredShelfOffset;
				Quaternion quaternion = builderDispenser.displayTransform.rotation * Quaternion.Euler(builderDispenser.pieceToSpawn.piecePrefab.desiredShelfRotationOffset);
				builderDispenser.spawnedPieceInstance.transform.SetPositionAndRotation(vector, quaternion);
				builderDispenser.spawnedPieceInstance.SetState(BuilderPiece.State.OnShelf, false);
			}
			return false;
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x000A94EB File Offset: 0x000A76EB
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00004D7F File Offset: 0x00002F7F
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x000A94EB File Offset: 0x000A76EB
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.<>2__current;
			}
		}

		// Token: 0x040028CA RID: 10442
		private int <>1__state;

		// Token: 0x040028CB RID: 10443
		private object <>2__current;

		// Token: 0x040028CC RID: 10444
		public BuilderDispenser <>4__this;
	}
}
