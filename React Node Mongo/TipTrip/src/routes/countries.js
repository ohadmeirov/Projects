const express = require('express');
const router = express.Router();
const { auth } = require('../middleware/auth');
const { countryValidation } = require('../middleware/validation');
const {
    getCountries,
    getCountry,
    searchCountries,
    createCountry,
    updateCountry,
    deleteCountry
} = require('../controllers/countryController');

// Get all countries
router.get('/', getCountries);

// Get single country
router.get('/:id', getCountry);

// Search countries
router.get('/search/:query', searchCountries);

// @route   POST api/countries
// @desc    Create a country
// @access  Private
router.post('/', auth, countryValidation, createCountry);

// @route   PUT api/countries/:id
// @desc    Update a country
// @access  Private
router.put('/:id', auth, countryValidation, updateCountry);

// @route   DELETE api/countries/:id
// @desc    Delete a country
// @access  Private
router.delete('/:id', auth, deleteCountry);

module.exports = router;
