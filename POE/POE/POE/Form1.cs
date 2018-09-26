using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace POE
{
    
    public partial class Form1 : Form
    {
        Map newMap;
        GameEngine MainEngine;
        public static Button[,] btnArr = new Button[20, 20];// generates map with buttons 
        public static Unit[,] units = new Unit[2, 400]; 
        int counter = 0;
        public int tick; // creates an empty integer to use to count and increment ticks for timer 
        [Serializable]
        public abstract class Building
        {
            protected int BuildingX, BuildingY,BuildingHp;
            protected string Team, Symbol;
                    public  Building() // building constructor
                    {
                    
                    }
                        public virtual string ToString()
                        {
                            return base.ToString();
                        }
                            public virtual void BuildingDestroyed() //virtual method for building destruction 
                            {

                            }

        }// end of building class
        [Serializable]
        public class ResourceBuilding:Building  //creating resource class inheriting building class 
        {
            Form1 form1;
            private string ResourceType;    //variable declarations 
            private int ResourcesLeft = 0;
            private int ResourcesPerTick;
            
            
                public ResourceBuilding(Form1 form1) // resource building constructor
                {
                this.form1 = form1;
                }
                public override void BuildingDestroyed() //override for destroyed building 
                    {
                        base.BuildingDestroyed();
                    }
                        public override string ToString()  //resource to string 
                        {
                            return base.ToString();
                        }
                            public void ResourceGenerate()
                            {
                                        //ResourceType = "Materials"; 
                                        //Random rn = new Random(); // will use a random variable to generate a random value for the resources left
                                        //ResourcesLeft = rn.Next(50, 150); // generates resources in the range of 50 - 150     
                                        Random rn = new Random();
                                        ResourcesPerTick = rn.Next(1, 5);
                                        ResourcesLeft += ResourcesPerTick;
                                        form1.ResourceNum.Text = ResourcesLeft.ToString();
                                        ResourcesPerTick = rn.Next(1, 5);
                                        ResourcesLeft += ResourcesPerTick;
                                        form1.ResourceNum2.Text = ResourcesLeft.ToString();
                           }
                            public void ClearResources()
                            {
                                    ResourcesLeft = 0;
                                    form1.ResourceNum.Text="0";
                            }
        }

        [Serializable]
        public class FactoryBuilding : Building
        {       //creating buildings will cost resources
            private string Units;   // variable declaration 
            private int TicksPerProduction;
            private int SpawnPoint;
        }
        [Serializable]
        public abstract class Unit:Button
        {
            public int team;
            protected string UnitName;
            private int  speed, hp, attackRange, attack;
            private string faction, symbol;
            public int xPos { get; protected set; }
            public int yPos { get; protected set; }
            public Unit(int xPos, int yPos) // Constructor 
                {
                    this.xPos = xPos;
                    this.yPos = yPos;
                }
                    public virtual void moveUnit( )
                    {
                        Unit otherUnit = nearestUnit();
                        int xChange = 0;
                        int yChange = 0;                        
                       
                        if (this.xPos > otherUnit.xPos)
                        {
                            xChange = -1;
                        }
                        else if(this.xPos < otherUnit.xPos)
                        {
                            xChange = 1;
                        }
                       
                        if (this.yPos < otherUnit.yPos)
                        {
                            yChange = -1;
                        }
                        else if (this.yPos > otherUnit.yPos)
                        {
                            yChange = 1;
                        }
                       
                        Form1.btnArr[this.xPos, this.yPos] = null;
                        if (this.xPos + xChange <20 && this.xPos +xChange>=0)
                        {
                            if (Form1.btnArr[this.xPos + xChange, this.yPos] == null)
                            {
                                this.xPos = this.xPos + xChange;
                            }
                        }
                        if (this.yPos + yChange < 20 && this.yPos + yChange >= 0)
                        {
                            if (Form1.btnArr[this.xPos, this.yPos + yChange] == null)
                            {
                                this.yPos = this.yPos + yChange;
                            }
                        }
                        Form1. btnArr[this.xPos, this.yPos] = this;
                //Console.WriteLine(this.team);
                //Console.WriteLine(this.xPos + "x" + otherUnit.xPos);
                //Console.WriteLine(this.yPos + "y" + otherUnit.yPos);
                //Console.WriteLine(xChange);
                //Console.WriteLine(yChange);
                //Console.WriteLine(" "); used to check unit movment to make sure it is correct
            }
                    public virtual void comabtUnit(int damage)
                    {

                    }
                    public virtual void inRange(int range)
                    {

                    }
                    public virtual Unit nearestUnit()
                    {
                           Unit closestUnit = null;
                            double closestDistance = 500;
                            int otherteam = 0;
                            if(this.team == otherteam)
                            {
                                otherteam = 1;
                            }
                            for (int i = 0; i < 400; i++)
                            {
                                if(units[otherteam,i] !=null)
                                {
                                                Unit otherUnit = units[otherteam, i];
                                    double x, y, distance;
                                    x = this.xPos - otherUnit.xPos;
                                    y= this.yPos - otherUnit.yPos;
                                    x = x * x;
                                    y = y * y;
                                    distance = x + y;
                                    distance = Math.Sqrt(distance);
                                    if(distance<closestDistance)
                                    {
                                        closestDistance = distance;
                                        closestUnit = otherUnit;
                                    }
                                }
                            }
                return closestUnit;
                    }
                    public virtual void unitDeath(int hp)
                    {

                    }
                    public virtual void unitInfo(string Info)
                    {

                    }

        }
        [Serializable]
        public class MeleeUnit : Unit
        {
            
            public override void unitInfo(string Info)  // returns all melee unit info as a string 
            {
                base.unitInfo(Info);        
                Hp.ToString();
                Speed.ToString();
                Attack.ToString();
                AttackRange.ToString();
                UnitName.ToString();
            }
                    public string Symbol //symbol getter 
                    { get; set; }

                    public int AttackRange //Attack Range Getter and setter
                    { get; set; }

                    public int Attack //Attack Getter and setter
                    { get; set; }

                    public int Hp //Hp Getter and setter
                    { get; set; }

                    public int Speed //SpeedGetter and setter
                    { get; set; }

            public MeleeUnit(int xPos, int yPos):base(xPos,yPos)// constructor
            {
                         
                Random rn = new Random(Guid.NewGuid().GetHashCode());
                UnitName = "";  // giving unit a blank name
                team = rn.Next(0, 2); // This is used to decide if the unit will be team 1 or 2
                    if (team == 0)
                    {
                        Symbol = "M";   //sets the ranged unit symbol to M
                        this.Text = Symbol;
                        
                    }
                    else
                    {
                        Symbol = "MM";
                        this.Text = Symbol;
                    }
               }

            public override void unitDeath(int hp) //overriding 
            {
                base.unitDeath(hp);
                if (hp == 0)
                {
                    Console.WriteLine("Unit is dead");
                }
            }
            public override void inRange(int range) // overriding 
            {
                int location;
                xPos = xPos * xPos;
                yPos = yPos * yPos;
                location = xPos + yPos;
                base.inRange(range);
                if (range <= location)
                {
                    comabtUnit(5);
                }
            }
            public override void comabtUnit(int damage)
            {
                base.comabtUnit(damage);           
                Hp = Hp - damage;   // applies damage to unit HP
            }
        }
        [Serializable]
        public class RangedUnit : Unit 
        {
           
            public RangedUnit(int xPos, int yPos): base(xPos, yPos)// Ranged unit constructor 
            {
                UnitName = "Ranged ";  // giving unit a blank name
                Random rn = new Random(Guid.NewGuid().GetHashCode());
                    team = rn.Next(0, 2);// This is used to decide if the unit will be team 1 or 2
                    if (team== 0)
                    {
                        Symbol = "R";   //sets the ranged unit symbol to R
                        this.Text = Symbol;
                    }
                    else
                    {
                        Symbol = "RR";
                        this.Text = Symbol;
                    }
            }
            public bool Team
            { get; set; }
            public string Symbol //symbol getter 
            { get; set; }

            public int AttackRange //Attack Range Getter and setter
            { get; set; }
            
            public int Attack //Attack Getter and setter
            { get; set; }

            public int Hp //Hp Getter and setter
            { get; set; }

            public int Speed //SpeedGetter and setter
            { get; set; }

            public override void unitDeath(int hp) //overriding 
            {
                base.unitDeath(hp);
                if (hp == 0)
                {
                    Console.WriteLine("Unit is dead");  // if unit is found to be dead 
                }
            }
            public override void unitInfo(string Info)  //returns all values of units to a string 
            {
                base.unitInfo(Info);
                Hp.ToString();
                Speed.ToString();
                Attack.ToString();
                AttackRange.ToString();
                UnitName.ToString();
            }
            public override void inRange(int range) // overriding 
            {
                int location; // calculating enemy location
                xPos = xPos * xPos;
                yPos = yPos * yPos;
                location = xPos + yPos;
                base.inRange(range);
                if (range <= location) // determines if enemy is in range
                {
                    comabtUnit(2);
                }
            }
            public override void comabtUnit(int damage) // ranged unit combat method
            {
                base.comabtUnit(damage);
                Hp = Hp - damage;
            }

        }
        public void UnitClick(object sender, EventArgs e)
        {
            textBox1.Text = "No Unit";          
        }

        public void MUnit(object sender, EventArgs e)
        {
            textBox1.Text = "MUnit";
        }

        public class GameEngine
        {

            Form1 form1; 
            public ResourceBuilding RB
           {get;set;}
            public int TicksPerProduction
            { get; set; }
            public GameEngine(Form1 form1)
            {
                this.form1 = form1;
                RB = new ResourceBuilding(form1);

            }
            
        }
        public void PlaceUnit()
        {

            Random rn = new Random();
            
            Unit unit; // creates an instance of Unit to be polymorphed based on the result of the random 

            int choice = rn.Next(0, 2); 

                    if(choice == 0)     //Results of the random will decide if its a melee or ranged unit
                    {
                            unit = new MeleeUnit(rn.Next(1, 20), rn.Next(1, 20));
                    }
                    else
                    {
                        unit = new RangedUnit(rn.Next(1, 20), rn.Next(1, 20));
                    }
            units[unit.team, unitFill(unit.team)] = unit;
            btnArr[unit.xPos, unit.yPos] = unit;
            GridLayout.Controls.Add((unit), unit.xPos, unit.yPos);

        }

        public void UnitMovement()
        {
            GridLayout.SuspendLayout();
            GridLayout.Controls.Clear();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0;j < 400; j++)
                {
                    if (units[i, j] != null)
                    {
                        units[i, j].moveUnit();
                    }
                }
            }
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if(btnArr[i,j] == null)
                    {
                        GridLayout.Controls.Add(new Button(), i, j);
                    }
                    else
                    {
                        GridLayout.Controls.Add(btnArr[i,j], i, j);
                    }
                }
            }
            GridLayout.ResumeLayout();

        }

        public Form1() 
        {
            
            InitializeComponent();
            Map newMap = new Map(this);
            MainEngine = new GameEngine(this);
            int width = this.Size.Width; //setting form size 
            int height = this.Size.Height;
            Width = 1300;
            Height = 1100;
            label1.Text = "0"; // sets timer label to 0
            timer1.Stop(); // stops the timer from starting with the form
            newMap.GenerateMap();
            ResourceNum.Text = "";
            ResourceNum2.Text = "";

        }

        public void startBtn_Click(object sender, EventArgs e)
        {
           
        } // ignore this 

        public class Map
        {
            public Form1 form1;
            public Map(Form1 form1)
            {
                this.form1 = form1;
            }
            public void GenerateMap()
            {

               Button btn = new Button(); //creates a button 
                form1.GridLayout.Controls.Clear();
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        Form1.btnArr[i, j] = null;
                        btn = new Button();
                        form1.GridLayout.Controls.Add(btn);
                    }
                }
            }
        }
        public void timer1_Tick(object sender, EventArgs e)    // every time the timer recieves a tick the tick is incremented and the label text is updated
        {
            tick++;
            label1.Text = tick.ToString(); //converts the timer to text onto the label
            MainEngine.RB.ResourceGenerate();
            UnitMovement();
        }
        private void startBtn_Click_1(object sender, EventArgs e)   // once the start button is pressed the timer begins and text is changed to stop. if pressed again timer is stopped and text is changed to start
        {

            
                if (counter < 2 )
                {
                    for (int i = 0; i < 12; i++)
                        {
                            PlaceUnit();
                            counter++;
                        }
                }
                if (startBtn.Text == "Start") // starts the timer by checking if the buttons text  = start
                {
                    timer1.Start();
                    startBtn.Text = "Stop";
                }
                else //stops the timer if the button text = stop
                {
                    timer1.Stop();
                    startBtn.Text = "Start";
                }
            
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            GridLayout.Controls.Clear();
            timer1.Stop();
            tick = 0;
            label1.Text = "0";
            startBtn.Text = "Start";
            counter = 0;
            textBox1.Text = "";
            ResourceNum2.Text = "";
            MainEngine.RB.ClearResources();
    
        }
        private void GridLayout_Paint(object sender, PaintEventArgs e)
        {

        }
        public int unitFill(int team)   // giving unit team 
        {
            int i;
            i = -1;
               do
                {
                    i++;
                }
                    while (units[team, i] != null);
                    return i;
        }

    }
  }

