using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Omlenet
{
    public class DataGridViewContextMenu : ContextMenuStrip
    {
        #region Code moved from a form's Designer
        private ToolStripMenuItem copyAllDgv;
        private ToolStripMenuItem copyRowDgv;
        private ToolStripMenuItem copyColumnDgv;
        private ToolStripMenuItem copyCellDgv;

        public DataGridViewContextMenu(System.ComponentModel.IContainer container) : base(container)
        {
            SuspendLayout();
            copyAllDgv = new ToolStripMenuItem();
            copyRowDgv = new ToolStripMenuItem();
            copyColumnDgv = new ToolStripMenuItem();
            copyCellDgv = new ToolStripMenuItem();

            // 
            // ctmDgvCopyMenu
            // 
            Items.AddRange(new ToolStripItem[] {
            copyAllDgv,
            copyRowDgv,
            copyColumnDgv,
            copyCellDgv});
            Name = "ctmReadOnlyTextMenu";
            Size = new System.Drawing.Size(181, 114);

            // 
            // copyAllDgv
            // 
            copyAllDgv.Name = "copyAllDgv";
            copyAllDgv.Size = new System.Drawing.Size(180, 22);
            copyAllDgv.Text = "&Copy All";
            copyAllDgv.Click += new System.EventHandler(copyAllDgv_Click);
            // 
            // copyRowDgv
            // 
            copyRowDgv.Name = "copyRowDgv";
            copyRowDgv.Size = new System.Drawing.Size(180, 22);
            copyRowDgv.Text = "Copy &Row";
            copyRowDgv.Click += new System.EventHandler(copyRowDgv_Click);
            // 
            // copyColumnDgv
            // 
            copyColumnDgv.Name = "copyColumnDgv";
            copyColumnDgv.Size = new System.Drawing.Size(180, 22);
            copyColumnDgv.Text = "C&opy Column";
            copyColumnDgv.Click += new System.EventHandler(copyColumnDgv_Click);
            // 
            // copyCellDgv
            // 
            copyCellDgv.Name = "copyCellDgv";
            copyCellDgv.Size = new System.Drawing.Size(180, 22);
            copyCellDgv.Text = "Co&py Cell";
            copyCellDgv.Click += new System.EventHandler(copyCellDgv_Click);

            ResumeLayout(false);
        }
#endregion

        private string dgvCellForCopy;
        private string dgvRowForCopy;
        private string dgvColumnForCopy;
        private string dgvTableForCopy;

        private string rowToString(DataGridViewRow row)
        {
            var cells = new List<string>();
            for (var x = 0; x < row.Cells.Count; x++) cells.Add(row.Cells[x].Value.ToString());
            return string.Join("\t", cells);
        }

        public void Show(DataGridView sender, int cellX, int cellY, Point position)
        {
            if (sender.Rows.Count == 0) return; //Don't display if there's nothing that can be copied.

            if (cellX != -1 && cellY != -1) dgvCellForCopy = sender.Rows[cellY].Cells[cellX].Value.ToString(); else dgvCellForCopy = null;
            if (cellX != -1)
            {
                var cells = new List<string>();
                for (var x = 0; x < sender.Rows.Count; x++) cells.Add(sender.Rows[x].Cells[cellX].Value.ToString());
                dgvColumnForCopy = string.Join(Environment.NewLine, cells);
            }
            else dgvColumnForCopy = null;
            if (cellY != -1)
            {
                dgvRowForCopy = rowToString(sender.Rows[cellY]);
            }
            else dgvRowForCopy = null;

            var rows = new List<string>();
            for (var x = 0; x < sender.Rows.Count; x++) rows.Add(rowToString(sender.Rows[x]));
            dgvTableForCopy = string.Join(Environment.NewLine, rows);

            copyAllDgv.Visible = true;
            copyRowDgv.Visible = (dgvRowForCopy != null);
            copyColumnDgv.Visible = (dgvColumnForCopy != null);
            copyCellDgv.Visible = (dgvCellForCopy != null);

            if (dgvCellForCopy != null) {
                var d = sender.GetCellDisplayRectangle(cellX, cellY, false);
                position.Offset(d.Left, d.Top);
            }
            else if (dgvRowForCopy != null)
            {
                var d = sender.GetRowDisplayRectangle(cellY, false);
                position.Offset(d.Left, d.Top);
            }
            else if (dgvColumnForCopy != null)
            {
                var d = sender.GetColumnDisplayRectangle(cellX, false);
                position.Offset(d.Left, d.Top);
            }
            Show(sender, position);
        }

        private void copyCellDgv_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(dgvCellForCopy);
        }

        private void copyColumnDgv_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(dgvColumnForCopy);
        }

        private void copyRowDgv_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(dgvRowForCopy);
        }

        private void copyAllDgv_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(dgvTableForCopy);
        }
    }
}
