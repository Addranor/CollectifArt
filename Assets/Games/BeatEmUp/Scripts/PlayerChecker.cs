using Collectif.BeatEmUp;
using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    public Entity entity;
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (entity is null) return;
        if (col.CompareTag("BEU_Enemy")) entity.AddEnemy(col.GetComponent<Entity>());
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (entity is null) return;
        if (col.CompareTag("BEU_Enemy")) entity.RemoveEnemy(col.GetComponent<Entity>());
    }
}
