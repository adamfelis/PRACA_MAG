using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupTool.Content
{
    class ViewModel
    {
        private Patterns.Observator.Observing.Observer observer;
        public ViewModel(Patterns.Observator.Observing.Observer observer)
        {
            this.observer = observer;
        }

        internal void GenerateBackupFile()
        {
            this.observer.Compute(Common.Scripts.SpecialScriptType.Backup, new List<Common.Containers.IData>());
        }
    }
}
