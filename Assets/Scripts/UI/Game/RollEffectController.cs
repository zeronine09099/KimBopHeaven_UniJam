using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

public class RollEffectController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    [SerializeField] private Transform endPos;
    [SerializeField] private int direction = 0;     // 0: up, 1: down, 2: left, 3: right

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (endPos != null)
        {
            float distance = Vector2.Distance(transform.position, endPos.position);
            if (distance < 0.1f)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(this.transform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.InBack));
                seq.Append(this.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
                Destroy(gameObject, 0.5f);
            }
        }
    }

    public void SetInfo(Transform endPos, int direction)
    {
        this.endPos = endPos;
        this.direction = direction;

        switch (direction)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.linearVelocityY = speed;
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                rb.linearVelocityY = - speed;
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                rb.linearVelocityX = -speed;
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                rb.linearVelocityX = speed;
                break;
            default:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }

    }


}
