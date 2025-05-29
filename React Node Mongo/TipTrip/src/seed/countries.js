const Country = require('../models/Country');
const axios = require('axios');

const seedCountries = async () => {
    try {
        // Clear existing countries
        await Country.deleteMany({});

        // Fetch all countries from REST Countries API
        const response = await axios.get('https://restcountries.com/v3.1/all');
        const countries = response.data.map(country => ({
            name: country.name.common,
            code: country.cca2,
            description: country.flags.alt || `Information about ${country.name.common}`,
            capital: country.capital?.[0] || 'N/A',
            languages: Object.values(country.languages || {}),
            currency: Object.keys(country.currencies || {})[0] || 'N/A',
            timezone: country.timezones?.[0] || 'UTC',
            language: Object.values(country.languages || {})[0] || 'English',
            location: {
                type: 'Point',
                coordinates: [
                    country.latlng?.[1] || 0,
                    country.latlng?.[0] || 0
                ]
            }
        }));

        // Insert all countries
        await Country.insertMany(countries);
        console.log(`Successfully seeded ${countries.length} countries`);
    } catch (error) {
        console.error('Error seeding countries:', error);
    }
};

module.exports = seedCountries;
