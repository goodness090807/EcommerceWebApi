using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Interfaces
{
    /// <summary>
    /// 檔案資料庫操作服務
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="extension"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        Task<string> SaveFile(IFormFile formFile, string extension, string containerName);
        /// <summary>
        /// 更新檔案
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="extension"></param>
        /// <param name="containerName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<string> UpdateFile(IFormFile formFile, string extension, string containerName, string filePath);
        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="fileRoute"></param>
        /// <param name="containerName"></param>
        /// <returns></returns>
        Task DeleteFile(string fileRoute, string containerName);
    }
}
