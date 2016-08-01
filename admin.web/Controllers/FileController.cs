using CsvHelper;
using CsvHelper.Configuration;
using DonorGateway.Data;
using DonorGateway.Domain;
using EntityFramework.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using admin.web.Services;
using admin.web.ViewModels;
using web.ViewModels;

namespace web.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private readonly DataContext context;

        public FileController()
        {
            context = new DataContext();
        }


        [HttpPost, Route("guest/{id:int}")]
        public IHttpActionResult Guest(int id)
        {
            var httpRequest = HttpContext.Current.Request;
            var startTime = DateTime.Now;
            try
            {
                var postedFile = httpRequest.Files[0];
                // Fix for IE file path issue.
                var filename = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                var filePath = HttpContext.Current.Server.MapPath(@"~\app_data\" + filename);
                postedFile.SaveAs(filePath);

                var configuration = new CsvConfiguration()
                {
                    IsHeaderCaseSensitive = false,
                    WillThrowOnMissingField = false,
                    IgnoreReadingExceptions = true,
                    ThrowOnBadData = false,
                    SkipEmptyRecords = true
                };
                var csv = new CsvReader(new StreamReader(filePath, Encoding.Default, true), configuration);

                csv.Configuration.RegisterClassMap<GuestMap>();
                var list = csv.GetRecords<Guest>().ToList();
                foreach (var guest in list)
                {
                    guest.EventId = id;
                }
                using (context)
                {
                    EFBatchOperation.For(context, context.Guests).InsertAll(list);
                }

                var message = $"Processed {list.Count} records";
                var result = new OperationResult(true, message, DateTime.Now.Subtract(startTime));

                csv.Dispose();
                return Ok(message);

            }
            catch (Exception ex)
            {
                var message = $"Error occurred processing records. {ex.Message}";
                return BadRequest(message);
            }

        }

    }
}