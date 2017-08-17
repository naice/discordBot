using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmiteImageMaker
{
    internal class ControlRenderer
    {
        private readonly string _resultPath;
        private readonly Control _control;

        public ControlRenderer(string resultPath, Control control)
        {
            _resultPath = resultPath;
            _control = control;
        }

        public async Task Render()
        {
            var size = new Size(_control.Width, _control.Height);
            _control.Measure(size);
            _control.Arrange(new Rect(size));
            _control.UpdateLayout();

            //await _control.Wait();
            //await Task.Delay(1000);            

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            
            bmp.Render(_control);
            
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (Stream stm = File.Create(_resultPath, 1024))
                encoder.Save(stm);
        }
    }
}
