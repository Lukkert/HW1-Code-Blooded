/*
--/ B2Image Class \--

Overview:
 This class is used to store the image data and file path of a `.b2img.txt` file.
    Static properties mean that it is not necessary to create an instance of the class to access the properties.
    Creating a new B2Image instance will set the `FilePath`, `ImageData`, `Width` and `Height` properties.
    Should only be created in the file manager class, when loading a new image.

*/

using Avalonia.Platform.Storage;

namespace HW1
{
    public class B2Image
    {
        public static string? FilePath { get; private set; }
        public static int[,]? ImageData { get; private set; }
        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public B2Image(IStorageFile file) 
        {
            FilePath = file.Path.LocalPath;
            ImageData = FileManager.ReadB2img(FilePath);
            Width = ImageData.GetLength(1);
            Height = ImageData.GetLength(0);
            // Debugging
            FileManager.WriteB2img(ImageData);
        }

    }
}