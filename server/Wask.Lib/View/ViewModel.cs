using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Wask.Lib.Model
{
    public class ViewModel
    {
        public List<string> Columns { get; }
        public ICollection<RowModel> Rows
        {
            get { return RowLookup.Values; }
        }

        private ConcurrentDictionary<string, RowModel> RowLookup { get; set; }


        public ViewModel()
        {
            Columns = new List<string>();
            RowLookup = new ConcurrentDictionary<string, RowModel>();
            Create();
        }

        private void Create()
        {
            Columns.Add("Symbol");
            Columns.Add("Volume");
            Columns.Add("NavPL*");
            Columns.Add("Currency");
            Columns.Add("Country");

            RowLookup.TryAdd("key_111", new RowModel("key_111", new object[] { "IBM", 10000, 123.456, "USD", "USA" }));
            RowLookup.TryAdd("key_222", new RowModel("key_222", new object[] { "DELL", 18800, 18823.456, "USD", "USA" }));
            RowLookup.TryAdd("key_333", new RowModel("key_333", new object[] { "VOD LN", 66000, 14423.456, "GBP", "UK" }));
        }

        public RowModel Tick()
        {
            var index = new Random().Next(Rows.Count);
            var volumeIndex = Columns.IndexOf("Volume");
            var key = new List<string>(RowLookup.Keys)[index];

            RowLookup[key].values[volumeIndex] = (int)(RowLookup[key].values[volumeIndex]) + 20;
            return RowLookup[key];
        }

        public RowModel AddRow()
        {
            var guid = Guid.NewGuid().ToString();
            RowModel newRow = new RowModel(guid, new object[] { "FOO", 66000, 14423.456, "CAD", "CAN" });
            RowLookup.TryAdd(guid.ToString(), newRow);
            return newRow;
        }

        internal RowModel DeleteRow()
        {
            var index = new Random().Next(Rows.Count);
            var volumeIndex = Columns.IndexOf("Volume");
            var key = new List<string>(RowLookup.Keys)[index];

            RowModel rowToRemove;
            RowLookup.TryRemove(key, out rowToRemove);
            return rowToRemove;
        }
    }

    public class RowModel
    {
        // lowercase for REST API standard compliance

        public string key { get; set; }
        public object[] values { get; set; }
        public RowModel(string key, object[] values)
        {
            this.key = key;
            this.values = values;
        }
    }
}