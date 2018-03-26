using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading;
using Wask.Lib.SignalR;
using Microsoft.AspNet.SignalR;

namespace Wask.Lib.Model
{
    public class DummyPublisher
    {
        private List<string> _columns;
        private List<RowData> _data = new List<RowData>();

        private Timer _pollingTimer;
        private IHubContext _context;
        private string _channel = Constants.ViewChannel;

        private static DummyPublisher _instance = new DummyPublisher();
        public static DummyPublisher Instance { get { return _instance; } }

        private DummyPublisher()
        {
            _columns = new List<string>() {
                "Symbol",
                "Portfolio",
                "Amount"
            };

            _context = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
            _pollingTimer = new Timer(AddRow, null, 0, 3000);

        }

        public void Init()
        {
        }


        private void AddRow(object state)
        {
            Console.WriteLine("addRow");
            object[] row = new object[] { "IBM", 1300, "USD", 0.999 };
            _context.Clients.All.addRow(row);
        }
    }
}
