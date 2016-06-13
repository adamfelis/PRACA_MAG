using Common.Containers;
using Common.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalToolCommunicator.ExternalMathToolCommunicationFacade
{
    sealed class MathToolFacade
    {
        private Dictionary<ScriptType, Scripts.Script> availableScripts;
        private Dictionary<SpecialScriptType, Scripts.Script> availableSpecialScripts;
        private MLApp.MLApp mlApp;

        internal MathToolFacade()
        {
            InitializeScripts();
            this.mlApp = new MLApp.MLApp();
            string mainDirectory = AppDomain.CurrentDomain.BaseDirectory;
#if !DEBUG
            mainDirectory += @"..\";
#endif
            mainDirectory += @"..\..\PRACA_MAG\Implementation\AircraftSimulator\AppInput\MatlabFiles\";
            this.mlApp.Execute(@"cd " + mainDirectory);
            this.mlApp.Execute(@"clear");
        }

        internal void ReleaseMathToolLibrary()
        {
            this.mlApp.Quit();
        }

        internal List<IData> RunScript(ScriptType scriptType, Scripts.Parameters.Parameters parameters)
        {
            if (!availableScripts.ContainsKey(scriptType))
                throw new Scripts.InvalidScriptTypeException();
            return availableScripts[scriptType].RunScript(mlApp, parameters);
        }
        internal List<IData> RunScript(SpecialScriptType specialScriptType, Scripts.Parameters.Parameters parameters)
        {
            if (!availableSpecialScripts.ContainsKey(specialScriptType))
                throw new Scripts.InvalidScriptTypeException();
            return availableSpecialScripts[specialScriptType].RunScript(mlApp, parameters);
        }

        private void InitializeScripts()
        {
            this.availableScripts = new Dictionary<ScriptType, Scripts.Script>();

            var scriptTypeValues = Enum.GetValues(typeof(ScriptType)).Cast<ScriptType>();
            foreach(var scriptTypeValue in scriptTypeValues)
            {
                switch(scriptTypeValue)
                {
                    case ScriptType.RungeKutta:
                        availableScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptRungeKutta());
                        break;
                    case ScriptType.LaplaceTransform:
                        availableScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptLaplaceTransform());
                        break;
                    default:
                        throw new Scripts.InvalidScriptTypeException();
                }
            }


            this.availableSpecialScripts = new Dictionary<SpecialScriptType, Scripts.Script>();

            var specialScriptTypeValues = Enum.GetValues(typeof(SpecialScriptType)).Cast<SpecialScriptType>();
            foreach (var scriptTypeValue in specialScriptTypeValues)
            {
                switch (scriptTypeValue)
                {
                    case SpecialScriptType.Inverse:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptInverse());
                        break;
                    case SpecialScriptType.WorkspaceInitializator:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptWorkspaceInitializator());
                        break;
                    case SpecialScriptType.StrategyCreator:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptStrategyCreator());
                        break;
                    case SpecialScriptType.GetState:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptGetState());
                        break;
                    case SpecialScriptType.WorkspaceUpdater:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptWorkspaceUpdater());
                        break;
                    case SpecialScriptType.MissileAdder:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptMissileAdder());
                        break;
                    case SpecialScriptType.SimulateMissile:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptSimulateMissile());
                        break;
                    case SpecialScriptType.Backup:
                        availableSpecialScripts.Add(scriptTypeValue, new Scripts.ConcreteScript.ConcreteScriptBackup());
                        break;
                    default:
                        throw new Scripts.InvalidScriptTypeException();
                }
            }
        }
    }
}
