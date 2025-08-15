using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.APP.PetCircle
{
    public class BatchUploadImages
    {
        // 图片存储路径
        private readonly string _imageStoragePath;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BatchUploadImages(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            // 设置图片存储目录
            _imageStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images");

            // 确保目录存在
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(_imageStoragePath);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // 1. 验证文件是否有效
            if (file == null || file.Length == 0)
                throw new ArgumentException("上传的文件为空");

            // 2. 验证文件类型（仅允许图片格式）
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new InvalidOperationException("不支持的文件类型，仅允许jpg、jpeg、png、gif格式");

            // 3. 验证文件大小（限制10MB以内）
            if (file.Length > 10 * 1024 * 1024) // 10MB
                throw new InvalidOperationException("文件大小不能超过10MB");

            // 4. 生成唯一文件名（避免冲突）
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            // 5. 确保上传目录存在
            var uploadDir = _imageStoragePath;
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            // 6. 保存文件到服务器
            var filePath = Path.Combine(uploadDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // 异步写入文件
            }

            // 7. 生成在线访问URL并返回
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}/images/{fileName}";
        }
    }

 }
