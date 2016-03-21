using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationManager
{
    public partial class ApplicationManagerFrame : Form
    {
        private ApplicationManager applicationManagerInstance;
        public ApplicationManagerFrame(ApplicationManager applicationManager)
        {
            this.applicationManagerInstance = applicationManager;
            InitializeComponent();
        }
    }
}
