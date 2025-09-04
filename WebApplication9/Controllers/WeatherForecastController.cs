using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace WebApplication9.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api2/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Tags("get")]
        [EndpointSummary("WeatherForecast")]
        [EndpointDescription("描述")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet(Name = "Test")]
        [Tags("get")]
        [EndpointSummary("Test")]
        [EndpointDescription("描述")]
        [AllowAnonymous]
        public IEnumerable<WeatherForecast> Test()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost(Name = "UpdateFile")]
        [Tags("post")]
        [EndpointSummary("UpdateFile")]
        [EndpointDescription("上传文件")]
        [AllowAnonymous]
        public async Task<OkObjectResult> UpdateFile(IFormFile[] files,string pass)
        {
            if (files == null || files.Length == 0 || !"332233".Equals(pass) || string.IsNullOrWhiteSpace(pass))
            { 
            
                return Ok("ok");
            }
            string wpath = @"file/";
            foreach (var file in files)
            {
                //using (var memoryStream = new MemoryStream())
                //{
                //    await file.CopyToAsync(memoryStream);
                //    byte[] fileBytes = memoryStream.ToArray();


                //}
                int bsize = 1024;
                using (FileStream fileStream = new FileStream(wpath + DateTime.Now.ToString("yyyy-MM-dd-")+file.FileName, FileMode.Create, FileAccess.Write, FileShare.None, bsize))
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    using (var reader = new BinaryReader(file.OpenReadStream()))
                    {
                        byte[] bytes = new byte[bsize];
                        int numBytesToRead = (int)file.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            //int n = bytes.Length < numBytesToRead ? bytes.Length : numBytesToRead;


                            // 读取固定大小的数据块，例如4096字节（1个页面）
                            int n = reader.Read(bytes, 0, bytes.Length); // 根据需要调整大小




                            //byte[] buffer = new byte[n]; // 创建缓冲区
                            //int bytesRead;
                            //while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                            //{
                            binaryWriter.Write(bytes, 0, n); // 写入文件
                                                                        //}

                            //using (FileStream destStream = new FileStream(wpath+file.FileName, FileMode.Create, FileAccess.Write)) // 创建或覆盖目标文件流
                            // {
                            //      destStream.Write(bytes, 0, bytes.Length); // 将读取的数据写入目标文件流中
                            //  }

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
            }
            //file.FileName = Path.GetFileName(file.FileName);
            return Ok($" filename: {files.Length}");
        }
    }
}
