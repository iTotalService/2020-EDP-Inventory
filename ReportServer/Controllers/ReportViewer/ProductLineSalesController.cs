using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ReportsCoreSamples.Controllers
{
    public class ProductLineSalesController : PreviewController
    {
        public IActionResult Index()
        {
            ViewBag.toolbarSettings = new Syncfusion.Reporting.Models.ReportViewer.ToolbarSettings();
            ViewBag.toolbarSettings.Items = Syncfusion.Reporting.ReportViewerEnums.ToolbarItems.Export
                                               & ~Syncfusion.Reporting.ReportViewerEnums.ToolbarItems.Parameters;
            return View();
        }
        
    }
}