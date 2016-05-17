using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsManager.Observator.Observing.ConcreteObserving;

namespace ToolsManager.ToolsManagement
{
    using System.IO;
    using System.Reflection;
    class ToolsManagement : IToolsManagement
    {
        private const string toolsPath = "DLLFiles";

        private ConcreteObservableSubject concreteObservableSubject;
        public ConcreteObservableSubject ConcreteObservableSubject
        {
            get
            {
                return concreteObservableSubject;
            }
        }

        internal ToolsManagement(Action<ToolAdapter.Tool.ITool> toolAddedAction)
        {
            concreteObservableSubject = new ConcreteObservableSubject();
            InitializeTools(toolAddedAction);
        }

        private void InitializeTools(Action<ToolAdapter.Tool.ITool> toolAddedAction)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
#if !DEBUG
            baseDirectory += @"..\";
#endif
            string dllsDirectory = baseDirectory + @"..\..\PRACA_MAG\Implementation\AircraftSimulator\AppInput\" + toolsPath;

            string[] toolPaths = Directory.GetFiles(dllsDirectory, "*Tool.dll", SearchOption.TopDirectoryOnly);

            foreach (string toolPath in toolPaths)
            {
                var DLL = Assembly.LoadFile(toolPath);
                string toolFile = toolPath.Substring((dllsDirectory + "\\").Length);
                string toolName = toolFile.Substring(0, toolFile.LastIndexOf('.'));

                var toolDLL = DLL.GetType(toolName + "." + toolName);
                dynamic tool = Activator.CreateInstance(toolDLL, ToolAdapter.Tool.ToolType.Diagrams);
                this.concreteObservableSubject.Subscribe((tool as ToolAdapter.Tool.ITool).Observer);
                toolAddedAction(tool);
            }
        }
    }
}
