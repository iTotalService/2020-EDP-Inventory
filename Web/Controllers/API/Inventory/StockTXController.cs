using iTotal.Master.Business;
using iTotal.Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    public class StockTXController : ApiBaseController
    {
        StockTXServices ss = new StockTXServices();
        protected StockTXServices sts = new StockTXServices();
        private string CtrlName = "StockTX";
        public StockTXController(ILogger<StockTXController> logger, InvContext context, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            Logger = logger;
            _accessor = accessor;
        }


        // GET: api/Inventory
        /// <summary>
        /// Retrieves stock items
        /// </summary>
        /// <returns>A response with Stock list</returns>
        /// <response code="200">Returns the Stock list</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("StockTX")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<object> GetAsync()
        {
            LoadUserInfo();
            Logger?.LogInformation("'API [{0}]' has start been invoked", nameof(GetAsync));

            var queryString = Request.Query;
            var pageNumber = Convert.ToInt32(queryString["$skip"]) == 0 ? 1 : Convert.ToInt32(queryString["$skip"]);
            var pageSize = Convert.ToInt32(queryString["$top"]) == 0 ? 1 : Convert.ToInt32(queryString["$top"]);
            var stockID = Convert.ToInt64(queryString["stockID"]) == 0 ? 0 : Convert.ToInt64(queryString["stockID"]);
            string filter = queryString["$filter"];
            string sort = queryString["$orderby"];

            var response = new PagedResponse<vStockTXDisplay2>();

            try
            {
                var data = sts.getDisplayStockTX(stockID);
                // Get the "proposed" query from repository
                //var data = _context.GetTXs(stockID);


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
                }


                // Set paging values
                response.PageSize = pageSize;
                response.PageNumber = pageNumber;

                // Get the total rows
                response.Count = data.ToList().Count;
                response.Items = data.ToList();
                // Get the specific page from database
                //if (pageNumber >= pageSize)
                //    response.Items = await data.Skip(pageNumber).Take(pageSize).ToListAsync();
                //else
                //    response.Items = await data.Skip(pageNumber - 1).Take(pageSize).ToListAsync();

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
        // api/Inventory/StockTX/5

        ///// <summary>
        ///// Retrieves a stock item by ID
        ///// </summary>
        ///// <param name="id">StockTX id</param>
        ///// <returns>A response with StockTX</returns>
        ///// <response code="200">Returns the StockTX list</response>
        ///// <response code="404">If StockTX is not exists</response>
        ///// <response code="500">If there was an internal server error</response>
        //[HttpGet("StockTX/{id}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> GetAsync(long id)
        //{
        //    LoadUserInfo();
        //    Logger?.LogDebug("'{0}' has been invoked", nameof(GetAsync));

        //    var response = new SingleResponse<StockTX>();

        //    try
        //    {
        //        // Get the stock item by id
        //        response.Model = await _context.GetbyTXIDAsync(new StockTX(id, UserID));
        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //        Logger?.LogInformation(CtrlName + " have been retrieved successfully. Record ID - [" + response.Model.ID.ToString() + "]");

        //    }
        //    catch (Exception ex)
        //    {
        //        response.DidError = true;
        //        response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";
        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //        Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1}", nameof(GetAsync), ex.Message);
        //    }

        //    return response.ToHttpResponse();
        //}

        //[HttpPost("StockTakeList")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(201)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> PostStockTakeAsync([FromBody]PostStockTXListRequest requests)
        //{
        //    LoadUserInfo();
        //    Logger?.LogDebug("'{0}' has been invoked", nameof(PostAsync));

        //    var listData = requests.Result;

        //    var response = new Response();
        //    try
        //    {
        //        foreach (var request in listData)
        //        {
        //            var entity = request.ToEntity();
        //            // Create entity from request model
        //            entity.UploadedDate = DateTime.Now;
        //            entity.Location = request.Location;
        //            entity.Section = request.Section;
        //            entity.UploadedID = request.Id;
        //            entity.CreateDate = request.CreateDate;
        //            entity.Remarks = "Stock Adjustment on" + Convert.ToDateTime(request.CreateDate).ToString("yyyy-MM-dd HH:MM");

        //            var stockTtl = _context.GetStockBalance(request.StockID);
        //            entity.Quantity = stockTtl - request.Quantity;

        //            // Add entity to repository
        //            _context.Add(entity);
        //            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    // Save entity in database
        //                    await _context.SaveChangesAsync();
        //                    transaction.Commit();
        //                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //                    Logger?.LogInformation(CtrlName + " have been inserted successfully. Record ID - [" + entity.ID.ToString() + "]");
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    response.DidError = true;
        //                    response.ErrorMessage = "There was an internal error, please contact to technical support. Message -[" + ex.Message + "]";
        //                    NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //                    Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1} with Message {2}", nameof(PostAsync), ex, ex.InnerException.Message);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.DidError = true;
        //        response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //        Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1} with Message {2}", nameof(PostAsync), ex, ex.StackTrace);
        //    }
        //    return response.ToHttpResponse();

        //}


        [HttpPost("StockTXList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostListAsync([FromBody]PostStockTXListRequest requests)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PostAsync));

            var listData = requests.Result;

            var response = new Response();
            try
            {
                foreach (var request in listData)
                {
                    var entity = request.ToEntity();
                    // Create entity from request model
                    entity.UploadedDate = DateTime.Now;
                    entity.Location = request.Location;
                    entity.Section = request.Section;
                    entity.UploadedID = request.Id;
                    entity.CreateDate = request.CreateDate;
                    entity.Designation = request.Designation;
                    // Add entity to repository
                    sts.Insert(entity);
                }
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";

                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
                Logger?.LogCritical(ex, "There was an error on '{0}' invocation: {1} with Message {2}", nameof(PostAsync), ex, ex.InnerException.Message);
            }
            return response.ToHttpResponse();

        }

        // POST
        // api/Inventory/Stock

        /// <summary>
        /// Creates a new StockTX
        /// </summary>
        /// <param name="request">Request model</param>
        /// <returns>A response with new StockTX</returns>
        /// <response code="200">Returns the StockTX list</response>
        /// <response code="201">A response as creation of StockTX</response>
        /// <response code="400">For bad request</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("StockTX")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody]PostStockTXRequest request)
        {
            LoadUserInfo();
            Logger?.LogDebug("'{0}' has been invoked", nameof(PostAsync));

            var response = new SingleResponse<StockTX>();
            try
            {
                //var existingEntity = await _context
                //    .GetbyIDAsync(new Stock { BarCode = request.BarCode });

                //if (existingEntity != null)
                //    ModelState.AddModelError("BarCode", "Item already exists");

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
                    sts.Insert(entity);

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

        //// PUT
        //// api//Inventory/StockTX/5

        ///// <summary>
        ///// Updates an existing StockTX
        ///// </summary>
        ///// <param name="request">Request model</param>
        ///// <returns>A response as update StockTX result</returns>
        ///// <response code="200">If StockTX was updated successfully</response>
        ///// <response code="400">For bad request</response>
        ///// <response code="500">If there was an internal server error</response>
        //[HttpPut("StockTX")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> PutAsync([FromBody]PutStockTXRequest request)
        //{
        //    LoadUserInfo();
        //    Logger?.LogDebug("'{0}' has been invoked", nameof(PutAsync));
        //    long id = request.ID;
        //    var response = new Response();

        //    try
        //    {
        //        // Get stock item by id
        //        var entity = await _context.GetbyTXIDAsync(new StockTX(id,UserID));

        //        // Validate if entity exists
        //        if (entity == null)
        //        {
        //            NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //            Logger?.LogInformation("Cannot update Company record for ID " + id.ToString() + " as not Found");
        //            return NotFound();
        //        }

        //        // Set changes to entity
        //        entity.StockID = request.StockID;
        //        entity.Quantity = request.Quantity;
        //        entity.Section = request.Section;
        //        entity.Location = request.Location;
        //        entity.LastUpdatedDate = DateTime.Now;

        //        // Update entity in repository
        //        _context.Update(entity);

        //        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Save entity in database
        //                await _context.SaveChangesAsync();
        //                transaction.Commit();
        //                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //                Logger?.LogInformation(CtrlName + " have been updated successfully. Record ID - [" + entity.ID.ToString () + "]");
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                response.DidError = true;
        //                response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";
        //                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //                Logger?.LogCritical(ex,"[User : " + UserID + "] There was an error on '{0}' invocation: {1} with Message {2}", nameof(PutAsync), ex, ex.InnerException.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.DidError = true;
        //        response.ErrorMessage = "There was an internal error. [" + ex.Message + "] [Stack : " + ex.StackTrace + "]";
        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //        Logger?.LogCritical(ex,"[User : " + UserID + "] There was an error on '{0}' invocation: {1}", nameof(PutAsync), ex);
        //    }

        //    return response.ToHttpResponse();
        //}

        //// DELETE
        //// api/Inventory/StockTX/5

        ///// <summary>
        ///// Deletes an existing StockTX
        ///// </summary>
        ///// <param name="id">Department ID</param>
        ///// <returns>A response as delete StockTX item result</returns>
        ///// <response code="200">If StockTX was deleted successfully</response>
        ///// <response code="500">If there was an internal server error</response>
        //[HttpDelete("StockTX/{id}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(500)]
        //public async Task<IActionResult> DeleteAsync(long id)
        //{
        //    LoadUserInfo();
        //    Logger?.LogDebug("'{0}' has been invoked", nameof(DeleteAsync));

        //    var response = new Response();

        //    try
        //    {
        //        // Get stock item by id
        //        var entity = await _context.GetbyTXIDAsync(new StockTX(id, UserID));

        //        // Validate if entity exists
        //        if (entity == null)
        //        {
        //            NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //            Logger?.LogInformation(CtrlName + " have been deleted. Record ID - [" + entity.ID.ToString() + "]");
        //            return NotFound();
        //        }
        //        // Set changes to entity
        //        entity.Deleted = true;
        //        entity.Deleted2 = 1;
        //        entity.DeletedDate = DateTime.Now;

        //        // Update entity in repository
        //        _context.Update(entity);

        //        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Save entity in database
        //                await _context.SaveChangesAsync();
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                response.DidError = true;
        //                response.ErrorMessage = "There was an internal error, please contact to technical support. Message -[" + ex.Message + "]";
        //                NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //                Logger?.LogCritical(ex,"There was an error on '{0}' invocation: {1} with Message {2}", nameof(DeleteAsync), ex, ex.InnerException.Message);
        //            }
        //        }



        //        //////// Get stock item by id
        //        //////var entity = await _context.GetDeptbyIDAsync(new Dept(id));

        //        //////// Validate if entity exists
        //        //////if (entity == null)
        //        //////    return NotFound();

        //        //////// Remove entity from repository
        //        //////_context.Remove(entity);

        //        //////// Delete entity in database
        //        //////await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        response.DidError = true;
        //        response.ErrorMessage = "There was an internal error, please contact to technical support. Message -[" + ex.Message + "]";
        //        NLog.GlobalDiagnosticsContext.Set("Response", JsonConvert.SerializeObject(response));
        //        Logger?.LogCritical(ex,"There was an error on '{0}' invocation: {1}", nameof(DeleteAsync), ex);
        //    }

        //    return response.ToHttpResponse();
        //}
    }
}