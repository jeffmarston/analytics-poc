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
        private ViewModel _view;
        private IHubContext _context;
        private Timer _tickTimer;
        private Timer _newRowTimer;
        private Timer _deleteRowTimer;

        private static DummyPublisher _instance = new DummyPublisher();
        public static DummyPublisher Instance { get { return _instance; } }

        private DummyPublisher()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
            _view = new ViewModel();

        }

        public ViewModel CreateView()
        {
            _tickTimer = new Timer(VolumeTick, null, 0, 50);
            _newRowTimer = new Timer(AddRow, null, 0, 8000);
            _deleteRowTimer = new Timer(DeleteRow, null, 10000, 10000);

            return _view;
        }

        public void Init()
        {
            _view = new ViewModel();
        }


        private void VolumeTick(object state)
        {
            try
            {
                Console.Write(".");
                RowModel affectedRow = _view.Tick();
                _context.Clients.All.updateRow(affectedRow);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<string> GetColumns()
        {
            return _view.Columns;
        }

        private void AddRow(object state)
        {
            try
            {
                Console.WriteLine();
                Console.Write("AddRow");
                RowModel newRow = _view.AddRow();
                _context.Clients.All.addRow(newRow);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void DeleteRow(object state)
        {
            try
            {
                Console.WriteLine();
                Console.Write("DeleteRow");
                RowModel deleteRow = _view.DeleteRow();
                _context.Clients.All.deleteRow(deleteRow);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
