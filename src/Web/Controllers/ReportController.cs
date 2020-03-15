using FastReport;
using FastReport.Export.Html;
using FastReport.Export.Pdf;
using FastReport.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Web.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace Microsoft.eShopWeb.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "admin")] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class ReportController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public ReportController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        // Create a collection of data of type Reports. Add two reports.
        Reports[] reportItems = new Reports[]
        {
            new Reports { Id = 1, ReportName = "sales.frx" },
            new Reports { Id = 2, ReportName = "zakaz.frx" }
        };

        [HttpGet()]
        public IActionResult IndexReport()
        {
            //new MainDataSet();
            return View();
        }

        //Атрибут имеет обязательный параметр id
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromQuery] ReportQuery query)
        {
            string mime = "application/" + query.Format; //MIME-заголовок со значением по умолчанию
                                                          // Найти отчет
            Reports reportItem = reportItems.FirstOrDefault((p) => p.Id == id); //получаем значение коллекции по идентификатору 
            if (reportItem != null)
            {
                string webRootPath = _hostingEnvironment.WebRootPath; //определяем путь к папке wwwroot
                string reportPath = (webRootPath + "/App_Data/" + reportItem.ReportName); //определяем путь к отчету

                string dataPath = "";
                switch (reportItem.Id)
                {
                    case 1:
                        dataPath = (webRootPath + "/App_Data/XmlDataSet2.xml");//определяем путь к базе данных
                        MainDataSet.ConnectToData(_configuration.GetConnectionString("CatalogConnection"), dataPath); 
                        break;
                    case 2:
                        dataPath = (webRootPath + "/App_Data/XmlDataSet.xml");//определяем путь к базе данных
                        MainDataSet.ConnectToDataReport2(_configuration.GetConnectionString("CatalogConnection"), dataPath);
                        break;
                }
                
                    using (MemoryStream stream = new MemoryStream()) //Создаем поток для отчета
                {
                    try
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            //Заполняем источник данными
                            dataSet.ReadXml(dataPath);
                            //Включаем веб режим FastReport
                            Config.WebMode = true;
                            using (Report report = new Report())
                            {
                                report.Load(reportPath); //Загружаем отчет
                                report.RegisterData(dataSet, "Connection"); //Регистрируем данные в отчете
                                if (query.Parameter != null)
                                {
                                    report.SetParameterValue("Parameter", query.Parameter); //задаем значение параметра отчета, если передано значение параметра в URL
                                }
                                report.Prepare();//подготавливаем отчет
                                                 //если выбран формат pdf
                                if (query.Format == "pdf")
                                {
                                    //Экспорт отчета в PDF
                                    PDFExport pdf = new PDFExport();
                                    //Используем поток для хранения отчета, чтобы не создавать лишние файлы
                                    report.Export(pdf, stream);
                                }
                                //если выбран формат отчета html
                                else if (query.Format == "html")
                                {
                                    //Экспорт отчета в HTML
                                    HTMLExport html = new HTMLExport();
                                    html.SinglePage = true; //отчет на одной странице
                                    html.Navigator = false; //навигационная панель сверху
                                    html.EmbedPictures = true; //встраивает изображения в документ
                                    report.Export(html, stream);
                                    mime = "text/" + query.Format; //переопределяем mime для html
                                }
                            }
                        }
                        //получаем имя результирующего файла отчета с нужным расширением
                        var file = String.Concat(Path.GetFileNameWithoutExtension(reportPath), ".", query.Format);
                        //если параметр inline истина, то открываем отчет в браузере
                        if (query.Inline)
                            return File(stream.ToArray(), mime);
                        else
                            //иначе скачиваем файл отчета
                            return File(stream.ToArray(), mime, file); // attachment
                    }
                    //Обрабатываем исключения
                    catch
                    {
                        return new NoContentResult();
                    }
                    finally
                    {
                        stream.Dispose();
                    }
                }
            }
            else
                return NotFound();
        }
        public class ReportQuery
        {
            // Format of resulting report: png, pdf, html
            public string Format { get; set; }
            // Enable Inline preview in browser (generates "inline" or "attachment")
            public bool Inline { get; set; }
            // Value of "Parameter" variable in report
            public string Parameter { get; set; }
        }
    }
}