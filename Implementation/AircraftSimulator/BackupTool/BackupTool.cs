using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Common.Containers;
using Patterns.Observator.Observing;
using ToolAdapter.Tool;

namespace BackupTool
{
    public sealed class BackupTool : Common.Initializer, ToolAdapter.Tool.ITool
    {
        private TabItem tabItem;
        public TabItem TabItem
        {
            get
            {
                return tabItem;
            }
        }

        private ToolType toolType;
        public ToolType ToolType
        {
            get
            {
                return toolType;
            }
        }

        private ConcreteObserver.ConcreteToolManagerObserver observer;
        public Observer Observer
        {
            get
            {
                return observer;
            }
        }

        private Content.ViewModel viewModel;
        public BackupTool(ToolType toolType)
        {
            this.toolType = toolType;
            this.tabItem = new TabItem();
            this.observer = new ConcreteObserver.ConcreteToolManagerObserver();

            this.viewModel = new Content.ViewModel(Observer);
            Initialize();
        }

        protected override void Initialize()
        {
            this.tabItem.Header = "Backup";

            this.tabItem.DataContext = this.viewModel;
            this.tabItem.Content = new Content.TabContent();
        }
    }
}
