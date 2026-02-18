using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip;
    public UI_SkillTree skillTree;
    private bool skillTreeEnabled;

    [Header("Pause details (made with the help of AI)")]
    private float previousTimeScale = 1f;
    private float previousFixedDeltaTime = 0.02f;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }

    public void ToggleSkillTree()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);

        #region Pause Game (made with the help of AI)
        if (skillTreeEnabled)
        {
            previousTimeScale = Time.timeScale;
            previousFixedDeltaTime = Time.fixedDeltaTime;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
        }
        else
        {
            Time.timeScale = previousTimeScale;
            Time.fixedDeltaTime = previousFixedDeltaTime;
        }
        #endregion
    }
}
