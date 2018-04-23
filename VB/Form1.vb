Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid.Drawing
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Repository

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
		Private _GridPainter As GridPainter
		Private _PBPainter As New ProgressBarPainter()
		Private _RI As New RepositoryItemProgressBar()
		Private Const progressBarWidth As Integer = 200

				Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Number", GetType(Integer))
			tbl.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To RowCount - 1
				tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) })
			Next i
			Return tbl
				End Function


		Public Sub New()
			InitializeComponent()
			gridControl1.DataSource = CreateTable(20)
			gridView1.Columns(0).GroupIndex = 0
			AddHandler gridView1.CustomDrawGroupRow, AddressOf gridView1_CustomDrawGroupRow
			_GridPainter = New GridPainter(gridView1)
		End Sub

		Public Function CreateViewInfo(ByVal bounds As Rectangle, ByVal percent As Single, ByVal g As Graphics) As ProgressBarBaseViewInfo
			Dim vi As New ProgressBarBaseViewInfo(_RI)
			vi.Bounds = bounds
			vi.ProgressInfo.Percent = percent
			vi.CalcViewInfo(g)
			Return vi
		End Function

		Private Sub gridView1_CustomDrawGroupRow(ByVal sender As Object, ByVal e As RowObjectCustomDrawEventArgs)
			Dim info As GridGroupRowInfo = CType(e.Info, GridGroupRowInfo)
			Dim groupText As String = info.GroupText
			info.GroupText = "[#image]"

			'default drawing
			_GridPainter.ElementsPainter.GroupRow.DrawGroupRowBackground(e.Info)
			ObjectPainter.DrawObject(e.Cache, _GridPainter.ElementsPainter.GroupRow, e.Info)
			info.GroupText = groupText

			'draw caption
			Dim bounds As Rectangle = info.ButtonBounds
			bounds.X = bounds.Right + 20
			bounds.Width = info.Bounds.Right - bounds.Right - progressBarWidth
			bounds.Offset(progressBarWidth, 0)
			info.CreateEditorInfo(bounds, info.Appearance.GetForeColor())
			ObjectPainter.DrawObject(e.Cache, info.CreatePainter(), info.EditorInfo)

			'draw progressbar
			bounds = info.ButtonBounds
			bounds.X =bounds.Right + 5
			bounds.Width = progressBarWidth
			bounds.Inflate(0, 2)
			Dim vi As ProgressBarBaseViewInfo = CreateViewInfo(bounds, CSng(0.50), e.Graphics)
			Dim args As New ControlGraphicsInfoArgs(vi, e.Cache, bounds)
			_PBPainter.Draw(args)
			e.Handled = True
		End Sub
	End Class
End Namespace