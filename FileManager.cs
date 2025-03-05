/*
--/ File Manager Class \--

Overview:
 This class is used to handle loading, reading and saving files.

- Initialize method sets the storage provider for the file manager.
    Not really necessary, but too lazy to refactor this.

- LoadB2img method opens a file explorer to select a `.b2img.txt` file.
    It then creates a new B2Image object and calls the DrawImage method to display the image
    (also calls the ReadB2img method to read the loaded file).

- SaveB2img method saves the current image data to the loaded file path.

- SaveAsB2img method opens a file explorer to save the current image data to a new file path.

- ReadB2img method reads a `.b2img.txt` file and returns a 2D integer array.
    The dimensions of the array depend on the values provided in the file.
    Returns an empty 2D array if the file is missing or formatted incorrectly.

- WriteB2img debugging method that prints a 2D array representation of an image to the console.
    This is useful for debugging or visualizing the image data.

- FlipHorizontal method flips the image data from left to right.

- FlipVertical method flips the image data from top to bottom.

*/
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform.Storage;

namespace HW1;

public static class FileManager
{

    private static IStorageProvider? StorageProvider;

    public static void Initialize(IStorageProvider storageProvider)
    {
        StorageProvider = storageProvider;
    }

    public static async Task LoadB2img(MainWindow callback)
    {
        var fileTypes = new FilePickerFileType[]
        {
        new("b2img.txt File")
        {
            Patterns = ["*.b2img.txt"]
        }
        };

        var options = new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = fileTypes,
            Title = "Select B2img File"
        };

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var result = await StorageProvider.OpenFilePickerAsync(options);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        if (result != null && result.Count > 0)
        {
            var file = result[0];
            new B2Image(file);
            callback.DrawImage();
            callback.InformationTextBlock.Text = $"Loaded a {B2Image.Height}x{B2Image.Width} image:";
        }
    }
    public static async Task SaveB2img()
    {
        if (B2Image.FilePath == null || B2Image.ImageData == null)
        {
            Console.WriteLine("No image loaded to save.");
            return;
        }

        // Save to existing file path
        string imageContent = $"{B2Image.ImageData.GetLength(0)} {B2Image.ImageData.GetLength(1)}\n";
        for (int i = 0; i < B2Image.ImageData.GetLength(0); i++)
        {
            for (int j = 0; j < B2Image.ImageData.GetLength(1); j++)
            {
                imageContent += B2Image.ImageData[i, j].ToString();
            }
        }

        await File.WriteAllTextAsync(B2Image.FilePath, imageContent);
    }
    public static async Task SaveAsB2img()
    {
        if (B2Image.ImageData == null)
        {
            Console.WriteLine("No image loaded to save.");
            return;
        }

        var fileTypes = new FilePickerFileType[]
        {
        new("b2img.txt File")
        {
            Patterns = ["*.b2img.txt"]
        }
        };

        var options = new FilePickerSaveOptions
        {
            FileTypeChoices = fileTypes,
            Title = "Save B2img File",
            DefaultExtension = ".b2img.txt",
        };

        var file = await StorageProvider!.SaveFilePickerAsync(options);
        if (file != null)
        {
            string imageContent = $"{B2Image.ImageData.GetLength(0)} {B2Image.ImageData.GetLength(1)}\n";
            for (int i = 0; i < B2Image.ImageData.GetLength(0); i++)
            {
                for (int j = 0; j < B2Image.ImageData.GetLength(1); j++)
                {
                    imageContent += B2Image.ImageData[i, j].ToString();
                }
            }
            var filePath = Path.ChangeExtension(file.Path.LocalPath, ".b2img.txt");
            await File.WriteAllTextAsync(filePath, imageContent);
        }
    }
    public static int[,] ReadB2img(string path = "smile.b2img.txt")
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("File not found.");
            return new int[0, 0];
        }

        string[] lines = File.ReadAllLines(path);
        if (lines.Length < 2)
        {
            Console.WriteLine("Invalid file format.");
            return new int[0, 0];
        }

        // Read image dimensions (height and width)
        string[] dimensions = lines[0].Split(' ');
        if (!int.TryParse(dimensions[0], out int height) || !int.TryParse(dimensions[1], out int width))
        {
            Console.WriteLine("Error: Invalid dimensions in file.");
            return new int[0, 0];
        }

        // Read pixel data and store in a 2D array
        string pixelData = lines[1];
        int[,] image = new int[height, width];
        int index = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (index < pixelData.Length)
                {
                    image[i, j] = pixelData[index] == '1' ? 1 : 0;
                    index++;
                }
            }
        }
        return image;
    }

    public static void WriteB2img(int[,] image)
    {
        int height = image.GetLength(0);
        int width = image.GetLength(1);

        Console.WriteLine($"Image {height}x{width}:");
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write($"{image[i, j]} ");
            }
            Console.WriteLine();
        }
    }
    public static void FlipHorizontal(MainWindow callback)
    {
        int rows = B2Image.Height;
        int cols = B2Image.Width;
        int[,] flipped = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
#pragma warning disable CS8602
                flipped[i, j] = B2Image.ImageData[i, cols - j - 1];
#pragma warning restore CS8602
            }
        }
        B2Image.ImageData = flipped;


        callback.DrawImage();
    }

    public static void FlipVertical(MainWindow callback)
    {
        int rows = B2Image.Height;
        int cols = B2Image.Width;
        int[,] flipped = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
#pragma warning disable CS8602
                flipped[i, j] = B2Image.ImageData[rows - i - 1, j];
#pragma warning restore CS8602
            }
        }
        B2Image.ImageData = flipped;

        callback.DrawImage();
    }

}