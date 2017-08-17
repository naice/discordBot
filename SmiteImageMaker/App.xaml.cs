using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmiteImageMaker
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {

            var cachePath = Path.Combine(Environment.CurrentDirectory, "cache");
            SmiteAPI.Model.CacheConfig.GodImagePath  = cachePath;
            SmiteAPI.Model.CacheConfig.ItemImagePath = cachePath;
            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

#if ONLY_TEST_WINDOW
            TestWindow wnd = new TestWindow();
            wnd.ShowDialog();
            return;
#else
            if (e.Args.Length < 2)
            {
                Shutdown(0x10);
                return;
            }

            var inputDataPath = e.Args[0];
            var resultImagePath = e.Args[1];

            Loader loader = new Loader(inputDataPath, cachePath);
            ControlRenderer renderer = new ControlRenderer(resultImagePath, loader.Control);
            await renderer.Render();

            Shutdown(0x1);
#endif
        }
    }
}
