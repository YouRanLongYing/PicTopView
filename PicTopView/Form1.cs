using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PicTopView
{
    public partial class Form1 : Form
    {
        private string picPath = "";
        private System.Drawing.Size oriSize;
        public string PicPath
        {
            get { return picPath; }
            set
            {
                if (File.Exists(value))
                {

                    FileInfo file = new FileInfo(value);
                    if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".jpeg" || file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".bmp")
                    {
                        FileStream fs = null;
                        try
                        {
                            fs = file.OpenRead();
                            Bitmap map = new Bitmap(fs);
                            pictureBox1.Image = map;
                            fs.Close();
                            fs.Dispose();
                            System.Drawing.Size size = this.ClientSize;
                            size.Height = map.Height;
                            size.Width = map.Width;
                            this.ClientSize = size;
                            rate = 1D;
                            oriSize = size;
                            this.Text = string.Format("{0}  Width:{1},Height:{2}", file.Name, this.Width, this.Height);
                            picPath = value;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);

                        }
                        finally
                        {
                            fs.Close();
                            fs.Dispose();
                        }

                        //this.ShowIcon = true;
                        //System.IntPtr iconHandle = map.GetHicon();
                        //this.Icon = Icon.FromHandle(iconHandle);
                    }
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            置顶ToolStripMenuItem.Text = "置顶";
            this.TopMost = 置顶ToolStripMenuItem.Checked;
            this.ShowIcon = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            notifyIcon1.Text = this.Text;
            this.notifyIcon1.Visible = false;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            oriSize = this.ClientSize;
            //this.OnMouseWheel
        }

        private void 更换图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuBtn = (ToolStripMenuItem)sender;
            OpenFileDialog oFD = new OpenFileDialog();
            if (PicPath == "")
                oFD.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop, Environment.SpecialFolderOption.None);
            else
            {
                FileInfo file = new FileInfo(picPath);
                oFD.InitialDirectory = file.Directory.FullName;
            }
            oFD.Title = "打开图片";
            oFD.ShowHelp = true;
            oFD.Filter = "图片|*.jpg;*.jpeg;*.png;*.bmp|所有文件|*.*";//过滤格式
            oFD.FilterIndex = 1;                                    //格式索引
            oFD.RestoreDirectory = false;
            oFD.Multiselect = false;                                 //是否多选
            if (oFD.ShowDialog() == DialogResult.OK)
            {
                PicPath = oFD.FileName;
            }

        }

        private void 置顶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuBtn = (ToolStripMenuItem)sender;
            置顶ToolStripMenuItem.Checked = 置顶ToolStripMenuItem.Checked ? false : true;
            this.TopMost = 置顶ToolStripMenuItem.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            System.Drawing.Size size = this.ClientSize;
            size.Height = this.pictureBox1.Image.Height;
            size.Width = this.pictureBox1.Image.Width;
            this.ClientSize = size;
            notifyIcon1.Icon = this.Icon;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                PicPath = s[0].Trim();
            }
        }


        Point downPoint;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - downPoint.X,
                    this.Location.Y + e.Y - downPoint.Y);
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        //private void toolStripMenuItem2_Click(object sender, EventArgs e)
        //{
        //    double value = double.Parse(((ToolStripMenuItem)sender).Text.TrimEnd('%'));

        //    this.Opacity = value/100D;
        //}

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            double d = this.Opacity;
            if (e.KeyCode == Keys.PageDown)
            {
                d -= 0.05D;
            }
            if (e.KeyCode == Keys.PageUp)
            {
                d += 0.05D;
            }
            if (e.KeyCode == Keys.Space)
            {
                Screen currentScreen = Screen.FromControl(this);
                Point newPos = new Point((int)(currentScreen.WorkingArea.Width * 0.5D - this.Width * 0.5D), (int)(currentScreen.WorkingArea.Height * 0.5D - this.Height * 0.5D));
                newPos = new Point(newPos.X + currentScreen.WorkingArea.X, newPos.Y + currentScreen.WorkingArea.Y);
                this.Location = newPos;
            }
            if (e.KeyCode == Keys.Home)
            {
                rate += 0.001;
                UpdateWindow();
            }
            if (e.KeyCode == Keys.End)
            {
                rate -= 0.001;
                UpdateWindow();
            }
            if (d > 1D)
                d = 1D;
            if (d < 0D)
                d = 0D;
            this.Opacity = d;

            if (e.KeyCode == Keys.Left)
            {
                this.Location = new Point(this.Location.X - 1, this.Location.Y);
            }
            if (e.KeyCode == Keys.Right)
            {
                this.Location = new Point(this.Location.X + 1, this.Location.Y);
            }
            if (e.KeyCode == Keys.Up)
            {
                this.Location = new Point(this.Location.X, this.Location.Y - 1);
            }
            if (e.KeyCode == Keys.Down)
            {
                this.Location = new Point(this.Location.X, this.Location.Y + 1);
            }
        }

        private void 最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(3000, " 程序最小化提示 ",
                     " 图标已经缩小到托盘，打开窗口请双击图标即可。 ",
                    ToolTipIcon.Info);
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //if(e.Button== System.Windows.Forms.MouseButtons.Right)
            //{
            //    contextMenuStrip1.Show(e.X,e.Y);
            //}
        }

        double rate = 1D;
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta >= 0)
            {
                rate += 0.01;
            }
            else
            {
                rate -= 0.01;
            }

            UpdateWindow();
        }

        void UpdateWindow()
        {
            Width = (int)(oriSize.Width * rate);
            Height = (int)(oriSize.Height * rate);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Width = Width;
            pictureBox1.Height = Height;
            ResizeRedraw = true;
            
            Refresh();
            Update();
            pictureBox1.Refresh();
            pictureBox1.Update();

        }


    }
}
