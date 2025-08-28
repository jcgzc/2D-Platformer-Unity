using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;          // Hastighet
    public float patrolDistance = 2f;  // Hur långt fram & tillbaka

    private Vector2 startPos;
    private int direction = 1;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        startPos = transform.position;

        // Slumpa start-riktning så inte alla går åt samma håll
        direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
    }

    void Update()
    {
        // Flytta monster horisontellt
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Spela Run-animation
        if (anim != null)
            anim.SetBool("isRunning", true);

        // Kontrollera om patrull-avståndet är nått
        float distanceFromStart = Mathf.Abs(transform.position.x - startPos.x);
        if (distanceFromStart >= patrolDistance)
        {
            // Byt riktning
            direction *= -1;

            // Flippa sprite horisontellt
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
            transform.localScale = scale;

            // Uppdatera startPos för nästa riktning
            startPos = transform.position;
        }
    }

}
