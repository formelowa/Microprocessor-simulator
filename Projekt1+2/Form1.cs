using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;


namespace OSK1

{
    public partial class Form1 : Form
    {
        public readonly byte ROZMIAR_STOSU = 20; 
        short[] stos;                            
        byte sp;                                 

        byte ah = 0, al = 0, bh = 0, bl = 0, ch = 0, cl = 0, dh = 0, dl = 0;
        int linia = 0;
        bool przypisanie, dodawanie, odejmowanie, dodajDoStosu, zdejmijZeStosu;
        string y, x;

        public Form1()
        {
            InitializeComponent();

            rozkaz.Items.Add("MOV");
            rozkaz.Items.Add("ADD");
            rozkaz.Items.Add("SUB");

            rozkaz.Items.Add("PUSH");
            rozkaz.Items.Add("POP");

            rozkaz.Items.Add("INT01");
            rozkaz.Items.Add("INT02");
            rozkaz.Items.Add("INT03");
            rozkaz.Items.Add("INT04");
            rozkaz.Items.Add("INT05");
            rozkaz.Items.Add("INT06");
            rozkaz.Items.Add("INT07");
            rozkaz.Items.Add("INT08");
            rozkaz.Items.Add("INT09");
            rozkaz.Items.Add("INT10");

            adresowanie.Items.Add("Rejestrowe");
            adresowanie.Items.Add("Natychmiastowe");

            rejestr.Items.Add("AH");
            rejestr.Items.Add("AL");
            rejestr.Items.Add("BH");
            rejestr.Items.Add("BL");
            rejestr.Items.Add("CH");
            rejestr.Items.Add("CL");
            rejestr.Items.Add("DH");
            rejestr.Items.Add("DL");

            adres.Items.Add("AH");
            adres.Items.Add("AL");
            adres.Items.Add("BH");
            adres.Items.Add("BL");
            adres.Items.Add("CH");
            adres.Items.Add("CL");
            adres.Items.Add("DH");
            adres.Items.Add("DL");


            adres.Enabled = false;
            wpis.Enabled = false;
            rejestr.Enabled = false;
            adresowanie.Enabled = false;
            ok.Enabled = false;
            pomoc.Enabled = false;

            stos = new short[ROZMIAR_STOSU];
            sp = (byte)(ROZMIAR_STOSU - 1);
        }

       

        private short zamienRejestry(byte H_bits, byte L_bits) //Zamień dwa rejestry na jeden  
        {
            return (short)((H_bits << 8) + L_bits);
        }

        private void stos_push(String rejestr)      //wrzucanie rejestrów nas stos
        {
            short doDodania = 0;
            if (rejestr.Equals("AX"))
            {
                doDodania = zamienRejestry(ah, al);
            }
            else if (rejestr.Equals("BX"))
            {
                doDodania = zamienRejestry(bh, bl);
            }
            else if (rejestr.Equals("CX"))
            {
                doDodania = zamienRejestry(ch, cl);
            }
            else if (rejestr.Equals("DX"))
            {
                doDodania = zamienRejestry(dh, dl);
            }
            stos[sp] = doDodania;
            sp -= 1;
        }

        private void pomoc_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void wiersz_TextChanged(object sender, EventArgs e)
        {

        }

        private short stos_pop()
        {
            sp += 1;
            return stos[sp];
        }

        private void rozkaz_SelectedIndexChanged(object sender, EventArgs e)
        {
            wiersz.Text = wiersz.Text + rozkaz.SelectedItem + " ";
            pomoc.Text = pomoc.Text + rozkaz.SelectedItem + " ";

            if (rozkaz.SelectedItem.ToString().Contains("INT"))       
            {
                wiersz.Text = wiersz.Text + "\r\n";
                pomoc.Text = pomoc.Text + "\r\n";
            }
            else                               
            {
                rejestr.Enabled = true;
                rozkaz.Enabled = false;
                krokowa.Enabled = true;
            }
        }

        private void rejestr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rejestr.SelectedIndex == 0)
            {
                wiersz.Text = wiersz.Text + "AH ";
                pomoc.Text = pomoc.Text + "AH ";
            }
            if (rejestr.SelectedIndex == 1)
            {
                wiersz.Text = wiersz.Text + "AL ";
                pomoc.Text = pomoc.Text + "AL ";
            }
            if (rejestr.SelectedIndex == 2)
            {
                wiersz.Text = wiersz.Text + "BH ";
                pomoc.Text = pomoc.Text + "BH ";
            }
            if (rejestr.SelectedIndex == 3)
            {
                wiersz.Text = wiersz.Text + "BL ";
                pomoc.Text = pomoc.Text + "BL ";
            }
            if (rejestr.SelectedIndex == 4)
            {
                wiersz.Text = wiersz.Text + "CH ";
                pomoc.Text = pomoc.Text + "CH ";
            }
            if (rejestr.SelectedIndex == 5)
            {
                wiersz.Text = wiersz.Text + "CL ";
                pomoc.Text = pomoc.Text + "CL ";
            }
            if (rejestr.SelectedIndex == 6)
            {
                wiersz.Text = wiersz.Text + "DH ";
                pomoc.Text = pomoc.Text + "DH ";
            }
            if (rejestr.SelectedIndex == 7)
            {
                wiersz.Text = wiersz.Text + "DL ";
                pomoc.Text = pomoc.Text + "DL ";
            }

            if (rozkaz.SelectedItem.Equals("POP") || rozkaz.SelectedItem.Equals("PUSH"))
            {
                wiersz.Text = wiersz.Text + "\r\n";
                pomoc.Text = pomoc.Text + "\r\n";
                rozkaz.Enabled = true;
                adresowanie.Enabled = false;
                rejestr.Enabled = false;
            }
            else
            {
                adresowanie.Enabled = true;
                rejestr.Enabled = false;
            }
        }

        private void adresowanie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (adresowanie.SelectedIndex == 0)
            {
                adres.Enabled = true;
                wpis.Enabled = false;
            }
            if (adresowanie.SelectedIndex == 1)
            {
                adres.Enabled = false;
                wpis.Enabled = true;
                ok.Enabled = true;
            }
            adresowanie.Enabled = false;
        }

        private void adres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (adres.SelectedIndex == 0)
            {
                wiersz.Text = wiersz.Text + "AH" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(ah) + "\r\n";
            }
            if (adres.SelectedIndex == 1)
            {
                wiersz.Text = wiersz.Text + "AL" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(al) + "\r\n";
            }
            if (adres.SelectedIndex == 2)
            {
                wiersz.Text = wiersz.Text + "BH" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(bh) + "\r\n";
            }
            if (adres.SelectedIndex == 3)
            {
                wiersz.Text = wiersz.Text + "BL" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(bl) + "\r\n";
            }
            if (adres.SelectedIndex == 4)
            {
                wiersz.Text = wiersz.Text + "CH" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(ch) + "\r\n";
            }
            if (adres.SelectedIndex == 5)
            {
                wiersz.Text = wiersz.Text + "CL" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(cl) + "\r\n";
            }
            if (adres.SelectedIndex == 6)
            {
                wiersz.Text = wiersz.Text + "DH" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(dh) + "\r\n";
            }
            if (adres.SelectedIndex == 7)
            {
                wiersz.Text = wiersz.Text + "DL" + "\r\n";
                pomoc.Text = pomoc.Text + Convert.ToString(dl) + "\r\n";
            }
            adres.Enabled = false;
            rozkaz.Enabled = true;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(wpis.Text) >= 0 && Convert.ToInt32(wpis.Text) <= 255)
            {
                wiersz.Text = wiersz.Text + wpis.Text + "\r\n";
                pomoc.Text = pomoc.Text + wpis.Text + "\r\n";
            }
            else
            {
                wiersz.Text = wiersz.Text + "0\r\n";
                pomoc.Text = pomoc.Text + "0\r\n";
            }
            ok.Enabled = false;
            rozkaz.Enabled = true;
            wpis.Clear();
        }

        private void praca_Click(object sender, EventArgs e)
        {
            krokowa.Enabled = false;
            for (linia = 0; linia < wiersz.Lines.Length-1; linia++)
            {
                wykonajLinie();
            }
            krokowa.Enabled = false;
            wiersz.Clear();
            pomoc.Clear();
        }

        private void wykonajLinie()
        {
            if (wiersz.Lines.Length > 0)
            {
                przypisanie = wiersz.Lines[linia].Contains("MOV");
                dodawanie = wiersz.Lines[linia].Contains("ADD");
                odejmowanie = wiersz.Lines[linia].Contains("SUB");
                bool INT01 = wiersz.Lines[linia].Contains("INT01");
                bool INT02 = wiersz.Lines[linia].Contains("INT02");
                bool INT03 = wiersz.Lines[linia].Contains("INT03");
                bool INT04 = wiersz.Lines[linia].Contains("INT04");
                bool INT05 = wiersz.Lines[linia].Contains("INT05");
                bool INT06 = wiersz.Lines[linia].Contains("INT06");
                bool INT07 = wiersz.Lines[linia].Contains("INT07");
                bool INT08 = wiersz.Lines[linia].Contains("INT08");
                bool INT09 = wiersz.Lines[linia].Contains("INT09");
                bool INT10 = wiersz.Lines[linia].Contains("INT10");
                bool pop = wiersz.Lines[linia].Contains("POP");
                bool push = wiersz.Lines[linia].Contains("PUSH");

                if (przypisanie)
                {
                    y = wiersz.Lines[linia].Split(' ')[1];
                    x = pomoc.Lines[linia].Split(' ')[2];

                    if (y == "AH")
                    {
                        ah = Convert.ToByte(x);
                    }
                    if (y == "AL")
                    {
                        al = Convert.ToByte(x);
                    }
                    if (y == "BH")
                    {
                        bh = Convert.ToByte(x);
                    }
                    if (y == "BL")
                    {
                        bl = Convert.ToByte(x);
                    }
                    if (y == "CH")
                    {
                        ch = Convert.ToByte(x);
                    }
                    if (y == "CL")
                    {
                        cl = Convert.ToByte(x);
                    }
                    if (y == "DH")
                    {
                        dh = Convert.ToByte(x);
                    }
                    if (y == "DL")
                    {
                        dl = Convert.ToByte(x);
                    }

                }
                else if (dodawanie)
                {
                    y = wiersz.Lines[linia].Split(' ')[1];
                    x = pomoc.Lines[linia].Split(' ')[2];

                    if (y == "AH")
                    {
                        ah += Convert.ToByte(x);
                    }
                    if (y == "AL")
                    {
                        al += Convert.ToByte(x);
                    }
                    if (y == "BH")
                    {
                        bh += Convert.ToByte(x);
                    }
                    if (y == "BL")
                    {
                        bl += Convert.ToByte(x);
                    }
                    if (y == "CH")
                    {
                        ch += Convert.ToByte(x);
                    }
                    if (y == "CL")
                    {
                        cl += Convert.ToByte(x);
                    }
                    if (y == "DH")
                    {
                        dh += Convert.ToByte(x);
                    }
                    if (y == "DL")
                    {
                        dl += Convert.ToByte(x);
                    }

                }
                else if (odejmowanie)
                {
                    y = wiersz.Lines[linia].Split(' ')[1];
                    x = pomoc.Lines[linia].Split(' ')[2];

                    if (y == "AH")
                    {
                        ah -= Convert.ToByte(x);
                    }
                    if (y == "AL")
                    {
                        al -= Convert.ToByte(x);
                    }
                    if (y == "BH")
                    {
                        bh -= Convert.ToByte(x);
                    }
                    if (y == "BL")
                    {
                        bl -= Convert.ToByte(x);
                    }
                    if (y == "CH")
                    {
                        ch -= Convert.ToByte(x);
                    }
                    if (y == "CL")
                    {
                        cl -= Convert.ToByte(x);
                    }
                    if (y == "DH")
                    {
                        dh -= Convert.ToByte(x);
                    }
                    if (y == "DL")
                    {
                        dl -= Convert.ToByte(x);
                    }
                }
                else if (push)
                {
                    y = wiersz.Lines[linia].Split(' ')[1];
                    if (y.Equals("AL") || y.Equals("AH"))
                    {
                        stos_push("AX");
                    }
                    else if (y.Equals("BL") || y.Equals("BH"))
                    {
                        stos_push("BX");
                    }
                    else if (y.Equals("CL") || y.Equals("CH"))
                    {
                        stos_push("CX");
                    }
                    else if (y.Equals("DL") || y.Equals("DH"))
                    {
                        stos_push("DX");
                    }
                }
                else if (pop)
                {
                    short zeStosu = stos_pop();
                    y = wiersz.Lines[linia].Split(' ')[1];
                    if (y.Equals("AL") || y.Equals("AH"))
                    {
                        ah = (byte)(zeStosu >> 8);
                        al = (byte)(zeStosu);
                    }
                    else if (y.Equals("BL") || y.Equals("BH"))
                    {
                        bh = (byte)(zeStosu >> 8);
                        bl = (byte)(zeStosu);
                    }
                    else if (y.Equals("CL") || y.Equals("CH"))
                    {
                        ch = (byte)(zeStosu >> 8);
                        cl = (byte)(zeStosu);
                    }
                    else if (y.Equals("DL") || y.Equals("DH"))
                    {
                        dh = (byte)(zeStosu >> 8);
                        dl = (byte)(zeStosu);
                    }
                }
                else if (INT01)
                {
                    MessageBox.Show("Współrzędne kursora: " + Control.MousePosition.ToString());
                }
                else if (INT02)
                {
                    MessageBox.Show("Uruchamianie Kalkulatora");
                    Process.Start("calculator.exe");
                }
                else if (INT03)
                {
                    MessageBox.Show("Uruchamianie określonej strony internetowej");
                    Process.Start("http://www.google.com");
                }
                else if (INT04)
                {
                    MessageBox.Show("Aktualny czas: " + DateTime.Now.ToString("HH:mm:ss tt"));
                }
                else if (INT05)
                {
                    MessageBox.Show("Aktualnie zalogowany użytkownik: " + Environment.UserName);
                }
                else if (INT06)
                {
                    MessageBox.Show("Pobiera lub ustawia w pełni kwalifikowana ścieżka bieżącego katalogu roboczego.");
                    MessageBox.Show("Aktualnie zalogowany użytkownik: " + Environment.CurrentDirectory);    
                }
                else if (INT07)
                {
                    MessageBox.Show("Otwieranie wskazanego pliku");
                    Process.Start("C:\\Desktop\\1\\posk.txt");
                }
                else if (INT08)
                {
                    MessageBox.Show("Od uruchomienia programu minęło " + Environment.TickCount.ToString() + " ms"); 
                }
                else if (INT09)
                {
                    MessageBox.Show("Informacje na temat czcionki: " + Control.DefaultFont);
                }
                else if (INT10)
                {
                    MessageBox.Show("Aktualnie wciśnięty guzik: " + Control.ModifierKeys);
                }

            }

            AH.Text = Convert.ToString(ah, 2).PadLeft(8, '0');
            BH.Text = Convert.ToString(bh, 2).PadLeft(8, '0');
            CH.Text = Convert.ToString(ch, 2).PadLeft(8, '0');
            DH.Text = Convert.ToString(dh, 2).PadLeft(8, '0');
            AL.Text = Convert.ToString(al, 2).PadLeft(8, '0');
            BL.Text = Convert.ToString(bl, 2).PadLeft(8, '0');
            CL.Text = Convert.ToString(cl, 2).PadLeft(8, '0');
            DL.Text = Convert.ToString(dl, 2).PadLeft(8, '0');
        }

        private void krokowa_Click(object sender, EventArgs e)
        {
            praca.Enabled = false;

            if (wiersz.Lines.Length > 0 && linia < wiersz.Lines.Length-1)
            {
                wykonajLinie();  
            }

            if (linia > wiersz.Lines.Length - 1)
            {
                krokowa.Enabled = false;
                praca.Enabled = true;
            }
            linia++;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            wiersz.Clear();
            rozkaz.Enabled = true;
            adres.Enabled = false;
            wpis.Enabled = false;
            rejestr.Enabled = false;
            adresowanie.Enabled = false;
            ok.Enabled = false;
            pomoc.Enabled = false;
            pomoc.Clear();
            praca.Enabled = true;
            krokowa.Enabled = true;
            linia = 0;
            ah = 0;
            al = 0;
            bh = 0;
            bl = 0;
            ch = 0;
            cl = 0;
            dh = 0;
            dl = 0;
            AH.Text = Convert.ToString(ah, 2).PadLeft(8, '0');
            BH.Text = Convert.ToString(bh, 2).PadLeft(8, '0');
            CH.Text = Convert.ToString(ch, 2).PadLeft(8, '0');
            DH.Text = Convert.ToString(dh, 2).PadLeft(8, '0');
            AL.Text = Convert.ToString(al, 2).PadLeft(8, '0');
            BL.Text = Convert.ToString(bl, 2).PadLeft(8, '0');
            CL.Text = Convert.ToString(cl, 2).PadLeft(8, '0');
            DL.Text = Convert.ToString(dl, 2).PadLeft(8, '0');
        }

        private void zapis_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = File.CreateText(@"C:\Users\User\Desktop\Posk projekt 1\plik.txt"))
            {
                for (linia = 0; linia < wiersz.Lines.Length; linia++)
                {
                    sw.WriteLine(wiersz.Lines[linia]);
                }
            }
        }

        private void odczyt_Click(object sender, EventArgs e)
        {
            StreamReader s = new StreamReader(@"C:\Users\User\Desktop\OSK1\plik.txt");
            pomoc.Text = pomoc.Text + s.ReadToEnd();
            wiersz.Text = pomoc.Text;
        }
    }

}