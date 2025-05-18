using BlazorFileUpload.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public FileUploadController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files["file"];
            if (file == null || file.Length == 0)
                return BadRequest("没有选择文件");

            var savePath = Path.Combine(_env.ContentRootPath, _config["FileUpload:SavePath"]);

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var filePath = Path.Combine(savePath, file.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Ok(new UploadResult { Message = "文件上传成功", FileName = file.FileName });
        }


        // 使用流式处理（在控制器中）
        [HttpPost("UploadChunk")]
        public async Task<IActionResult> UploadChunk()
        {
            var file = Request.Form.Files["file"];
            var chunkIndex = int.Parse(Request.Form["chunkIndex"]);
            var totalChunks = int.Parse(Request.Form["totalChunks"]);

            // 创建临时文件
            var tempPath = Path.Combine(_env.ContentRootPath, "temp", $"{file.FileName}.part{chunkIndex}");
            using var tempStream = new FileStream(tempPath, FileMode.Create);
            await file.CopyToAsync(tempStream);

            // 合并分片逻辑（需自行实现）
            // ...

            return Ok();
        }
    }
}
