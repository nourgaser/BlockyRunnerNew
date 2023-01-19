using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] Vector3 dist;

    // Animation start delay in milliseconds. 
    [SerializeField] float delay = 0f;
    [SerializeField] float speed = 1f;

    Vector3 orgPos;

    Animation anim;
    AnimationClip clip;
    private void Awake()
    {
        if (dist == null || dist == Vector3.zero)
        {
            Debug.Log($"End position not set; cannot move. ({gameObject.name}[{gameObject.tag}] | {gameObject.GetInstanceID()})");
            this.enabled = false;
            return;
        }

        orgPos = transform.localPosition;
        anim = gameObject.AddComponent<Animation>();
        anim.wrapMode = WrapMode.PingPong;
    }

    private void Start()
    {
        Invoke("SetClipAndPlay", delay / 1000);
    }

    void SetClipAndPlay()
    {
        anim.Stop();
        if (clip != null) anim.RemoveClip(clip);

        clip = new AnimationClip();
        clip.legacy = true;

        // Clamped; guaranteed to be in bounds of the 16x20 chunk bounds.
        var endPos = dist + orgPos;
        endPos.x = Mathf.Clamp(endPos.x, -5f + transform.localScale.x / 2, 11f - transform.localScale.x / 2);
        endPos.y = Mathf.Clamp(endPos.y, 1f + transform.localScale.y / 2, 20f - transform.localScale.y / 2);

        // update the clip to a change the red color
        clip.SetCurve("", typeof(Transform), "localPosition.x", AnimationCurve.Linear(0.0f, orgPos.x, Mathf.Abs(endPos.x - orgPos.x) / speed, endPos.x));
        clip.SetCurve("", typeof(Transform), "localPosition.y", AnimationCurve.Linear(0.0f, orgPos.y, Mathf.Abs(endPos.y - orgPos.y) / speed, endPos.y));
        clip.SetCurve("", typeof(Transform), "localPosition.z", AnimationCurve.Linear(0.0f, orgPos.z, Mathf.Abs(endPos.z - orgPos.z) / speed, orgPos.z + endPos.z));

        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);

        // For testing only, allows changes in serialized variables to reflect
        Invoke("SetClipAndPlay", clip.length * 2);
    }
}
