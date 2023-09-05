using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageEditing
{
	static class ImageEditor
	{

		public static unsafe Bitmap EditImage(Bitmap inputBitmap, ImageEffect effect)
		{
			Bitmap imageBitmap = inputBitmap;

			Rectangle FullImage = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);
			BitmapData bitmapData = imageBitmap.LockBits(FullImage, ImageLockMode.ReadWrite, imageBitmap.PixelFormat);

			int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
			int widthInBytes = bitmapData.Width * bytesPerPixel;

			byte* bmStart = (byte*)bitmapData.Scan0;

			switch (effect)
			{
				case ImageEffect.Greyscale:
					Parallel.For(0, imageBitmap.Height, y =>
					{
						byte* currentLine = bmStart + (y * bitmapData.Stride);

						Parallel.For(0, bitmapData.Width, x =>
						{
							int index = x * bytesPerPixel;

							int redValue = currentLine[index];
							int GreenValue = currentLine[index + 1];
							int BlueValue = currentLine[index + 2];

							byte newValue = (byte)((redValue + GreenValue + BlueValue) / 3);

							currentLine[index] = newValue;
							currentLine[index + 1] = newValue;
							currentLine[index + 2] = newValue;
						});
					});
					break;
				case ImageEffect.BlackNWhite:
					byte Threshold = 0;
					Parallel.For(0, imageBitmap.Height, y =>
					{
						byte* currentLine = bmStart + (y * bitmapData.Stride);

						Parallel.For(0, bitmapData.Width, x =>
						{
							int index = x * bytesPerPixel;

							int redValue = currentLine[index];
							int GreenValue = currentLine[index + 1];
							int BlueValue = currentLine[index + 2];

							byte newValue = (byte)((redValue + GreenValue + BlueValue) / 3);
							Threshold += newValue;
						});
					});

					Parallel.For(0, imageBitmap.Height, y =>
					{
						byte* currentLine = bmStart + (y * bitmapData.Stride);

						Parallel.For(0, bitmapData.Width, x =>
						{
							int index = x * bytesPerPixel;

							int redValue = currentLine[index];
							int GreenValue = currentLine[index + 1];
							int BlueValue = currentLine[index + 2];

							byte newValue = (byte)((redValue + GreenValue + BlueValue) / 3);

							currentLine[index] = newValue < Threshold ? (byte)255 : (byte)0;
							currentLine[index + 1] = newValue < Threshold ? (byte)255 : (byte)0;
							currentLine[index + 2] = newValue < Threshold ? (byte)255 : (byte)0;
						});
					});
					break;
			}

			imageBitmap.UnlockBits(bitmapData);
			return imageBitmap;
		}
	}

	enum ImageEffect
	{
		Greyscale,
		BlackNWhite,
		Darken
	}
}
