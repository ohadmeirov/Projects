const express = require('express');
const router = express.Router();
const { register, login, getProfile, updateProfile, getUserPosts } = require('../controllers/userController');
const auth = require('../middleware/auth');

// @route   POST /api/users/register
// @desc    Register user
// @access  Public
router.post('/register', register);

// @route   POST /api/users/login
// @desc    Login user
// @access  Public
router.post('/login', login);

// @route   GET /api/users/profile
// @desc    Get user profile
// @access  Private
router.get('/profile', auth, getProfile);

// @route   PUT /api/users/profile
// @desc    Update user profile
// @access  Private
router.put('/profile', auth, updateProfile);

// @route   GET /api/users/posts
// @desc    Get user posts
// @access  Private
router.get('/posts', auth, getUserPosts);

module.exports = router;
