using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        // 图片存储路径
        private readonly string _imageStoragePath;
        private readonly BatchUploadImagesService _batchUploadImagesService;

        public ImageController(BatchUploadImagesService batchUploadImagesService)
        {
            _batchUploadImagesService = batchUploadImagesService;
            // 设置图片存储目录
            _imageStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images");

            // 确保目录存在
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            try
            {
                // 检查文件是否为空
                if (image == null || image.Length == 0)
                {
                    return BadRequest("未上传图片文件");
                }

                // 检查文件类型是否为图片
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("只允许上传jpg、jpeg、png、gif格式的图片");
                }

                // 生成唯一的文件名，避免重复
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(_imageStoragePath, fileName);

                // 保存文件到本地
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // 生成在线访问链接
                var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";

                // 返回结果
                return Ok(new
                {
                    code = 200,
                    Success = true,
                    Message = "图片上传成功",
                    ImageUrl = imageUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"上传失败: {ex.Message}");
            }
        }
            /// <summary>
            /// 批量上传图片
            /// </summary>
            /// <param name="dto"></param>
            /// <returns></returns>
            [HttpPost("images/batch")]
            public async Task<IActionResult> BatchUpload([FromForm] BatchUploadDto dto)
            {
                try
                {
                    var imageUrls = await _batchUploadImagesService.BatchUploadImagesAsync(dto.Files);
                    return Ok(new
                    {
                        code = 200,
                        message = "批量上传成功",
                        data = new { imageUrls = imageUrls }
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { code = 400, message = ex.Message });
                }
            }
        }
    public class BatchUploadDto
    {
        // 用于接收多个文件，参数名需与前端保持一致
        public List<IFormFile> Files { get; set; }
    }
}
