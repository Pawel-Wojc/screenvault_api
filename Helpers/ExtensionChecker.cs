namespace api_screenvault.Helpers
{
    public static class ExtensionChecker
    {
        public  static readonly List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".webp", ".svg", ".heic", ".ico" };
        public static bool ExtensionIsValid(string fileName) { 
            var extension = System.IO.Path.GetExtension(fileName);
            if (allowedExtensions.All(ext => ext != extension))
            {
                return false;
            }
            return true;
        }
    }
}
