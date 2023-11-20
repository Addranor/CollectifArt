using Collectif.BeatEmUp;
using UnityEngine;

public class EnemyChecker : MonoBehaviour
{
    public Entity entity;
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (entity is null) return;
        if (col.CompareTag("BEU_Player")) entity.IsPlayerHere = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (entity is null) return;
        if (col.CompareTag("BEU_Player")) entity.IsPlayerHere = false;
    }
}
