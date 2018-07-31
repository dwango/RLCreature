using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public class ToolBar
    {
        private const string ToolBarCategoryButtonPrefabPath = "ToolBarCategoryElement";
        private const string ToolBarButtonPrefabPath = "ToolBarElement";
        
        private readonly Dictionary<string, Dictionary<string, Action>> _actions = new Dictionary<string, Dictionary<string, Action>>();

        private readonly RectTransform _categoryListContent;
        private readonly RectTransform _listContent;
        private readonly RectTransform _listRoot;

        private readonly ToolBarCell _hideListButton;

        public bool ListVisible
        {
            get { return _listRoot.gameObject.activeSelf; }
            set
            {
                _listRoot.gameObject.SetActive(value);
            }
        }

        public ToolBar(RectTransform categoryListContent, RectTransform listContent, RectTransform listRoot)
        {
            _categoryListContent = categoryListContent;
            _listContent = listContent;
            _listRoot = listRoot;

            // Menu Off Button
            _hideListButton = CreateButton();
            _hideListButton.Button.onClick.AddListener(() =>
            {
                ListVisible = false;
            });
            _hideListButton.Name.text = "OFF";
            ListVisible = false;
        }

        public void Add(IToolBarActions actions)
        {
            var category = actions.GetCategory();
            CreateCategory(category);
            foreach (var kv in actions.GetActions())
            {
                _actions[category].Add(kv.Key, kv.Value);
            }
        }
        
        public void Add(string category, string name, Action action)
        {
            CreateCategory(category);
            _actions[category].Add(name, action);
        }

        private ToolBarCell CreateButton()
        {
            var btn = UnityEngine.Object.Instantiate(Resources.Load<ToolBarCell>(ToolBarCategoryButtonPrefabPath));
            btn.GetComponent<RectTransform>().SetParent(_categoryListContent, false);
            return btn;
        }

        private void CreateCategory(string category)
        {
            if (_actions.ContainsKey(category)) return;
            
            _actions.Add(category, new Dictionary<string, Action>());

            var btn = CreateButton();
            btn.Button.onClick.AddListener(() => Select(category));
            btn.Name.text = category;
        }

        public void Select(string category)
        {
            if (!_actions.ContainsKey(category)) return;
            
            RemoveAllTools();
            _listContent.anchoredPosition = Vector2.zero;
            foreach (var kv in _actions[category])
            {
                AddTool(kv.Key, kv.Value);
            }
            if (!ListVisible)
            {
                ListVisible = true;
            }
        }

        private void RemoveAllTools()
        {
            var children = new List<GameObject>();
            for (var i = 0; i < _listContent.childCount; ++i)
            {
                children.Add(_listContent.GetChild(i).gameObject);
            }
            foreach (var child in children)
            {
                GameObject.Destroy(child);
            }
        }
        
        private void AddTool(string name, Action action)
        {
            var button = UnityEngine.Object.Instantiate(Resources.Load<ToolBarCell>(ToolBarButtonPrefabPath));
            button.GetComponent<RectTransform>().SetParent(_listContent, false);
            button.Button.onClick.AddListener(() => action());
            button.Name.text = name;
        }
    }
}