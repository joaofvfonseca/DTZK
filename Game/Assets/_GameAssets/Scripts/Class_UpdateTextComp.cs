using UnityEngine;

public class Class_UpdateTextComp : MonoBehaviour
{
    [SerializeField]
    protected string text;
    protected int param;

    public virtual void GoAndUpdate()
    {
        Debug.Log("parent being called, not child");
    }
}
