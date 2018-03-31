using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Wask.Lib.Model
{
    public class ViewModel
    {
        private Random _random = new Random();
        private List<string> _secMaster = new List<string>();

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
            for (int i=0; i<20; i++)
            {
                _secMaster.Add(RandomSymbol());
            }
            Console.WriteLine("Generated SecMaster: "+string.Join(", ", _secMaster.ToArray()));
            Create();
        }

        private void Create()
        {
            Columns.Add("Symbol");
            Columns.Add("Volume");
            Columns.Add("Price");
            Columns.Add("Currency");
            Columns.Add("Country");
            
            for (int i=0; i<300; i++)
            {
                AddRow();
            }
        }

        public List<RowModel> Tick()
        {
            var volumeIndex = Columns.IndexOf("Volume");
            var priceIndex = Columns.IndexOf("Price");
            var symbolIndex = Columns.IndexOf("Symbol");

            var rows = new List<RowModel>(RowLookup.Values);
            var symbol = rows[_random.Next(Rows.Count)].values[symbolIndex];
            var rowsToTick = rows.FindAll(o => o.values[symbolIndex] == symbol);
            if (rowsToTick.Count == 0)
            {
                return rowsToTick;
            }

            var newVol = (int)(rowsToTick[0].values[volumeIndex]) + 20;
            var newPrc = (decimal)(rowsToTick[0].values[priceIndex]) + (decimal)(_random.Next(0, 10) - 5) / 100m;

            foreach (var row in rowsToTick)
            {
                row.values[volumeIndex] = newVol;
                row.values[priceIndex] = newPrc;
            }
            return rowsToTick;
        }

        private string RandomSymbol()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var symbol = "";
            for (int i = 0; i < 3; i++)
            {
                symbol += chars[_random.Next(chars.Length)];
            }
            return symbol;
        }

        public RowModel AddRow()
        {
            var guid = Guid.NewGuid().ToString();
            RowModel newRow = new RowModel(guid, new object[] {
                _secMaster[_random.Next(0, _secMaster.Count)],
                _random.Next(11,550)*100,
                _random.Next(10, 5000) / 100m,
                "USD",
                "USA" });
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