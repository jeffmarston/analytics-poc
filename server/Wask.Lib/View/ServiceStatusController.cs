using System.Web.Http;
using System.Collections.Generic;
using Wask.Lib.Model;

namespace Wask.Lib.SignalR
{
    /// <summary>
    /// Service Info OWIN Controller
    /// </summary>
    [RoutePrefix(Constants.ViewChannel)]
    public class ServiceStatusController : ApiController
    {        
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
        public IEnumerable<string> GetServices()
        {
            DummyPublisher.Instance.Init(); 
            List<string> columns = new List<string>() { "Symbol", "Amount", "Sector" };
            return columns;
        }
        
    }
}
