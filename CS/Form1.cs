using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        private GridPainter _GridPainter;
        ProgressBarPainter _PBPainter = new ProgressBarPainter();
        RepositoryItemProgressBar _RI = new RepositoryItemProgressBar();
        const int progressBarWidth = 200;

                private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Number", typeof(int));
            tbl.Columns.Add("Date", typeof(DateTime));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) });
            return tbl;
        }


        public Form1()
        {
            InitializeComponent();
            gridControl1.DataSource = CreateTable(20);
            gridView1.Columns[0].GroupIndex = 0;
            gridView1.CustomDrawGroupRow += new RowObjectCustomDrawEventHandler(gridView1_CustomDrawGroupRow);
            _GridPainter = new GridPainter(gridView1);
        }

        public ProgressBarBaseViewInfo CreateViewInfo(Rectangle bounds, float percent, Graphics g)
        {
            ProgressBarBaseViewInfo vi = new ProgressBarBaseViewInfo(_RI);
            vi.Bounds = bounds;
            vi.ProgressInfo.Percent = percent;
            vi.CalcViewInfo(g);
            return vi;
        }

        void gridView1_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            GridGroupRowInfo info = (GridGroupRowInfo)e.Info;
            string groupText = info.GroupText;
            info.GroupText = "[#image]";

            //default drawing
            _GridPainter.ElementsPainter.GroupRow.DrawGroupRowBackground(e.Info);
            ObjectPainter.DrawObject(e.Cache, _GridPainter.ElementsPainter.GroupRow, e.Info);
            info.GroupText = groupText;
           
            //draw caption
            Rectangle bounds = info.ButtonBounds;
            bounds.X = bounds.Right + 20;
            bounds.Width = info.Bounds.Right - bounds.Right - progressBarWidth;
            bounds.Offset(progressBarWidth, 0);
            info.CreateEditorInfo(bounds, info.Appearance.GetForeColor());
            ObjectPainter.DrawObject(e.Cache, info.CreatePainter(), info.EditorInfo);

            //draw progressbar
            bounds = info.ButtonBounds;
            bounds.X =bounds.Right +  5;
            bounds.Width = progressBarWidth;
            bounds.Inflate(0, 2);
            ProgressBarBaseViewInfo vi = CreateViewInfo(bounds, (float)0.50, e.Graphics);
            ControlGraphicsInfoArgs args = new ControlGraphicsInfoArgs(vi, e.Cache, bounds);
            _PBPainter.Draw(args);
            e.Handled = true;
        }
    }
}