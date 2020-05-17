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
    public class LocationController : ApiBaseController
    {
        LocationServices ls = new LocationServices();
        StockServices ss = new StockServices();

        private string CtrlName = "Location";
        public LocationController(ILogger<LocationController> logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            Logger = logger;
            _accessor = accessor;
        }

        // GET: api/Inventory
        /// <summary>
        /// Retrieves Location items
        /// </summary>
        /// <returns>A response with Location list</returns>
        /// <response code="200">Returns the Location list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("mLocation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetMobileAsync()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetAsync));

            var response = new PagedResponse<Location>();

            try
            {
                // Get the "proposed" query from repository
                var data = ls.getRecords();

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
        /// Retrieves Location items
        /// </summary>
        /// <returns>A response with Location list</returns>
        /// <response code="200">Returns the Location list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Location")]
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

            var response = new PagedResponse<Location>();

            try
            {
                // Get the "proposed" query from repository
                var data = ls.getRecords();

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
                        case "LocationCode":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.LocationCode.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.LocationCode.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.LocationCode.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.LocationCode == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.LocationCode != filtervalue.ToString());
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
                        case "LocationCode":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.LocationCode);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.LocationCode);
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
                //if (pageNumber >= pageSize)
                //    response.Items = await data.Skip(pageNumber).Take(pageSize).ToListAsync();
                //else
                //    response.Items = await data.Skip(pageNumber - 1).Take(pageSize).ToListAsync();
                response.Items = data.ToList();

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
        // api/v1/Warehouse/LocationItem/5

        /// <summary>
        /// Retrieves a Location item by ID
        /// </summary>
        /// <param name="id">Department id</param>
        /// <returns>A response with Department</returns>
        /// <response code="200">Returns the Department list</response>
        /// <response code="404">If Department is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Location/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(long id)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAsync));

            var response = new SingleResponse<Location>();

            try
            {
                // Get the Location item by id
                response.Model = ls.getLocation(id);
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
        // api/Inventory/Location

        /// <summary>
        /// Creates a new Location item
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new Location</returns>
        /// <response code="200">Returns the Location list</response>
        /// <response code="201">A response as creation of Location</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("Location")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody]PostLocationRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PostAsync));

            var response = new SingleResponse<Location>();
            try
            {
                var existingEntity = ls.getLocation(request.LocationCode);

                if (existingEntity != null)
                    ModelState.AddModelError("LocationCode", "Item already exists");

                if (!ModelState.IsValid)
                {
                    response.DidError = true;
                    response.ErrorMessage = "LocationCode item already exists";
                }
                //return BadRequest();
                else
                {
                    // Create entity from request model
                    var entity = request.ToEntity();

                    // Add entity to repository
                    ls.Insert(entity);

                    // Set the entity to response model
                    response.Model = entity;
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
        // api//Inventory/Location/5

        /// <summary>
        /// Updates an existing Location
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response as update Location result</returns>
        /// <response code="200">If Department was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("Location")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutAsync([FromBody]PutLocationRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PutAsync));
            long id = request.ID;
            var response = new Response();

            try
            {
                // Get Location item by id
                var entity = ls.getLocation(id);

                // Validate if entity exists
                if (entity == null)
                {
                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                    Logger?.LogInformation("Cannot update Company record for ID " + id.ToString() + " as not Found");
                    return NotFound();
                }

                // Set changes to entity
                entity.LocationCode = request.LocationCode;
                entity.Description = request.Description;
                entity.LastUpdatedDate = DateTime.Now;

                // Update entity in repository
                ls.Update(entity);
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
        /// <returns>A response as delete Location item result</returns>
        /// <response code="200">If Department was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("Location/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(DeleteAsync));

            var response = new Response();

            try
            {
                // Get Location item by id
                var entity = ls.getLocation(id);

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
                ls.Update(entity);
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
        ///// Retrieves Location items
        ///// </summary>
        ///// <returns>A response with Location list</returns>
        ///// <response code="200">Returns the Location list</response>
        ///// <response code="500">If there was an internal server error</response>
        //[HttpGet("GetLocationIdName")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        //public async Task<object> GetLocationIdName()
        //{
        //    LoadUserInfo();
        //    Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetLocationIdName));

        //    var queryString = Request.Query;
        //    var pageNumber = Convert.ToInt32(queryString["$skip"]) == 0 ? 1 : Convert.ToInt32(queryString["$skip"]);
        //    var pageSize = Convert.ToInt32(queryString["$top"]) == 0 ? 1 : Convert.ToInt32(queryString["$top"]);
        //    string filter = queryString["$filter"];
        //    string sort = queryString["$orderby"];

        //    var response = new DropDownListResponse<Location>();

        //    try
        //    {
        //        // Get the "proposed" query from repository
        //        var data = ss.getRecords();

  

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
        //        Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetLocationIdName), ex.Message);
        //    }

        //    return response;
        //}


    }
}