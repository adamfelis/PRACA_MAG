using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Common.Containers;
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
        public IObserver<IData> Observer
        {
            get
            {
                return observer;
            }
        }

        public BackupTool(ToolType toolType)
        {
            this.toolType = toolType;
            this.tabItem = new TabItem();
            this.observer = new ConcreteObserver.ConcreteToolManagerObserver();
            Initialize();
        }

        protected override void Initialize()
        {
            this.tabItem.Header = "Backup";
        }
    }
}
