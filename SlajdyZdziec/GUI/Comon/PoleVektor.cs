using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlajdyZdziec.GUI.Comon //jest klasa ktora wykorzystuje klase pobierz wartosc,dziedziczy po usercontrol,dwie lable i dwa pola wartosci,do przesuwania obrazkow
{
    public partial class PoleVektor : UserControl
    {
        private static readonly object KluczZmiany = new object();
        private static readonly object KluczEnter = new object();
        public PoleVektor()
        {
            InitializeComponent();
            WartoscX.KeyDown += WartoscX_KeyDown;
        }
        public bool PobierzVektor(out Point p)
        {
            p = new Point();
            int x = 0, y = 0;
            if (WartoscX.PobierzWartoscInt(out x) && WartoscY.PobierzWartoscInt(out y)) //jezeli w dwoch polach jest liczba to zwraca true
            {
                p.X = x;
                p.Y = y;
                return true;
            }
            return false;
        }
        public bool PobierzSize(out Size p)
        {
            p = new Size();
            int x = 0, y = 0;
            if (WartoscX.PobierzWartoscInt(out x) && WartoscY.PobierzWartoscInt(out y)) //jezeli w dwoch polach jest liczba to zwraca true
            {
                p.Width = x;
                p.Height = y;
                return true;
            }
            return false;
        }
        public bool PobierzVektorF(out PointF p) // do obracania obrazow
        {
            p = new PointF();
            float x = 0, y = 0;
            if (WartoscX.PobierzWartoscFloat(out x) && WartoscY.PobierzWartoscFloat(out y))
            {
                p.X = x;
                p.Y = y;
                return true;
            }
            return false;
        }
        [Localizable(true)]
        public Point Wartość
        {
            get
            {
                Point p;
                PobierzVektor(out p);
                return p;
            }
            set
            {
                WartoscX.Text = value.X.ToString();
                WartoscY.Text = value.Y.ToString();
            }
        }
        [Localizable(true)]
        public Size WartośćSize
        {
            get
            {
                Size p;
                PobierzSize(out p);
                return p;
            }
            set
            {
                WartoscX.Text = value.Width.ToString();
                WartoscY.Text = value.Height.ToString();
            }
        }
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text { get => label3.Text; set => base.Text = label3.Text = value; }
        public int Lab
        {
            get
            {
                return lab;
            }

            set
            {
                lab = value;
            }
        }

        int lab;
        public event EventHandler Zmiana // obsluga pod zdarzenia zmiana
        {
            add
            {
                Events.AddHandler(KluczZmiany, value); //events do dodania zdarzenia 
            }
            remove
            {
                Events.RemoveHandler(KluczZmiany, value);
            }
        }
        public event EventHandler KliknietyEnter // obsluga pod zdarzenia zmiana
        {
            add
            {
                Events.AddHandler(KluczEnter, value); //events do dodania zdarzenia 
            }
            remove
            {
                Events.RemoveHandler(KluczEnter, value);
            }
        }
        private void WartoscX_TextChanged(object sender, EventArgs e)
        {
            EventHandler eh = Events[KluczZmiany] as EventHandler; // wywolanie zdarzenia
            if (eh != null)
            {
                eh(this, EventArgs.Empty);
            }
        }

        private void WartoscY_TextChanged(object sender, EventArgs e)
        {

            EventHandler eh = Events[KluczZmiany] as EventHandler; // wywolanie zdarzenia
            if (eh != null)
            {
                eh(this, EventArgs.Empty);
            }
        }

        private void PoleVektor_Load(object sender, EventArgs e)
        {

        }
        public void Strzalki(KeyEventArgs e)
        {
            float WX = 0, WY = 0;
            bool CzyDziała = WartoscX.PobierzWartoscFloat(out WX) && WartoscY.PobierzWartoscFloat(out WY);
            if (CzyDziała)
            {
                if (e.KeyCode == Keys.Down)
                {
                    WY++;
                    WartoscY.Text = WY.ToString();
                }
                if (e.KeyCode == Keys.Up)
                {
                    WY--;
                    WartoscY.Text = WY.ToString();
                }
                if (e.KeyCode == Keys.Right)
                {
                    WX++;
                    WartoscX.Text = WX.ToString();
                }
                if (e.KeyCode == Keys.Left)
                {
                    WX--;
                    WartoscX.Text = WX.ToString();
                }
            }
        }

        private void WartoscX_KeyDown(object sender, KeyEventArgs e)
        {
            Strzalki(e);
            EventHandler eh = Events[KluczEnter] as EventHandler; // wywolanie zdarzenia
            if (eh != null && e.KeyCode == Keys.Enter)
            {
                eh(this, EventArgs.Empty);
            }
        }
    }
}
