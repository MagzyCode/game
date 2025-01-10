using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Windows;
using SharpDX.Direct2D1;
using SharpDX.WIC;

using Direct2D1 = SharpDX.Direct2D1;
using WIC = SharpDX.WIC;
using RenderTargetFactory = SharpDX.Direct2D1.Factory;


namespace EngineLibrary.EngineComponents
{
    /// <summary>
    /// Класс создания системы отрисовки
    /// </summary>
    public class RenderingSystem : IDisposable
    {
        private RenderTargetFactory renderTargetFactory;

        /// <summary>
        /// Окно отрисовки
        /// </summary>
        public WindowRenderTarget RenderTarget { get => renderTarget; }
        private WindowRenderTarget renderTarget;

        private static ImagingFactory imageFactory;
        private static WindowRenderTarget staticRenderTarget;

        /// <summary>
        /// Коснтруктор, создаюший систему отрисовки в форме
        /// </summary>
        /// <param name="form">Форма для отрисовки</param>
        public RenderingSystem(RenderForm form)
        {
            renderTargetFactory = new RenderTargetFactory();
            imageFactory = new ImagingFactory();

            RenderTargetProperties renderProperties = new RenderTargetProperties()
            {
                DpiX = 0,
                DpiY = 0,
                MinLevel = FeatureLevel.Level_10,
                PixelFormat = new Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };

            HwndRenderTargetProperties winProperties = new HwndRenderTargetProperties()
            {
                Hwnd = form.Handle,
                PixelSize = new Size2(form.ClientSize.Width, form.ClientSize.Height),
                PresentOptions = PresentOptions.None
            };

            renderTarget = new WindowRenderTarget(renderTargetFactory, renderProperties, winProperties);
            staticRenderTarget = renderTarget;
        }

        /// <summary>
        /// Загрузка изображения из файла
        /// </summary>
        /// <param name="imageFileName">Путь к файлу</param>
        /// <returns>Изображение</returns>
        public static Direct2D1.Bitmap LoadBitmap(string imageFileName)
        {
            BitmapDecoder decoder = new BitmapDecoder(imageFactory, imageFileName, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode frame = decoder.GetFrame(0);
            FormatConverter converter = new FormatConverter(imageFactory);
            converter.Initialize(frame, WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);

            Direct2D1.Bitmap bitmap = Direct2D1.Bitmap.FromWicBitmap(staticRenderTarget, converter);

            Utilities.Dispose(ref converter);
            Utilities.Dispose(ref frame);
            Utilities.Dispose(ref decoder);

            return bitmap;
        }

        /// <summary>
        /// Загрузка последовательности изображений для анимации
        /// </summary>
        /// <param name="pathToFiles">Путь к файлам</param>
        /// <param name="countOfBitmaps">Количество файлов для загрузки</param>
        /// <returns>Список изображений</returns>
        public static List<Direct2D1.Bitmap> LoadAnimation(string pathToFiles, int countOfBitmaps)
        {
            List<Direct2D1.Bitmap> bitmaps = new List<Direct2D1.Bitmap>();

            for(int i = 1; i <= countOfBitmaps; i++)
            {
                string fileName = pathToFiles + i + ".png";
                bitmaps.Add(LoadBitmap(fileName));
            }

            return bitmaps;
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            Utilities.Dispose(ref renderTarget);
            Utilities.Dispose(ref imageFactory);
            Utilities.Dispose(ref renderTargetFactory);
        }
    }
}
