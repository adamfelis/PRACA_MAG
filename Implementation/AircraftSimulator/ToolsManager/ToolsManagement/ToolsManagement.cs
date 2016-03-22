using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Patterns.Observator.Observing.ConcreteObserving;
using Common.Patterns.Observator.Observing;

namespace ToolsManager.ToolsManagement
{
    using System.Reflection;
    class ToolsManagement : IToolsManagement
    {
        private const string toolsPath = "ToolsDll";

        private ConcreteObservableSubject concreteObservableSubject;
        public ConcreteObservableSubject ConcreteObservableSubject
        {
            get
            {
                return concreteObservableSubject;
            }
        }

        internal ToolsManagement()
        {
            concreteObservableSubject = new ConcreteObservableSubject();
            InitializeTools();
        }

        private void InitializeTools()
        {
            //strange tests
            //string s = AppDomain.CurrentDomain.BaseDirectory;
            //string s2 = s + @"..\..\..\AppInput\DLLFiles\DiagramTool.dll";
            //var DLL = Assembly.LoadFile(s2);
            //var toolDLL = DLL.GetType("DiagramTool.DiagramTool");
            
            //dynamic c = Activator.CreateInstance(toolDLL);


            //int a = 0;
            //a++;
            //c.
        }

    }
}
