using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AC130
{
    public partial class AC130 : Form
    {
        static Thread AutoClick;
        static Thread AutoClick2;
        static int militime;
        static int counter = 0;
        public static int WM_HOTKEY = 0x312;
        private int x1 = 0;
        private int y1 = 0;
        private int x2 = 0;
        private int y2 = 0;

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void AClick()
        {
            while (true)
            {
                int x = Cursor.Position.X;
                int y = Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                counter++;
                label1.Text = counter.ToString();
                Thread.Sleep(militime);
            }
        }

        public void AClick2()
        {
            while (true)
            {
                SetCursorPos(x1, y1);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x1, y1, 0, 0);
                Thread.Sleep(militime);
                SetCursorPos(x2, y2);
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x2, y2, 0, 0);
                counter++;
                label1.Text = counter.ToString();
                Thread.Sleep(militime);
            }
        }
 
        public AC130()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                try
                {
                    militime = int.Parse(textBox1.Text);
                }
                catch
                {
                    militime = 500;
                }
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                if (key == Keys.F1)
                {
                    if (!AutoClick.IsAlive)
                    {
                        AutoClick2.Abort();
                        AutoClick2 = new Thread(AClick2);
                        AutoClick.Start();
                        lstate.Text = "On";
                        lstate.ForeColor = System.Drawing.Color.Green;
                        Controls.Add(lstate);
                    }
                    else
                    {
                        AutoClick.Abort();
                        AutoClick = new Thread(AClick);
                        lstate.Text = "Off";
                        lstate.ForeColor = System.Drawing.Color.Red;
                        Controls.Add(lstate);
                    }
                } 
                if (key == Keys.F5)
                {
                    if (!AutoClick2.IsAlive)
                    {
                        AutoClick.Abort();
                        AutoClick = new Thread(AClick);
                        AutoClick2.Start();
                        lstate.Text = "On";
                        lstate.ForeColor = System.Drawing.Color.Green;
                        Controls.Add(lstate);
                    }
                    else
                    {
                        AutoClick2.Abort();
                        AutoClick2 = new Thread(AClick2);
                        lstate.Text = "Off";
                        lstate.ForeColor = System.Drawing.Color.Red;
                        Controls.Add(lstate);
                    }
                }
                if (key == Keys.F6)
                {
                    AutoClick2.Abort();
                    AutoClick2 = new Thread(AClick2);
                    lstate.Text = "Off";
                    lstate.ForeColor = System.Drawing.Color.Red;
                    x1 = Cursor.Position.X;
                    y1 = Cursor.Position.Y;
                    label2.Text = String.Format("{0}, {1}", x1, y1);
                }
                if (key == Keys.F7)
                {
                    AutoClick2.Abort();
                    AutoClick2 = new Thread(AClick2);
                    lstate.Text = "Off";
                    lstate.ForeColor = System.Drawing.Color.Red;
                    x2 = Cursor.Position.X;
                    y2 = Cursor.Position.Y;
                    label3.Text = String.Format("{0}, {1}", x2, y2);
                }
            }
        }
        private void AC130_Load(object sender, EventArgs e)
        {
            AutoClick = new Thread(AClick);
            AutoClick2 = new Thread(AClick2);
            RegisterHotKey(this.Handle, (int)Keys.F1, 0, (uint)Keys.F1);
            RegisterHotKey(this.Handle, (int)Keys.F5, 0, (uint)Keys.F5);
            RegisterHotKey(this.Handle, (int)Keys.F6, 0, (uint)Keys.F6);
            RegisterHotKey(this.Handle, (int)Keys.F7, 0, (uint)Keys.F7);
            lstate.Text = "Off";
            lstate.ForeColor = System.Drawing.Color.Red;
            Controls.Add(lstate);
            AutoClick.IsBackground = true;
        }
    }
}
