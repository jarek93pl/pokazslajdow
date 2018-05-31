using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SlajdyZdziec.GUI.Comon
{
    class PoleWartosci:TextBox // nowa klasa ktora dziedziczy po texboxie
    {
        float zmiana = 1;
        bool AktywnyKlawisz = false;

        public float Zmiana
        {
            get
            {
                return zmiana;
            }

            set
            {
                zmiana = value;
            }
        }

        public bool AktywnyKlawisz1
        {
            get
            {
                return AktywnyKlawisz;
            }

            set
            {
                AktywnyKlawisz = value;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            float Wartość;
            if (AktywnyKlawisz&&PobierzWartoscFloat(out Wartość))
            {
                if (e.KeyCode == Keys.Down)
                {
                    Wartość--;

                }
                if (e.KeyCode == Keys.Up)
                {
                    Wartość++;

                }
                Text = Wartość.ToString();
            }

            base.OnKeyDown(e);
        }
        public bool PobierzWartoscInt(out int Zwracana) // metoda ktora sprawdza czy tekst jest liczba
        {
            bool b = int.TryParse(Text, out Zwracana);
            return b;
        }
        public bool PobierzWartoscFloat(out float Zwracana)
        {
            bool b = Single.TryParse(Text, out Zwracana);
            return b;
        }
       
        public bool PobierzWartoscStopni(out float Kąt) //pobiera wart w stopniach.czyli dzieli przez pi
        {
            if (PobierzWartoscFloat(out Kąt))
            {
                Kąt /= (180 / Convert.ToSingle(Math.PI));
                return true;
            }
            else if(Text.ToLower()=="pi")
            {
                Kąt = Convert.ToSingle(Math.PI); ;
                return true;
            }
            
            else if(Text.Length>2&& Text.Substring(Text.Length-2).ToLower()=="pi") //dwie ostatnie litery
            {
                bool b = float.TryParse(Text.Substring(0, Text.Length - 2), out Kąt);
                return b;
            }
            return false;
        }
    }
}
