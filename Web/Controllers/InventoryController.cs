using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.QueryBuilder;
using iTotal.Master.Business;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.XlsIO;
using System.Drawing;
using System.Globalization;
using System.IO;
using iTotal.Master.Model;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Diagnostics;
using Syncfusion.EJ2.Base;
using System.Collections;

namespace Web.Controllers
{
    public class InventoryController : Controller
    {
        protected StockServices stockServices = new StockServices();
        protected StockTXServices txServices = new StockTXServices();
        protected LocationServices locationServices = new LocationServices();
        protected StatusServices statusServices = new StatusServices();
        protected SectionServices sectionServices = new SectionServices();
        protected sysServices _sysServices = new sysServices();

        private IHostingEnvironment hostingEnv;

        public InventoryController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }
        #region Status
        public IActionResult SStatus()
        {
            return View();
        }
        public IActionResult _statusDatasource([FromBody]DataManagerRequest dm)
        {
            IEnumerable DataSource = statusServices.getRecords();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Status>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult _statusInsert([FromBody]GridParams<Status> request)
        {
            var entity = statusServices.chkRecord(new Status { Description = request.value.Description });
            statusServices.Insert(request.value);
            return Json(request);
        }
        public IActionResult _statusDelete([FromBody]GridParams<Status> request)
        {
            statusServices.Delete(request.key);
            return Json(request);
        }
        public ActionResult _statusUpdate([FromBody]GridParams<Status> request)
        {
            var entity = statusServices.chkRecord(new Status { Description = request.value.Description });
            if (entity)
            {
                statusServices.Update(request.value);
            }
            return Json(request);
        }
        #endregion

        #region Section
        public IActionResult Section()
        {
            return View();
        }
        public IActionResult _sectionDatasource([FromBody]DataManagerRequest dm)
        {
            IEnumerable DataSource = sectionServices.getRecords();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Section>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult _sectionInsert([FromBody]GridParams<Section> request)
        {
            var entity = sectionServices.chkRecord(new Section { SectionCode = request.value.SectionCode });
            sectionServices.Insert(request.value);
            return Json(request);
        }
        public IActionResult _sectionDelete([FromBody]GridParams<Section> request)
        {
            sectionServices.Delete(request.key);
            return Json(request);
        }
        public ActionResult _sectionUpdate([FromBody]GridParams<Section> request)
        {
            var entity = sectionServices.chkRecord(new Section { SectionCode = request.value.SectionCode });
            if (entity)
            {
                sectionServices.Update(request.value);
            }
            return Json(request);
        }
        #endregion

        #region Location
        public IActionResult Location()
        {
            return View();
        }
        public IActionResult _LocationDatasource([FromBody]DataManagerRequest dm)
        {
            IEnumerable DataSource = locationServices.getRecords();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Location>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult _LocationInsert([FromBody]GridParams<Location> request)
        {
            var entity = locationServices.chkRecord(new Location { LocationCode = request.value.LocationCode });
            locationServices.Insert(request.value);
            return Json(request);
        }
        public IActionResult _LocationDelete([FromBody]GridParams<Location> request)
        {
            locationServices.Delete(request.key);
            return Json(request);
        }
        public ActionResult _LocationUpdate([FromBody]GridParams<Location> request)
        {
            var entity = locationServices.chkRecord(new Location { LocationCode = request.value.LocationCode });
            if (entity)
            {
                locationServices.Update(request.value);
            }
            return Json(request);
        }
        #endregion
        public IActionResult Uploader()
        {
            return View();
        }

        #region Stock
        public IActionResult Stock()
        {
            //QueryBuilderRule rule = new QueryBuilderRule()
            //{
            //    Condition = "or",
            //    Rules = new List<QueryBuilderRule>()
            //    {
            //        new QueryBuilderRule { Label="BarCode", Field="BarCode", Type="string", Operator="equal", Value = "" }
            //    }
            //};

            ViewBag.sectionList = sectionServices.getRecords().ToList();
            ViewBag.locationList = locationServices.getRecords().ToList();
            ViewBag.statusList = statusServices.getRecords().ToList();

            return View();
        }
        public IActionResult _StockDatasource([FromBody]DataManagerRequest dm)
        {
            IEnumerable DataSource = stockServices.getRecords();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<Stock>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public IActionResult _StockInsert([FromBody]GridParams<Stock> request)
        {
            var entity = stockServices.chkRecord(new Stock { BarCode = request.value.BarCode });
            stockServices.Insert(request.value);
            return Json(request);
        }
        public IActionResult _StockDelete([FromBody]GridParams<Stock> request)
        {
            stockServices.Delete(request.key);
            return Json(request);
        }
        public ActionResult _StockUpdate([FromBody]GridParams<Stock> request)
        {
            var entity = stockServices.chkRecord(new Stock { BarCode = request.value.BarCode });
            if (entity)
            {
                stockServices.Update(request.value);
            }
            return Json(request);
        }
        #endregion

        public IActionResult TXStock()
        {
            ViewBag.sectionList = sectionServices.getRecords().ToList();
            ViewBag.locationList = locationServices.getRecords().ToList();
            return View();
        }

        #region Stock Transcation
        public IActionResult TXStock2()
        {
            ViewBag.sectionList = sectionServices.getRecords().ToList();
            ViewBag.locationList = locationServices.getRecords().ToList();
            return View();
        }
        public IActionResult _txDatasource([FromBody]TXDataRequest dm)
        {
            if (dm == null) return null;
            IEnumerable DataSource = txServices.getDisplayStockTX(dm.stockID);
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<StockTX2>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        //public IActionResult _txInsert([FromBody]GridParams<StockTX> request)
        //{
        //    var entity = txServices.chkRecord(new StockTX { BarCode = request.value.BarCode });
        //    txServices.Insert(request.value);
        //    return Json(request);
        //}
        //public IActionResult _txDelete([FromBody]GridParams<StockTX> request)
        //{
        //    txServices.Delete(request.key);
        //    return Json(request);
        //}
        //public ActionResult _txUpdate([FromBody]GridParams<StockTX> request)
        //{
        //    var entity = txServices.chkRecord(new Stock { BarCode = request.value.BarCode });
        //    if (entity)
        //    {
        //        txServices.Update(request.value);
        //    }
        //    return Json(request);
        //}
        #endregion
        public IActionResult Enquire()
        {
            ViewBag.sectionList = sectionServices.getRecords().ToList();
            ViewBag.locationList = locationServices.getRecords().ToList();
            return View();
        }
        public IActionResult _EnquireDatasource([FromBody]TXDataRequest dm)
        {
            IEnumerable DataSource = stockServices.getStockTotal();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<StockTotal2>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        #region
        public IActionResult _StockTX(string id)
        {
            ViewBag.stockID = id;
            return PartialView("_StockTX");
        }
        #endregion

        #region Stock Upload
        // Upload method for chunk-upload and normal upload
        public void StockUpload(IList<IFormFile> chunkFile, IList<IFormFile> UploadFiles)
        {
            long size = 0;
            var currTime = DateTime.Now.ToString("yyyyMMddHHmmss_");
            try
            {
                // for chunk-upload
                foreach (var file in chunkFile)
                {
                    var filename = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                    var errfilename = hostingEnv.WebRootPath + $@"\files\error_" + $@"{filename}";
                    filename = hostingEnv.WebRootPath + $@"\files\" + currTime + $@"{filename}";
                    size += file.Length;
                    if (System.IO.File.Exists(errfilename))
                    {
                        System.IO.File.Delete(errfilename);
                    }
                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        using (FileStream fs = System.IO.File.Open(filename, FileMode.Append))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    if (uploadProcess(filename, errfilename))
                    {
                        Response.Clear();
                        Response.StatusCode = 300;
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Testing Completed!!";
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please check the File";
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }

            // for normal upload
            try
            {
                foreach (var file in UploadFiles)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    var errfilename = hostingEnv.WebRootPath + $@"\files\error_" + $@"{filename}";
                    filename = hostingEnv.WebRootPath + $@"\files\" + currTime + $@"{filename}";
                    size += file.Length;
                    if (System.IO.File.Exists(errfilename))
                    {
                        System.IO.File.Delete(errfilename);
                    }
                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    if (uploadProcess(filename, errfilename))
                    {
                        Response.Clear();
                        Response.StatusCode = 300;
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Testing Completed!!";
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please check the File";
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 300;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [HttpGet]
        public IActionResult GetErrorDownload(string filename)
        {
            var errfilename = hostingEnv.WebRootPath + $@"\files\error_" + $@"{filename}";
            var contentType = "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = System.IO.File.ReadAllBytes(errfilename);

            return File(fileBytes, contentType, "error_" + filename);
        }

        private bool uploadProcess(string filename, string errfile)
        {
            bool errFind = false;
            //New instance of ExcelEngine is created 
            //Equivalent to launching Microsoft Excel with no workbooks open
            //Instantiate the spreadsheet creation engine
            ExcelEngine excelEngine = new ExcelEngine();

            //Instantiate the Excel application object
            IApplication application = excelEngine.Excel;

            //Assigns default application version
            application.DefaultVersion = ExcelVersion.Excel2013;

            //A existing workbook is opened.              
            string basePath = filename;
            FileStream sampleFile = new FileStream(basePath, FileMode.Open);

            IWorkbook workbook = application.Workbooks.Open(sampleFile);

            //Access first worksheet from the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];

            //Read Excel Cell Value
            List<PostStockRequest> slist = new List<PostStockRequest>();

            for (var row = 4; row<= worksheet.Rows.Count(); row++)
            {
                var section = sectionServices.getRecordsbyName( worksheet[row, 3].Text);
                var location = locationServices.getRecordsbyName( worksheet[row, 4].Text );
                var status = statusServices.getRecordsbyName(worksheet[row, 5].Text);
                var minQty = worksheet[row, 6].CalculatedValue;
                var maxQty = worksheet[row, 7].CalculatedValue;
                var remarks = worksheet[row, 8].Text;
                var tmp1 = worksheet[row, 9].Text;
                if ((section > 0) && (location > 0) && (status > 0))
                {
                    PostStockRequest stock = new PostStockRequest()
                    {
                        BarCode = worksheet[row, 1].Text,
                        Description = worksheet[row, 2].Text,
                        Section = section,
                        Location = location,
                        Status = status,
                        MinQty = Convert.ToInt32(minQty),
                        MaxQty = Convert.ToInt32(maxQty),
                        Remarks = remarks
                    };
                    slist.Add(stock);
                }
                else
                {
                    worksheet[row, 9].Text = "Error : Section / Location / Section not found!";
                    errFind = true;
                }
            }

            PostStockListRequest tmp = new PostStockListRequest() { Result = slist };
            postDatatoHost(tmp);

            if (errFind)
            {
                workbook.Version = ExcelVersion.Excel2013;
                //Saving the workbook
                using (FileStream fs = System.IO.File.Create(errfile))
                {
                    workbook.SaveAs(fs);
                    fs.Flush();
                }
            }

            //Closing the workbook.
            workbook.Close();

            //Dispose the Excel engine
            excelEngine.Dispose();

            return errFind;
        }

        public async Task<int> postDatatoHost(PostStockListRequest data)
        {
            HttpClient _client = new HttpClient();
            var serverAddress = _sysServices.getConfigValue("Server.url");
            var currentLink = serverAddress + "api/Inventory/{0}";
            var uri = new Uri(string.Format(currentLink, "uploadStock"));
            try
            {
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(uri, content);

                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tpostDatatoHost successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return -1;
            }
            return 0;
        }
        #endregion
    }

    public class TXDataRequest : DataManagerRequest
    {
        public long stockID { get; set; }
    }
}