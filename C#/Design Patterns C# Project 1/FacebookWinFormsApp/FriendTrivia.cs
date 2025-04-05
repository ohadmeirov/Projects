using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    class FriendTrivia
    {
        private User m_LoggedInUser;
        private Random m_Random = new Random();
        private const int k_OptionsCount = 5;
        private readonly ICollection<User> r_Friends;
        private readonly List<City> r_Cities = new List<City>();

        public User ChosenFriend { get; private set; }
        public List<DateTime> DateOptions { get; private set; } = new List<DateTime>();
        public List<User.eRelationshipStatus> RelationStatusOptions { get; private set; } = new List<User.eRelationshipStatus>();
        public List<int> AgeOptions { get; private set; } = new List<int>();
        public List<User.eGender> GenderOptions { get; private set; } = new List<User.eGender>();
        public List<City> CitiesOptions { get; private set; } = new List<City>();

        public FriendTrivia(User i_LoggedInUser)
        {
            m_LoggedInUser = i_LoggedInUser;
            r_Friends = m_LoggedInUser.Friends;
            initUserData();
        }

        private void initUserData()
        {
            initAvailableCities();
            initGenderOptions();
        }

        public void StartGame()
        {
            ChosenFriend = getRandomFriend(r_Friends);

            if (ChosenFriend != null)
            {
                initOptions();
            }
        }

        public bool GuessDate(DateTime i_Date)
        {
            bool result = false;

            if (i_Date == DateTime.Parse(ChosenFriend.Birthday))
            {
                result = true;
            }

            return result;
        }

        public bool GuessAge(int i_Age)
        {
            bool result = false;
            DateTime chosenUserBirthDay = DateTime.Parse(ChosenFriend.Birthday);

            if (i_Age == calculateAge(chosenUserBirthDay))
            {
                result = true;
            }

            return result;
        }

        public bool GuessRelationStatus(User.eRelationshipStatus i_RelationStatus)
        {
            bool result = false;

            if (i_RelationStatus == ChosenFriend.RelationshipStatus)
            {
                result = true;
            }

            return result;
        }

        public bool GuessCity(City i_City)
        {
            bool result = false;

            if (i_City == ChosenFriend.Location)
            {
                result = true;
            }

            return result;
        }

        public bool GuessGender(User.eGender i_Gender)
        {
            bool result = false;

            if (i_Gender == ChosenFriend.Gender)
            {
                result = true;
            }

            return result;
        }

        private void initOptions()
        {
            initDatesOptions();
            initRelationStatusOptions();
            initAgeOptions();
            initCitiesOptions();
            addChosenFriendToOptions();
        }

        private void addChosenFriendToOptions()
        {
            DateTime chosenFriendBirthday = DateTime.Parse(ChosenFriend.Birthday);
            int chosenFriendAge = calculateAge(chosenFriendBirthday);

            DateOptions.Add(chosenFriendBirthday);
            RelationStatusOptions.Add(ChosenFriend.RelationshipStatus.Value);
            AgeOptions.Add(chosenFriendAge);
            CitiesOptions.Add(ChosenFriend.Location);
        }

        private void initDatesOptions()
        {
            DateTime chosenFriendBirthday = DateTime.Parse(ChosenFriend.Birthday);
            DateTime randomDate;

            for (int i = 0; i < k_OptionsCount; i++)
            {
                randomDate = getRandomDate(chosenFriendBirthday.Year);
                DateOptions.Add(randomDate);
            }
        }

        private void initRelationStatusOptions()
        {
            User.eRelationshipStatus randomStatus;

            for (int i = 0; i < k_OptionsCount; i++)
            {
                randomStatus = getRandomRelationshipStatus();
                RelationStatusOptions.Add(randomStatus);
            }
        }

        private void initAgeOptions()
        {
            int randomAge;

            for (int i = 0; i < k_OptionsCount; i++)
            {

                randomAge = m_Random.Next(18, 60);
                AgeOptions.Add(randomAge);
            }
        }

        private void initGenderOptions()
        {
            GenderOptions.Add(User.eGender.male);
            GenderOptions.Add(User.eGender.female);
        }

        private void initCitiesOptions()
        {
            City randomCity;

            if (r_Cities.Count > 0)
            {
                for (int i = 0; i < k_OptionsCount; i++)
                {
                    randomCity = r_Cities[m_Random.Next(r_Cities.Count)];
                    CitiesOptions.Add(randomCity);
                }
            }
        }

        private User getRandomFriend(ICollection<User> i_Friends)
        {
            User result;
            int randomIndex;

            if (i_Friends == null || i_Friends.Count == 0)
            {
                result = null;

            }
            else
            {
                randomIndex = m_Random.Next(i_Friends.Count);
                result = i_Friends.ElementAt(randomIndex);
            }

            return result;
        }

        private DateTime getRandomDate(int i_Year)
        {
            int month = m_Random.Next(1, 13);
            int daysInMonth = DateTime.DaysInMonth(i_Year, month);
            int day = m_Random.Next(1, daysInMonth + 1);

            return new DateTime(i_Year, month, day);
        }

        private User.eRelationshipStatus getRandomRelationshipStatus()
        {
            Array values = Enum.GetValues(typeof(User.eRelationshipStatus));
            User.eRelationshipStatus randomStatus = (User.eRelationshipStatus)values.GetValue(m_Random.Next(values.Length));

            return randomStatus;
        }


        private void initAvailableCities()
        {
            foreach (User friend in m_LoggedInUser.Friends)
            {
                if (friend != ChosenFriend)
                {
                    r_Cities.Append(friend.Location);
                }
            }
        }

        private static int calculateAge(DateTime i_BirthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - i_BirthDate.Year;

            if (i_BirthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}