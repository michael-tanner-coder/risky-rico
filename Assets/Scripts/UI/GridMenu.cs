using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GridMenu : Menu
{
    [Header("CONTENT")]
    [SerializeField] private List<ItemData> _itemList = new List<ItemData>();

    [Header("GRID")]
    [Tooltip("The UI object to represent a cell in the grid")]
    [SerializeField] private GameObject _gridItem;
    private List<GameObject> _gridItems = new List<GameObject>();
    [SerializeField] private GridLayoutGroup _gridLayout;

    void Start()
    {
        base.Start();
        SpawnGrid();
    }

    void Update()
    {
        if (_input.actions["navigate"].triggered)
        {
            int newCursorRow = (int) _input.actions["navigate"].ReadValue<Vector2>().y * -1 * _gridLayout.constraintCount;
            int newCursorColumn = (int) _input.actions["navigate"].ReadValue<Vector2>().x;
            int newCursorIndex = _cursorIndex + newCursorColumn + newCursorRow;
            SetCursorIndex(newCursorIndex);
            SetActiveInput();
        }

         SetCursorPosition();
    }

    void SetCursorPosition()
    {
        if (_fields.Count > 0 && _fields[_cursorIndex] != null)
        {
            GameObject currentItem = _gridItems[_cursorIndex].gameObject;
            Vector3 currentItemPosition = currentItem.transform.position;
            _cursor.transform.position = currentItemPosition;
        }
    }

    void SpawnGrid()
    {
       foreach (ItemData item in _itemList)
       {
            // spawn the next item in the grid 
            GameObject newGridItem = Instantiate(_gridItem, transform.position, transform.rotation);
            newGridItem.transform.parent = gameObject.transform;
            _gridItems.Add(newGridItem);
            
            // reset rect scale of UI element
            RectTransform rect = newGridItem.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localScale = new Vector3(1f, 1f, 1f);
            }

            // associate item data with shop item field
            ShopItemField itemField = newGridItem.GetComponent<ShopItemField>();
            if (itemField != null)
            {
                itemField.SetItemData(item);
                _fields.Add(itemField);
            }
       }
    }
}
