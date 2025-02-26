/*
--/ File Manager Class \--

Made by: Gene

Overview:
 This class is used to handle reading and saving files.

- ReadB2img method reads a `.b2img.txt` file and returns a 2D integer array.
    The dimensions of the array depend on the values provided in the file.
    Returns an empty 2D array if the file is missing or formatted incorrectly.

- WriteB2img method prints a 2D array representation of an image to the console.
    This is useful for debugging or visualizing the image data.

*/
using System;
using System.IO;

namespace HW1;

public static class FileManager
{
    public static int[,] ReadB2img(string path = "smile.b2img.txt")
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("File not found.");
            return new int[0, 0]; // Kinda weird, but it works
        }   

        // (temp) Read the whole file into a string array, where lines[0] is the first line and lines[1] is the second line.
        string[] lines = File.ReadAllLines(path);  // (temp) Example: {"6 7", "010001000000000000000000000010000010111110"}
        // (temp) If the file has more than 2 lines the method fails.
        if (lines.Length < 2)
        {
            Console.WriteLine("Invalid file format.");
            return new int[0, 0]; // Kinda weird, but it works
        }

        // Read image dimensions (height and width)
        // (temp) Splits "6 7" -> {"6", "7"}
        string[] dimensions = lines[0].Split(' ');
        // (temp) Parse the array's first and second index(height and width) and make sure it is an integer
        if (!int.TryParse(dimensions[0], out int height) || !int.TryParse(dimensions[1], out int width))
        {
            Console.WriteLine("Error: Invalid dimensions in file.");
            return new int[0, 0];
        }

        // Read pixel data and store in a 2D array
        string pixelData = lines[1]; // (temp) "010001000000000000000000000010000010111110"
        int[,] image = new int[height, width]; // (temp) Instantiate a new 2D array based on height and width
                                               // (temp) A pretty straigth forward algorithm to convert the string to a 2D array.
        int index = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (index < pixelData.Length) // (temp) Failsafe to make sure you don't try to index a non-existent value.
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
}