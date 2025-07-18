using UnityEngine;

public class IceMagicCircle : MonoBehaviour
{
    public float duration = 1f;

    private SpriteRenderer sr;
    private float timer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Fade in no primeiro meio segundo
        if (timer <= duration / 2)
        {
            float alpha = Mathf.Lerp(0f, 1f, timer / (duration / 2));
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        // Fade out no segundo meio segundo
        else if (timer <= duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, (timer - duration / 2) / (duration / 2));
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
