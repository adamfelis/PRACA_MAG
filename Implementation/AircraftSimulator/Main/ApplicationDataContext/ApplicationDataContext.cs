using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Main.ApplicationDataContext
{
    class ApplicationDataContext
    {
        public MainApplication MainApplication
        {
            get
            {
                return MainApplication.Instance;
            }
        }

        public ObservableCollection<ServerCommunication.ServerCommunication.Client> ClientsCollection
        {
            get
            {
                return MainApplication.ServerCommunication.ClientsCollection;
            }
        }

        public Collection<global::Common.Scripts.ScriptType> AlgorithmsCollection
        {
            get
            {
                return new Collection<global::Common.Scripts.ScriptType>(Enum.GetValues(typeof(global::Common.Scripts.ScriptType)).
                    OfType<global::Common.Scripts.ScriptType>().ToList());
            }
        }


        public ObservableCollection<Shooters.ShooterType> OptionalStrategiesCollection
        {
            get
            {
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.OptionalStrategies;
            }
        }

        public ObservableCollection<Shooters.ShooterType> StrategiesCollection
        {
            get
            {
                //int selectedItem = int.Parse((listView.SelectedValue as ServerCommunication.ServerCommunication.Client).Name);
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.ActiveStrategies;
            }
        }

        public ObservableCollection<float[]> A_lateralMatrix
        {
            get
            {
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.A_lateralMatrix;
            }
        }
        public ObservableCollection<float[]> B_lateralMatrix
        {
            get
            {
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.B_lateralMatrix;
            }
        }

        public ObservableCollection<float[]> A_longitudinalMatrix
        {
            get
            {
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.A_longitudinalMatrix;
            }
        }
        public ObservableCollection<float[]> B_longitudinalMatrix
        {
            get
            {
                return MainApplication.AircraftsManagerCommunication.ManagerInstance.B_longitudinalMatrix;
            }
        }


        public ApplicationDataContext(Common.IDispatchable iDispatchable, Action<ToolAdapter.Tool.ITool> toolAddedAction)
        {
            MainApplication.InformAboutDispatcher(iDispatchable);
            MainApplication.SetActionForToolsAdding(toolAddedAction);
            strategiesImages = new ObservableCollection<Image>();
            optionalStrategiesImages = new ObservableCollection<Image>();
            StrategiesCollection.CollectionChanged += StrategiesCollection_CollectionChanged;
            OptionalStrategiesCollection.CollectionChanged += OptionalStrategiesCollection_CollectionChanged;
        }

        private void OptionalStrategiesCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                Shooters.ShooterType shooterType = (Shooters.ShooterType)e.NewItems[0];
                optionalStrategiesImages.Add(new Image() { Source = new BitmapImage(new Uri(@"../Resources/" + shooterType.ToString() + "_small.png", UriKind.Relative)), Tag = shooterType });
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                Shooters.ShooterType shooterType = (Shooters.ShooterType)e.OldItems[0];
                foreach (Image image in optionalStrategiesImages)
                {
                    if ((Shooters.ShooterType)image.Tag == shooterType)
                    {
                        optionalStrategiesImages.Remove(image);
                        return;
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                optionalStrategiesImages.Clear();
            }
        }

        private void StrategiesCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                Shooters.ShooterType shooterType = (Shooters.ShooterType)e.NewItems[0];
                strategiesImages.Add(new Image() { Source = new BitmapImage(new Uri(@"../Resources/" + shooterType.ToString() + "_small.png", UriKind.Relative)), Tag = shooterType });
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                Shooters.ShooterType shooterType = (Shooters.ShooterType)e.OldItems[0];
                foreach (Image image in strategiesImages)
                {
                    if((Shooters.ShooterType)image.Tag == shooterType)
                    {
                        strategiesImages.Remove(image);
                        return;
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                strategiesImages.Clear();
            }
        }

        private ObservableCollection<Image> strategiesImages;
        public ObservableCollection<Image> StrategiesImages
        {
            get
            {
                //foreach (Shooters.ShooterType shooterType in Enum.GetValues(typeof(Shooters.ShooterType)))
                //{
                //    Uri uri = new Uri(@"../Resources/" + shooterType.ToString() + "_small.png", UriKind.Relative);
                //    Main.CustomControls.ShooterStrategy shooterStrategy = new CustomControls.ShooterStrategy();
                //    shooterStrategy.StrategyImage.Source = new BitmapImage(uri);
                //    shooterStrategy.StrategyImage.Tag = shooterType.ToString();
                //    collection.Add(shooterStrategy);
                //}
                return strategiesImages;
            }
        }

        private ObservableCollection<Image> optionalStrategiesImages;
        public ObservableCollection<Image> OptionalStrategiesImages
        {
            get
            {
                return optionalStrategiesImages;
            }
        }
    }
}
