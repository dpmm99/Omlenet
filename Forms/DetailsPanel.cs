using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static Omlenet.ProgramState;

namespace Omlenet
{
    public partial class DetailsPanel : DockContent
    {
        public DetailsPanel()
        {
            InitializeComponent();
        }

        private void DetailsPanel_Load(object sender, EventArgs e)
        {

        }

        public void UpdateDetails(int foodId)
        {
            var item = foodDescs.FirstOrDefault(p => p.id == foodId);
            lblFoodDetail.Text = "Food details:" + Environment.NewLine +
    "Full name: " + item.longDesc + Environment.NewLine +
    "Group: " + foodGroups.First(p => p.id == item.foodGroupId); //TODO: Add tags and stuff
            //TODO: Output details like GASolver.GenerateChromosomeText but without the totals
        }

        string contextMenuTargetText = "";
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(contextMenuTargetText);
        }

        private void lblFoodDetail_MouseDown(object sender, MouseEventArgs e)
        {
            contextMenuTargetText = ((Control)sender).Text;
            ctmReadOnlyTextMenu.Show((Control)sender, e.Location);
        }

    }
}
