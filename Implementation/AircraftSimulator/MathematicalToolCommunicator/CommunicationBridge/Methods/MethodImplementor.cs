using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalToolCommunicator.ExternalMathToolCommunicationFacade.Scripts.Parameters;
using Common.Containers;

namespace MathematicalToolCommunicator.CommunicationBridge.Methods
{
    public abstract class MethodImplementor
    {
        public virtual Parameters TemplateComputeMethod(Parameters parameters)
        {
            Parameters result = null;

            return result; 
        }

        internal abstract List<IData> Compute(Parameters parameters);

        public static MethodImplementor PrepareConcreteMethodImplementor(Common.Scripts.ScriptType scriptType)
        {
            MethodImplementor result = null;
            switch (scriptType)
            {
                case Common.Scripts.ScriptType.LaplaceTransform:
                    result = new ConcreteMethods.ConcreteMethodImplementorLaplaceTransform();
                    break;
                case Common.Scripts.ScriptType.RungeKutta:
                    result = new ConcreteMethods.ConcreteMethodImplementorRungeKutta();
                    break;
                default:
                    throw new ExternalMathToolCommunicationFacade.Scripts.InvalidScriptTypeException();
            }
            return result;
        }

        public static MethodImplementor PrepareConcreteMethodImplementor(Common.Scripts.SpecialScriptType specialScriptType)
        {
            MethodImplementor result = null;
            switch (specialScriptType)
            {
                case Common.Scripts.SpecialScriptType.Inverse:
                    result = new ConcreteMethods.ConcreteMethodImplementorInverse();
                    break;
                case Common.Scripts.SpecialScriptType.WorkspaceInitializator:
                    result = new ConcreteMethods.ConcreteMethodImplementorWorkspaceInitializator();
                    break;
                case Common.Scripts.SpecialScriptType.StrategyCreator:
                    result = new ConcreteMethods.ConcreteMethodImplementorStrategyCreator();
                    break;
                case Common.Scripts.SpecialScriptType.GetState:
                    result = new ConcreteMethods.ConcreteMethodImplementorGetState();
                    break;
                case Common.Scripts.SpecialScriptType.WorkspaceUpdater:
                    result = new ConcreteMethods.ConcreteMethodImplementorWorkspaceUpdater();
                    break;
                case Common.Scripts.SpecialScriptType.MissileAdder:
                    result = new ConcreteMethods.ConcreteMethodImplementorMissileAdder();
                    break;
                case Common.Scripts.SpecialScriptType.SimulateMissile:
                    result = new ConcreteMethods.ConcreteMethodImplementorSimulateMissile();
                    break;
                case Common.Scripts.SpecialScriptType.Backup:
                    result = new ConcreteMethods.ConcreteMethodImplementorBackup();
                    break;
                default:
                    throw new ExternalMathToolCommunicationFacade.Scripts.InvalidScriptTypeException();
            }
            return result;
        }
    }
}
