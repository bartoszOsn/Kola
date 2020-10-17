using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Kola
{
    //TODO: implement popup to search wiktionary and wikipedia
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public InstanceManager InstanceManager { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = null;
            Model.Model model = new Model.Model();
            InstanceManager = new InstanceManager("Kola.ComicBookReader");

            InstanceManager.ReceiveArgs += (s, args) =>
            {
                model.Add(args.Args);
            };

            InstanceManager.Start(e.Args);
            if(InstanceManager.IsServer)
            {
                window = new MainWindow(model);
                window.Show();
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            InstanceManager.Close();
        }
    }
}
