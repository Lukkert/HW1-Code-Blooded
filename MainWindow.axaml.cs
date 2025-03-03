using System;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Media;

namespace HW1;

public partial class MainWindow : Window
{
    public MainWindow()
    {

        InitializeComponent();
        InitializeButtons();
        FileManager.Initialize(StorageProvider);
    }

    private void InitializeButtons()
    {
        LoadButton.Click += async (sender, e) => await FileManager.LoadB2img(this);
        SaveButton.Click += async (sender, e) => await FileManager.SaveB2img();
        SaveAsButton.Click += async (sender, e) => await FileManager.SaveAsB2img();
    }

    // Draw image on canvas, can also be used to redraw an image
        // This method takes care of editing the image as well, by the use of a pointer pressed event.
    public void DrawImage()
    {
        PixelCanvas.Children.Clear();

        if (B2Image.ImageData == null || B2Image.Width == 0 || B2Image.Height == 0)
            return;

        double pixelSize = Math.Min( Width * 0.8 / B2Image.Width, Height * 0.8 / B2Image.Height);

        PixelCanvas.Width = B2Image.Width * pixelSize;  // Set the size of the canvas so that it fits with the loaded image
        PixelCanvas.Height = B2Image.Height * pixelSize;

        for (int y = 0; y < B2Image.Height; y++)
        {
            for (int x = 0; x < B2Image.Width; x++)
            {
                var border = new Border // Border is used to create a square aka pixel
                {
                    Width = pixelSize,
                    Height = pixelSize,
                    Background = B2Image.ImageData[y, x] == 1 ? Brushes.Black : Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1)
                };

                Canvas.SetLeft(border, x * pixelSize); // Set the position of the pixel
                Canvas.SetTop(border, y * pixelSize); // top for y, left for x

                border.PointerPressed += (_, __) =>
                {
                    B2Image.ImageData[y, x] ^= 1; // Cool way to switch between 0 and 1
                    DrawImage();
                };

                PixelCanvas.Children.Add(border);
            }
        }
    }
}