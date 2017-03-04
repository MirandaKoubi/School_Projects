using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Salvo_GUI
{
    public partial class Form1 : Form
    {
        bool hit = false;
        bool yourTurn = false;
        bool horizontal = true;
        bool battleshipPlaced = false;
        bool cruiserPlaced = false;
        bool submarinePlaced = false;
        bool destroyerPlaced = false;

        string buttonClickedToAttack = "";

        Semaphore mutex = new Semaphore(0, 1);

        Dictionary<Color, string[]> positions = new Dictionary<Color, string[]>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Ships_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Selected = (string)Ships.SelectedItem;

            switch (Selected)
            {
                case "Battleship":
                    S1.BackColor = Color.Blue;
                    S2.BackColor = Color.Blue;
                    S3.BackColor = Color.Blue;
                    S4.BackColor = Color.Blue;
                    S5.BackColor = Color.Blue;
                    break;
                case "Cruiser":
                    S1.BackColor = Color.Yellow;
                    S2.BackColor = Color.Yellow;
                    S3.BackColor = Color.Yellow;
                    S4.BackColor = Color.Yellow;
                    S5.BackColor = Color.CadetBlue;
                    break;
                case "Submarine":
                    S1.BackColor = Color.Red;
                    S2.BackColor = Color.Red;
                    S3.BackColor = Color.Red;
                    S4.BackColor = Color.CadetBlue;
                    S5.BackColor = Color.CadetBlue;
                    break;
                case "Destroyer":
                    S1.BackColor = Color.White;
                    S2.BackColor = Color.White;
                    S3.BackColor = Color.CadetBlue;
                    S4.BackColor = Color.CadetBlue;
                    S5.BackColor = Color.CadetBlue;
                    break;
            }
        }

        private void Horizontal_Click(object sender, EventArgs e)
        {
            horizontal = true;
        }

        private void Vertical_Click(object sender, EventArgs e)
        {
           horizontal = false;
        }

        private void Place_Ship_Click(object sender, EventArgs e)
        {
            
            Object selected = Ships.SelectedItem;

            if ( selected != null)
            {
                string shipSelected = selected.ToString();
                Button buttonClicked = (Button)sender;

                switch(shipSelected)
                {
                    case "Battleship":
                        if (buttonClicked.BackColor == Color.CadetBlue || buttonClicked.BackColor == Color.Blue)
                        {
                            if (horizontal)
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton[0] != 'G' && nameOfButton[0] != 'H' && nameOfButton[0] != 'I' && nameOfButton[0] != 'J')
                                {
                                    bool canBePlaced = true;

                                    for (int i = 1; i < 5; i++)
                                    {
                                        // create next button name
                                        string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + i) + nameOfButton.Substring(1);

                                        // find next button
                                        Control[] newButton = this.Controls.Find(newButtonName, true);

                                        // if a button was not found or the color of the button is not default/blue
                                        if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.Blue))
                                        {
                                            canBePlaced = false;
                                        }
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !battleshipPlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.Blue;

                                        // create string array to add to dictionary
                                        string[] locations = new string[5];
                                        locations[0] = nameOfButton;

                                        for (int i = 1; i < 5; i++)
                                        {
                                            // create next button name
                                            string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + i) + nameOfButton.Substring(1);

                                            // find next button
                                            Control[] newButton = this.Controls.Find(newButtonName, true);

                                            if (newButton != null)
                                            {
                                                // change next button color
                                                newButton[0].BackColor = Color.Blue;

                                                // add next button location to string array
                                                locations[i] = newButtonName;
                                            }
                                        }

                                        // add location to dictionary
                                        battleshipPlaced = true;
                                        positions.Add(Color.Blue, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && battleshipPlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[5];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.Blue];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 5; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }

                                        for (int i = 0; i < 4; i++)
                                        {
                                            // create next new button name
                                            string newestButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + (i + 1)) + nameOfButton.Substring(1);

                                            // find next new button
                                            Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                            // change color to blue and add location to new location string array
                                            if (newestButton.Length != 0)
                                            {
                                                newestButton[0].BackColor = Color.Blue;
                                                newLocations[i + 1] = newestButtonName;
                                            }
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.Blue;
                                        positions[Color.Blue] = newLocations;
                                    }
                                }
                            }
                            else    // Orientation set as vertical
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton.Substring(1) != "7" && nameOfButton.Substring(1) != "8" && nameOfButton.Substring(1) != "9" && nameOfButton.Substring(1) != "10")
                                {
                                    bool canBePlaced = true;

                                    for (int i = 1; i < 5; i++)
                                    {
                                        // create next button name
                                        string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + i);

                                        if (num == ":")
                                        {
                                            num = "10";
                                        }
                                            
                                        string newButtonName = nameOfButton[0] + num;

                                        // find next button
                                        Control[] newButton = this.Controls.Find(newButtonName, true);

                                        // if a button was not found or the color of the button is not default/blue
                                        if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.Blue))
                                        {
                                            canBePlaced = false;
                                        }
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !battleshipPlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.Blue;

                                        // create string array to add to dictionary
                                        string[] locations = new string[5];
                                        locations[0] = nameOfButton;

                                        for (int i = 1; i < 5; i++)
                                        {
                                            // create next button name
                                            string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + i);

                                            if (num == ":")
                                            {
                                                num = "10";
                                            }

                                            string newButtonName = nameOfButton[0] + num;

                                            // find next button
                                            Control[] newButton = this.Controls.Find(newButtonName, true);

                                            if (newButton != null)
                                            {
                                                // change next button color
                                                newButton[0].BackColor = Color.Blue;

                                                // add next button location to string array
                                                locations[i] = newButtonName;
                                            }
                                        }

                                        // add location to dictionary
                                        battleshipPlaced = true;
                                        positions.Add(Color.Blue, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && battleshipPlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[5];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.Blue];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 5; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }

                                        for (int i = 0; i < 4; i++)
                                        {
                                            // create next new button name
                                            string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + (i + 1));

                                            if (num == ":")
                                            {
                                                num = "10";
                                            }

                                            string newestButtonName = nameOfButton[0] + num;

                                            // find next new button
                                            Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                            // change color to blue and add location to new location string array
                                            if (newestButton.Length != 0)
                                            {
                                                newestButton[0].BackColor = Color.Blue;
                                                newLocations[i + 1] = newestButtonName;
                                            }
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.Blue;
                                        positions[Color.Blue] = newLocations;
                                    }
                                }
                            }
                        }
                        break;

                    case "Cruiser":
                        if (buttonClicked.BackColor == Color.CadetBlue || buttonClicked.BackColor == Color.Yellow)
                        {
                            if (horizontal)
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton[0] != 'H' && nameOfButton[0] != 'I' && nameOfButton[0] != 'J')
                                {
                                    bool canBePlaced = true;

                                    for (int i = 1; i < 4; i++)
                                    {
                                        // create next button name
                                        string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + i) + nameOfButton.Substring(1);

                                        // find next button
                                        Control[] newButton = this.Controls.Find(newButtonName, true);

                                        // if a button was not found or the color of the button is not default/blue
                                        if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.Yellow))
                                        {
                                            canBePlaced = false;
                                        }
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !cruiserPlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.Yellow;

                                        // create string array to add to dictionary
                                        string[] locations = new string[4];
                                        locations[0] = nameOfButton;

                                        for (int i = 1; i < 4; i++)
                                        {
                                            // create next button name
                                            string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + i) + nameOfButton.Substring(1);

                                            // find next button
                                            Control[] newButton = this.Controls.Find(newButtonName, true);

                                            if (newButton != null)
                                            {
                                                // change next button color
                                                newButton[0].BackColor = Color.Yellow;

                                                // add next button location to string array
                                                locations[i] = newButtonName;
                                            }
                                        }

                                        // add location to dictionary
                                        cruiserPlaced = true;
                                        positions.Add(Color.Yellow, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && cruiserPlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[4];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.Yellow];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 4; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }

                                        for (int i = 0; i < 3; i++)
                                        {
                                            // create next new button name
                                            string newestButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + (i + 1)) + nameOfButton.Substring(1);

                                            // find next new button
                                            Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                            // change color to blue and add location to new location string array
                                            if (newestButton.Length != 0)
                                            {
                                                newestButton[0].BackColor = Color.Yellow;
                                                newLocations[i + 1] = newestButtonName;
                                            }
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.Yellow;
                                        positions[Color.Yellow] = newLocations;
                                    }
                                }
                            }
                            else    // Orientation set as vertical
                            {
                                string nameOfButton = buttonClicked.Name;

                                if (nameOfButton.Substring(1) != "8" && nameOfButton.Substring(1) != "9" && nameOfButton.Substring(1) != "10")
                                {
                                    bool canBePlaced = true;

                                    for (int i = 1; i < 4; i++)
                                    {
                                        // create next button name
                                        string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + i);

                                        if (num == ":")
                                        {
                                            num = "10";
                                        }

                                        string newButtonName = nameOfButton[0] + num;

                                        // find next button
                                        Control[] newButton = this.Controls.Find(newButtonName, true);

                                        // if a button was not found or the color of the button is not default/blue
                                        if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.Yellow))
                                        {
                                            canBePlaced = false;
                                        }
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !cruiserPlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.Yellow;

                                        // create string array to add to dictionary
                                        string[] locations = new string[4];
                                        locations[0] = nameOfButton;

                                        for (int i = 1; i < 4; i++)
                                        {
                                            // create next button name
                                            string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + i);

                                            if (num == ":")
                                            {
                                                num = "10";
                                            }

                                            string newButtonName = nameOfButton[0] + num;

                                            // find next button
                                            Control[] newButton = this.Controls.Find(newButtonName, true);

                                            if (newButton != null)
                                            {
                                                // change next button color
                                                newButton[0].BackColor = Color.Yellow;

                                                // add next button location to string array
                                                locations[i] = newButtonName;
                                            }
                                        }

                                        // add location to dictionary
                                        cruiserPlaced = true;
                                        positions.Add(Color.Yellow, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && cruiserPlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[4];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.Yellow];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 4; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }

                                        for (int i = 0; i < 3; i++)
                                        {
                                            // create next new button name
                                            string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + (i + 1));

                                            if (num == ":")
                                            {
                                                num = "10";
                                            }

                                            string newestButtonName = nameOfButton[0] + num;

                                            // find next new button
                                            Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                            // change color to blue and add location to new location string array
                                            if (newestButton.Length != 0)
                                            {
                                                newestButton[0].BackColor = Color.Yellow;
                                                newLocations[i + 1] = newestButtonName;
                                            }
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.Yellow;
                                        positions[Color.Yellow] = newLocations;
                                    }
                                }
                            }
                        }
                        break;
                    case "Submarine":
                        if (buttonClicked.BackColor == Color.CadetBlue || buttonClicked.BackColor == Color.Red)
                        {
                            if (horizontal)
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton[0] != 'I' && nameOfButton[0] != 'J')
                                {
                                    bool canBePlaced = true;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        // create next button name
                                        string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + i) + nameOfButton.Substring(1);

                                        // find next button
                                        Control[] newButton = this.Controls.Find(newButtonName, true);

                                        // if a button was not found or the color of the button is not default/blue
                                        if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.Red))
                                        {
                                            canBePlaced = false;
                                        }
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !submarinePlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.Red;

                                        // create string array to add to dictionary
                                        string[] locations = new string[3];
                                        locations[0] = nameOfButton;

                                        for (int i = 1; i < 3; i++)
                                        {
                                            // create next button name
                                            string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + i) + nameOfButton.Substring(1);

                                            // find next button
                                            Control[] newButton = this.Controls.Find(newButtonName, true);

                                            if (newButton != null)
                                            {
                                                // change next button color
                                                newButton[0].BackColor = Color.Red;

                                                // add next button location to string array
                                                locations[i] = newButtonName;
                                            }
                                        }

                                        // add location to dictionary
                                        submarinePlaced = true;
                                        positions.Add(Color.Red, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && submarinePlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[3];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.Red];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 3; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }

                                        for (int i = 0; i < 2; i++)
                                        {
                                            // create next new button name
                                            string newestButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + (i + 1)) + nameOfButton.Substring(1);

                                            // find next new button
                                            Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                            // change color to blue and add location to new location string array
                                            if (newestButton.Length != 0)
                                            {
                                                newestButton[0].BackColor = Color.Red;
                                                newLocations[i + 1] = newestButtonName;
                                            }
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.Red;
                                        positions[Color.Red] = newLocations;
                                    }
                                }
                            }
                            else    // Orientation set as vertical
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton.Substring(1) != "9" && nameOfButton.Substring(1) != "10")
                                {
                                    bool canBePlaced = true;

                                    for (int i = 1; i < 3; i++)
                                    {
                                        // create next button name
                                        string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + i);

                                        if (num == ":")
                                        {
                                            num = "10";
                                        }

                                        string newButtonName = nameOfButton[0] + num;

                                        // find next button
                                        Control[] newButton = this.Controls.Find(newButtonName, true);

                                        // if a button was not found or the color of the button is not default/blue
                                        if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.Red))
                                        {
                                            canBePlaced = false;
                                        }
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !submarinePlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.Red;

                                        // create string array to add to dictionary
                                        string[] locations = new string[3];
                                        locations[0] = nameOfButton;

                                        for (int i = 1; i < 3; i++)
                                        {
                                            // create next button name
                                            string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + i);

                                            if (num == ":")
                                            {
                                                num = "10";
                                            }

                                            string newButtonName = nameOfButton[0] + num;

                                            // find next button
                                            Control[] newButton = this.Controls.Find(newButtonName, true);

                                            if (newButton != null)
                                            {
                                                // change next button color
                                                newButton[0].BackColor = Color.Red;

                                                // add next button location to string array
                                                locations[i] = newButtonName;
                                            }
                                        }

                                        // add location to dictionary
                                        submarinePlaced = true;
                                        positions.Add(Color.Red, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && submarinePlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[3];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.Red];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 3; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }

                                        for (int i = 0; i < 2; i++)
                                        {
                                            // create next new button name
                                            string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + (i + 1));

                                            if (num == ":")
                                            {
                                                num = "10";
                                            }

                                            string newestButtonName = nameOfButton[0] + num;

                                            // find next new button
                                            Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                            // change color to blue and add location to new location string array
                                            if (newestButton.Length != 0)
                                            {
                                                newestButton[0].BackColor = Color.Red;
                                                newLocations[i + 1] = newestButtonName;
                                            }
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.Red;
                                        positions[Color.Red] = newLocations;
                                    }
                                }
                            }
                        }
                        break;
                    case "Destroyer":
                        if (buttonClicked.BackColor == Color.CadetBlue || buttonClicked.BackColor == Color.White)
                        {
                            if (horizontal)
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton[0] != 'J')
                                {
                                    bool canBePlaced = true;

                                    // create next button name
                                    string newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + 1) + nameOfButton.Substring(1);

                                    // find next button
                                    Control[] newButton = this.Controls.Find(newButtonName, true);

                                    // if a button was not found or the color of the button is not default/blue
                                    if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.White))
                                    {
                                        canBePlaced = false;
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !destroyerPlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.White;

                                        // create string array to add to dictionary
                                        string[] locations = new string[2];
                                        locations[0] = nameOfButton;
                                     
                                        // create next button name
                                        newButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + 1) + nameOfButton.Substring(1);

                                        // find next button
                                        newButton = this.Controls.Find(newButtonName, true);

                                        if (newButton != null)
                                        {
                                            // change next button color
                                            newButton[0].BackColor = Color.White;

                                            // add next button location to string array
                                            locations[1] = newButtonName;
                                        }
                                        
                                        // add location to dictionary
                                        destroyerPlaced = true;
                                        positions.Add(Color.White, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && destroyerPlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[2];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.White];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 2; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }
                                    
                                        // create next new button name
                                        string newestButtonName = Char.ConvertFromUtf32(((char)nameOfButton[0]) + 1) + nameOfButton.Substring(1);

                                        // find next new button
                                        Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                        // change color to blue and add location to new location string array
                                        if (newestButton.Length != 0)
                                        {
                                            newestButton[0].BackColor = Color.White;
                                            newLocations[1] = newestButtonName;
                                        }
                                        
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.White;
                                        positions[Color.White] = newLocations;
                                    }
                                }
                            }
                            else    // Orientation set as vertical
                            {
                                string nameOfButton = buttonClicked.Name;
                                if (nameOfButton.Substring(1) != "10")
                                {
                                    bool canBePlaced = true;
                                    
                                    // create next button name
                                    string num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + 1);

                                    if (num == ":")
                                    {
                                        num = "10";
                                    }

                                    string newButtonName = nameOfButton[0] + num;

                                    // find next button
                                    Control[] newButton = this.Controls.Find(newButtonName, true);

                                    // if a button was not found or the color of the button is not default/blue
                                    if (newButton.Length == 0 || (newButton[0].BackColor != Color.CadetBlue && newButton[0].BackColor != Color.White))
                                    {
                                        canBePlaced = false;
                                    }

                                    // if okay to place and first time placing ship
                                    if (canBePlaced && !destroyerPlaced)
                                    {
                                        // change current button color
                                        buttonClicked.BackColor = Color.White;

                                        // create string array to add to dictionary
                                        string[] locations = new string[2];
                                        locations[0] = nameOfButton;
                                        
                                        // create next button name
                                        num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + 1);

                                        if (num == ":")
                                        {
                                            num = "10";
                                        }

                                        newButtonName = nameOfButton[0] + num;

                                        // find next button
                                        newButton = this.Controls.Find(newButtonName, true);

                                        if (newButton != null)
                                        {
                                            // change next button color
                                            newButton[0].BackColor = Color.White;

                                            // add next button location to string array
                                            locations[1] = newButtonName;
                                        }
                                        
                                        // add location to dictionary
                                        destroyerPlaced = true;
                                        positions.Add(Color.White, locations);
                                    }
                                    // if ship has already been placed
                                    else if (canBePlaced && destroyerPlaced)
                                    {
                                        // create string array for new location of ship
                                        string[] newLocations = new string[2];
                                        newLocations[0] = nameOfButton;

                                        // grab location of old placement
                                        string[] locations = positions[Color.White];

                                        // find old buttons and change color to default
                                        for (int i = 0; i < 2; i++)
                                        {
                                            Control[] oldButton = this.Controls.Find(locations[i], true);
                                            oldButton[0].BackColor = Color.CadetBlue;
                                        }


                                        // create next new button name
                                        num = Char.ConvertFromUtf32(((char)nameOfButton[1]) + 1);

                                        if (num == ":")
                                        {
                                            num = "10";
                                        }

                                        string newestButtonName = nameOfButton[0] + num;

                                        // find next new button
                                        Control[] newestButton = this.Controls.Find(newestButtonName, true);

                                        // change color to blue and add location to new location string array
                                        if (newestButton.Length != 0)
                                        {
                                            newestButton[0].BackColor = Color.White;
                                            newLocations[1] = newestButtonName;
                                        }
                                        // change current location color and update dictionary with new location
                                        buttonClicked.BackColor = Color.White;
                                        positions[Color.White] = newLocations;
                                    }
                                }
                            }
                        }

                        break;
                }
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private String attackReceived;

        private void HostGame_Click(object sender, EventArgs e)
        {
            string address = GetLocalIPAddress();
            MessageBox.Show("Your IP to connect to is: " + address);

            yourTurn = true;

            TcpListener listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            client = listener.AcceptTcpClient();
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync();
        }

        private void JoinGame_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(ipAddress.Text), 51111);

            try
            {
                client.Connect(IpEnd);
                if (client.Connected)
                {
                    writer = new StreamWriter(client.GetStream());
                    reader = new StreamReader(client.GetStream());
                    writer.AutoFlush = true;

                    backgroundWorker1.RunWorkerAsync();  // start receiving data in the background
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    if (yourTurn)
                    {
                        mutex.WaitOne();

                        string message = reader.ReadLine();

                        if (message != null)
                        {
                            if (message == "hit")
                            {
                                hit = true;
                            }
                            else if (message == "miss")
                            {
                                hit = false;
                            }

                            Control[] attackLocation = this.Controls.Find(buttonClickedToAttack, true);

                            if (hit)
                            {
                                attackLocation[0].BackColor = Color.Green;
                            }
                            else
                            {
                                attackLocation[0].BackColor = Color.Black;
                            }

                            buttonClickedToAttack = "";
                            messageSender("switch turns");  
                        }

                        yourTurn = false;
                    }
                    else
                    {
                        attackReceived = reader.ReadLine();

                        Control[] attackLocation = this.Controls.Find(attackReceived, true);

                        if (attackLocation.Length != 0)
                        {
                            hit = false;
                            if (attackLocation[0].BackColor != Color.CadetBlue && attackLocation[0].BackColor != Color.Green && attackLocation[0].BackColor != Color.Black)
                            {
                                hit = true;
                            }

                            attackLocation[0].Invoke(new MethodInvoker(delegate()
                            {
                                if (hit)
                                {
                                    attackLocation[0].BackColor = Color.Green;
                                    messageSender("hit");
                                }
                                else
                                {
                                    attackLocation[0].BackColor = Color.Black;
                                    messageSender("miss");
                                }
                            }));
                        }

                        if (attackReceived == "switch turns")
                        {
                            yourTurn = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Attack_Opponent_Click(object sender, EventArgs e)
        {
            if (yourTurn)
            {
                Button buttonClicked = (Button)sender;
                buttonClickedToAttack = buttonClicked.Name;
                messageSender(buttonClicked.Name.Substring(1));
                mutex.Release();
            }
        }

        private async void messageSender(String toSend)
        {
            try
            {
                await writer.WriteLineAsync(toSend);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }
    }
}
