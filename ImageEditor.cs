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
		public static unsafe Bitmap ApplyGreyScale(Bitmap inputBitmap)
		{
			Bitmap imageBitmap = inputBitmap;

			Rectangle FullImage = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);
			BitmapData bitmapData = imageBitmap.LockBits(FullImage, ImageLockMode.ReadWrite, imageBitmap.PixelFormat); ;
			try
			{
				int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
				int widthInBytes = bitmapData.Width * bytesPerPixel;

				byte* bmStart = (byte*)bitmapData.Scan0;

				Parallel.For(0, imageBitmap.Height, y =>
				{
					byte* currentLine = bmStart + (y * bitmapData.Stride);

					for (int x = 0; x < bitmapData.Width; x++)
					{
						int index = x * bytesPerPixel;

						int redValue = currentLine[index];
						int greenValue = currentLine[index + 1];
						int blueValue = currentLine[index + 2];

						byte newValue = (byte)((redValue + greenValue + blueValue) / 3);

						currentLine[index] = newValue;
						currentLine[index + 1] = newValue;
						currentLine[index + 2] = newValue;
					}
				});
			}
			finally
			{
				imageBitmap.UnlockBits(bitmapData);
			}

			return imageBitmap;
		}

		public static unsafe Bitmap ApplyBlackNWhite(Bitmap inputBitmap)
		{
			Bitmap imageBitmap = inputBitmap;

			Rectangle FullImage = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);
			BitmapData bitmapData = imageBitmap.LockBits(FullImage, ImageLockMode.ReadWrite, imageBitmap.PixelFormat);

			try
			{
				int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
				int widthInBytes = bitmapData.Width * bytesPerPixel;

				byte* bmStart = (byte*)bitmapData.Scan0;

				byte Threshold = 0;
				Parallel.For(0, imageBitmap.Height, y =>
				{
					byte* currentLine = bmStart + (y * bitmapData.Stride);

					for (int x = 0; x < bitmapData.Width; x++)
					{
						int index = x * bytesPerPixel;

						int redValue = currentLine[index];
						int greenValue = currentLine[index + 1];
						int blueValue = currentLine[index + 2];

						byte newValue = (byte)((redValue + greenValue + blueValue) / 3);
						Threshold += newValue;
					}
				});

				Parallel.For(0, imageBitmap.Height, y =>
				{
					byte* currentLine = bmStart + (y * bitmapData.Stride);

					for (int x = 0; x < bitmapData.Width; x++)
					{
						int index = x * bytesPerPixel;

						int redValue = currentLine[index];
						int GreenValue = currentLine[index + 1];
						int BlueValue = currentLine[index + 2];

						byte newValue = (byte)((redValue + GreenValue + BlueValue) / 3);

						currentLine[index] = newValue > Threshold ? (byte)255 : (byte)0;
						currentLine[index + 1] = newValue > Threshold ? (byte)255 : (byte)0;
						currentLine[index + 2] = newValue > Threshold ? (byte)255 : (byte)0;
					}
				});
			}
			finally
			{
				imageBitmap.UnlockBits(bitmapData);
			}

			return imageBitmap;
		}

		public static unsafe Bitmap ApplyBlackNWhite(Bitmap inputBitmap, int Threshold)
		{
			Bitmap imageBitmap = inputBitmap;

			Rectangle FullImage = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);
			BitmapData bitmapData = imageBitmap.LockBits(FullImage, ImageLockMode.ReadWrite, imageBitmap.PixelFormat);

			try
			{
				int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
				int widthInBytes = bitmapData.Width * bytesPerPixel;

				byte* bmStart = (byte*)bitmapData.Scan0;

				Parallel.For(0, imageBitmap.Height, y =>
				{
					byte* currentLine = bmStart + (y * bitmapData.Stride);

					for (int x = 0; x < bitmapData.Width; x++)
					{
						int index = x * bytesPerPixel;

						int redValue = currentLine[index];
						int greenValue = currentLine[index + 1];
						int blueValue = currentLine[index + 2];

						byte newValue = (byte)((redValue + greenValue + blueValue) / 3);

						currentLine[index] = newValue > Threshold ? (byte)255 : (byte)0;
						currentLine[index + 1] = newValue > Threshold ? (byte)255 : (byte)0;
						currentLine[index + 2] = newValue > Threshold ? (byte)255 : (byte)0;
					}
				});
			}
			finally
			{
				imageBitmap.UnlockBits(bitmapData);
			}

			return imageBitmap;
		}

		public static unsafe Bitmap ApplyInvertedColors(Bitmap inputBitmap)
		{
			Bitmap imageBitmap = inputBitmap;

			Rectangle FullImage = new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height);
			BitmapData bitmapData = imageBitmap.LockBits(FullImage, ImageLockMode.ReadWrite, imageBitmap.PixelFormat); ;
			try
			{
				int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
				int widthInBytes = bitmapData.Width * bytesPerPixel;

				byte* bmStart = (byte*)bitmapData.Scan0;

				Parallel.For(0, imageBitmap.Height, y =>
				{
					byte* currentLine = bmStart + (y * bitmapData.Stride);

					for (int x = 0; x < bitmapData.Width; x++)
					{
						int index = x * bytesPerPixel;

						int redValue = currentLine[index];
						int greenValue = currentLine[index + 1];
						int blueValue = currentLine[index + 2];

						redValue = 255 - redValue;
						greenValue = 255 - greenValue;
						blueValue = 255 - blueValue;

						currentLine[index] = (byte)redValue;
						currentLine[index + 1] = (byte)greenValue;
						currentLine[index + 2] = (byte)blueValue;
					}
				});
			}
			finally
			{
				imageBitmap.UnlockBits(bitmapData);
			}

			return imageBitmap;
		}
	}
}
