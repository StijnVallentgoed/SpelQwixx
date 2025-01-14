using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpelQwixx
{
    public partial class Form1 : Form
    {
        private const int speelkaartRijen = 4;
        private const int speelkaartKolommen = 11;
        private const int dobbelstenenRij = 1;
        private const int dobbelstenenKolommen = 6;

        private const int aantalSpelers = 4;

        private int huidigeSpelerIndex = 0;

        private Button[][,] spelersSpeelvelden;
        private Button[,] huidigSpeelveld;

        private Button losseDobbelsteenButton; // Button voor de losse dobbelsteen

        private Random rand = new Random(); // Random object om willekeurige getallen te genereren

        public Form1()
        {
            InitializeComponent();
            MaakSpeelvelden();
            MaakDobbelstenen();
            UpdatePuntentelling();
        }

        private void MaakSpeelvelden()
        {
            int totaalKleineKnoppen = 4; // Totaal aantal kleine knoppen verdeeld over alle spelers
            int knoppenPerSpeler = totaalKleineKnoppen / aantalSpelers; // Aantal kleine knoppen per speler

            spelersSpeelvelden = new Button[aantalSpelers][,];
            for (int spelerIndex = 0; spelerIndex < aantalSpelers; spelerIndex++)
            {
                spelersSpeelvelden[spelerIndex] = new Button[speelkaartRijen, speelkaartKolommen + 1]; // +1 voor extra knop
                for (int rij = 0; rij < speelkaartRijen; rij++)
                {
                    int xOffset = spelerIndex % 2 == 0 ? 0 : 15 * 50;
                    int yOffset = spelerIndex < 2 ? 0 : (speelkaartRijen + 2) * 50;

                    // Aanpassing van de yOffset voor speler 3 en 4
                    if (spelerIndex >= 2)
                    {
                        yOffset += 100; // Verplaats de speelvelden 100 pixels naar beneden voor speler 3 en 4
                    }

                    for (int kolom = 0; kolom < speelkaartKolommen; kolom++)
                    {
                        var button = new Button();
                        button.Size = new Size(50, 50);
                        button.Click += Button_Click;
                        button.ForeColor = Color.Black;

                        if (rij == 0)
                            button.BackColor = Color.Red;
                        else if (rij == 1)
                            button.BackColor = Color.Yellow;
                        else if (rij == 2)
                            button.BackColor = Color.Green;
                        else if (rij == 3)
                            button.BackColor = Color.Blue;

                        button.Text = (rij < 2) ? (kolom + 2).ToString() : (12 - kolom).ToString();

                        button.Location = new Point(kolom * 50 + xOffset, rij * 50 + yOffset);

                        Controls.Add(button);
                        spelersSpeelvelden[spelerIndex][rij, kolom] = button;
                    }

                    // Voeg de extra knop toe aan het einde van elke rij
                    var extraButton = new Button();
                    extraButton.Size = new Size(50, 50);
                    extraButton.BackColor = Color.Gray; // Hier kun je de achtergrondkleur van de extra knop instellen
                    extraButton.Location = new Point(speelkaartKolommen * 50 + xOffset, rij * 50 + yOffset);
                    extraButton.Click += ExtraButton_Click; // Voeg hier de event handler toe
                    Controls.Add(extraButton);
                    spelersSpeelvelden[spelerIndex][rij, speelkaartKolommen] = extraButton;

                    // Voeg de extra kleine knoppen toe
                    for (int extraButtonIndex = 0; extraButtonIndex < knoppenPerSpeler; extraButtonIndex++)
                    {
                        var extraSmallButton = new Button();
                        extraSmallButton.Size = new Size(50, 50);
                        extraSmallButton.BackColor = Color.Orange; // Achtergrondkleur van de knop
                        extraSmallButton.Location = new Point(speelkaartKolommen * 50 + extraButtonIndex * 25 + xOffset + 50, rij * 50 + yOffset + 30); // Aangepaste locatie met extra 50 pixels naar rechts en 30 pixels omhoog
                        extraSmallButton.Click += ExtraSmallButton_Click; // Voeg de event handler toe
                        extraSmallButton.Tag = spelerIndex; // Voeg een tag toe om de spelerindex bij te houden
                        Controls.Add(extraSmallButton);
                    }
                }

                // Voeg de tekst toe onder elk speelveld
                var playerNameLabel = new Label();
                playerNameLabel.Text = "Player " + (spelerIndex + 1);
                playerNameLabel.AutoSize = true; // Zorg ervoor dat de grootte van de label automatisch wordt aangepast aan de tekst
                int labelX = 15 + spelerIndex % 2 * (15 * 50);
                int labelY = (speelkaartRijen + 1) * 50 + (spelerIndex >= 2 ? 100 : 0); // Bereken de juiste verticale positie
                playerNameLabel.Location = new Point(labelX, labelY);
                Controls.Add(playerNameLabel);
            }
            huidigSpeelveld = spelersSpeelvelden[huidigeSpelerIndex];
        }



        private void MaakDobbelstenen()
        {
            // Maak 6 dobbelstenen in de gewenste kleuren en voeg ze toe aan de form
            Color[] kleuren = { Color.White, Color.White, Color.Red, Color.Yellow, Color.Green, Color.Blue };
            int totalDiceWidth = dobbelstenenKolommen * 70; // Totale breedte van alle dobbelstenen
            int totalDiceHeight = 70; // Totale hoogte van de dobbelstenen (één rij)
            int startX = (Width - totalDiceWidth) / 2 + 120; // X-positie om de dobbelstenen iets meer naar rechts in het midden van het formulier te plaatsen
            int startY = (Height - totalDiceHeight) / 2; // Y-positie om de dobbelstenen in het midden van het formulier te plaatsen

            for (int i = 0; i < dobbelstenenKolommen; i++)
            {
                var dobbelsteenButton = new Button();
                dobbelsteenButton.Size = new Size(50, 50);
                dobbelsteenButton.BackColor = kleuren[i];
                dobbelsteenButton.Click += btnDobbelsteen_Click;
                dobbelsteenButton.Text = "0"; // Startwaarde
                dobbelsteenButton.Location = new Point(startX + i * 70, startY); // Aanpassen locatie
                dobbelsteenButton.Tag = "dobbelsteen"; // Voeg een tag toe om de dobbelsteenknoppen te identificeren
                Controls.Add(dobbelsteenButton);
            }
        }





        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (clickedButton == losseDobbelsteenButton)
            {
                Random rand = new Random();
                int randomRij = rand.Next(0, speelkaartRijen);
                losseDobbelsteenButton.Location = new Point(rand.Next(0, speelkaartKolommen) * 50, randomRij * 50);
                UpdatePuntentelling(); // Update de puntentelling na elke zet
                return;
            }

            if (clickedButton.BackColor == Color.Gray)
            {
                return;
            }

            if (!clickedButton.Enabled)
            {
                return;
            }

            bool buttonFound = false;
            for (int i = 0; i < huidigSpeelveld.GetLength(0); i++)
            {
                for (int j = 0; j < huidigSpeelveld.GetLength(1); j++)
                {
                    if (huidigSpeelveld[i, j] == clickedButton)
                    {
                        buttonFound = true;
                        break;
                    }
                }
                if (buttonFound)
                {
                    break;
                }
            }

            if (!buttonFound)
            {
                return;
            }

            DrawCross(clickedButton);
            clickedButton.Enabled = false;

            int clickedRow = GetButtonRow(clickedButton);
            int clickedColumn = GetButtonColumn(clickedButton);

            for (int i = 0; i < clickedColumn; i++)
            {
                Button button = huidigSpeelveld[clickedRow, i];
                if (button.Enabled)
                {
                    button.Enabled = false;
                    button.Text = "-";
                }
            }

            // Controleer of een kleur gesloten kan worden
            if (KleurKanWordenGesloten(clickedRow))
            {
                // Controleer of minimaal 5 andere knoppen zijn weggestreept in dezelfde rij
                int weggestreepteKnoppenCount = 0;
                for (int i = 0; i < speelkaartKolommen - 1; i++) // Laatste knop (extraButton) niet meerekenen
                {
                    if (!huidigSpeelveld[clickedRow, i].Enabled)
                    {
                        weggestreepteKnoppenCount++;
                    }
                }

                if (weggestreepteKnoppenCount >= 5)
                {
                    // Activeer de extra knop (laatste knop in de rij) zodat deze klikbaar wordt
                    huidigSpeelveld[clickedRow, speelkaartKolommen - 1].Enabled = true;
                }
            }

            UpdatePuntentelling(); // Update de puntentelling na elke zet
        }

        private void ExtraSmallButton_Click(object sender, EventArgs e)
        {
            Button extraSmallButton = (Button)sender;
            int subtractPoints = 5; // Punten die moeten worden afgetrokken

            // Zoek de spelerindex die aan deze knop is gekoppeld via de tag
            int playerIndex = (int)extraSmallButton.Tag;

            // Als de spelerindex niet overeenkomt met de huidige speler, return dan zonder actie te ondernemen
            if (playerIndex != huidigeSpelerIndex)
                return;

            // Zoek het label dat de totale score weergeeft voor de huidige speler
            Label totaalPuntenLabel = null;
            foreach (Control control in Controls)
            {
                if (control is Label label && label.Tag != null && label.Tag.ToString() == $"totaalPuntenLabel_{huidigeSpelerIndex}")
                {
                    totaalPuntenLabel = label;
                    break;
                }
            }

            // Trek de punten af van de totale punten
            if (totaalPuntenLabel != null)
            {
                int totaalPunten = int.Parse(totaalPuntenLabel.Text.Split(' ')[1]); // Haal de huidige totale punten op uit de tekst van het label
                totaalPunten -= subtractPoints; // Trek de punten af
                totaalPuntenLabel.Text = $"Totaal: {totaalPunten}"; // Werk de tekst van het label bij met de nieuwe totale puntenwaarde
            }

            // Schakel de knop uit zodat deze niet opnieuw kan worden aangekruist
            extraSmallButton.Enabled = false;
        }


        private void ExtraButton_Click(object sender, EventArgs e)
        {
            Button extraButton = (Button)sender;

            // Zoek de rij waarop is geklikt
            int clickedRow = -1;
            int currentPlayerIndex = -1;
            Color clickedColor = Color.Gray; // Houd de kleur van de verwijderde rij bij
            for (int i = 0; i < aantalSpelers; i++)
            {
                for (int j = 0; j < speelkaartRijen; j++)
                {
                    for (int k = 0; k < speelkaartKolommen + 1; k++) // +1 voor de extra knop
                    {
                        if (spelersSpeelvelden[i][j, k] == extraButton)
                        {
                            clickedRow = j;
                            currentPlayerIndex = i;
                            clickedColor = spelersSpeelvelden[i][j, 0].BackColor; // Neem de kleur van de verwijderde rij
                            break;
                        }
                    }
                    if (clickedRow != -1)
                    {
                        break;
                    }
                }
                if (clickedRow != -1)
                {
                    break;
                }
            }

            // Controleer of de rij binnen de verwachte rijen ligt
            if (clickedRow < 0 || clickedRow >= speelkaartRijen)
            {
                MessageBox.Show("Ongeldige rij!");
                return;
            }

            // Controleer of ten minste 5 knoppen met een "X" in dezelfde rij zijn aangeklikt voor de huidige speler
            int aangeklikteKnoppenCount = 0;
            for (int i = 0; i < speelkaartKolommen; i++)
            {
                if (spelersSpeelvelden[currentPlayerIndex][clickedRow, i].Text == "X")
                {
                    aangeklikteKnoppenCount++;
                }
            }

            if (aangeklikteKnoppenCount >= 5)
            {
                // Verwijder de kleur van de rij voor alle spelers
                for (int i = 0; i < aantalSpelers; i++)
                {
                    for (int j = 0; j < speelkaartKolommen; j++)
                    {
                        spelersSpeelvelden[i][clickedRow, j].BackColor = Color.Gray;
                    }
                }

                // Schakel de extra knop uit voor alle spelers
                for (int i = 0; i < aantalSpelers; i++)
                {
                    spelersSpeelvelden[i][clickedRow, speelkaartKolommen].Enabled = false;
                }

                // Verwijder de overeenkomstige dobbelsteen
                VerwijderDobbelsteen(clickedColor);
            }
            else
            {
                MessageBox.Show("Je moet ten minste 5 knoppen met een 'X' in dezelfde rij hebben aangeklikt om de kleur te verwijderen!");
            }

            UpdatePuntentelling();
        }


        private bool KleurKanWordenGesloten(int rij)
        {
            // Controleer of het meest rechtse getal van een rij weggestreept is
            if (!huidigSpeelveld[rij, speelkaartKolommen - 2].Enabled)
            {
                // Controleer of minimaal 5 andere getallen weggestreept zijn in dezelfde rij
                int weggestreepteKnoppenCount = 0;
                for (int i = 0; i < speelkaartKolommen - 2; i++) // Laatste knop (extraButton) niet meerekenen
                {
                    if (!huidigSpeelveld[rij, i].Enabled)
                    {
                        weggestreepteKnoppenCount++;
                    }
                }

                return weggestreepteKnoppenCount >= 5;
            }

            return false;
        }


        private void VerwijderDobbelsteen(Color kleur)
        {
            foreach (Control control in Controls)
            {
                if (control is Button dobbelsteenButton && dobbelsteenButton.Tag != null && dobbelsteenButton.Tag.ToString() == "dobbelsteen" && dobbelsteenButton.BackColor == kleur)
                {
                    Controls.Remove(dobbelsteenButton);
                    break; // Omdat er maar één dobbelsteen van elke kleur is, hoeven we niet verder te zoeken
                }
            }
        }
        private int BerekenPuntenVoorRij(Button[,] speelveld, int rij)
        {
            int punten = 0;
            int kruisjes = 0;

            for (int kolom = 0; kolom < speelkaartKolommen; kolom++)
            {
                if (speelveld[rij, kolom].Text == "X")
                {
                    kruisjes++;
                }
            }

            // Bereken punten volgens de regels
            switch (kruisjes)
            {
                case 1:
                    punten = 1;
                    break;
                case 2:
                    punten = 3;
                    break;
                case 3:
                    punten = 6;
                    break;
                case 4:
                    punten = 10;
                    break;
                case 5:
                    punten = 15;
                    break;
                case 6:
                    punten = 21;
                    break;
                case 7:
                    punten = 28;
                    break;
                case 8:
                    punten = 36;
                    break;
                case 9:
                    punten = 45;
                    break;
                case 10:
                    punten = 55;
                    break;
                case 11:
                    punten = 66;
                    break;
                case 12:
                    punten = 78;
                    break;
                default:
                    punten = 0;
                    break;
            }

            return punten;
        }

        private void UpdatePuntentelling()
        {
            for (int spelerIndex = 0; spelerIndex < aantalSpelers; spelerIndex++)
            {
                int totaalPunten = 0;

                // Bereken totaalpunten over alle rijen
                for (int rij = 0; rij < speelkaartRijen; rij++)
                {
                    int punten = BerekenPuntenVoorRij(spelersSpeelvelden[spelerIndex], rij);
                    totaalPunten += punten;
                }

                // Zoek het label dat de totale score moet weergeven voor de huidige speler
                Label totaalPuntenLabel = null;
                foreach (Control control in Controls)
                {
                    if (control is Label label && label.Tag != null && label.Tag.ToString() == $"totaalPuntenLabel_{spelerIndex}")
                    {
                        totaalPuntenLabel = label;
                        break;
                    }
                }

                // Als het label niet gevonden is, maak het dan aan en voeg het toe aan de form
                if (totaalPuntenLabel == null)
                {
                    totaalPuntenLabel = new Label();
                    totaalPuntenLabel.AutoSize = true;
                    totaalPuntenLabel.Tag = $"totaalPuntenLabel_{spelerIndex}";

                    // Bereken de locatie van het label op basis van de locatie van de knoppen in het speelveld
                    int xPos = spelersSpeelvelden[spelerIndex][0, 0].Location.X;
                    int yPos = spelersSpeelvelden[spelerIndex][speelkaartRijen - 1, 0].Location.Y + 70; // Onderste rij knoppen plus een kleine marge
                    totaalPuntenLabel.Location = new Point(xPos, yPos);

                    Controls.Add(totaalPuntenLabel);
                }

                // Werk de tekst van het label bij met de totale puntenwaarde
                totaalPuntenLabel.Text = $"Totaal: {totaalPunten}";
            }
        }













        private int GetButtonRow(Button button)
        {
            for (int row = 0; row < speelkaartRijen; row++)
            {
                for (int col = 0; col < speelkaartKolommen; col++)
                {
                    if (huidigSpeelveld[row, col] == button)
                    {
                        return row;
                    }
                }
            }
            return -1;
        }

        private int GetButtonColumn(Button button)
        {
            for (int row = 0; row < speelkaartRijen; row++)
            {
                for (int col = 0; col < speelkaartKolommen; col++)
                {
                    if (huidigSpeelveld[row, col] == button)
                    {
                        return col;
                    }
                }
            }
            return -1;
        }

        private void DrawCross(Button button)
        {
            button.Text = "X";
            button.Font = new Font(button.Font.FontFamily, 20);
            button.Enabled = false;
        }

        private void btnDobbelsteen_Click(object sender, EventArgs e)
        {
            Random rand = new Random();

            // Loop alleen door de dobbelsteenknoppen en stel een willekeurig getal in
            foreach (Control control in Controls)
            {
                if (control is Button dobbelsteenButton && dobbelsteenButton.Tag != null && dobbelsteenButton.Tag.ToString() == "dobbelsteen")
                {
                    // Stel een willekeurig getal tussen 1 en 6 in
                    int randomGetal = rand.Next(1, 7);
                    dobbelsteenButton.Text = randomGetal.ToString();
                }
            }
        }




        private void btnVolgendebeurt_Click(object sender, EventArgs e)
        {
            huidigeSpelerIndex++;
            if (huidigeSpelerIndex >= aantalSpelers)
            {
                huidigeSpelerIndex = 0;
            }
            huidigSpeelveld = spelersSpeelvelden[huidigeSpelerIndex];
            MessageBox.Show($"Het is nu de beurt aan Speler {huidigeSpelerIndex + 1}.");

            UpdatePuntentelling();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Lege methode, kan eventueel later worden geïmplementeerd
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            // Implementeer de gewenste functionaliteit voor het bijwerken van de tekeningen op de PictureBox
        }

        private void picCanvas_Click(object sender, EventArgs e)
        {

        }
    }
}