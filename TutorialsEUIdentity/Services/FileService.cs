namespace TutorialsEUIdentity.Services
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);

        public bool DeleteImage(string imageFileName);

        public string GetRootPath(string imagePath);
    }

    public class FileService : IFileService
    {
        private IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this._webHostEnvironment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }

        public string GetRootPath(string imagePath)
        {
                var wwwPath = this._webHostEnvironment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\");

                return path;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var contentPath = this._webHostEnvironment.WebRootPath;
                //
                var path = Path.Combine(contentPath, "Uploads\\");
                Console.WriteLine(path);
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    Console.WriteLine("path not created");
                }

                //
                var extensions = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".PNG" };

                if (!allowedExtensions.Contains(extensions))
                {
                    string msg = string.Format("Only {0} extensions allowed", string.Join(" ", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + extensions;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();

                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }
    }
}
