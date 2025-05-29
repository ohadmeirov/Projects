const { check } = require('express-validator');

// Validation for posts
exports.postValidation = [
    check('title', 'Title is required').not().isEmpty(),
    check('content', 'Content is required').not().isEmpty(),
    check('country', 'Country is required').not().isEmpty(),
    check('rating', 'Rating is required').isFloat({ min: 1, max: 5 }),
    check('location.coordinates', 'Location coordinates are required').isArray(),
    check('location.coordinates', 'Location must have longitude and latitude').isLength({ min: 2, max: 2 })
];

// Validation for users
exports.userValidation = [
    check('name', 'Name is required').not().isEmpty(),
    check('email', 'Please include a valid email').isEmail(),
    check('password', 'Please enter a password with 6 or more characters').isLength({ min: 6 })
];

// Validation for countries
exports.countryValidation = [
    check('name', 'Name is required').not().isEmpty(),
    check('code', 'Country code is required').not().isEmpty(),
    check('code', 'Country code must be 2 characters').isLength({ min: 2, max: 2 }),
    check('capital', 'Capital is required').not().isEmpty(),
    check('currency', 'Currency is required').not().isEmpty(),
    check('language', 'Language is required').not().isEmpty(),
    check('location.coordinates', 'Location coordinates are required').isArray(),
    check('location.coordinates', 'Location must have longitude and latitude').isLength({ min: 2, max: 2 })
];
