using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
#if NET_4_6

#endif

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public class CreatureInfoList
    {
        public IObservable<CreatureInfoCell> SelectedCell
        {
            get { return _selectedCellSubject; }
        }

        private readonly ReactiveProperty<CreatureInfoCell> _selectedCellSubject =
            new ReactiveProperty<CreatureInfoCell>();

        private readonly RectTransform _contentRect;

        private readonly Dictionary<GameObject, CreatureInfoCell> _cells =
            new Dictionary<GameObject, CreatureInfoCell>();

        private static readonly string CreatureCellPrefabPath = "CreatureListElement";

        public CreatureInfoList(RectTransform content)
        {
            _contentRect = content;
        }

        public void Select(GameObject creature)
        {
            if (creature == null || !_cells.ContainsKey(creature))
            {
                foreach (var cell in _cells)
                {
                    cell.Value.Selected.Value = false;
                }
                _selectedCellSubject.Value = null;
            }
            else
            {
                _cells[creature].Selected.Value = true;
            }
        }

        public void Add(GameObject creature, string name)
        {
            var cell = Object.Instantiate(Resources.Load<CreatureInfoCell>(CreatureCellPrefabPath));
            cell.Creature = creature;
            cell.DisplayName = name;
            var cellRect = cell.GetComponent<RectTransform>();
            cellRect.SetParent(_contentRect, false);
            cell.Selected
                .Where(x => x)
                .Subscribe(_ =>
                {
                    foreach (var c in _cells.Values.Where(c => c != cell))
                    {
                        c.Selected.Value = false;
                    }
                    _selectedCellSubject.Value = cell;
                })
                .AddTo(cell);
            _cells.Add(creature, cell);
        }

        public void Remove(GameObject creature)
        {
            if (!_cells.ContainsKey(creature)) return;

            var cell = _cells[creature];
            _cells.Remove(creature);
            DestroyRecursive(cell.transform);
        }

        public int CurrentCreaturesCount()
        {
            return _cells.Count;
        }

        public void UpdateList()
        {
            foreach (var cell in _cells.Select(p => p.Value))
            {
                cell.gameObject.SetActive(cell.IsActive());
            }
        }

        private void DestroyRecursive(Transform target)
        {
            foreach (Transform child in target)
            {
                DestroyRecursive(child);
            }
            Object.Destroy(target.gameObject);
        }
    }
}
