const mongoose = require('mongoose');
const Country = require('../models/Country');
const config = require('../config/config');

const countries = [
    {
        name: 'Israel',
        code: 'IL',
        capital: 'Jerusalem',
        currency: 'ILS',
        language: 'Hebrew',
        location: {
            type: 'Point',
            coordinates: [35.2137, 31.7683]
        }
    },
    {
        name: 'United States',
        code: 'US',
        capital: 'Washington, D.C.',
        currency: 'USD',
        language: 'English',
        location: {
            type: 'Point',
            coordinates: [-77.0369, 38.9072]
        }
    },
    {
        name: 'United Kingdom',
        code: 'GB',
        capital: 'London',
        currency: 'GBP',
        language: 'English',
        location: {
            type: 'Point',
            coordinates: [-0.1276, 51.5074]
        }
    },
    {
        name: 'France',
        code: 'FR',
        capital: 'Paris',
        currency: 'EUR',
        language: 'French',
        location: {
            type: 'Point',
            coordinates: [2.3522, 48.8566]
        }
    },
    {
        name: 'Germany',
        code: 'DE',
        capital: 'Berlin',
        currency: 'EUR',
        language: 'German',
        location: {
            type: 'Point',
            coordinates: [13.4050, 52.5200]
        }
    },
    {
        name: 'Italy',
        code: 'IT',
        capital: 'Rome',
        currency: 'EUR',
        language: 'Italian',
        location: {
            type: 'Point',
            coordinates: [12.4964, 41.9028]
        }
    },
    {
        name: 'Spain',
        code: 'ES',
        capital: 'Madrid',
        currency: 'EUR',
        language: 'Spanish',
        location: {
            type: 'Point',
            coordinates: [-3.7038, 40.4168]
        }
    },
    {
        name: 'Japan',
        code: 'JP',
        capital: 'Tokyo',
        currency: 'JPY',
        language: 'Japanese',
        location: {
            type: 'Point',
            coordinates: [139.6917, 35.6895]
        }
    },
    {
        name: 'Australia',
        code: 'AU',
        capital: 'Canberra',
        currency: 'AUD',
        language: 'English',
        location: {
            type: 'Point',
            coordinates: [149.1300, -35.2809]
        }
    },
    {
        name: 'Canada',
        code: 'CA',
        capital: 'Ottawa',
        currency: 'CAD',
        language: 'English',
        location: {
            type: 'Point',
            coordinates: [-75.6972, 45.4215]
        }
    }
];

const seedCountries = async () => {
    try {
        await mongoose.connect(config.mongoURI);
        console.log('Connected to MongoDB');

        // Clear existing countries
        await Country.deleteMany({});
        console.log('Cleared existing countries');

        // Insert new countries
        await Country.insertMany(countries);
        console.log('Countries seeded successfully');

        process.exit(0);
    } catch (error) {
        console.error('Error seeding countries:', error);
        process.exit(1);
    }
};

seedCountries(); 