using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EJ2CoreSampleBrowser.Models;
//File Manager's base functions are available in the below package
using Syncfusion.EJ2.FileManager.Base;
//File Manager's operations are available in the below package
using Syncfusion.EJ2.FileManager.PhysicalFileProvider;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using iTotal.Master.Model;
using System.Net.Http;
using System.Diagnostics;
using System.Text;
using Syncfusion.XlsIO;
using System.IO;

namespace Web.Controllers
{
    public partial class StockUploaderController : FileManagerController
    {
        public StockUploaderController(IHostingEnvironment hostingEnvironment, InvContext context) : base(hostingEnvironment, context)
        {
            // Map the path of the files to be accessed with the host
            this.basePath = hostingEnvironment.ContentRootPath;
            this.operation = new PhysicalFileProvider();
            // Assign the mapped path as root folder
            this.operation.RootFolder(this.basePath + "\\" + this.root);
        }
   
        protected override bool uploadProcess(string filename)
        {
            bool errFind = false;
            if (filename.IndexOf(".csv") > 0)
            {
                string basePath = root + "\\" + filename;
                var lines = System.IO.File.ReadAllLines(basePath).Select(a => a.Split(','));
                var csv = from line in lines
                          select (from piece in line
                                  select piece).ToList();
                List<StockErrorcsv> errList = new List<StockErrorcsv>();
                List<PostStockRequest> slist = new List<PostStockRequest>();
                for (var row = 0; row < csv.ToList().Count(); row++)
                {
                    if (csv.ToList()[row].Count() == 7)
                    {
                        var status = _context.GetByNameAsync(new Status() { Description = csv.ToList()[row][3].ToString() });
                        var minQty = csv.ToList()[row][5].ToString();
                        var maxQty = csv.ToList()[row][6].ToString();
                        var remarks = csv.ToList()[row][4].ToString();
                        if ((status.Result != null))
                        {
                            PostStockRequest stock = new PostStockRequest()
                            {
                                BarCode = csv.ToList()[row][1].ToString(),
                                Description = csv.ToList()[row][2].ToString(),
                                Status = status.Result.ID,
                                MinQty = Convert.ToInt32(minQty),
                                MaxQty = Convert.ToInt32(maxQty),
                                Remarks = remarks
                            };
                            slist.Add(stock);
                        }
                        else
                        {
                            StockErrorcsv err = new StockErrorcsv()
                            {
                                BarCode = csv.ToList()[row][1].ToString(),
                                Status = csv.ToList()[row][3].ToString(),
                                ErrorMsg = "Error : Status not found!"
                            };
                            errList.Add(err);
                            errFind = true;
                        }
                    }
                }
                PostStockListRequest tmp = new PostStockListRequest() { Result = slist };
                postDatatoHost(tmp);
                if (errFind)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("Barcode,Status, ErrorMsg");
                    foreach (var item in errList)
                    {
                        sb.AppendLine(item.ToString());
                    }

                    Console.WriteLine(sb.ToString());
                    System.IO.File.WriteAllText(
                        root + "\\_error_" + filename,
                        sb.ToString());
                }
            }
            else
            {
                //New instance of ExcelEngine is created 
                //Equivalent to launching Microsoft Excel with no workbooks open
                //Instantiate the spreadsheet creation engine
                ExcelEngine excelEngine = new ExcelEngine();

                //Instantiate the Excel application object
                IApplication application = excelEngine.Excel;

                //Assigns default application version
                application.DefaultVersion = ExcelVersion.Excel2013;

                //A existing workbook is opened.    
                string basePath = root + "\\" + filename;
                FileStream sampleFile = new FileStream(basePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(sampleFile);

                //Access first worksheet from the workbook.
                IWorksheet worksheet = workbook.Worksheets[0];

                //Read Excel Cell Value
                List<PostStockRequest> slist = new List<PostStockRequest>();

                for (var row = 3; row <= worksheet.Rows.Count(); row++)
                {
                    //var section = _context.GetByNameAsync(new Section() { Description = worksheet[row, 3].Text.Trim() });
                    //var location = _context.GetByNameAsync(new Location() { Description = worksheet[row, 4].Text.Trim() });
                    var status = _context.GetByNameAsync(new Status() { Description = worksheet[row, 4].DisplayText.Trim() });
                    var minQty = worksheet[row, 6].DisplayText;
                    var maxQty = worksheet[row, 7].DisplayText;
                    var remarks = worksheet[row, 5].Text;
                    var tmp1 = worksheet[row, 9].Text;
                    if ((status.Result != null))
                    {
                        PostStockRequest stock = new PostStockRequest()
                        {
                            BarCode = worksheet[row, 2].Text,
                            Description = worksheet[row, 3].Text,
                            Status = status.Result.ID,
                            MinQty = Convert.ToInt32(minQty),
                            MaxQty = Convert.ToInt32(maxQty),
                            Remarks = remarks
                        };
                        slist.Add(stock);
                    }
                    else
                    {
                        worksheet[row, 11].Text = "Error : Status not found!";
                        errFind = true;
                    }
                }

                PostStockListRequest tmp = new PostStockListRequest() { Result = slist };
                postDatatoHost(tmp);

                if (errFind)
                {
                    //Set the version of the workbook
                    workbook.Version = ExcelVersion.Excel2013;
                    //Saving the workbook
                    FileStream outputStream = new FileStream(root + "\\_error_" + filename, FileMode.Create);
                    workbook.SaveAs(outputStream);
                    //Closing the workbook.
                    outputStream.Close();
                }
                sampleFile.Close();
                //Closing the workbook.
                workbook.Close();

                //Dispose the Excel engine
                excelEngine.Dispose();
            }
            return errFind;
        }

        public async Task<int> postDatatoHost(PostStockListRequest data)
        {
            HttpClient _client = new HttpClient();
            var serverAddress = _context.GetConfigValueAsync("Server.url");
            var currentLink = serverAddress.Result.configValue + "api/Inventory/{0}";
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
    }
}
