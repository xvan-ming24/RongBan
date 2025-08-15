using Dao.APP.PetCircle;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BatchUploadImagesService
    {
        private readonly  BatchUploadImages _batchUploadImages;
        public BatchUploadImagesService(BatchUploadImages batchUploadImages)
        {
            _batchUploadImages = batchUploadImages;
        }
        public async Task<List<string>> BatchUploadImagesAsync(List<IFormFile> files)
        {
            var imageUrls = new List<string>();

            // 验证文件列表是否为空
            if (files == null || !files.Any())
                throw new ArgumentException("未上传任何文件");

            // 验证文件总数（限制最多9张，可根据需求调整）
            if (files.Count > 9)
                throw new InvalidOperationException("最多只能上传9张图片");

            // 循环处理每个文件
            foreach (var file in files)
            {
                // 复用单张上传的逻辑（调用之前的UploadImageAsync方法）
                var url = await _batchUploadImages.UploadImageAsync(file);
                imageUrls.Add(url);
            }

            return imageUrls;
        }
    }
}
