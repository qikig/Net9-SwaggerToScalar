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
        [EndpointDescription("����")]
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
        [EndpointDescription("����")]
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
        [EndpointDescription("�ϴ��ļ�")]
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


                            // ��ȡ�̶���С�����ݿ飬����4096�ֽڣ�1��ҳ�棩
                            int n = reader.Read(bytes, 0, bytes.Length); // ������Ҫ������С




                            //byte[] buffer = new byte[n]; // ����������
                            //int bytesRead;
                            //while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                            //{
                            binaryWriter.Write(bytes, 0, n); // д���ļ�
                                                                        //}

                            //using (FileStream destStream = new FileStream(wpath+file.FileName, FileMode.Create, FileAccess.Write)) // �����򸲸�Ŀ���ļ���
                            // {
                            //      destStream.Write(bytes, 0, bytes.Length); // ����ȡ������д��Ŀ���ļ�����
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
