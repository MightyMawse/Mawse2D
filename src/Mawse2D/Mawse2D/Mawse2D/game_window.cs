//=========================================================================
//Author: Mawse Software
//
//Modification: Zombie RPG Game Development Build
//
//Mawse_2D Engine - 2021
//=========================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mawse2D
{
    public partial class game_window : Form
    {
        public game_window()
        {
            InitializeComponent();

            //Initialize KeyDown
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Mawse2D_KeyStroke);
            this.KeyPreview = true;

            //Update function
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(Mawse2D_Update);
            timer.Enabled = true;
        }

        #region Mawse2D_Frame_Var

        //game window name
        public static string gameWindowName = "Mawse2D";

        //Game title
        public static string gameTitle = "Mawse2D";

        //Build Version
        public static string buildVer = "v0.3";

        //dev map
        public string[] dev_map = {
            "0000000000000000000",
            "000P000000000000000",
            "0000000000000000000",
            "000000000Z000000000",
            "0000000000000000000",
            "0000000----00000000",
            "0000000000000000000",
            "0000000001111100000",
            "0000000000000000000",
            "0000000000000000000",
            "0000000000000000000" };

        public string OpenmapName = "";
        public bool mapSet = false;

        //Input types for key press
        public enum INPUT
        {
            LEFT, RIGHT, UP, DOWN
        }
        //Assertion Types
        public enum AssertType
        {
            WARNING, ERROR, FATAL_ERROR, INF
        }
        //Control creation types
        public enum ControlType
        {
            CTRL, BTN, PB, LBL
        }
        

        //npc count
        public static int npc_count = 0;

        //npc list
        public static List<CNPCMovement> movementList = new List<CNPCMovement>();


        //loaded map start pos
        public static Point map_startPos;

        //contains all dynamically loaded ui elements
        public static List<Control> dynamic_ui_List = new List<Control>();
        #endregion


        #region Engine Core Functions
        //==================================================
        //game_window_Load
        //Game Window Init
        //==================================================
        private void game_window_Load(object sender, EventArgs e)
        {
            if (Mawse2D_Init())
            {
                
            }
        }




        //==================================================
        //Mawse2D_KeyStroke
        //Gets keystrokes
        //==================================================
        public void Mawse2D_KeyStroke(object sender, KeyEventArgs e)
        {

            //Player Input Conversions
            if (e.KeyCode == Keys.Left)
                PlayerMovement(INPUT.LEFT);
            else if (e.KeyCode == Keys.Right)
                PlayerMovement(INPUT.RIGHT);
            else if (e.KeyCode == Keys.Up)
                PlayerMovement(INPUT.UP);
            else if (e.KeyCode == Keys.Down)
                PlayerMovement(INPUT.DOWN);

            //Open dev console
            if(e.KeyCode == Keys.F1)
            {
                Mawse2D.Console console = new Console();
                console.Show();
            }
        }



        public void Mawse2D_Update(object sender, EventArgs e)
        {

            Update_Debug();
            //Mawse2D_Assert(AssertType.INF, "Update Ping"); //for debug purposes

            //Check to see if npc has reached target
            for(int i = 0; i < movementList.Count; i++)
            {
                Control npc = Fetch("npc" + i);//get current npc

                if (npc != null)
                {
                    if(movementList[i].moveDir == INPUT.LEFT)
                    {
                        if (npc.Location.X <= npc.Location.X - movementList[i].distance)
                            NPCMovementGenerator(i);
                    }
                    else if(movementList[i].moveDir == INPUT.RIGHT)
                    {
                        if (npc.Location.X >= npc.Location.X + movementList[i].distance)
                            NPCMovementGenerator(i);
                    }
                    else
                    {
                        NPCMovement(i, movementList[i]);
                    }

                    NPCMovement(i, movementList[i]);
                }
                else
                    Mawse2D_Assert(AssertType.WARNING, "npc not found");
                
            }
        }



        //==================================================
        //Mawse2D_Init
        //Engine Init, Fetches Data from GameInfo
        //==================================================
        public bool Mawse2D_Init()
        {
            string line;
            System.IO.StreamReader gameInfo = new System.IO.StreamReader(@"C:\Mawse2D\base\gameinfo.txt");

            string str_menuCondition = "";
            //Get game title from gameinfo

            while ((line = gameInfo.ReadLine()) != null)
            {
                //Set Game Name
                if (line.Contains("game"))
                {
                    gameWindowName = null;
                    for (int i = 5; i < line.Length; i++)
                    {
                        gameWindowName += line[i]; //write game name
                    }
                }

                //Check for menu
                if (line.Contains("menu"))
                {
                    for (int a = 5; a < line.Length; a++)
                    {
                        str_menuCondition += line[a];//Get state of menu bool
                    }

                    if (Mawse2D_stob(str_menuCondition))
                        Mawse2D_GUI("main_menu");

                }

                //Set game title
                if (line.Contains("title"))
                {
                    string gameTitleBuffer = "";

                    gameTitle = null;
                    for (int b = 6; b < line.Length; b++)
                    {
                        gameTitleBuffer += line[b];
                    }

                    if (gameTitleBuffer != null)
                        gameTitle = gameTitleBuffer;
                    else //If title = null, then set to default
                        gameTitle = "Mawse2D";
                }
            }
            this.Text = gameWindowName; //Set gameName

            return true;
        }



        //==================================================
        //Mawse2D_stob
        //String to Bool
        //==================================================
        public bool Mawse2D_stob(string str)
        {
            if (str.ToLower() == "true")
                return true;
            else if (str.ToLower() == "false")
                return false;
            else
                Mawse2D_Assert(AssertType.ERROR, "Mawse2D_stob(); - Invalid Type");

            return false;
        }


        //==================================================
        //Mawse2D_Assert
        //Assertion Handler
        //==================================================
        public void Mawse2D_Assert(AssertType type, string msg)
        {
            switch (type)
            {
                case AssertType.WARNING:
                    {
                        MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                case AssertType.ERROR:
                    {
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                case AssertType.FATAL_ERROR:
                    {
                        MessageBox.Show(msg, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case AssertType.INF:
                    {
                        MessageBox.Show(msg, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
            }
        }


        //==================================================
        //Mawse2D_WorldLoader
        //Loads Maps
        //==================================================
        public void Mawse2D_WorldLoader(string[] map_)
        {
            mapSet = false;
            Mawse2D_GUI("Clear");

            //Load Skybox
            if (map_ != dev_map)
                Mawse2D_GUI("skybox");

            //wipe npc movement list
            movementList.Clear();
            

            if(map_ != null)
            {
                for(int y = 0; y < 11; y++)
                {//Vertical

                    for(int x = 0; x < 19; x++)
                    {//Horizontal

                        if(map_[y][x] == '1')
                        {
                            //Create Wall
                            PictureBox wall = new PictureBox();

                            wall.Location = new Point(50 * x, 50 * y);

                            wall.Name = "Wall";
                            wall.Size = new Size(50, 50);
                            wall.BackColor = Color.Gray;
                            wall.BringToFront();
                            wall.BorderStyle = BorderStyle.Fixed3D;

                            Mawse2D_TextureAssign(wall, '1');

                            this.Controls.Add(wall);
                        }
                        else if(map_[y][x] == 'P')
                        {
                            //Create Player
                            PictureBox Player = new PictureBox();

                            Player.Location = new Point(50 * x, 50 * y);
                            map_startPos = Player.Location;//save position

                            Player.Name = "Player";
                            Player.Size = new Size(50, 50);
                            Player.BringToFront();
                            Player.BorderStyle = BorderStyle.None;
                            Player.BackgroundImageLayout = ImageLayout.Stretch;

                            this.Controls.Add(Player);
                            Mawse2D_GUI("player");
                        }
                        else if(map_[y][x] == 'W')
                        {
                            //Create Weapon
                            newItem(new CControlInfo("weapon", null, new Size(50, 50), new Point(50 * x, 50 * y)), ControlType.PB).BringToFront();
                        }
                        else if (map_[y][x] == 'Z')
                        {
                            //Create NPC
                            Control control;
                            control = newItem(new CControlInfo("npc" + npc_count.ToString(), null, new Size(50, 50), new Point(50 * x, 50 * y)), ControlType.PB);
                            control.BringToFront();
                            //put index position on end of name

                            movementList.Add(NPCMovementGenerator(npc_count));//add movement info which can be accessed by index stored in npc name
                            npc_count++;
                        }
                        else if (map_[y][x] == 'U')
                        {
                            //Create Prop
                            Control prop = newItem(new CControlInfo("prop", null, new Size(50, 50), new Point(50 * x, 50 * y)), ControlType.PB);
                            prop.BringToFront();
                            (prop as PictureBox).BorderStyle = BorderStyle.None;
                            prop.BackgroundImageLayout = ImageLayout.Stretch;
                            Mawse2D_TextureAssign(prop, 'U');
                            prop.BackColor = Color.LightGray;
                        }
                        else if(map_[y][x] == '-')
                        {
                            //Create No collide
                            Control nocol = newItem(new CControlInfo("nocol", null, new Size(50, 50), new Point(50 * x, 50 * y)), ControlType.PB);

                            (nocol as PictureBox).BorderStyle = BorderStyle.None;
                            nocol.SendToBack();
                            nocol.BackColor = Color.LightGray;
                            Mawse2D_TextureAssign(nocol, '-');

                        }
                        else if (map_[y][x] == 'T')//ONLY FOR DEMO
                        {
                            //Tree
                            Control tree = newItem(new CControlInfo("tree", null, new Size(50, 50), new Point(50 * x, 50 * y)), ControlType.PB);

                            (tree as PictureBox).BorderStyle = BorderStyle.None;
                            tree.BringToFront();
                            tree.BackColor = Color.Transparent;
                            tree.BackgroundImageLayout = ImageLayout.Stretch;
                            tree.BackgroundImage = new Bitmap(@"C:\Mawse2D\base\Material\world\tree.png");
                        }
                        else if(map_[y][x] == '0')
                        {
                            //Void
                            foreach(Control control in this.Controls)
                            {
                                if(control.Location == new Point(50 * x, 50 * y))
                                {
                                    control.Dispose();
                                }
                            }
                        }


                    }
                }
            }
        }


        //==================================================
        //Mawse2D_TextureLoader
        //Checks textureData and returns true if there is data for caller
        //==================================================
        public string Mawse2D_TextureLoader(char senderchar)
        {
            System.IO.StreamReader mapFile = null;

            try
            {
                mapFile = new System.IO.StreamReader(@"C:\Mawse2D\base\Maps\textureData\" + OpenmapName + ".td");

                string line;
                int count = 0;//how many lines
                while ((line = mapFile.ReadLine()) != null)
                    count++;

                string path = "";
                for (int i = 1; i < count + 1; i++)
                {
                    if (FileReader(@"C:\Mawse2D\base\Maps\textureData\" + OpenmapName + ".td", i)[0] == senderchar)//does the first char of the line = senderchar
                    {
                        for(int j = 1; j < FileReader(@"C:\Mawse2D\base\Maps\textureData\" + OpenmapName + ".td", i).Length; j++)
                        {
                            path += FileReader(@"C:\Mawse2D\base\Maps\textureData\" + OpenmapName + ".td", i)[j];
                        }
                        return path;
                    }
                }
            }
            catch
            {
                Mawse2D_Assert(AssertType.ERROR, "Unable to access texture data");
            }
                        
            return null;
        }

        //==================================================
        //Mawse2D_TextureAssign
        //Assigns textures to object from textureData
        //==================================================
        public void Mawse2D_TextureAssign(Control obj, char senderchar)
        {
            try
            {
                string path = @"" + Mawse2D_TextureLoader(senderchar);
                Bitmap bmp = new Bitmap(path);

                obj.BackgroundImage = bmp;
                obj.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch
            {
                Mawse2D_Assert(AssertType.ERROR, "Could not load texture\n");
            }
        }


        //==================================================
        //Mawse2D_MapFetcher
        //Called by conVars, looks in map file directory
        //==================================================
        public void Mawse2D_MapFetcher()
        {
            //Open Map

            try
            {
                if (OpenmapName != "devmap")
                {
                    string line;
                    System.IO.StreamReader map = null;

                    map = new System.IO.StreamReader(@"C:\Mawse2D\base\Maps\" + OpenmapName + ".lvl");

                    int count = 0;
                    string[] newMap = new string[12];

                    while ((line = map.ReadLine()) != null)
                    {
                        newMap[count] = line;//set each line of new map string

                        count++;
                    }

                    Mawse2D_WorldLoader(newMap);
                }
                else
                    Mawse2D_WorldLoader(dev_map);
                
            }
            catch(InvalidExpressionException e)
            {
                Mawse2D_Assert(AssertType.ERROR, "Cannot Load Map File" + e.Message);
            }

        }



        //==================================================
        //Mawse2D_ConVar
        //Called when a conVar is executed
        //==================================================
        public void Mawse2D_ConVar(ConVar conVar)
        {
            switch (conVar.ConVar_Code)
            {
                case 1:
                    {
                        //Open map
                        MapNameInput mapName = new MapNameInput();
                        mapName.Show();
                        break;
                    }
                case 2:
                    {
                        //Exit
                        Application.Exit();
                        break;
                    }
                case 3:
                    {
                        //Dev
                        Mawse2D_Assert(AssertType.INF, "convar recieved");
                        break;
                    }
                case 4:
                    {
                        //Iterate through all assertions
                        Mawse2D_Assert(AssertType.ERROR, "Err.");
                        Mawse2D_Assert(AssertType.FATAL_ERROR, "Ftl_Err.");
                        Mawse2D_Assert(AssertType.INF, "Inf.");
                        Mawse2D_Assert(AssertType.WARNING, "Warn.");
                        break;
                    }
            }
        }


        //==================================================
        //Mawse2D_MenuEvent
        //Handles events for menu buttons
        //==================================================
        private void Mawse2D_MenuEvent(object sender, EventArgs e)
        {
            if ((sender as Control).Name.Contains("map"))
            {
                string mapName = FileReader(@"C:\Mawse2D\base\cfg\mapinf.cfg", 2);

                OpenmapName = mapName;
                Mawse2D_MapFetcher();
            }
            else if ((sender as Control).Name.Contains("ext"))
                Application.Exit();
        }


        //==================================================
        //Mawse2D_GUI
        //Loads all menu interfaces and others
        //==================================================
        public void Mawse2D_GUI(string gui)
        {
            if(gui.ToLower() == "main_menu")
            {
                //Set BackGround
                try
                {
                    this.BackgroundImage = new Bitmap(@"C:\Mawse2D\base\Material\ui\menu.jpg"); //Try set main menu 
                }
                catch (InvalidExpressionException e)
                {
                    Mawse2D_Assert(AssertType.FATAL_ERROR, e.Message); //Make assertion if not achieveable
                }

                //New Button UI
                Control newgame_btn = new Control();

                CControlInfo controlInfo = new CControlInfo("newGame_btn_map", "New Game", new Size(95, 24), new Point(12, 315));

                newgame_btn = newItem(controlInfo, ControlType.BTN);
                newgame_btn.BackColor = Color.Gray;
                newgame_btn.ForeColor = Color.Black;              
                newgame_btn.Click += new EventHandler(Mawse2D_MenuEvent);

                //Exit button
                Control extgame_btn = new Control();

                CControlInfo exitInfo = new CControlInfo("exitGame_btn_ext", "Quit", new Size(95, 24), new Point(12, 339));

                extgame_btn = newItem(exitInfo, ControlType.BTN);
                extgame_btn.BackColor = Color.Gray;
                extgame_btn.ForeColor = Color.Black;
                extgame_btn.Click += new EventHandler(Mawse2D_MenuEvent);

                this.Controls.Add(extgame_btn);

                //Main menu Title
                Control main_title = new Control();

                CControlInfo info = new CControlInfo("gametitle", gameTitle, new Size(327, 46), new Point(12, 241));

                main_title = newItem(info, ControlType.LBL);
                main_title.BackColor = Color.Transparent;
                main_title.Font = new Font("Arial", 30, FontStyle.Regular);
            }
            else if(gui.ToLower() == "player")
            {
                string line;
                System.IO.StreamReader gameInfo = new System.IO.StreamReader(@"C:\Mawse2D\base\gameinfo.txt");

                //Get Player Texture Name
                string PlayerTexture = "";
                while ((line = gameInfo.ReadLine()) != null)
                {
                    if (line.Contains("player"))
                    {
                        for(int i = 7; i < line.Length; i++)
                        {
                            PlayerTexture += line[i];//Set Player Texture name from gameinfo
                        }
                    }
                    
                }



                try
                {
                    (Fetch("Player") as PictureBox).BackgroundImage = new Bitmap(@"C:\Mawse2D\base\Material\player\" + PlayerTexture + ".png");
                    (Fetch("Player") as PictureBox).BackColor = Color.Transparent;
                }
                catch
                {
                    Mawse2D_Assert(AssertType.FATAL_ERROR, "Unable to set player texture");
                    (Fetch("Player") as PictureBox).BackColor = Color.White;
                }
            }
            else if (gui.ToLower() == "skybox")
            {

                //Get Skybox material name
                string line = "";
                System.IO.StreamReader mapFile = null;

                mapFile = new System.IO.StreamReader(@"C:\Mawse2D\base\Maps\" + OpenmapName + ".lvl");

                for (int i = 1; i < 13; i++)
                {
                    line = mapFile.ReadLine();//refresh the line until reaching final line
                }

                //Assign skybox material
                try
                {
                    Bitmap bmp = new Bitmap(@"C:\Mawse2D\base\Material\background\" + line + ".png");

                    this.BackgroundImage = bmp;
                }
                catch
                {
                    Mawse2D_Assert(AssertType.FATAL_ERROR, "Couldn't Load Sky Material\nCheck File Format or Name");
                    this.BackgroundImage = null;
                    this.BackColor = Color.Black;
                }

            }
            else if(gui.ToLower() == "clear")
            {
                //Destroy all gui elements
                if (Fetch("gametitle") != null && Fetch("newGame_btn") != null)
                {
                    Fetch("gametitle").Dispose();
                    Fetch("newGame_btn_map").Dispose();
                    Fetch("exitGame_btn_ext").Dispose();
                }
            }

        }
        #endregion


        #region Utilities



        //==================================================
        //Fetch
        //Go find object and bring him to me
        //==================================================
        public Control Fetch(string name)
        {
            foreach(Control control in this.Controls)
            {
                if (control.Name.Contains(name))
                    return control;
            }
            return null;
        }



        //==================================================
        //FileReader
        //Gets and returns line
        //==================================================
        public string FileReader(string path, int line)
        {
            string lines = "";
            System.IO.StreamReader mapFile = null;

            mapFile = new System.IO.StreamReader(@"" + path);

            for(int i = 0; i < line; i++)
            {
                lines = mapFile.ReadLine();//will overwrite each line until stops
            }

            return lines;
        }



        //==================================================
        //Exist
        //Check if object Exists
        //==================================================
        public bool Exist(string name)
        {
            foreach(Control control in this.Controls)
            {
                if (control.Name.Contains(name))
                    return true;
            }
            return false;
        }



        //==================================================
        //Update_Debug
        //Updates the on-screen dubug info
        //==================================================
        public void Update_Debug()
        {
            debug.BringToFront();
            if (OpenmapName != null && Fetch("Player") != null)
                debug.Text = "Debug\nX:" + Fetch("Player").Location.X + "\nY:" + Fetch("Player").Location.Y + "\nmap:" + OpenmapName + "\nbuildversion:" + buildVer;
        }


        //==================================================
        //newItem
        //Utility to quickly create new screen elements
        //==================================================
        public Control newItem(CControlInfo info, ControlType type)
        {

            switch (type)
            {
                case ControlType.BTN://button -_-
                    {
                        Button btn = new Button();
                        btn.Name = info.name;
                        btn.Text = info.text;
                        btn.Size = info.size;
                        btn.Location = info.location;

                        this.Controls.Add(btn);

                        return btn;
                    }
                case ControlType.CTRL://control
                    {
                        Control ctrl = new Control();
                        ctrl.Name = info.name;
                        ctrl.Text = info.text;
                        ctrl.Size = info.size;
                        ctrl.Location = info.location;

                        this.Controls.Add(ctrl);

                        return ctrl;
                    }
                case ControlType.PB://picture box
                    {
                        PictureBox pb = new PictureBox();
                        pb.Name = info.name;
                        pb.Text = info.text;
                        pb.Size = info.size;
                        pb.Location = info.location;
                        pb.BackColor = Color.Green;

                        this.Controls.Add(pb);

                        return pb;
                    }
                case ControlType.LBL://label
                    {
                        Label lbl = new Label();
                        lbl.Name = info.name;
                        lbl.Text = info.text;
                        lbl.Size = info.size;
                        lbl.Location = info.location;

                        this.Controls.Add(lbl);

                        return lbl;
                    }
            }

            return null;
        }



        //==================================================
        //Rndm
        //quite self explanatory, returns a random
        //==================================================
        public int Rndm(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }
        #endregion


        //==================================================
        //NPCMovementGenerator
        //Redraws npc movemnt class
        //==================================================
        public CNPCMovement NPCMovementGenerator(int callerNum)
        {
            Random rnd = new Random();

            List<INPUT> a_dirs = new List<INPUT>();
            a_dirs.Add(INPUT.UP);
            a_dirs.Add(INPUT.DOWN);
            a_dirs.Add(INPUT.LEFT);
            a_dirs.Add(INPUT.RIGHT);

            CNPCMovement movement = new CNPCMovement(map_startPos, Rndm(1, 5), INPUT.RIGHT);//a_dirs[Rndm(0, a_dirs.Count)]

            if (movementList.Count > 0)
                if (movementList[callerNum] != null)
                    movementList[callerNum] = movement;   //replace the existing one if it exists

            return movement;
        }


        //==================================================
        //NPCMovement
        //Handles npc movement called on update
        //==================================================
        public void NPCMovement(int indexposition, CNPCMovement movement)
        {
            //get npc
            Control npc = Fetch("npc" + indexposition);

            switch (movement.moveDir)
            {
                case INPUT.LEFT:
                    {
                        npc.Location = new Point(npc.Location.X - 50, npc.Location.Y);
                        break;
                    }
                case INPUT.RIGHT:
                    {
                        npc.Location = new Point(npc.Location.X + 50, npc.Location.Y);
                        break;
                    }
            }
        }


        public int NPCCollision(Control sender, INPUT dir)
        {
            //Look ahead

            foreach(Control control in this.Controls)
            {
                for (int i = 0; i < 128; i++)
                {
                    switch (dir)
                    {
                        case INPUT.RIGHT:
                            {
                                if (sender.Location == new Point(control.Location.X + 50 * i, control.Location.Y) && sender.Location.X + 50 <= 910)
                                    return i;
                                break;
                            }
                        case INPUT.LEFT:
                            {
                                if (sender.Location == new Point(control.Location.X - 50 * i, control.Location.Y) && sender.Location.X - 50 >= 10)
                                    return i;
                                break;
                            }
                    }
                }
            }
            
            return 0;
        }


        //==================================================
        //PlayerMovement
        //Moves Player Around
        //==================================================
        public void PlayerMovement(INPUT input)
        {
            if (Exist("Player"))
            {
                Update_Debug(); //update position

                Control m_Player = Fetch("Player");

                m_Player.BackColor = Color.Transparent;

                switch (input)
                {
                    case INPUT.UP:
                        {
                            if(m_Player.Location.Y - 50 >= 0 && Collision(input)) //Check window borders
                                m_Player.Location = new Point(m_Player.Location.X, m_Player.Location.Y - 50);
                            break;
                        }
                    case INPUT.DOWN:
                        {
                            if (m_Player.Location.Y + 50 <= 510 && Collision(input))
                                m_Player.Location = new Point(m_Player.Location.X, m_Player.Location.Y + 50);
                            break;
                        }
                    case INPUT.LEFT:
                        {
                            if (m_Player.Location.X - 50 >= 10 && Collision(input))
                                m_Player.Location = new Point(m_Player.Location.X - 50, m_Player.Location.Y);
                            break;
                        }
                    case INPUT.RIGHT:
                        {
                            if (m_Player.Location.X + 50 <= 910 && Collision(input))
                                m_Player.Location = new Point(m_Player.Location.X + 50, m_Player.Location.Y);
                            break;
                        }
                }
            }
            
        }




        //==================================================
        //Collision
        //Player Collision
        //==================================================
        public bool Collision(INPUT input)
        {
            Control m_Player = Fetch("Player");

            foreach(Control control in this.Controls)
            {
                switch (input)
                {
                    case INPUT.UP:
                        {
                            if (m_Player.Location == new Point(control.Location.X, control.Location.Y + 50))
                            {
                                if (control.Name.Contains("nocol"))
                                {
                                    m_Player.BackColor = (control as PictureBox).BackColor;
                                    return true;
                                }
                                else
                                    return false;

                            }
                            break;
                        }
                    case INPUT.DOWN:
                        {
                            if (m_Player.Location == new Point(control.Location.X, control.Location.Y - 50))
                            {
                                if (control.Name.Contains("nocol"))
                                {
                                    m_Player.BackColor = (control as PictureBox).BackColor;
                                    return true;
                                }
                                else
                                    return false;
                            }
                            break;
                        }
                    case INPUT.LEFT:
                        {
                            if (m_Player.Location == new Point(control.Location.X + 50, control.Location.Y))
                            {
                                if (control.Name.Contains("nocol"))
                                {
                                    m_Player.BackColor = (control as PictureBox).BackColor;
                                    return true;
                                }
                                else
                                    return false;
                            }
                            break;
                        }
                    case INPUT.RIGHT:
                        {
                            if (m_Player.Location == new Point(control.Location.X - 50, control.Location.Y))
                            {
                                if (control.Name.Contains("nocol"))
                                {
                                    m_Player.BackColor = (control as PictureBox).BackColor;
                                    return true;
                                }
                                else
                                    return false;
                            }
                            break;
                        }
                }
            }
            

            return true;
        }


    }


    //==================================================
    //CControlInfo
    //Creation info for new control
    //==================================================
    public class CControlInfo
    {
        public string name;
        public string text;
        public Size size;
        public Point location;

        public CControlInfo(string _name, string _text, Size _size, Point _location)
        {
            name = _name;
            text = _text;
            size = _size;
            location = _location;
        }
    }


    //==================================================
    //CNPCMovement
    //Movement Data for npc
    //==================================================
    public class CNPCMovement
    {
        public Point startPos;
        public int distance;//how far we are moving
        public game_window.INPUT moveDir;

        public CNPCMovement(Point _startPos, int _distance, game_window.INPUT _moveDir)
        {
            startPos = _startPos;
            distance = _distance;
            moveDir = _moveDir;
        }
    }

}
