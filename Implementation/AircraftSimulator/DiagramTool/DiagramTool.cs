using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ToolAdapter.Tool;
using System.Windows.Media;
using Common.Containers;

namespace DiagramTool
{
    public sealed class DiagramTool : Common.Initializer, ToolAdapter.Tool.ITool
    {
        private ToolType toolType;
        public ToolType ToolType
        {
            get
            {
                return toolType;
            }
        }
        private TabItem tabItem;
        public TabItem TabItem
        {
            get
            {
                return tabItem;
            }
        }

        private ConcreteObserver.ConcreteToolManagerObserver observer;
        public Patterns.Observator.Observing.Observer Observer
        {
            get
            {
                return observer;
            }
        }

        public DiagramTool()
        {

        }

        public DiagramTool(ToolType toolType)
        {
            this.toolType = toolType;
            this.tabItem = new TabItem();
            Initialize();
            this.observer = new ConcreteObserver.ConcreteToolManagerObserver(this.viewModel);
        }

        private Content.ViewModel viewModel;

        protected override void Initialize()
        {
            this.tabItem.Header = "Diagrams";

            this.viewModel = new Content.ViewModel();
            this.tabItem.DataContext = this.viewModel;
            this.tabItem.Content = new Content.TabContent();
            CompositionTarget.Rendering += CompositionTargetRendering;
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            viewModel.UpdateModels();
            (this.tabItem.Content as Content.TabContent).LongitudinalPlotProperty.RefreshPlot(true);
            (this.tabItem.Content as Content.TabContent).LateralPlotProperty.RefreshPlot(true);
        }
    }
}
