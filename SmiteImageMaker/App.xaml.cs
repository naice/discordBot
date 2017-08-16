using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            var x = new DesignTimeLastMatch();

            if (e.Args.Length < 2)
            {
                Shutdown(0x10);
                return;
            }

            var inputDataPath = e.Args[0];
            var resultImagePath = e.Args[1];

            Loader loader = new Loader(inputDataPath);
            ControlRenderer renderer = new ControlRenderer(resultImagePath, loader.Control);
            renderer.Render();

            Shutdown(0x1);
        }
    }
}
