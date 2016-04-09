using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.CommunicationBridge;
using Common.Containers;

namespace ToolsManager
{
    public sealed class ToolsManager : Common.Tool.IToolsManagerForCommonUsage
    {
        private static ToolsManager instance;
        private MathToolCommunicator mathToolCommunicator;
        private ToolsManagement.IToolsManagement toolsManagement;

        public static ToolsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ToolsManager();
                return instance;
            }
        }

        public ToolsManagement.IToolsManagement ToolsManagement
        {
            get
            {
                return toolsManagement;
            }
        }

        private ToolsManager()
        {
            toolsManagement = new ToolsManagement.ToolsManagement();
            mathToolCommunicator = new RefinedMathToolCommunicator();
        }

        public void SetScriptTypeForComputations(Common.Scripts.ScriptType scriptType)
        {
            mathToolCommunicator.SetMethodImplementor(scriptType);
        }

        public List<IData> Compute(List<IData> parameters)
        {
            return mathToolCommunicator.Compute(parameters);
        }

        public List<IData> Compute(Common.Scripts.SpecialScriptType specialScriptType, List<IData> parameters)
        {
            return mathToolCommunicator.Compute(specialScriptType, parameters);
        }

    }
}
