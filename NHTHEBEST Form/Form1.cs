using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NHTHEBEST.Forms
{

    public partial class NForm : Form
    {
        //*
        public override string Text
        {
            get
            {
                if (label1 != null)
                    return label1.Text;
                else
                    return "";
            }
            set
            {
                if (label1 != null)
                    label1.Text = value;
            }
        }
        public override Color ForeColor
        {
            get
            {
                if (label1 != null)
                    return label1.BackColor;
                else
                    return new Color();
            }
            set
            {
                if (label1 != null)
                    label1.ForeColor = value;
            }
        }
        ///////////////////////////////////////////////////////
        public Color CloseButton
        {
            get
            {
                if (panel2 != null)
                    return panel2.BackColor;
                else
                    return new Color();
            }
            set
            {
                if (panel2 != null)
                    panel2.BackColor = value;
            }
        }
        public Color MaximizeButton
        {
            get
            {
                if (panel3 != null)
                    return panel3.BackColor;
                else
                    return new Color();
            }
            set
            {
                if (panel3 != null)
                    panel3.BackColor = value;
            }
        }
        public Color MinimizeButton
        {
            get
            {
                if (panel4 != null)
                    return panel4.BackColor;
                else
                    return new Color();
            }
            set
            {
                if (panel4 != null)
                    panel4.BackColor = value;
            }
        }
        /////////////////////////////////////////////////////
        public override Color BackColor
        {
            get
            {
                if (panel1 != null)
                    return panel1.BackColor;
                else
                    return new Color();
            }
            set
            {
                if (panel1 != null)
                    panel1.BackColor = value;
            }
        }
        //*/
        public override Font Font { get => base.Font; set => base.Font = value; }
        
        public bool DarkMode
        {
            get
            {
                return Properties.Settings.Default.DarkMode;
            }
            set
            {
                Properties.Settings.Default.DarkMode = value;
                Properties.Settings.Default.Save();
            }
        }

        public NForm()
        {
            InitializeComponent();

            if (DarkMode)
            {
                BackColor = Color.FromArgb(20,20,20);
            }

            this.FormBorderStyle = FormBorderStyle.None; // no borders
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }
        protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        {
            e.Graphics.FillRectangle(Brushes.Transparent, Top);
            e.Graphics.FillRectangle(Brushes.Transparent, Left);
            e.Graphics.FillRectangle(Brushes.Transparent, Right);
            e.Graphics.FillRectangle(Brushes.Transparent, Bottom);
        }
        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            WM_NCLBUTTONDOWN = 0xA1,
            HT_CAPTION = 0x2,
            HTBOTTOMRIGHT = 17;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();



        const int _ = 10; // you can rename this variable if you like

        private void NForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else
                WindowState = FormWindowState.Maximized;
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }

        Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }
    }
}
