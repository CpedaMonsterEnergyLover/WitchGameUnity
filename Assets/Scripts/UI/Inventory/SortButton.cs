using UnityEngine;
using UnityEngine.UI;

public class SortButton : MonoBehaviour
{
    public Text sortingModeText;
    public SortingMode currentSortingMode = SortingMode.Name;

    public void ToggleSortingMode()
    {
        currentSortingMode = currentSortingMode switch
        {
            SortingMode.Name => SortingMode.Amount,
            SortingMode.Amount => SortingMode.Type,
            SortingMode.Type => SortingMode.Name,
            _ => currentSortingMode
        };

        sortingModeText.text = currentSortingMode.ToString()[0].ToString();
    }
}

public enum SortingMode
{
    Name,
    Amount,
    Type
}