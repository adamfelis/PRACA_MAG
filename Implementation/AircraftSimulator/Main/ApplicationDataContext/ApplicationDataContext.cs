﻿using Common.Containers;
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
            strategiesParameters = new ObservableCollection<float>();
            strategiesMinimumParameters = new ObservableCollection<float>();
            strategiesMaximumParameters = new ObservableCollection<float>();
            for (int i = 0; i < 43; i++)//43 - number of aircraft parameters
            {
                this.strategiesParameters.Add(0.0f);
                this.strategiesMinimumParameters.Add(-1.0f);
                this.strategiesMaximumParameters.Add(1.0f);
            }
            //strategiesParameters.CollectionChanged += StrategiesParameters_CollectionChanged;
            StrategiesCollection.CollectionChanged += StrategiesCollection_CollectionChanged;
            OptionalStrategiesCollection.CollectionChanged += OptionalStrategiesCollection_CollectionChanged;
        }

        public void StrategiesParameters_CollectionChanged()// object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
              //  return;
            this.MainApplication.AircraftsManagerCommunication.ManagerInstance.RefreshAircraftParameters(
                new AircraftsManager.Aircraft.Strategy.AircraftParameters()
                {
                    S = StrategiesParameters[0],
                    p = StrategiesParameters[1],
                    m = StrategiesParameters[2],
                    g = StrategiesParameters[3],

                    Y_v = StrategiesParameters[4],
                    L_v = StrategiesParameters[5],
                    N_v = StrategiesParameters[6],
                    Y_p = StrategiesParameters[7],
                    L_p = StrategiesParameters[8],
                    N_p = StrategiesParameters[9],
                    Y_r = StrategiesParameters[10],
                    L_r = StrategiesParameters[11],
                    N_r = StrategiesParameters[12],
                    Y_xi = StrategiesParameters[13],
                    L_xi = StrategiesParameters[14],
                    N_xi = StrategiesParameters[15],
                    Y_zeta = StrategiesParameters[16],
                    L_zeta = StrategiesParameters[17],
                    N_zeta = StrategiesParameters[18],
                    b = StrategiesParameters[19],
                    I_x = StrategiesParameters[20],
                    I_z = StrategiesParameters[21],
                    I_xz = StrategiesParameters[22],


                    c = StrategiesParameters[23],
                    I_y = StrategiesParameters[24],
                    X_dot_u = StrategiesParameters[25],
                    Z_dot_u = StrategiesParameters[26],
                    M_dot_u = StrategiesParameters[27],
                    X_dot_w = StrategiesParameters[28],
                    Z_dot_w = StrategiesParameters[29],
                    M_dot_w = StrategiesParameters[30],
                    X_dot_w_dot = StrategiesParameters[31],
                    Z_dot_w_dot = StrategiesParameters[32],
                    M_dot_w_dot = StrategiesParameters[33],
                    X_dot_q = StrategiesParameters[34],
                    Z_dot_q = StrategiesParameters[35],
                    M_dot_q = StrategiesParameters[36],
                    X_dot_ni = StrategiesParameters[37],
                    Z_dot_ni = StrategiesParameters[38],
                    M_dot_ni = StrategiesParameters[39],
                    X_dot_tau = StrategiesParameters[40],
                    Z_dot_tau = StrategiesParameters[41],
                    M_dot_tau = StrategiesParameters[42],
                }
                );
            }

        private void OptionalStrategiesCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                Shooters.ShooterType shooterType = (Shooters.ShooterType)e.NewItems[0];
                optionalStrategiesImages.Add(new Image() { Margin = new System.Windows.Thickness(0), Source = new BitmapImage(new Uri(@"../Resources/" + shooterType.ToString() + "_small.png", UriKind.Relative)), Tag = shooterType });
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
                strategiesImages.Add(new Image() { Margin = new System.Windows.Thickness(0), Source = new BitmapImage(new Uri(@"../Resources/" + shooterType.ToString() + "_small.png", UriKind.Relative)), Tag = shooterType });
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

        private ObservableCollection<float> strategiesParameters;
        public ObservableCollection<float> StrategiesParameters
        {
            get
            {
                return strategiesParameters;
            }
        }

        private ObservableCollection<float> strategiesMinimumParameters;
        public ObservableCollection<float> StrategiesMinimumParameters
        {
            get
            {
                return strategiesMinimumParameters;
            }
        }

        private ObservableCollection<float> strategiesMaximumParameters;
        public ObservableCollection<float> StrategiesMaximumParameters
        {
            get
            {
                return strategiesMaximumParameters;
            }
        }

        public void UpdateStrategiesParameters(AircraftsManager.Aircraft.Strategy.IAircraftParameters aircraftParameters)
        {
            //this.strategiesParameters.Clear();

            //this.strategiesParameters.Add(aircraftParameters.S);
            //this.strategiesParameters.Add(aircraftParameters.p);
            //this.strategiesParameters.Add(aircraftParameters.m);
            //this.strategiesParameters.Add(aircraftParameters.g);


            //this.strategiesParameters.Add(aircraftParameters.Y_v);
            //this.strategiesParameters.Add(aircraftParameters.L_v);
            //this.strategiesParameters.Add(aircraftParameters.N_v);
            //this.strategiesParameters.Add(aircraftParameters.Y_p);
            //this.strategiesParameters.Add(aircraftParameters.L_p);
            //this.strategiesParameters.Add(aircraftParameters.N_p);
            //this.strategiesParameters.Add(aircraftParameters.Y_r);
            //this.strategiesParameters.Add(aircraftParameters.L_r);
            //this.strategiesParameters.Add(aircraftParameters.N_r);
            //this.strategiesParameters.Add(aircraftParameters.Y_xi);
            //this.strategiesParameters.Add(aircraftParameters.L_xi);
            //this.strategiesParameters.Add(aircraftParameters.N_xi);
            //this.strategiesParameters.Add(aircraftParameters.Y_zeta);
            //this.strategiesParameters.Add(aircraftParameters.L_zeta);
            //this.strategiesParameters.Add(aircraftParameters.N_zeta);
            //this.strategiesParameters.Add(aircraftParameters.b);
            //this.strategiesParameters.Add(aircraftParameters.I_x);
            //this.strategiesParameters.Add(aircraftParameters.I_z);
            //this.strategiesParameters.Add(aircraftParameters.I_xz);



            //this.strategiesParameters.Add(aircraftParameters.c);
            //this.strategiesParameters.Add(aircraftParameters.I_y);
            //this.strategiesParameters.Add(aircraftParameters.X_dot_u);
            //this.strategiesParameters.Add(aircraftParameters.Z_dot_u);
            //this.strategiesParameters.Add(aircraftParameters.M_dot_u);
            //this.strategiesParameters.Add(aircraftParameters.X_dot_w);
            //this.strategiesParameters.Add(aircraftParameters.Z_dot_w);
            //this.strategiesParameters.Add(aircraftParameters.M_dot_w);
            //this.strategiesParameters.Add(aircraftParameters.X_dot_w_dot);
            //this.strategiesParameters.Add(aircraftParameters.Z_dot_w_dot);
            //this.strategiesParameters.Add(aircraftParameters.M_dot_w_dot);
            //this.strategiesParameters.Add(aircraftParameters.X_dot_q);
            //this.strategiesParameters.Add(aircraftParameters.Z_dot_q);
            //this.strategiesParameters.Add(aircraftParameters.M_dot_q);
            //this.strategiesParameters.Add(aircraftParameters.X_dot_ni);
            //this.strategiesParameters.Add(aircraftParameters.Z_dot_ni);
            //this.strategiesParameters.Add(aircraftParameters.M_dot_ni);
            //this.strategiesParameters.Add(aircraftParameters.X_dot_tau);
            //this.strategiesParameters.Add(aircraftParameters.Z_dot_tau);
            //this.strategiesParameters.Add(aircraftParameters.M_dot_tau);

            //this.strategiesParameters.Clear();

            this.strategiesParameters[0] = aircraftParameters.S;
            this.strategiesParameters[1] = aircraftParameters.p;
            this.strategiesParameters[2] = aircraftParameters.m;
            this.strategiesParameters[3] = aircraftParameters.g;


            this.strategiesParameters[4] = aircraftParameters.Y_v;
            this.strategiesParameters[5] = aircraftParameters.L_v;
            this.strategiesParameters[6] = aircraftParameters.N_v;
            this.strategiesParameters[7] = aircraftParameters.Y_p;
            this.strategiesParameters[8] = aircraftParameters.L_p;
            this.strategiesParameters[9] = aircraftParameters.N_p;
            this.strategiesParameters[10] = aircraftParameters.Y_r;
            this.strategiesParameters[11] = aircraftParameters.L_r;
            this.strategiesParameters[12] = aircraftParameters.N_r;
            this.strategiesParameters[13] = aircraftParameters.Y_xi;
            this.strategiesParameters[14] = aircraftParameters.L_xi;
            this.strategiesParameters[15] = aircraftParameters.N_xi;
            this.strategiesParameters[16] = aircraftParameters.Y_zeta;
            this.strategiesParameters[17] = aircraftParameters.L_zeta;
            this.strategiesParameters[18] = aircraftParameters.N_zeta;
            this.strategiesParameters[19] = aircraftParameters.b;
            this.strategiesParameters[20] = aircraftParameters.I_x;
            this.strategiesParameters[21] = aircraftParameters.I_z;
            this.strategiesParameters[22] = aircraftParameters.I_xz;



            this.strategiesParameters[23] = aircraftParameters.c;
            this.strategiesParameters[24] = aircraftParameters.I_y;
            this.strategiesParameters[25] = aircraftParameters.X_dot_u;
            this.strategiesParameters[26] = aircraftParameters.Z_dot_u;
            this.strategiesParameters[27] = aircraftParameters.M_dot_u;
            this.strategiesParameters[28] = aircraftParameters.X_dot_w;
            this.strategiesParameters[29] = aircraftParameters.Z_dot_w;
            this.strategiesParameters[30] = aircraftParameters.M_dot_w;
            this.strategiesParameters[31] = aircraftParameters.X_dot_w_dot;
            this.strategiesParameters[32] = aircraftParameters.Z_dot_w_dot;
            this.strategiesParameters[33] = aircraftParameters.M_dot_w_dot;
            this.strategiesParameters[34] = aircraftParameters.X_dot_q;
            this.strategiesParameters[35] = aircraftParameters.Z_dot_q;
            this.strategiesParameters[36] = aircraftParameters.M_dot_q;
            this.strategiesParameters[37] = aircraftParameters.X_dot_ni;
            this.strategiesParameters[38] = aircraftParameters.Z_dot_ni;
            this.strategiesParameters[39] = aircraftParameters.M_dot_ni;
            this.strategiesParameters[40] = aircraftParameters.X_dot_tau;
            this.strategiesParameters[41] = aircraftParameters.Z_dot_tau;
            this.strategiesParameters[42] = aircraftParameters.M_dot_tau;

            //this.strategiesMinimumParameters.Clear();

            this.strategiesMinimumParameters[0] = aircraftParameters.S - Math.Abs(aircraftParameters.S)/2.0f;
            this.strategiesMinimumParameters[1] = aircraftParameters.p - Math.Abs(aircraftParameters.p) / 2.0f;
            this.strategiesMinimumParameters[2] = aircraftParameters.m - Math.Abs(aircraftParameters.m) / 2.0f;
            this.strategiesMinimumParameters[3] = aircraftParameters.g - Math.Abs(aircraftParameters.g) / 2.0f;


            this.strategiesMinimumParameters[4] = aircraftParameters.Y_v - Math.Abs(aircraftParameters.Y_v) / 2.0f;
            this.strategiesMinimumParameters[5] = aircraftParameters.L_v - Math.Abs(aircraftParameters.L_v) / 2.0f;
            this.strategiesMinimumParameters[6] = aircraftParameters.N_v - Math.Abs(aircraftParameters.N_v) / 2.0f;
            this.strategiesMinimumParameters[7] = aircraftParameters.Y_p - Math.Abs(aircraftParameters.Y_p) / 2.0f;
            this.strategiesMinimumParameters[8] = aircraftParameters.L_p - Math.Abs(aircraftParameters.L_p) / 2.0f;
            this.strategiesMinimumParameters[9] = aircraftParameters.N_p - Math.Abs(aircraftParameters.N_p) / 2.0f;
            this.strategiesMinimumParameters[10] = aircraftParameters.Y_r - Math.Abs(aircraftParameters.Y_r) / 2.0f;
            this.strategiesMinimumParameters[11] = aircraftParameters.L_r - Math.Abs(aircraftParameters.L_r) / 2.0f;
            this.strategiesMinimumParameters[12] = aircraftParameters.N_r - Math.Abs(aircraftParameters.N_r) / 2.0f;
            this.strategiesMinimumParameters[13] = aircraftParameters.Y_xi - Math.Abs(aircraftParameters.Y_xi) / 2.0f;
            this.strategiesMinimumParameters[14] = aircraftParameters.L_xi - Math.Abs(aircraftParameters.L_xi) / 2.0f;
            this.strategiesMinimumParameters[15] = aircraftParameters.N_xi - Math.Abs(aircraftParameters.N_xi) / 2.0f;
            this.strategiesMinimumParameters[16] = aircraftParameters.Y_zeta - Math.Abs(aircraftParameters.Y_zeta) / 2.0f;
            this.strategiesMinimumParameters[17] = aircraftParameters.L_zeta - Math.Abs(aircraftParameters.L_zeta) / 2.0f;
            this.strategiesMinimumParameters[18] = aircraftParameters.N_zeta - Math.Abs(aircraftParameters.N_zeta) / 2.0f;
            this.strategiesMinimumParameters[19] = aircraftParameters.b - Math.Abs(aircraftParameters.b) / 2.0f;
            this.strategiesMinimumParameters[20] = aircraftParameters.I_x - Math.Abs(aircraftParameters.I_x) / 2.0f;
            this.strategiesMinimumParameters[21] = aircraftParameters.I_z - Math.Abs(aircraftParameters.I_z) / 2.0f;
            this.strategiesMinimumParameters[22] = aircraftParameters.I_xz - Math.Abs(aircraftParameters.I_xz) / 2.0f;



            this.strategiesMinimumParameters[23] = aircraftParameters.c - Math.Abs(aircraftParameters.c) / 2.0f;
            this.strategiesMinimumParameters[24] = aircraftParameters.I_y - Math.Abs(aircraftParameters.I_y) / 2.0f;
            this.strategiesMinimumParameters[25] = aircraftParameters.X_dot_u - Math.Abs(aircraftParameters.X_dot_u) / 2.0f;
            this.strategiesMinimumParameters[26] = aircraftParameters.Z_dot_u - Math.Abs(aircraftParameters.Z_dot_u) / 2.0f;
            this.strategiesMinimumParameters[27] = aircraftParameters.M_dot_u - Math.Abs(aircraftParameters.M_dot_u) / 2.0f;
            this.strategiesMinimumParameters[28] = aircraftParameters.X_dot_w - Math.Abs(aircraftParameters.X_dot_w) / 2.0f;
            this.strategiesMinimumParameters[29] = aircraftParameters.Z_dot_w - Math.Abs(aircraftParameters.Z_dot_w) / 2.0f;
            this.strategiesMinimumParameters[30] = aircraftParameters.M_dot_w - Math.Abs(aircraftParameters.M_dot_w) / 2.0f;
            this.strategiesMinimumParameters[31] = aircraftParameters.X_dot_w_dot - Math.Abs(aircraftParameters.X_dot_w_dot) / 2.0f;
            this.strategiesMinimumParameters[32] = aircraftParameters.Z_dot_w_dot - Math.Abs(aircraftParameters.Z_dot_w_dot) / 2.0f;
            this.strategiesMinimumParameters[33] = aircraftParameters.M_dot_w_dot - Math.Abs(aircraftParameters.M_dot_w_dot) / 2.0f;
            this.strategiesMinimumParameters[34] = aircraftParameters.X_dot_q - Math.Abs(aircraftParameters.X_dot_q) / 2.0f;
            this.strategiesMinimumParameters[35] = aircraftParameters.Z_dot_q - Math.Abs(aircraftParameters.Z_dot_q) / 2.0f;
            this.strategiesMinimumParameters[36] = aircraftParameters.M_dot_q - Math.Abs(aircraftParameters.M_dot_q) / 2.0f;
            this.strategiesMinimumParameters[37] = aircraftParameters.X_dot_ni - Math.Abs(aircraftParameters.X_dot_ni) / 2.0f;
            this.strategiesMinimumParameters[38] = aircraftParameters.Z_dot_ni - Math.Abs(aircraftParameters.Z_dot_ni) / 2.0f;
            this.strategiesMinimumParameters[39] = aircraftParameters.M_dot_ni - Math.Abs(aircraftParameters.M_dot_ni) / 2.0f;
            this.strategiesMinimumParameters[40] = aircraftParameters.X_dot_tau - Math.Abs(aircraftParameters.X_dot_tau) / 2.0f;
            this.strategiesMinimumParameters[41] = aircraftParameters.Z_dot_tau - Math.Abs(aircraftParameters.Z_dot_tau) / 2.0f;
            this.strategiesMinimumParameters[42] = aircraftParameters.M_dot_tau - Math.Abs(aircraftParameters.M_dot_tau) / 2.0f;

            for(int i = 0; i < this.strategiesMinimumParameters.Count; i++)
                if (this.strategiesMinimumParameters[i] == 0)
                    this.strategiesMinimumParameters[i] = -1;
            
            //this.strategiesMaximumParameters.Clear();

            this.strategiesMaximumParameters[0] = aircraftParameters.S + Math.Abs(aircraftParameters.S) / 2.0f;
            this.strategiesMaximumParameters[1] = aircraftParameters.p + Math.Abs(aircraftParameters.p) / 2.0f;
            this.strategiesMaximumParameters[2] = aircraftParameters.m + Math.Abs(aircraftParameters.m) / 2.0f;
            this.strategiesMaximumParameters[3] = aircraftParameters.g + Math.Abs(aircraftParameters.g) / 2.0f;


            this.strategiesMaximumParameters[4] = aircraftParameters.Y_v + Math.Abs(aircraftParameters.Y_v) / 2.0f;
            this.strategiesMaximumParameters[5] = aircraftParameters.L_v + Math.Abs(aircraftParameters.L_v) / 2.0f;
            this.strategiesMaximumParameters[6] = aircraftParameters.N_v + Math.Abs(aircraftParameters.N_v) / 2.0f;
            this.strategiesMaximumParameters[7] = aircraftParameters.Y_p + Math.Abs(aircraftParameters.Y_p) / 2.0f;
            this.strategiesMaximumParameters[8] = aircraftParameters.L_p + Math.Abs(aircraftParameters.L_p) / 2.0f;
            this.strategiesMaximumParameters[9] = aircraftParameters.N_p + Math.Abs(aircraftParameters.N_p) / 2.0f;
            this.strategiesMaximumParameters[10] = aircraftParameters.Y_r + Math.Abs(aircraftParameters.Y_r) / 2.0f;
            this.strategiesMaximumParameters[11] = aircraftParameters.L_r + Math.Abs(aircraftParameters.L_r) / 2.0f;
            this.strategiesMaximumParameters[12] = aircraftParameters.N_r + Math.Abs(aircraftParameters.N_r) / 2.0f;
            this.strategiesMaximumParameters[13] = aircraftParameters.Y_xi + Math.Abs(aircraftParameters.Y_xi) / 2.0f;
            this.strategiesMaximumParameters[14] = aircraftParameters.L_xi + Math.Abs(aircraftParameters.L_xi) / 2.0f;
            this.strategiesMaximumParameters[15] = aircraftParameters.N_xi + Math.Abs(aircraftParameters.N_xi) / 2.0f;
            this.strategiesMaximumParameters[16] = aircraftParameters.Y_zeta + Math.Abs(aircraftParameters.Y_zeta) / 2.0f;
            this.strategiesMaximumParameters[17] = aircraftParameters.L_zeta + Math.Abs(aircraftParameters.L_zeta) / 2.0f;
            this.strategiesMaximumParameters[18] = aircraftParameters.N_zeta + Math.Abs(aircraftParameters.N_zeta) / 2.0f;
            this.strategiesMaximumParameters[19] = aircraftParameters.b + Math.Abs(aircraftParameters.b) / 2.0f;
            this.strategiesMaximumParameters[20] = aircraftParameters.I_x + Math.Abs(aircraftParameters.I_x) / 2.0f;
            this.strategiesMaximumParameters[21] = aircraftParameters.I_z + Math.Abs(aircraftParameters.I_z) / 2.0f;
            this.strategiesMaximumParameters[22] = aircraftParameters.I_xz + Math.Abs(aircraftParameters.I_xz) / 2.0f;



            this.strategiesMaximumParameters[23] = aircraftParameters.c + Math.Abs(aircraftParameters.c) / 2.0f;
            this.strategiesMaximumParameters[24] = aircraftParameters.I_y + Math.Abs(aircraftParameters.I_y) / 2.0f;
            this.strategiesMaximumParameters[25] = aircraftParameters.X_dot_u + Math.Abs(aircraftParameters.X_dot_u) / 2.0f;
            this.strategiesMaximumParameters[26] = aircraftParameters.Z_dot_u + Math.Abs(aircraftParameters.Z_dot_u) / 2.0f;
            this.strategiesMaximumParameters[27] = aircraftParameters.M_dot_u + Math.Abs(aircraftParameters.M_dot_u) / 2.0f;
            this.strategiesMaximumParameters[28] = aircraftParameters.X_dot_w + Math.Abs(aircraftParameters.X_dot_w) / 2.0f;
            this.strategiesMaximumParameters[29] = aircraftParameters.Z_dot_w + Math.Abs(aircraftParameters.Z_dot_w) / 2.0f;
            this.strategiesMaximumParameters[30] = aircraftParameters.M_dot_w + Math.Abs(aircraftParameters.M_dot_w) / 2.0f;
            this.strategiesMaximumParameters[31] = aircraftParameters.X_dot_w_dot + Math.Abs(aircraftParameters.X_dot_w_dot) / 2.0f;
            this.strategiesMaximumParameters[32] = aircraftParameters.Z_dot_w_dot + Math.Abs(aircraftParameters.Z_dot_w_dot) / 2.0f;
            this.strategiesMaximumParameters[33] = aircraftParameters.M_dot_w_dot + Math.Abs(aircraftParameters.M_dot_w_dot) / 2.0f;
            this.strategiesMaximumParameters[34] = aircraftParameters.X_dot_q + Math.Abs(aircraftParameters.X_dot_q) / 2.0f;
            this.strategiesMaximumParameters[35] = aircraftParameters.Z_dot_q + Math.Abs(aircraftParameters.Z_dot_q) / 2.0f;
            this.strategiesMaximumParameters[36] = aircraftParameters.M_dot_q + Math.Abs(aircraftParameters.M_dot_q) / 2.0f;
            this.strategiesMaximumParameters[37] = aircraftParameters.X_dot_ni + Math.Abs(aircraftParameters.X_dot_ni) / 2.0f;
            this.strategiesMaximumParameters[38] = aircraftParameters.Z_dot_ni + Math.Abs(aircraftParameters.Z_dot_ni) / 2.0f;
            this.strategiesMaximumParameters[39] = aircraftParameters.M_dot_ni + Math.Abs(aircraftParameters.M_dot_ni) / 2.0f;
            this.strategiesMaximumParameters[40] = aircraftParameters.X_dot_tau + Math.Abs(aircraftParameters.X_dot_tau) / 2.0f;
            this.strategiesMaximumParameters[41] = aircraftParameters.Z_dot_tau + Math.Abs(aircraftParameters.Z_dot_tau) / 2.0f;
            this.strategiesMaximumParameters[42] = aircraftParameters.M_dot_tau + Math.Abs(aircraftParameters.M_dot_tau) / 2.0f;


            for (int i = 0; i < this.strategiesMaximumParameters.Count; i++)
                if (this.strategiesMaximumParameters[i] == 0)
                    this.strategiesMaximumParameters[i] = 1;

        }
    }
}
