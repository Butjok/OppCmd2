using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Bulka.CommandLine
{
    public class History
    {
        private List<string> _records = new List<string> {string.Empty};
        private int _index ;
        public string ActiveRecord => _records[_records.Count - 1];

        public struct Delta
        {
            public int OldIndex { get; set; }
            public string OldInput { get; set; }
            public bool NewRecordWasAdded { get; set; }
        }

        public History()
        { }
        public History(string filename)
        {
            Load(filename);
        }
        public void Load(string filename)
        {
            if (!File.Exists(filename))
                return;

            var list = new List<string>();
            list.AddRange(File.ReadLines(filename).Where(line => !string.IsNullOrWhiteSpace(line)));
            list.Add(string.Empty);

            _records = list;
            _index   = _records.Count - 1;
        }
        public void Save(string filename)
        {
            File.WriteAllLines(filename, _records.Take(_records.Count - 1));
        }
        public void Clear()
        {
            _records.Clear();
            _records.Add(string.Empty);
            _index = 0;
        }
        public void Touch(string text)
        {
            _records[_records.Count - 1] = text;
            _index                       = _records.Count - 1;
        }
        public bool TryLookup(int offset, out string record)
        {
            var oldIndex = _index;
            _index = Mathf.Clamp(_index + offset, 0, _records.Count - 1);
            record = _records[_index];
            return oldIndex != _index;
        }
        public Delta Add(string text)
        {
            var delta = new Delta
            {
                OldInput          = _records[_records.Count - 1],
                OldIndex          = _index,
                NewRecordWasAdded = false
            };

            if (!string.IsNullOrWhiteSpace(text) && (_records.Count - 2 < 0 || _records[_records.Count - 2] != text))
            {
                _records[_records.Count - 1] = text;
                _records.Add(string.Empty);
                delta.NewRecordWasAdded = true;
            }

            _index = _records.Count - 1;
            return delta;
        }
        public void Rollback(Delta delta)
        {
            if (delta.NewRecordWasAdded)
                _records.RemoveAt(_records.Count - 1);

            _records[_records.Count - 1] = delta.OldInput;
            _index                       = delta.OldIndex;
        }
    }
}