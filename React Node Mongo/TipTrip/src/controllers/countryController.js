const Country = require('../models/Country');
const Post = require('../models/Post');
const { validationResult } = require('express-validator');

// @desc    Get all countries
// @route   GET /api/countries
// @access  Public
exports.getCountries = async (req, res) => {
    try {
        const countries = await Country.find()
            .populate({
                path: 'posts',
                select: 'title content rating createdAt',
                model: 'Post',
                options: { sort: { createdAt: -1 } }
            });

        res.json(countries);
    } catch (error) {
        console.error('Error fetching countries:', error);
        res.status(500).json({ message: 'Error fetching countries' });
    }
};

// @desc    Get country by ID
// @route   GET /api/countries/:id
// @access  Public
exports.getCountry = async (req, res) => {
    try {
        const country = await Country.findById(req.params.id)
            .populate({
                path: 'posts',
                select: 'title content rating createdAt author',
                populate: {
                    path: 'author',
                    select: 'username avatar',
                    model: 'User'
                },
                model: 'Post',
                options: { sort: { createdAt: -1 } }
            });

        if (!country) {
            return res.status(404).json({ message: 'Country not found' });
        }

        res.json(country);
    } catch (error) {
        console.error('Error fetching country:', error);
        res.status(500).json({ message: 'Error fetching country' });
    }
};

// @desc    Create country
// @route   POST /api/countries
// @access  Private
exports.createCountry = async (req, res) => {
    try {
        const { name, code, flag, description, location, population, capital, languages, currency, timezone, language } = req.body;

        const country = new Country({
            name,
            code,
            flag,
            description,
            location,
            population,
            capital,
            languages,
            currency,
            timezone,
            language
        });

        await country.save();
        res.status(201).json(country);
    } catch (error) {
        console.error('Error creating country:', error);
        res.status(500).json({ message: 'Error creating country' });
    }
};

// @desc    Update country
// @route   PUT /api/countries/:id
// @access  Private
exports.updateCountry = async (req, res) => {
    try {
        const { name, code, flag, description, location, population, capital, languages, currency, timezone, language } = req.body;
        const country = await Country.findById(req.params.id);

        if (!country) {
            return res.status(404).json({ message: 'Country not found' });
        }

        country.name = name || country.name;
        country.code = code || country.code;
        country.flag = flag || country.flag;
        country.description = description || country.description;
        country.location = location || country.location;
        country.population = population || country.population;
        country.capital = capital || country.capital;
        country.languages = languages || country.languages;
        country.currency = currency || country.currency;
        country.timezone = timezone || country.timezone;
        country.language = language || country.language;

        await country.save();
        res.json(country);
    } catch (error) {
        console.error('Error updating country:', error);
        res.status(500).json({ message: 'Error updating country' });
    }
};

// @desc    Delete country
// @route   DELETE /api/countries/:id
// @access  Private
exports.deleteCountry = async (req, res) => {
    try {
        const country = await Country.findById(req.params.id);

        if (!country) {
            return res.status(404).json({ message: 'Country not found' });
        }

        // Delete all posts associated with this country
        await Post.deleteMany({ country: country._id });

        await country.remove();
        res.json({ message: 'Country removed' });
    } catch (error) {
        console.error('Error deleting country:', error);
        res.status(500).json({ message: 'Error deleting country' });
    }
};

// @desc    Search countries
// @route   GET /api/countries/search
// @access  Public
exports.searchCountries = async (req, res) => {
    try {
        const { query } = req.query;
        const searchQuery = {};

        if (query) {
            searchQuery.$or = [
                { name: { $regex: query, $options: 'i' } },
                { description: { $regex: query, $options: 'i' } }
            ];
        }

        const countries = await Country.find(searchQuery)
            .populate({
                path: 'posts',
                select: 'title content rating createdAt',
                model: 'Post',
                options: { sort: { createdAt: -1 } }
            });

        res.json(countries);
    } catch (error) {
        console.error('Error searching countries:', error);
        res.status(500).json({ message: 'Error searching countries' });
    }
};

// Get countries by location
exports.getCountriesByLocation = async (req, res) => {
    try {
        const { longitude, latitude, maxDistance } = req.query;

        const countries = await Country.find({
            location: {
                $near: {
                    $geometry: {
                        type: 'Point',
                        coordinates: [Number(longitude), Number(latitude)]
                    },
                    $maxDistance: Number(maxDistance) || 1000000 // Default 1000km
                }
            }
        });

        res.json(countries);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};
