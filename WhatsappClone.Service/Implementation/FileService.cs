using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Service.Abstract;

namespace WhatsappClone.Service.Implementation
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env; // ده بنستخدمه عشان نعرف مسار الـ Root بتاع المشروع
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subDirectory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("الملف فارغ أو غير موجود.");



            // 1. تحديد مسار المجلد الأساسي للحفظ (مثلاً wwwroot/uploads)
            // ده بيعتمد على البيئة اللي فيها التطبيق (Development, Production)
            var uploadsFolder = Path.Combine(_env.WebRootPath, subDirectory);

            // 2. إنشاء المجلد لو مش موجود
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);

            }

            // 3. توليد اسم فريد للملف
            // هنا بنستخدم Guid.NewGuid() عشان نضمن إن مفيش ملفين بنفس الاسم
            // وبناخد امتداد الملف الأصلي (مثلاً .jpg)
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 4. حفظ الملف على السيرفر
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream); // بننسخ محتوى الملف للـ FileStream
            }

            // 5. بنرجع المسار النسبي للملف عشان يتخزن في قاعدة البيانات أو يرجع للـ Client
            // مثلاً: /uploads/some-guid.jpg
            var context = httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{context?.Scheme}://{context?.Host}";
            return $"{baseUrl}/{subDirectory}/{uniqueFileName}";
        }


        // ممكن تضيف ميثود للحذف
        public Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.CompletedTask;

            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return Task.CompletedTask;
        }

    }
}
