namespace Company.PL.Helpers
{
    public class DocumentSettings
    {
        // 1. Upload
        // ImageName
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Location
            //string folderPath = "D:\\C#\\Company\\Company.PL\\wwwroot\\files\\" + folderName;

            //var folerPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + @"\\wwwroot\files", folderName);

            // 2. Get File Name and make it unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            // File Path
            var filePath = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }

        // 2. Delete
        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
