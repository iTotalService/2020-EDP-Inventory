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
    public class StockController : ApiBaseController
    {
     
        private string CtrlName = "Stock";
        public StockController(ILogger<StockController> logger, InvContext context, IHttpContextAccessor accessor) : base(logger, context, accessor)
        {
            Logger = logger;
            _context = context;
            _accessor = accessor;
        }

        // GET: api/Inventory
        /// <summary>
        /// Retrieves stock items
        /// </summary>
        /// <returns>A response with Stock list</returns>
        /// <response code="200">Returns the Stock list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("mStock")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetMobileAsync()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetAsync));

            var response = new PagedResponse<Stock>();

            try
            {
                // Get the "proposed" query from repository
                var data = _context.GetItems();

                // Get the total rows
                response.Count = await data.CountAsync();

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
        /// Retrieves stock items
        /// </summary>
        /// <returns>A response with Stock list</returns>
        /// <response code="200">Returns the Stock list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Stock")]
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

            var response = new PagedResponse<Stock>();

            try
            {
                // Get the "proposed" query from repository
                var data = _context.GetItems();

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
                        case "BarCode":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.BarCode.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.BarCode.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.BarCode.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.BarCode == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.BarCode != filtervalue.ToString());
                            }
                            break;
                        case "CO_NAME":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.BarCode.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.BarCode.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.BarCode.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.BarCode == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.BarCode != filtervalue.ToString());
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
                        case "BarCode":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.BarCode);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.BarCode);
                            break;
                        case "Description":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Description);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Description);
                            break;
                        case "Location":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Location);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Location);
                            break;
                        case "Section":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Section);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Section);
                            break;
                    }
                }


                // Set paging values
                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                // Get the total rows
                response.Count = await data.CountAsync();

                // Get the specific page from database
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + response.Count + "]");
                if (pageNumber >= pageSize)
                    response.Items = await data.Skip(pageNumber).Take(pageSize).ToListAsync();
                else
                    response.Items = await data.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

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

        // GET: api/Inventory
        /// <summary>
        /// Retrieves stock items
        /// </summary>
        /// <returns>A response with Stock list</returns>
        /// <response code="200">Returns the Stock list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("StockwithQty")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetStockwithQtyAsync()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetStockwithQtyAsync));

            var queryString = Request.Query;
            var pageNumber = Convert.ToInt32(queryString["$skip"]) == 0 ? 1 : Convert.ToInt32(queryString["$skip"]);
            var pageSize = Convert.ToInt32(queryString["$top"]) == 0 ? 1 : Convert.ToInt32(queryString["$top"]);
            string filter = queryString["$filter"];
            string sort = queryString["$orderby"];

            var response = new PagedResponse<StockTotal>();

            try
            {
                // Get the "proposed" query from repository
                var data = _context.GetItemswithQty();

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
                        case "BarCode":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.BarCode.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.BarCode.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.BarCode.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.BarCode == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.BarCode != filtervalue.ToString());
                            }
                            break;
                        case "CO_NAME":
                            if (filter.Contains("startswith"))
                            {
                                data = data.Where(x => x.BarCode.StartsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("endswith"))
                            {
                                data = data.Where(x => x.BarCode.EndsWith(filtervalue.ToString()));
                            }
                            else if (filter.Contains("substringof"))
                            {
                                data = data.Where(x => x.BarCode.Contains(filtervalue.ToString()));
                            }
                            else if (filter.Contains("eq"))
                            {
                                data = data.Where(x => x.BarCode == filtervalue.ToString());
                            }
                            else if (filter.Contains("ne"))
                            {
                                data = data.Where(x => x.BarCode != filtervalue.ToString());
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
                        case "BarCode":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.BarCode);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.BarCode);
                            break;
                        case "Description":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Description);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Description);
                            break;
                        case "Location":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Location);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Location);
                            break;
                        case "Section":
                            if (orderby == "")
                                data = data.AsQueryable().OrderBy(x => x.Section);
                            else
                                data = data.AsQueryable().OrderByDescending(x => x.Section);
                            break;
                    }
                }


                // Set paging values
                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                // Get the total rows
                response.Count = await data.CountAsync();

                // Get the specific page from database
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + response.Count + "]");
                if (pageNumber >= pageSize)
                    response.Items = await data.Skip(pageNumber).Take(pageSize).ToListAsync();
                else
                    response.Items = await data.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

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
                Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetStockwithQtyAsync), ex.Message);
            }

            return response.ToHttpResponse();
        }

        // GET
        // api/v1/Warehouse/StockItem/5

        /// <summary>
        /// Retrieves a stock item by ID
        /// </summary>
        /// <param name="id">Department id</param>
        /// <returns>A response with Department</returns>
        /// <response code="200">Returns the Department list</response>
        /// <response code="404">If Department is not exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("Stock/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(long id)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAsync));

            var response = new SingleResponse<Stock>();

            try
            {
                // Get the stock item by id
                response.Model = await _context.GetbyIDAsync(new Stock(id, UserID));
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
        // api/Inventory/Stock

        /// <summary>
        /// Creates a new stock item
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new Stock</returns>
        /// <response code="200">Returns the Stock list</response>
        /// <response code="201">A response as creation of Stock</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("Stock")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody]PostStockRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PostAsync));

            var response = new SingleResponse<Stock>();
            try
            {
                var existingEntity = await _context
                    .GetbyIDAsync(new Stock { BarCode = request.BarCode });

                if (existingEntity != null)
                    ModelState.AddModelError("BarCode", "Item already exists");

                if (!ModelState.IsValid)
                {
                    response.DidError = true;
                    response.ErrorMessage = "Stock item already exists";
                }
                //return BadRequest();
                else
                {
                    // Create entity from request model
                    var entity = request.ToEntity();

                    // Add entity to repository
                    _context.Add(entity);

                    using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Save entity in database
                            await _context.SaveChangesAsync();
                            transaction.Commit();
                            NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                            Logger?.LogInformation(CtrlName + " have been inserted successfully. Record ID - [" + entity.Description + "]");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.DidError = true;
                            response.ErrorMessage = "There was an internal error, please contact to technical support. Message -[" + ex.Message + "]";
                            NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                            Logger?.LogCritical(ex,"There was an error on '{0}' invocation: {1} with Message {2}", nameof(PostAsync), ex, ex.InnerException.Message);
                        }
                    }
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
        // api//Inventory/Stock/5

        /// <summary>
        /// Updates an existing Stock
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response as update Stock result</returns>
        /// <response code="200">If Department was updated successfully</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("Stock")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutAsync([FromBody]PutStockRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PutAsync));
            long id = request.ID;
            var response = new Response();

            try
            {
                // Get stock item by id
                var entity = await _context.GetbyIDAsync(new Stock(id,UserID));

                // Validate if entity exists
                if (entity == null)
                {
                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                    Logger?.LogInformation("Cannot update Company record for ID " + id.ToString() + " as not Found");
                    return NotFound();
                }

                // Set changes to entity
                entity.BarCode = request.BarCode;
                entity.Description = request.Description;
                entity.Section = request.Section;
                entity.Location = request.Location;
                entity.Status = request.Status;
                entity.Remarks = request.Remarks;
                entity.LastUpdatedDate = DateTime.Now;

                // Update entity in repository
                _context.Update(entity);

                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Save entity in database
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                        Logger?.LogInformation(CtrlName + " have been updated successfully. Record ID - [" + entity.Description + "]");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.DidError = true;
                        response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";
                        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                        Logger?.LogCritical(ex,"[User : " + UserID + "] There was an error on '{0}' invocation: {1} with Message {2}", nameof(PutAsync), ex, ex.InnerException.Message);
                    }
                }
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
        /// <returns>A response as delete stock item result</returns>
        /// <response code="200">If Department was deleted successfully</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("Stock/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(DeleteAsync));

            var response = new Response();

            try
            {
                // Get stock item by id
                var entity = await _context.GetbyIDAsync(new Stock(id, UserID));

                // Validate if entity exists
                if (entity == null)
                {
                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                    Logger?.LogInformation(CtrlName + " have been deleted. Record ID - [" + entity.Description + "]");
                    return NotFound();
                }
                // Set changes to entity
                entity.DELETED = true;
                entity.DeletedDate = DateTime.Now;

                // Update entity in repository
                _context.Update(entity);

                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Save entity in database
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.DidError = true;
                        response.ErrorMessage = "There was an internal error, please contact to technical support. Message -[" + ex.Message + "]";
                        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                        Logger?.LogCritical(ex,"There was an error on '{0}' invocation: {1} with Message {2}", nameof(DeleteAsync), ex, ex.InnerException.Message);
                    }
                }



                //////// Get stock item by id
                //////var entity = await _context.GetDeptbyIDAsync(new Dept(id));

                //////// Validate if entity exists
                //////if (entity == null)
                //////    return NotFound();

                //////// Remove entity from repository
                //////_context.Remove(entity);

                //////// Delete entity in database
                //////await _context.SaveChangesAsync();
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


        // GET: api/Inventory
        /// <summary>
        /// Retrieves stock items
        /// </summary>
        /// <returns>A response with Stock list</returns>
        /// <response code="200">Returns the Stock list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("GetStockIdName")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetStockIdName()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetStockIdName));

            var queryString = Request.Query;
            var pageNumber = Convert.ToInt32(queryString["$skip"]) == 0 ? 1 : Convert.ToInt32(queryString["$skip"]);
            var pageSize = Convert.ToInt32(queryString["$top"]) == 0 ? 1 : Convert.ToInt32(queryString["$top"]);
            string filter = queryString["$filter"];
            string sort = queryString["$orderby"];

            var response = new DropDownListResponse<Stock>();

            try
            {
                // Get the "proposed" query from repository
                var data = _context.GetItems();

  

                // Set paging values
                //response.PageSize = pageSize;
                //response.PageNumber = pageNumber;

                // Get the total rows
                //response.Count = await data.CountAsync();

                //// Get the specific page from database
                //response.Items = await data.Paging(pageSize, pageNumber).ToListAsync();

                //response.Message = string.Format("Page {0} of {1}, Total of {3}: {2}.", pageNumber, response.PageCount, response.Count, CtrlName);

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(data.ToList()));
                Logger?.LogInformation(CtrlName + " have been retrieved successfully. - Total Record [" + data.Count() + "]");
                if (data.Count() > 0)
                    return data.ToList();
                else
                    return data.ToList();
            }
            catch (Exception ex)
            {
                //response.DidError = true;
                //response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetStockIdName), ex.Message);
            }

            return response;
        }


    }
}