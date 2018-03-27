using System.Web.Http;
using System.Collections.Generic;
using Wask.Lib.Model;

namespace Wask.Lib.SignalR
{
    /// <summary>
    /// Service Info OWIN Controller
    /// </summary>
    [RoutePrefix(Constants.ViewChannel)]
    public class AnalyticsController : ApiController
    {
        /// <summary>
        /// Create new view
        /// </summary>
        /// <response code="200">Returns ID of the new view</response>
        [Route("create")]
        [HttpPost]
        public ViewModel CreateView(string parameter)
        {
            DummyPublisher.Instance.Init();
            return DummyPublisher.Instance.CreateView();
        }

        /// <summary>
        /// Get all columns
        /// </summary>
        /// <remarks>
        /// blah blah
        ///  
        ///     GET /columns
        /// 
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [Route("columns")]
        [HttpGet]
        public IEnumerable<string> GetColumns() {
            var columns = DummyPublisher.Instance.GetColumns();
            return columns;
        }

    }
}
