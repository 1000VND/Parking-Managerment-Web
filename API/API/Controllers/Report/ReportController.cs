using API.Dto.Report;
using DevExpress.DataAccess.Json;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers.Report
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReportController : BaseApiController
    {
        #region -- Car report
        [HttpPost("CarReport")]
        public async Task<ActionResult> CarReport([FromBody] CarReportDto input)
        {
            XtraReport report = new CarReportPrint();

            string repJsonP = JsonSerializer.Serialize(input);

            var jsonDataSource = new JsonDataSource
            {
                JsonSource = new CustomJsonSource(repJsonP)
            };

            jsonDataSource.Fill();

            report.DataSource = jsonDataSource;

            report.DataMember = null;

            string contentType = string.Format("application/{0}", "docx");

            byte[] fileBytes = await ExportFileTypeAsync(report, "docx", contentType, true);

            if (fileBytes != null)
                return File(fileBytes, contentType);
            else
                return BadRequest();
        }
        #endregion

        #region -- Loại file xuất
        private async Task<byte[]> ExportFileTypeAsync(XtraReport report, string format, string contentType, [AllowNull] bool editDoc)
        {
            byte[] _byteArray;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    switch (format)
                    {
                        case "pdf":
                            await report.ExportToPdfAsync(ms);
                            break;
                        case "docx":
                            if (editDoc == true)
                            {
                                // Using DocxExportOptions to export Origional Microsoft Word to allow users can modify after exporting
                                DocxExportOptions DocxExportOptions = new DocxExportOptions()
                                {
                                    ExportMode = DocxExportMode.SingleFile,
                                    ExportPageBreaks = false,
                                    TableLayout = true
                                };
                                await report.ExportToDocxAsync(ms, DocxExportOptions);
                            }
                            else
                            {
                                // Umcomment the snippet code to export Doc file that user cannot modify exported file
                                DocxExportOptions docxExportOptions = new DocxExportOptions()
                                {
                                    ExportMode = DocxExportMode.SingleFilePageByPage
                                };
                                await report.ExportToDocxAsync(ms, docxExportOptions);

                            }
                            break;
                        case "xls":
                            await report.ExportToXlsAsync(ms);
                            break;
                        case "xlsx":
                            await report.ExportToXlsxAsync(ms);
                            break;
                        case "rtf":
                            await report.ExportToRtfAsync(ms);
                            break;
                        case "mht":
                            await report.ExportToMhtAsync(ms);
                            break;
                        case "html":
                            await report.ExportToHtmlAsync(ms);
                            break;
                        case "txt":
                            await report.ExportToTextAsync(ms);
                            break;
                        case "csv":
                            CsvExportOptionsEx csvExportOptionsEx = new CsvExportOptionsEx()
                            {
                                Separator = ",",
                                SkipEmptyColumns = false,
                                SkipEmptyRows = false,
                                //EncodeExecutableContent = DefaultBoolean.True
                            };
                            await report.ExportToCsvAsync(ms, csvExportOptionsEx);
                            break;
                        case "png":
                            await report.ExportToImageAsync(ms, new ImageExportOptions() { Format = System.Drawing.Imaging.ImageFormat.Png });
                            break;
                    }
                    _byteArray = ms.ToArray();
                }
            }
            catch
            {
                _byteArray = null;
            }
            report.Dispose();
            return _byteArray;
        }
        #endregion
    }
}
