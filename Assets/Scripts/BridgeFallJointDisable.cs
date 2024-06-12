using System.Collections;
using UnityEngine;

public class BridgeFallJointDisable : MonoBehaviour
{
    private HingeJoint2D _hingeJoint;

    public void Start()
    {
        _hingeJoint = GetComponent<HingeJoint2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (_hingeJoint is null) return;
        StartCoroutine(DisableHingeJointAfterDelay(0.1f));
    }

    private IEnumerator DisableHingeJointAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _hingeJoint.enabled = false;
    }
}
