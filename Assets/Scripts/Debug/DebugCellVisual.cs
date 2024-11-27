using UnityEngine;
using UnityEngine.UI;

public class DebugCellVisual : MonoBehaviour
{
    [SerializeField] private Image _image = null;

    public void SetAlive(bool isAlive)
    {
        _image.color = isAlive ? Color.green : Color.white;
    }
}
