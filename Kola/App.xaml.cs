using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kola
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public InstanceManager InstanceManager { get; set; }
        public MainWindow Window { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //TEST
            //var pages = Task.Run(()=> Helpers.Wiki.Wiki.Search("javascript")).Result;
            //var content = pages.Select(t => Task.Run(() => t.GetContent()).Result).ToList();
            //TEST

            Window = null;
            Model.Model model = new Model.Model();
            InstanceManager = new InstanceManager("Kola.ComicBookReader");

            InstanceManager.ReceiveArgs += (s, args) =>
            {
                model.Add(args.Args);
            };

            InstanceManager.Start(e.Args);
            if(InstanceManager.IsServer)
            {
                Window = new MainWindow(model);
                Window.Show();
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            InstanceManager.Close();
        }
    }
}
