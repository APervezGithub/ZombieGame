using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Summative2020
{
    public partial class Form1 : Form
    {
        // Key input
        bool a = false;
        bool s = false;
        bool d = false;
        bool w = false;
        bool r = false;

        // Mouse input
        bool clicking = false;

        // Player info
        double xPos = 300;
        double yPos = 300;
        double mouseX = 0;
        double mouseY = 0;
        double playerSpeed = 1.5;
        double playerAngle = 0;
        int prevIndex = 0;
        int health = 100;
        int regenTimer = 200;

        // Wave info
        int currentWave = 0;
        int currentEnemy = 0;
        bool nextWave = false;

        // Store info
        int selStoreI = 0;
        List<gunInfo> gunList = new List<gunInfo>();
        int[] storePositions = { 122, 175, 231, 286, 342, 397, 456 }; // Position for underline

        // Money info
        int money = 300;
        int moneyTimer = 200;

        // Weapon info
        int selectedIndex = 0;
        int firetimer = 0;
        int[] ammoCounts = { 20, 6, 40, 30, 10, 4, 200 }; // Initial magazine
        int[] ammoReserves = { 40, 12, 80, 60, 20, 8, 600 }; // Extra magazines
        // Pictures
        Image[] gunPics = { Properties.Resources.semiautorifle, Properties.Resources.pumpshotgun, Properties.Resources.submachinegun, Properties.Resources.autorifle, Properties.Resources.semishotgun, Properties.Resources.sniperrifle, Properties.Resources.lightmachinegun };
        Image[] gunPicsS = { Properties.Resources.semiautorifle2, Properties.Resources.pumpshotgun2, Properties.Resources.submachinegun2, Properties.Resources.autorifle2, Properties.Resources.semishotgun2, Properties.Resources.sniperrifle2, Properties.Resources.lightmachinegun2 };
        int[] guns = { 1, 0, 0, 0, 0, 0, 0 }; // Unlocked guns
        gunInfo currentGun;
        bool reloading = false;
        int reloadTimer;

        // Sounds
        SoundPlayer[] shootSounds = { new SoundPlayer(Properties.Resources.rifleShot), new SoundPlayer(Properties.Resources.pumpshotgunShot), new SoundPlayer(Properties.Resources.rifleShot), new SoundPlayer(Properties.Resources.rifleShot), new SoundPlayer(Properties.Resources.pumpshotgunShot), new SoundPlayer(Properties.Resources.rifleShot), new SoundPlayer(Properties.Resources.rifleShot) };
        SoundPlayer[] reloadSounds = { new SoundPlayer(Properties.Resources.rifleReload), new SoundPlayer(Properties.Resources.shotgunReload), new SoundPlayer(Properties.Resources.rifleReload), new SoundPlayer(Properties.Resources.rifleReload), new SoundPlayer(Properties.Resources.shotgunReload), new SoundPlayer(Properties.Resources.rifleReload), new SoundPlayer(Properties.Resources.rifleReload) };
        SoundPlayer shellFall = new SoundPlayer(Properties.Resources.shellFall);
        SoundPlayer zombieMoan = new SoundPlayer(Properties.Resources.zombieMoan);
        SoundPlayer zombieGrunt = new SoundPlayer(Properties.Resources.zombieGrunt);
        SoundPlayer chaching = new SoundPlayer(Properties.Resources.chaching);
        SoundPlayer woodBreak = new SoundPlayer(Properties.Resources.doorBreak);
        SoundPlayer manDie= new SoundPlayer(Properties.Resources.death);
        int deathSoundDelay = 6; // Unused


        // Random numbers
        Random random = new Random();
        Random random2 = new Random();

        // Wave info: 1 is a normal zombie, 2 is a fast zombie, 3 is a strong zombie and 4 is the boss zombie
        List<int[]> waves = new List<int[]>();
        int[] wave1 = { 1, 1, 1, 1, 1 };
        int[] wave2 = { 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 2, 2, 2 };
        int[] wave3 = { 3, 1, 1, 1, 1, 3, 2, 2, 2, 2 };
        int[] wave4 = { 3, 3, 3, 1, 2, 2, 1, 2, 2, 3, 2, 2, 3, 2, 2 };
        int[] wave5 = { 3, 3, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 1, 1, 1, 1, 1 };
        int[] wave6 = { 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 3, 3, 2, 2, 2, 2 };
        int[] wave7 = { 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        int[] wave8 = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 1, 1, 3, 1, 1, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 4, 1, 1, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

        // Wave delay info: Every 10 is one second, delay between spawning of each zombie in above list
        List<int[]> delays = new List<int[]>();
        int[] delay1 = { 30, 10, 10, 10, 10 };
        int[] delay2 = { 30, 5, 5, 50, 5, 5, 50, 5, 5, 5, 5, 10, 10, 10 };
        int[] delay3 = { 30, 20, 20, 20, 20, 100, 20, 20, 10, 10 };
        int[] delay4 = { 30, 10, 10, 5, 10, 10, 30, 10, 10, 60, 10, 10, 20, 10, 10 };
        int[] delay5 = { 30, 5, 10, 10, 10, 10, 10, 10, 40, 5, 5, 5, 10, 10, 10, 10, 10 };
        int[] delay6 = { 30, 5, 5, 5, 5, 60, 10, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
        int[] delay7 = { 30, 1, 15, 1, 15, 1, 15, 1, 15, 1, 15, 1, 15, 1, 15, 1, 15, 1, 15, 100, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        int[] delay8 = { 30, 5, 5, 5, 5, 2, 2, 2, 10, 10, 10, 10, 10, 10, 10, 5, 5, 5, 5, 5, 100, 1, 1, 1, 1, 1, 1, 1, 10, 5, 5, 5, 5, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };

        // Lists for spawned things
        // Cleanup is for removing things that are no longer needed, I thought it would be better to just do it separately in case removing from a list that I am looping through caused bugs
        List<bullet> bullets = new List<bullet>();
        List<int> bulletCleanup = new List<int>();

        List<hitMarker> hitmarkers = new List<hitMarker>();
        List<int> hitmarkerCleanup = new List<int>();

        List<zombie> zombies = new List<zombie>();
        List<int> zombieCleanup = new List<int>();

        public Form1()
        {
            InitializeComponent();
            // Gun stats
            gunInfo semiautoRifle = new gunInfo(  20 , 70 , 2 , 1, 20 , 100, 0, 0   , 50 , 80 , 110, 150, 140);
            gunInfo pumpShotgun = new gunInfo(    50 , 40 , 15, 8, 6  , 150, 1, 100 , 20 , 40 , 150, 60 , 110);
            gunInfo submachineGun = new gunInfo(  5  , 30 , 8 , 1, 40 , 70 , 2, 500 , 30 , 200, 120, 50 , 80 );
            gunInfo automaticRifle = new gunInfo( 7  , 30 , 3 , 1, 30 , 100, 3, 1000, 50 , 170, 90 , 100, 100);
            gunInfo semiautoShotgun = new gunInfo(40 , 70 , 6 , 3, 10 , 120, 4, 2000, 20 , 90 , 140, 80 , 70 );
            gunInfo sniperRifle = new gunInfo(    150, 600, 0 , 1, 4  , 300, 5, 3000, 100, 10 , 200, 200, 60 );
            gunInfo lightMachinegun = new gunInfo(12 , 40 , 3 , 1, 200, 600, 6, 5000, 200, 130, 100, 100, 200);
            gunList.Add(semiautoRifle);
            gunList.Add(pumpShotgun);
            gunList.Add(submachineGun);
            gunList.Add(automaticRifle);
            gunList.Add(semiautoShotgun);
            gunList.Add(sniperRifle);
            gunList.Add(lightMachinegun);
            currentGun = semiautoRifle; // Equipped gun
            selectedIndex = 0; // Equipped gun
            reloadTimer = currentGun.rel;
            firetimer = currentGun.rof;
            gunPic.Image = gunPics[currentGun.ind];
            // Adding waves to wavelist
            waves.Add(wave1);
            delays.Add(delay1);
            waves.Add(wave2);
            delays.Add(delay2);
            waves.Add(wave3);
            delays.Add(delay3);
            waves.Add(wave4);
            delays.Add(delay4);
            waves.Add(wave5);
            delays.Add(delay5);
            waves.Add(wave6);
            delays.Add(delay6);
            waves.Add(wave7);
            delays.Add(delay7);
            waves.Add(wave8);
            delays.Add(delay8);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            // Going into game
            timer1.Enabled = true; // Starting timers and tick events
            waveTimer.Enabled = true; // Starting timers and tick events
            showSpawn(); // Showing  spawned elements, the method hideSpawn(); will hide spawned elements, used throughout the next few event handlers
            tabControl1.SelectedIndex = 2; // Changing page
            prevIndex = 0; // Setting index of menu page (for use with back button on instructions page)
        }
        // Hover effects (repeated for each button)
        private void playButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            playButton.Image = Properties.Resources.PlayButtonHover;
        }

        private void playButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            playButton.Image = Properties.Resources.PlayButton;
        }

        private void instructionsButton_Click(object sender, EventArgs e)
        {
            // Going into instructions
            timer1.Enabled = false;
            waveTimer.Enabled = false;
            tabControl1.SelectedIndex = 1;
            prevIndex = 0;
        }

        private void instructionsButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            instructionsButton.Image = Properties.Resources.InstructionsButtonHover;
        }

        private void instructionsButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            instructionsButton.Image = Properties.Resources.InstructionsButton;
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            // Going back from instructions
            hideSpawn();
            tabControl1.SelectedIndex = prevIndex;
        }
        private void backButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            backButton.Image = Properties.Resources.BackButtonHover;
        }
        private void backButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            backButton.Image = Properties.Resources.BackButton;
        }
        private void backButtonS_Click(object sender, EventArgs e)
        {
            // Going back from store
            showSpawn();
            timer1.Enabled = true;
            waveTimer.Enabled = true;
            tabControl1.SelectedIndex = 2;
            prevIndex = 3;
        }
        private void backButtonS_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            backButtonS.Image = Properties.Resources.backhover;
        }
        private void backButtonS_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            backButtonS.Image = Properties.Resources.back;
        }
        private void buyammoButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            buyammoButton.Image = Properties.Resources.buyammohover;
        }

        private void buyammoButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            buyammoButton.Image = Properties.Resources.buyammo;
        }

        private void buygunButton_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            buygunButton.Image = Properties.Resources.buygunhover;
        }

        private void buygunButton_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            buygunButton.Image = Properties.Resources.buygun;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs key)
        {
            // Key input
            if (key.KeyCode == Keys.A)
            {
                a = true;
            }
            if (key.KeyCode == Keys.S)
            {
                s = true;
            }
            if (key.KeyCode == Keys.D)
            {
                d = true;
            }
            if (key.KeyCode == Keys.W)
            {
                w = true;
            }
            // Switching guns
            if (key.KeyCode == Keys.E && !reloading)
            {
                // Checking if there is space left to go back, and if there is, going back to the closest unlocked gun in the backwards direction
                if (selectedIndex < gunList.Count - 1)
                {
                    do
                    {
                        selectedIndex++;
                    } while (selectedIndex < gunList.Count - 1 && guns[selectedIndex] == 0);
                    if (guns[guns.Length - 1] == 0 && selectedIndex == guns.Length - 1) // This is because the do while will go all the way through and end with index at the LMG if it finds no unlocked guns, quick and lazy workaround
                    {
                        selectedIndex = currentGun.ind;
                    }
                    // Resetting gun information
                    currentGun = gunList[selectedIndex];
                    reloadTimer = currentGun.rel;
                    firetimer = 0;
                    gunPic.Image = gunPics[currentGun.ind];
                }
            }
            // Same as above, except for going forwards
            if (key.KeyCode == Keys.Q && !reloading)
            {
                if (selectedIndex > 0)
                {
                    do
                    {
                        selectedIndex--;
                    } while (selectedIndex > 0 && guns[selectedIndex] == 0);
                    // The end condition is not required here, because there is always a gun at postion 0 (semi auto rifle) that is unlocked for do while to end at
                    currentGun = gunList[selectedIndex];
                    reloadTimer = currentGun.rel;
                    firetimer = 0;
                    gunPic.Image = gunPics[currentGun.ind];
                }
            }
            // Reload
            if (key.KeyCode == Keys.R)
            {
                r = true;
            }
            // Go to instructions (Explained in detail above)
            if (key.KeyCode == Keys.I)
            {
                timer1.Enabled = false;
                waveTimer.Enabled = false;
                hideSpawn();
                prevIndex = tabControl1.SelectedIndex;
                tabControl1.SelectedIndex = 1;
            }
            // Go to menu
            if (key.KeyCode == Keys.M)
            {
                timer1.Enabled = false;
                waveTimer.Enabled = false;
                hideSpawn();
                prevIndex = tabControl1.SelectedIndex;
                tabControl1.SelectedIndex = 0;
            }
            // Go to store from game
            if (key.KeyCode == Keys.T && tabControl1.SelectedIndex == 2)
            {
                timer1.Enabled = false;
                waveTimer.Enabled = false;
                hideSpawn();
                tabControl1.SelectedIndex = 3;
                selStoreI = 0;
                // Setting various displays in the store to match selected gun, probably should have been in a method, but for some reason I can't access form controls from methods, do you know why?
                if (guns[selStoreI] == 0)
                {
                    priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr1);
                    buygunButton.Visible = true;
                }
                if (guns[selStoreI] == 1)
                {
                    priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr2);
                    buygunButton.Visible = false;
                }
                gunImage.Image = gunPicsS[selStoreI];
                underline.Location = new Point(83, storePositions[selStoreI]);
                dmgBar.Width = gunList[selStoreI].dmgbar;
                rofBar.Width = gunList[selStoreI].rofbar;
                accBar.Width = gunList[selStoreI].accbar;
                magBar.Width = gunList[selStoreI].magbar;
                moneyDisplay.Text = Convert.ToString(money);
                prevIndex = 3;
            }
            // Move around in gun store
            if (key.KeyCode == Keys.Down)
            {
                if (tabControl1.SelectedIndex == 3)
                {
                    if (selStoreI < gunList.Count - 1)
                    {
                        selStoreI++;
                    }
                }
                // If the gun is already bought, then it will show ammo cost, otherwise it will show gun cost
                if (guns[selStoreI] == 0)
                {
                    priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr1);
                    buygunButton.Visible = true;
                }
                if (guns[selStoreI] == 1)
                {
                    priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr2);
                    buygunButton.Visible = false;
                }
                // Setting various displays in the store to match selected gun
                gunImage.Image = gunPicsS[selStoreI];
                underline.Location = new Point(83, storePositions[selStoreI]);
                dmgBar.Width = gunList[selStoreI].dmgbar;
                rofBar.Width = gunList[selStoreI].rofbar;
                accBar.Width = gunList[selStoreI].accbar;
                magBar.Width = gunList[selStoreI].magbar;
            }
            // Same as above except for the upwards direction
            if (key.KeyCode == Keys.Up)
            {
                if(tabControl1.SelectedIndex == 3)
                {
                    if (selStoreI > 0)
                    {
                        selStoreI--;
                    }
                }
                if (guns[selStoreI] == 0)
                {
                    priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr1);
                    buygunButton.Visible = true;
                }
                if (guns[selStoreI] == 1)
                {
                    priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr2);
                    buygunButton.Visible = false;
                }
                gunImage.Image = gunPicsS[selStoreI];
                underline.Location = new Point(83, storePositions[selStoreI]);
                dmgBar.Width = gunList[selStoreI].dmgbar;
                rofBar.Width = gunList[selStoreI].rofbar;
                accBar.Width = gunList[selStoreI].accbar;
                magBar.Width = gunList[selStoreI].magbar;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs key)
        {
            // More key input
            if (key.KeyCode == Keys.A)
            {
                a = false;
            }
            if (key.KeyCode == Keys.S)
            {
                s = false;
            }
            if (key.KeyCode == Keys.D)
            {
                d = false;
            }
            if (key.KeyCode == Keys.W)
            {
                w = false;
            }
            if (key.KeyCode == Keys.R)
            {
                r = false;
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            // Finding mouse position
            Point pointToWindow = Cursor.Position;
            mouseX = pointToWindow.X - Left - 12;
            mouseY = pointToWindow.Y - Top - 53;
        }

        public static Bitmap RotateImage(Image image, PointF offset, double angle)
        {
            // Code copied directly from source (linked in report), comments written myself
            // Rotating image
            if (image == null) // Exception if image is invalid
                throw new ArgumentNullException("image");

            // Creating a bitmap image to store the final product of the translations
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution); // Setting correct size
            Graphics g = Graphics.FromImage(rotatedBmp);

            g.TranslateTransform(offset.X, offset.Y); // Move to account for rotating around 0 point

            g.RotateTransform((float)angle); // Do the rotation

            g.TranslateTransform(-offset.X, -offset.Y); // Move back

            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Game events

            // Health regen
            if(regenTimer > 0)
            {
                regenTimer--;
            } else if(health < 100)
            {
                health += 10;
                regenTimer = 200;
            }
            // Money increases over time
            if (moneyTimer > 0)
            {
                moneyTimer--;
            }
            else
            {
                money += 10;
                moneyTimer = 200;
            }
            coinCounter.Text = Convert.ToString(money);
            // Movement input
            if (a && xPos > 0) // Keeping player in walls (repeated for other movements)
            {
                if (!(xPos + 25 - playerSpeed > 245 && yPos + 25 > 350 && xPos + 25 - playerSpeed < 555)) // Making sure player is not crossing fence (repeated for other movements)
                {
                    xPos -= playerSpeed;
                }
            }
            if (s && yPos < 509)
            {
                if (!(xPos + 25 > 245 && yPos + 25 + playerSpeed > 350 && xPos + 25 < 555))
                {
                    yPos += playerSpeed;
                }
            }
            if (d && xPos < 752)
            {
                if (!(xPos + 25 + playerSpeed > 245 && yPos + 25 > 350 && xPos + 25 + playerSpeed < 555))
                {
                    xPos += playerSpeed;
                }
            }
            if (w && yPos > 0)
            {
                yPos -= playerSpeed;
            }
            // set player position and angle
            playerImage.Location = new Point((int)xPos, (int)yPos); // Updating position
            playerAngle = -findAngle((int)mouseX, (int)mouseY, (int)xPos, (int)yPos) + 180; // Finding angle and accounting for the orientation of the original image
            playerImage.Image = RotateImage(Properties.Resources.Player, new Point(13, 13), playerAngle + 90); // Applying rotating method

            // set gun hud elements
            fireTimerBar.Width = 69 * (currentGun.rof - firetimer) / currentGun.rof; // Setting the size of the bar that shows how long until you can shoot again
            currentAmmo.Text = ammoCounts[currentGun.ind] + " / " + ammoReserves[currentGun.ind];
            if (!reloading) // Setting the size of the bar that shows either how long until reload complete or how much ammo is left
            {
                reloadTimerBar.Width = 69 * ammoCounts[currentGun.ind] / currentGun.mag;
            }
            else
            {
                reloadTimerBar.Width = 69 * (currentGun.rel - reloadTimer) / currentGun.rel;
            }
            // Shooting
            if (firetimer > 0) // Controlling rate of fire
            {
                firetimer--;
            }
            if (clicking && firetimer == 0 && ammoCounts[currentGun.ind] > 0 && !reloading) // Gun fires
            {
                int numBullets = 0; // For shotguns
                shootSounds[currentGun.ind].Play();
                while (numBullets < currentGun.num) // Create enough bullets, 1 for rifle, multiple for shotgun
                {
                    int inaccuracy = random.Next(-currentGun.acc, currentGun.acc); // Dispersion of bullet from aim
                    bullets.Add(new bullet(xPos + 10, yPos + 30, playerAngle - 90 + inaccuracy, 7, 3, currentGun.dmg, 1)); // Creating the bullet
                    // Initializing bullet's image
                    Label tempPicture = new Label();
                    Controls.Add(tempPicture);
                    bullets[bullets.Count - 1].picture = tempPicture;
                    bullets[bullets.Count - 1].initialize();
                    firetimer = currentGun.rof; // Resetting firetimer
                    numBullets++;
                }
                ammoCounts[currentGun.ind]--; // Take ammo
            }
            // Ammo alerts
            if(ammoCounts[currentGun.ind] == 0 && ammoReserves[currentGun.ind] > 0 && !reloading)
            {
                AlertPanel.Visible = true;
            }
            else
            {
                AlertPanel.Visible = false;
            }
            if (ammoCounts[currentGun.ind] == 0 && ammoReserves[currentGun.ind] == 0)
            {
                AlertPanel2.Visible = true;
            }
            else
            {
                AlertPanel2.Visible = false;
            }

            if (r && ammoCounts[currentGun.ind] < currentGun.mag && ammoReserves[currentGun.ind] > 0) // Start reload
            {
                shellFall.Play();
                reloading = true;
            }
            if (reloadTimer == 0) // End reload
            {
                reloadSounds[currentGun.ind].Play();
                reloading = false;
                while (ammoReserves[currentGun.ind] > 0 && ammoCounts[currentGun.ind] < currentGun.mag) // Done this way so that you can reload into an incomplete magazine
                {
                    ammoReserves[currentGun.ind] -= 1;
                    ammoCounts[currentGun.ind] += 1;
                }
                reloadTimer = currentGun.rel;
            }
            if (reloading)
            {
                reloadTimer--;
            }
            // Bullets
            for (int i = 0; i < bullets.Count; i++)
            {
                bullet b = bullets[i];
                b.move();
                if (b.x < 0 || b.x > 780 || b.y < 0 || b.y > 600)
                {
                    bulletCleanup.Add(i); // Check bullet going out of bounds and removing them
                }
                // Hitting zombies
                if (b.penetrations > 0)
                {
                    for (int j = 0; j < zombies.Count; j++)
                    {
                        zombie z = zombies[j];
                        if (((z.x + z.size/2 - b.x) * (z.x + z.size / 2 - b.x) + (z.y + z.size / 2 - b.y) * (z.y + z.size / 2 - b.y)) < (z.size / 2) * (z.size / 2) && b.penetrations > 0) // Check distance from zombies, avoiding use of both power and sqrt to reduce time
                        {
                            b.penetrations--; // Bullet getting used up to avoid multiple collisions
                            z.health -= b.damage;
                        }
                    }
                }
                else
                {
                    // Adding a hitmarker where the bullet hits, then initializing (same process as creating bullet)
                    hitmarkers.Add(new hitMarker((int)b.x, (int)b.y));
                    Label tempPicture = new Label();
                    Controls.Add(tempPicture);
                    hitmarkers[hitmarkers.Count - 1].picture = tempPicture;
                    hitmarkers[hitmarkers.Count - 1].initialize();
                    hitmarkers[hitmarkers.Count - 1].picture.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);
                    hitmarkers[hitmarkers.Count - 1].picture.MouseUp += new MouseEventHandler(pictureBox2_MouseUp);
                    hitmarkers[hitmarkers.Count - 1].picture.MouseMove += new MouseEventHandler(pictureBox2_MouseMove);
                    bulletCleanup.Add(i);
                }
            }
            // Removing the bullets that are gone
            for (int i = 0; i < bulletCleanup.Count; i++)
            {
                Controls.Remove(bullets[bulletCleanup[i]].picture);
                bullets.RemoveAt(bulletCleanup[i]);
                for (int j = 0; j < bulletCleanup.Count; j++) // Reducing the position of the bullets awaiting removal to account for the shorter size of the bullet list
                {
                    bulletCleanup[j] -= 1;
                }
            }
            bulletCleanup = new List<int>(); // Resetting clean up list
            // Hit markers
            for (int i = 0; i < hitmarkers.Count; i++)
            {
                hitmarkers[i].decay();
                if(hitmarkers[i].timer == 0)
                {
                    hitmarkerCleanup.Add(i);
                }
            }
            // Removing the zombies that are gone
            for (int i = 0; i < hitmarkerCleanup.Count; i++)
            {
                Controls.Remove(hitmarkers[hitmarkerCleanup[i]].picture);
                hitmarkers.RemoveAt(hitmarkerCleanup[i]);
                for (int j = 0; j < hitmarkerCleanup.Count; j++) // Reducing the position of the hitmarkers awaiting removal to account for the shorter size of the hitmarker list
                {
                    hitmarkerCleanup[j] -= 1;
                }
            }
            hitmarkerCleanup = new List<int>(); // Resetting clean up list
            // Zombies
            for (int i = 0; i < zombies.Count; i++)
            {
                zombie z = zombies[i];
                // Same methods as player
                z.findTarget((int)xPos, (int)yPos);
                z.angle = -findAngle((int)z.x, (int)z.y, (int)z.tX, (int)z.tY) + 90;
                z.picture.Image = RotateImage(Properties.Resources.zombiewithC, new Point(122, 122), z.angle);
                z.move();
                if(z.hit((int)xPos, (int)yPos, ref health) == 1) // Hitting player, damage is dealt in the method
                {
                    zombieMoan.Play();
                }
                else if(z.hit((int)xPos, (int)yPos, ref health) == 2) // Hitting fence, immediate loss
                {
                    hideSpawn();
                    timer1.Enabled = false;
                    waveTimer.Enabled = false;
                    woodBreak.Play();
                    tabControl1.SelectedIndex = 4;
                    scoreDisplay1.Text = "Final Score: " + Convert.ToString(money + (currentWave - 1) * 100);
                    scoreDisplay2.Text = "Final Score: " + Convert.ToString(money + (currentWave - 1) * 100);
                }
                if (z.health <= 0) // Zombie dies and you get money
                {
                    //deathSoundTimer.Enabled = true;
                    money += z.value;
                    zombieCleanup.Add(i);
                }
            }
            // Dying
            if (health <= 0)
            {
                hideSpawn();
                timer1.Enabled = false;
                waveTimer.Enabled = false;
                manDie.Play();
                tabControl1.SelectedIndex = 4;
                scoreDisplay1.Text = "Final Score: " + Convert.ToString(money + (currentWave - 1) * 100); // Score calculating formula, accounts for money left and wave reached
                scoreDisplay2.Text = "Final Score: " + Convert.ToString(money + (currentWave - 1) * 100);
            }
            // Removing the zombies that are gone
            for (int i = 0; i < zombieCleanup.Count; i++)
            {
                Controls.Remove(zombies[zombieCleanup[i]].picture);
                zombies.RemoveAt(zombieCleanup[i]);
                for (int j = 0; j < zombieCleanup.Count; j++) // Reducing the position of the zombies awaiting removal to account for the shorter size of the zombie list
                {
                    zombieCleanup[j] -= 1;
                }
            }
            zombieCleanup = new List<int>(); 
            healthBar.Width = health * 3;
        }
        // Detecting clicks
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            clicking = true;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            clicking = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            clicking = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            clicking = false;
        }

        // Class to create gun stats with
        class gunInfo
        {
            public int rof { get; set; } // How long until gun can shoot again
            public double dmg { get; set; } // Damage per hit
            public int acc { get; set; } // Dispersion
            public int num { get; set; } // Number of bullets
            public int mag { get; set; } // Magazine size
            public int rel { get; set; } // Reload time
            public int ind { get; set; } // Position in list
            public int pr1 { get; set; } // Price for gun
            public int pr2 { get; set; } // Price for magazine
            public int rofbar { get; set; } // Stats for store (disproportional to actual stats)
            public int dmgbar { get; set; }
            public int accbar { get; set; }
            public int magbar { get; set; }


            public gunInfo(int rof_, double dmg_, int acc_, int num_, int mag_, int rel_, int ind_, int pr1_, int pr2_, int rofbar_, int dmgbar_, int accbar_, int magbar_)
            {
                rof = rof_;
                dmg = dmg_;
                acc = acc_;
                num = num_;
                mag = mag_;
                rel = rel_;
                ind = ind_;
                pr1 = pr1_;
                pr2 = pr2_;
                rofbar = rofbar_;
                dmgbar = dmgbar_;
                accbar = accbar_;
                magbar = magbar_;
            }
        }

        // Hit marker class
        class hitMarker
        {
            // Position of hit marker
            public double x { get; set; }
            public double y { get; set; }
            public Label picture { get; set; }
            public int timer { get; set; } // Life left for hitmarker
            public hitMarker(int x_, int y_)
            {
                x = x_;
                y = y_;
                timer = 255;
            }
            public void initialize()
            {
                // Setting all inital variables of the picture box used to represent the hitmarker
                picture.Location = new Point((int)x, (int)y);
                picture.Size = new Size(5, 5);
                picture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                picture.BackColor = Color.FromArgb(255, 255, 0, 0);
                picture.TabIndex = 1;
                picture.TabStop = false;
                picture.Visible = true;
                picture.BringToFront();
            }
            public void decay() // Hitmarker fade
            {
                if (timer > 10)
                {
                    picture.BackColor = Color.FromArgb(timer, timer, (255 - timer) / 2, 0);
                }
                timer -= 5;
            }
        }

        // Bullet class
        class bullet
        {
            // Setting information about bullet
            public double x { get; set; }
            public double y { get; set; }
            public double angle { get; set; }
            public double speed { get; set; }
            public int size { get; set; }
            public double damage { get; set; }
            public Label picture { get; set; }
            public int penetrations { get; set; }
            public bullet(double x_, double y_, double angle_, double speed_, int size_, double damage_, int penetrations_)
            {
                x = x_;
                y = y_;
                angle = angle_;
                size = size_;
                speed = speed_;
                damage = damage_;
                picture = new Label();
                penetrations = penetrations_;
            }
            public void initialize()
            {
                // Same as above
                picture.Location = new Point((int)x, (int)y);
                picture.Size = new Size(size, size);
                picture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                picture.BackColor = Color.FromArgb(255, 0, 0, 0);
                picture.TabIndex = 1;
                picture.TabStop = false;
                picture.Visible = true;
                picture.BringToFront();
            }

            public void move()
            {
                // Angular momentum using trigonometry
                x += speed * Math.Sin(-angle * (Math.PI / 180.0));
                y += speed * Math.Cos(-angle * (Math.PI / 180.0));
                picture.Location = new Point((int)x, (int)y);
            }
        }

        // Zombie class
        class zombie
        {
            // Information for zombie
            public double x { get; set; }
            public double y { get; set; }
            public double tX { get; set; }
            public double tY { get; set; }
            public double angle { get; set; }
            public double speed { get; set; }
            public int size { get; set; }
            public double health { get; set; }
            public double damage { get; set; }
            public PictureBox picture { get; set; }
            public int hitTimer { get; set; }
            public Image image { get; set; }
            public int value { get; set; }

            public zombie(double x_, double y_, double angle_)
            {
                x = x_;
                y = y_;
                angle = angle_;
                picture = new PictureBox();
                hitTimer = 140; // Time until zombie can hit the player again
            }

            public void initialize()
            {
                // Same as above
                picture.Location = new Point((int)x, (int)y);
                picture.Size = new Size(size, size);
                picture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                picture.Image = image;
                picture.BackgroundImage = Properties.Resources.Faketransparent;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.TabIndex = 1;
                picture.TabStop = false;
                picture.Visible = true;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                picture.BringToFront();
            }

            public void move()
            {
                // Same as above
                x += speed * Math.Cos((angle - 90) * (Math.PI / 180.0));
                y += speed * Math.Sin((angle - 90) * (Math.PI / 180.0));
                picture.Location = new Point((int)Math.Round(x), (int)Math.Round(y));
            }

            public void findTarget(int playerX, int playerY) // Checking if player is close and targeting them, otherwise targeting the gate
            {
                if (((x + 12.5 - playerX) * (x + 12.5 - playerX) + (y + 12.5 - playerY) * (y + 12.5 - playerY)) < 280 * 280)
                {
                    tX = playerX;
                    tY = playerY;
                }
                else
                {
                    tX = 405 - size;
                    tY = 375 - size;
                }
            }
            public int hit(int playerX, int playerY, ref int health) // Hitting the player
            {
                if(tX == 405 - size && tY == 375 - size && x > 400 - size && y > 370 - size && x < 410 - size && y < 380 - size)
                {
                    return 2; // Hits the door
                }
                if (hitTimer > 0)
                {
                    hitTimer--;
                    return 0; // Still charging hit
                }
                else
                {
                    if (((x - playerX) * (x - playerX) + (y - playerY) * (y - playerY)) < 5 * 5)
                    {
                        health -= (int)damage;
                        hitTimer = 140;
                        return 1; // Hits the player
                    }
                    return 0; // Missed the player
                }
            }
        }

        // Types of zombies
        class minion : zombie // Basic zombie
        {
            public minion(double x_, double y_, double angle_) : base(x_, y_, angle_)
            {
                speed = 1;
                damage = 20;
                size = 25;
                health = 200;
                value = 50;
                image = Properties.Resources.zombiewithC;
            }
        }
        class runner : zombie // Fast high damage zombie
        {
            public runner(double x_, double y_, double angle_) : base(x_, y_, angle_)
            {
                speed = 1.5;
                damage = 50;
                size = 15;
                health = 50;
                value = 100;
                image = Properties.Resources.zombiewithC;
            }
        }

        class giant : zombie // Slow high health zombie
        {
            public giant(double x_, double y_, double angle_) : base(x_, y_, angle_)
            {
                speed = 0.5;
                damage = 10;
                size = 50;
                health = 800;
                value = 500;
                image = Properties.Resources.zombiewithC;
            }
        }

        class behemoth : zombie // Slow very high health zombie
        {
            public behemoth(double x_, double y_, double angle_) : base(x_, y_, angle_)
            {
                speed = 0.5;
                damage = 30;
                size = 100;
                health = 4000;
                image = Properties.Resources.zombiewithC;
            }
        }
        // Angle finding method
        public double findAngle(int x, int y, int xt, int yt)
        {
            // Finding angle in the 4 quadrants, with special cases for the 90 degree intervals just in case
            // Maybe should have used one line to calculate the atan part, then set it to negative or positive and did other operations in the ifs, but it works and doesn't take up much more processing time so its fine
            double angle = 0;
            if (xt > x && yt < y) // Quadrant 1
            {
                angle = -Math.Atan((double)(yt - y) / (double)(xt - x)) * 180 / Math.PI;
            }
            else if (xt < x && yt < y) // Quadrant 2
            {
                angle = 180 + Math.Atan((double)(yt - y) / (double)(-(xt - x))) * 180 / Math.PI;
            }
            else if (xt < x && yt > y) // Quadrant 3
            {
                angle = 180 - Math.Atan((double)(-(yt - y)) / (double)(-(xt - x))) * 180 / Math.PI;
            }
            else if (xt > x && yt > y) // Quadrant 4
            {
                angle = 360 + Math.Atan((double)(-(yt - y)) / (double)(xt - x)) * 180 / Math.PI;
            }
            else if (xt > x && yt == y) // Positive x axis
            {
                angle = 0;
            }
            else if (xt == x && yt < y) // Positive y axis
            {
                angle = 90;
            }
            else if (xt < x && yt == y) // Negative x axis
            {
                angle = 180;
            }
            else if (xt == x && yt > y) // Negative y axis
            {
                angle = 270;
            }
            return angle;
        }

        private void currentAmmo_Click(object sender, EventArgs e)
        {

        }

        // Unused because of bug
        private void deathSoundTimer_Tick(object sender, EventArgs e)
        {
            deathSoundDelay--;
            if (deathSoundDelay == 0)
            {
                zombieMoan.Play();
                deathSoundDelay = 6;
                deathSoundTimer.Enabled = false;
            }
        }

        // Waves
        private void waveTimer_Tick(object sender, EventArgs e)
        {
            delays[currentWave][currentEnemy]--; // Reducing timer
            if(delays[currentWave][currentEnemy] == 0) // Spawning in the zombie, and checking which type it is to find out which type to spawn
            {
                if(waves[currentWave][currentEnemy] == 1)
                {
                    zombies.Add(new minion(random2.Next(0, 600), -50, 0));
                }
                else if(waves[currentWave][currentEnemy] == 2)
                {
                    zombies.Add(new runner(random2.Next(0, 600), -50, 0));
                }
                else if (waves[currentWave][currentEnemy] == 3)
                {
                    zombies.Add(new giant(random2.Next(0, 600), -50, 0));
                }
                else if (waves[currentWave][currentEnemy] == 4)
                {
                    zombies.Add(new behemoth(random2.Next(0, 600), -50, 0));
                }
                // Initialzing image, same as above
                PictureBox tempPicture2 = new PictureBox();
                Controls.Add(tempPicture2);
                zombies[zombies.Count - 1].picture = tempPicture2;
                zombies[zombies.Count - 1].picture.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);
                zombies[zombies.Count - 1].picture.MouseUp += new MouseEventHandler(pictureBox2_MouseUp);
                zombies[zombies.Count - 1].picture.MouseMove += new MouseEventHandler(pictureBox2_MouseMove);
                zombies[zombies.Count - 1].initialize();
                if (currentEnemy < waves[currentWave].Length - 1) // Moving on to next enemy as long as its not the last in the wave
                {
                    currentEnemy++;
                }
                else // Moving on to next wave
                {
                    nextWave = true;
                }
            }
            if (currentWave < waves.Count - 1 && zombies.Count == 0 && nextWave) // Updating wave info
            {
                currentWave++;
                waveText.Text = "Wave " + Convert.ToString(currentWave + 1);
                currentEnemy = 0;
                nextWave = false;
            }
            if (currentWave == waves.Count - 1 && zombies.Count == 0 && nextWave) // Winning
            {
                hideSpawn();
                timer1.Enabled = true;
                waveTimer.Enabled = true;
                tabControl1.SelectedIndex = 4;
                scoreDisplay1.Text = "Final Score: " + Convert.ToString(money + (currentWave - 1) * 100);
                scoreDisplay2.Text = "Final Score: " + Convert.ToString(money + (currentWave - 1) * 100);
            }
        }
        // Hiding and showing spawned things when switching off game screen
        private void hideSpawn()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].picture.Visible = false;
            }
            for (int i = 0; i < hitmarkers.Count; i++)
            {
                hitmarkers[i].picture.Visible = false;
            }
            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].picture.Visible = false;
            }
        }
        private void showSpawn()
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].picture.Visible = true;
            }
            for (int i = 0; i < hitmarkers.Count; i++)
            {
                hitmarkers[i].picture.Visible = true;
            }
            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].picture.Visible = true;
            }
        }
        // Buying in store
        private void buyammoButton_Click(object sender, EventArgs e)
        {
            if(money >= gunList[selStoreI].pr2) // Buying ammo if you have enough money
            {
                money -= gunList[selStoreI].pr2;
                ammoReserves[selStoreI] += gunList[selStoreI].mag;
                moneyDisplay.Text = Convert.ToString(money);
                chaching.Play();
            }
        }

        private void buygunButton_Click(object sender, EventArgs e)
        {
            if (money >= gunList[selStoreI].pr1) // Buying gun if you have enough money
            {
                money -= gunList[selStoreI].pr1;
                guns[selStoreI] = 1;
                buygunButton.Visible = false;
                moneyDisplay.Text = Convert.ToString(money);
                chaching.Play();
                priceText.Text = "$" + Convert.ToString(gunList[selStoreI].pr2);
            }
        }

        // Restarting game
        private void restartButton1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void restartButton2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}

