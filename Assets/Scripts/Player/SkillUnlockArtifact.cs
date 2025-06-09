using UnityEngine;

public class SkillUnlockArtifact : MonoBehaviour
{
    public PlayerStateList.SkillType skillToUnlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateList playerState = other.GetComponent<PlayerStateList>();

            if (playerState != null)
            {
                playerState.UnlockSkill(skillToUnlock);
                Destroy(gameObject);
            }
        }
    }
}
