using iTotal.Master.Business;
using iTotal.Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    public class StatusController : ApiBaseController
    {
        StatusServices _statusServices = new StatusServices();
        StockServices _stockServices = new StockServices();
     
        private string CtrlName = "Status";
        public StatusController(ILogger<StatusController> logger, IHttpContextAccessor accessor) : base(logger,  accessor)
        {
            Logger = logger;
            _accessor = accessor;
        }

        // GET: api/Inventory
        /// <summary>
        /// Retrieves Status items
        /// </summary>
        /// <returns>A response with Status list</returns>
        /// <response code="200">Returns the Status list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("mStatus")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetMobileAsync()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetAsync));

            var response = new PagedResponse<Status>();

            try
            {
                // Get the "proposed" query from repository
                var data = _statusServices.getRecords();

                // Get the total rows
                response.Count = data.Count();

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + data.Count() + "]");
                return data.ToList();
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetAsync), ex.Message);
            }

            return response.ToHttpResponse();
        }

        // GET: api/Inventory
        /// <summary>
        /// Retrieves Status items
        /// </summary>
        /// <returns>A response with Status list</returns>
        /// <response code="200">Returns the Status list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetAsync()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetAsync));

            var queryString = Request.Query;
            var pageNumber = Convert.ToInt32(queryString["$skip"]) == 0 ? 1 : Convert.ToInt32(queryString["$skip"]);
            var pageSize = Convert.ToInt32(queryString["$top"]) == 0 ? 1 : Convert.ToInt32(queryString["$top"]);
            string filter = queryString["$filter"];
            string sort = queryString["$orderby"];

            var response = new PagedResponse<Status>();

            try
            {
                // Get the "proposed" query from repository
                var data = _statusServices.getRecords();

                //Set Filter or Sort
                if (filter != null)
                {
                    var newfiltersplits = filter;
                    var filtersplits = newfiltersplits.Split('(', ')', ' ');
                    var filterfield = filtersplits[1];
                    var filtervalue = filtersplits[3];

                    if (filter.Contains("eq") || filter.Contains("ne"))
                    {
                        filterfield = filter.Split('(', ')', '\'')[2];
                        filtervalue = filter.Split('(', ')', '\'')[4];
                    }

                    if (filter.Contains("substringof"))
                    {
                        filterfield = filter.Split('(', ')', '\'')[5];
                        filtervalue = filter.Split('(', ')', '\'')[3];
                    }

                    if (filter.Contains("startswith") || filter.Contains("endswith"))
                    {
                        filterfield = filter.Split('(', ')', '\'')[3];
                        filtervalue = filter.Split('(', ')', '\'')[5];
                    }

                    switch (filterfield)
                    {
                        case "StatusCode":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.StatusCode.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.StatusCode.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.StatusCode.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.StatusCode == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.StatusCode != filtervalue.ToString());
                            }
                            break;
                        case "Description":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.Description.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.Description.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.Description.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.Description == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.Description != filtervalue.ToString());
                            }
                            break;
                    }
                }

                if (sort != null)
                {
                    int keys = sort.Split(new[] { " " }, StringSplitOptions.None).Count();
                    var column = "";
                    var orderby = "";
                    if (keys > 1)
                    {
                        string[] key1 = sort.Split(new[] { " " }, StringSplitOptions.None);
                        column = key1[0];
                        orderby = key1[1];
                    }
                    else
                        column = sort;

                    // Required linq dynamic
                    switch (column)
                    {
                        case "Description":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Description);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Description);
                            break;
                        case "StatusCode":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.StatusCode);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.StatusCode);
                            break;
                    }
                }


                // Set paging values
                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                // Get the total rows
                response.Count = data.Count();

                // Get the specific page from database
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + response.Count + "]");
                if (pageNumber >= pageSize)
                    response.Items = data.Skip(pageNumber).Take(pageSize).ToList();
                else
                    response.Items = data.Skip(pageNumber - 1).Take(pageSize).ToList();

                response.Message = string.Format("Page {0} of {1}, Total of {3}: {2}.", pageNumber, response.PageCount, response.Count, CtrlName);

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + response.Count + "]");
                return new { Items = response.Items, Count = response.Count };
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetAsync), ex.Message);
            }

            return response.ToHttpResponse();
        }

        // GET
        // api/v1/Warehouse/StatusItem/5

        /// <summary>
        /// Retrieves a Status item by ID
        /// </summary>
        /// <param name="id">Department id</param>
        /// <returns>A response with Department</returns>
        /// <response code="200">Returns the Department list</response>
        /// <response code="404">If Department is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Status/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(long id)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAsync));

            var response = new SingleResponse<Status>();

            try
            {
                // Get the Status item by id
                response.Model =  _statusServices.getRecords(id);
                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. Record ID - [" + response.Model.Description + "]");

            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";
                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetAsync), ex.Message);
            }

            return response.ToHttpResponse();
        }

        // POST
        // api/Inventory/Status

        /// <summary>
        /// Creates a new Status item
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new Status</returns>
        /// <response code="200">Returns the Status list</response>
        /// <response code="201">A response as creation of Status</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("Status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody]PostStatusRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PostAsync));

            var response = new SingleResponse<Status>();
            try
            {
                var existingEntity = _statusServices.getRecords(request.StatusCode);

                if (existingEntity != null)
                    ModelState.AddModelError("StatusCode", "Item already exists");

                if (!ModelState.IsValid)
                {
                    response.DidError = true;
                    response.ErrorMessage = "StatusCode item already exists";
                }
                //return BadRequest();
                else
                {
                    // Create entity from request model
                    var entity = request.ToEntity();

                    // Add entity to repository
                    _statusServices.Insert(entity);

                }
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex,"There was an error on '{0}' invocation: {1} with Message {2}", nameof(PostAsync), ex, ex.InnerException.Message);
            }
            return response.ToHttpResponse();
        }

        // PUT
        // api//Inventory/Status/5

        /// <summary>
        /// Updates an existing Status
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response as update Status result</returns>
        /// <response code="200">If Department was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("Status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutAsync([FromBody]PutStatusRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PutAsync));
            long id = request.ID;
            var response = new Response();

            try
            {
                // Get Status item by id
                var entity = _statusServices.getRecords(id);

                // Validate if entity exists
                if (entity == null)
                {
                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                    Logger?.LogInformation("Cannot update Company record for ID " + id.ToString() + " as not Found");
                    return NotFound();
                }

                // Set changes to entity
                entity.StatusCode = request.StatusCode;
                entity.Description = request.Description;
                entity.LastUpdatedDate = DateTime.Now;

                // Update entity in repository
                _statusServices.Update(entity);

            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";
                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex,"[User : " + UserID + "] There was an error on '{0}' invocation: {1}", nameof(PutAsync), ex);
            }

            return response.ToHttpResponse();
        }

        // DELETE
        // api/Master/Dept/5

        /// <summary>
        /// Deletes an existing Department
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <returns>A response as delete Status item result</returns>
        /// <response code="200">If Department was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("Status/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(DeleteAsync));

            var response = new Response();

            try
            {
                // Get Status item by id
                var entity = _statusServices.getRecords(id);

                // Validate if entity exists
                if (entity == null)
                {
                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                    Logger?.LogInformation(CtrlName + " have been deleted. Record ID - [" + entity.Description + "]");
                    return NotFound();
                }
                // Set changes to entity
                entity.Deleted = true;
                entity.Deleted2 = 1;
                entity.DeletedDate = DateTime.Now;

                // Update entity in repository
                _statusServices.Update(entity);

           }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error, please contact to technical support. Message -[" + ex.Message + "]";
                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex,"There was an error on '{0}' invocation: {1}", nameof(DeleteAsync), ex);
            }

            return response.ToHttpResponse();
        }


        //// GET: api/Inventory
        ///// <summary>
        ///// Retrieves Status items
        ///// </summary>
        ///// <returns>A response with Status list</returns>
        ///// <response code="200">Returns the Status list</response>
        ///// <response code="500">If there was an internal server error</response>
        //[HttpGet("GetStatusIdName")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        //public async Task<object> GetStatusIdName()
        //{
        //    LoadUserInfo();
        //    Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetStatusIdName));

        //    var queryString = Request.Query;
        //    var pageNumber = Convert.ToInt32(queryString["$skip"]) == 0 ? 1 : Convert.ToInt32(queryString["$skip"]);
        //    var pageSize = Convert.ToInt32(queryString["$top"]) == 0 ? 1 : Convert.ToInt32(queryString["$top"]);
        //    string filter = queryString["$filter"];
        //    string sort = queryString["$orderby"];

        //    var response = new DropDownListResponse<Status>();

        //    try
        //    {
        //        // Get the "proposed" query from repository
        //        var data = _context.GetItems();

  

        //        // Set paging values
        //        //response.PageSize = pageSize;
        //        //response.PageNumber = pageNumber;

        //        // Get the total rows
        //        //response.Count = await data.CountAsync();

        //        //// Get the specific page from database
        //        //response.Items = await data.Paging(pageSize, pageNumber).ToListAsync();

        //        //response.Message = string.Format("Page {0} of {1}, Total of {3}: {2}.", pageNumber, response.PageCount, response.Count, CtrlName);

        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(data.ToList()));
        //        Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + data.Count() + "]");
        //        if (data.Count() > 0)
        //            return data.ToList();
        //        else
        //            return data.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        //response.DidError = true;
        //        //response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //        Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetStatusIdName), ex.Message);
        //    }

        //    return response;
        //}


    }
}