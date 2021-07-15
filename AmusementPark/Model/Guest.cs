using AmusementPark.Model.Entities;
using System;
using System.Collections.Generic;

namespace AmusementPark.Model
{
    [Serializable]
    public class Guest
    {
        public Entity Target { get; set; }
        public (int, int) TargetXY { get; set; }
        public int Happiness;
        public int Hungriness;
        public int Money;
        public bool IsWantGoHome { get { return Happiness == 0 || Money <= 0; } }
        public (int, int) CurrentPosition { get; set; }
        public List<(int, int)> Path { get; set; } = new List<(int, int)>();

        public Guest() // constructor of the class guest
        {
            Happiness = 100; // set properties
            Hungriness = 100;
            Random random = new Random();
            Money = random.Next(0, 501);
        }

        public void ChangeHungriness(int amount) // method to increase the guest's hungirenss
        {
            Hungriness = Hungriness + amount;

            if (Hungriness < 0)
            {
                Hungriness = 0;
            }
        }

        public void ChangeHappiness(int amount) // method to change the guest's happiness
        {
            Happiness = Happiness - 1;

            if (Hungriness < 75)
            {
                Happiness = Happiness - 1;
            }
            else if (Hungriness < 50)
            {
                Happiness = Happiness - 2;
            }
            else if (Hungriness < 25)
            {
                Happiness = Happiness - 3;
            }

            Happiness = Happiness + amount;

            if (Happiness < 0)
            {
                Happiness = 0;
            }

            if (Happiness > 100)
            {
                Happiness = 100;
            }
        }

        public void Pay(int money) // method to lower the guest's money
        {
            if (money > Money)
            {
                return;
            }
            Money = Money - money;
        }

        public bool IsWilling(int money, bool isEntrance = false) //check and follow the guest's  mood and will
        {
            if (money > Money)
            {
                return false;
            }

            if (isEntrance)
            {
                return true;
            }


            int additionalChance = 0;
            if (money <= 5)
            {
                additionalChance = 95;
            }
            else if (money > 5 && money <= 25)
            {
                additionalChance = 75;
            }
            else if (money > 25 && money <= 75)
            {
                additionalChance = 35;
            }
            else if (money > 75 && money <= 100)
            {
                additionalChance = 2;
            }

            Random random = new Random();
            int chance = random.Next(0, 101);

            if (!(chance <= 5 + additionalChance))
            {
                return false;
            }

            return true;
        }
    }
}
