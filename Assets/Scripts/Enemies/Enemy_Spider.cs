using UnityEngine;

public class Enemy_Spider : Enemy
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.gravityScale = 12f;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Recibir daño
    /// </summary>
    public override void TakeDamage(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.TakeDamage(_damageDone, _hitDirection, _hitForce);
    }

}
