using UnityEngine;
using System.Collections;

public class SkillUnlockArtifact : MonoBehaviour
{
    public PlayerStateList.SkillType skillToUnlock;

    [SerializeField] private GameObject skillUnlockUI = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateList playerState = other.GetComponent<PlayerStateList>();

            if (playerState != null)
            {
                playerState.UnlockSkill(skillToUnlock);
                if (skillUnlockUI) skillUnlockUI.GetComponent<NewSkillUnlockShow>().Show();
                Destroy(gameObject);
            }
        }
    }
}
