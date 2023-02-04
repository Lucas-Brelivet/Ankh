using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private BoxCollider2D bounds;
    [SerializeField] private AnimationCurve knockbackCurve;
    [SerializeField] private Health health;
    public float maxY = 1;
    private bool facingRight = true;
    public float xSpeed = 2;
    public float ySpeed = 1;
    [SerializeField] private Collider col;
    private Coroutine knockbackRoutine;


    public void Move(Vector2 moveDirection)
    {
        if (health.Stunned()) return;
        if (moveDirection.x < 0) facingRight = false;
        else if (moveDirection.x > 0) facingRight = true;
        if ((moveDirection.x > 0 && transform.localScale.x < 0) || (moveDirection.x < 0 && transform.localScale.x > 0))
            Flip();
        Vector3 nextPosition = transform.position + Vector3.right * moveDirection.x * xSpeed * Time.deltaTime;
        if (ConfirmXMove((nextPosition - transform.position).x))
        {
            transform.position = nextPosition;
        }
        if (Global.depthSlope % 180 != 0)
        {
            nextPosition = transform.position + moveDirection.y * ySpeed * Time.deltaTime * (Vector3.up + Vector3.forward / Mathf.Tan(Global.depthSlope / 180 * Mathf.PI));
            if (ConfirmYMove((nextPosition - transform.position).y)) {
                transform.position = nextPosition;
            }
        }
    }

    private bool ConfirmXMove(float xDelta)
    {
        bool minCondition = xDelta > 0 || col.bounds.min.x + xDelta > bounds.bounds.min.x;
        bool maxCondition = xDelta < 0 || col.bounds.max.x + xDelta < bounds.bounds.max.x;
        return minCondition && maxCondition;
    }
    private bool ConfirmYMove(float yDelta)
    {
        bool minCondition = yDelta > 0 || col.bounds.min.y + yDelta > bounds.bounds.min.y;
        bool maxCondition = yDelta < 0 || (col.bounds.max.y + yDelta < bounds.bounds.max.y && col.bounds.min.y + yDelta < maxY);
        return minCondition && maxCondition;
    }

    public bool isFacingRight()
    {
        return facingRight;
    }

    public void Knockback(float distance, float duration, bool knockbackedRight)
    {
        if (knockbackRoutine != null)
            StopCoroutine(knockbackRoutine);
        knockbackRoutine = StartCoroutine(KnockbackRoutine(distance, duration, knockbackedRight));
    }
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }
    public IEnumerator KnockbackRoutine(float distance, float duration, bool knockbackedRight)
    {
        float currentTime = 0;
        float currentDistance = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            if (currentTime > duration) currentTime = 0;
            transform.Translate(Vector3.right * (knockbackedRight ? 1 : -1) * (knockbackCurve.Evaluate(currentTime) - currentDistance));
            currentDistance = knockbackCurve.Evaluate(currentTime);
            yield return 0;
        }
    }
}
