using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DynamicHorizontalLayout : MonoBehaviour
{
    private HorizontalLayoutGroup layoutGroup;

    private void Awake()
    {
        layoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        // Get all child objects
        Transform[] childObjects = new Transform[transform.childCount];
        for (int i = 0; i < childObjects.Length; i++)
        {
            childObjects[i] = transform.GetChild(i);
        }

        // Set the preferred width of the layout group to fit all child objects
        float preferredWidth = layoutGroup.padding.left + layoutGroup.padding.right;
        for (int i = 0; i < childObjects.Length; i++)
        {
            preferredWidth += childObjects[i].GetComponent<RectTransform>().rect.width + layoutGroup.spacing;
        }
        
        // Set the new preferred width and force the layout group to update its layout
        // layoutGroup.preferredWidth = preferredWidth;
        layoutGroup.SetLayoutHorizontal();
    }

    // Called whenever a child object is added or removed from the hierarchy
    private void OnTransformChildrenChanged()
    {
        UpdateLayout();
    }
}