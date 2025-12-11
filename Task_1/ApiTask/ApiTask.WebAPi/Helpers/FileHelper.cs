namespace ApiTask.WebApi.Helpers
{
    public static class FileHelper
    {
        public static async Task<string?> SaveFileAsync(IFormFile? file, string uploadPath, string[]? allowedExtensions = null)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            // Check file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (allowedExtensions != null && !allowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File extension {extension} is not allowed.");
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public static void DeleteFile(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return;
            }

            try
            {
                File.Delete(filePath);
            }
            catch
            {
                // Ignore deletion errors
            }
        }
    }
}
