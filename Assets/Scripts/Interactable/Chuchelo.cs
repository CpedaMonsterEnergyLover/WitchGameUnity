using UnityEngine;

public class Chuchelo : Interactable, IFlammableInteractable
{
    public ParticleSystem fire;
    public ParticleSystem smoke;
    public GameObject bossGO;
    
    public bool Flame()
    {
        Debug.Log("Flame");
        fire.gameObject.SetActive(true);
        smoke.gameObject.SetActive(true);
        fire.Play();
        smoke.Play();
        SetUnloadable();
        Invoke(nameof(SpawnBoss), 3.5f);
        return true;
    }

    private void SetUnloadable() => tile.IsBlockedForLoading = true;
    private void SetLoadable() => tile.IsBlockedForLoading = false;
    
    private void SpawnBoss()
    {
        SetLoadable();
        var position = new Vector3(tile.Position.x, tile.Position.y, 0);
        BattleArena.Instance.SetPosition(position);
        BattleArena.Instance.PaintArena();
        Boss boss = Instantiate(bossGO).GetComponent<Boss>();
        boss.transform.position = position;
        Kill();
    }
}
