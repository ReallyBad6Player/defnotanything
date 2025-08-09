using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class BuilderArmShelf : MonoBehaviour
{
	// Token: 0x06001FD7 RID: 8151 RVA: 0x000A81B8 File Offset: 0x000A63B8
	private void Start()
	{
		this.ownerRig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001FD8 RID: 8152 RVA: 0x000A81C6 File Offset: 0x000A63C6
	public bool IsOwnedLocally()
	{
		return this.ownerRig != null && this.ownerRig.isLocal;
	}

	// Token: 0x06001FD9 RID: 8153 RVA: 0x000A81E3 File Offset: 0x000A63E3
	public bool CanAttachToArmPiece()
	{
		return this.ownerRig != null && this.ownerRig.scaleFactor >= 1f;
	}

	// Token: 0x06001FDA RID: 8154 RVA: 0x000A820C File Offset: 0x000A640C
	public void DropAttachedPieces()
	{
		if (this.ownerRig != null && this.piece != null)
		{
			Vector3 vector = Vector3.zero;
			if (this.piece.firstChildPiece == null)
			{
				return;
			}
			BuilderTable table = this.piece.GetTable();
			Vector3 vector2 = table.roomCenter.position - this.piece.transform.position;
			vector2.Normalize();
			Vector3 vector3 = Quaternion.Euler(0f, 180f, 0f) * vector2;
			vector = BuilderTable.DROP_ZONE_REPEL * vector3;
			BuilderPiece builderPiece = this.piece.firstChildPiece;
			while (builderPiece != null)
			{
				table.RequestDropPiece(builderPiece, builderPiece.transform.position + vector3 * 0.1f, builderPiece.transform.rotation, vector, Vector3.zero);
				builderPiece = builderPiece.nextSiblingPiece;
			}
		}
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000026E9 File Offset: 0x000008E9
	public BuilderArmShelf()
	{
	}

	// Token: 0x0400289A RID: 10394
	[HideInInspector]
	public BuilderPiece piece;

	// Token: 0x0400289B RID: 10395
	public Transform pieceAnchor;

	// Token: 0x0400289C RID: 10396
	private VRRig ownerRig;
}
