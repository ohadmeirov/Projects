const express = require('express');
const router = express.Router();
const { auth } = require('../middleware/auth');
const { userValidation } = require('../middleware/validation');
const {
    register,
    login,
    getCurrentUser,
    updateProfile,
    followUser,
    unfollowUser,
    getUserById,
    getUserPosts
} = require('../controllers/userController');

// @route   POST api/users/register
// @desc    Register user
// @access  Public
router.post('/register', userValidation, register);

// @route   POST api/users/login
// @desc    Login user
// @access  Public
router.post('/login', userValidation, login);

// @route   GET api/users/me
// @desc    Get current user
// @access  Private
router.get('/me', auth, getCurrentUser);

// @route   GET api/users/:id
// @desc    Get user by ID
// @access  Private
router.get('/:id', auth, getUserById);

// @route   GET api/users/:id/posts
// @desc    Get user posts
// @access  Private
router.get('/:id/posts', auth, getUserPosts);

// @route   PUT api/users/profile
// @desc    Update user profile
// @access  Private
router.put('/profile', auth, updateProfile);

// @route   POST api/users/follow/:id
// @desc    Follow user
// @access  Private
router.post('/follow/:id', auth, followUser);

// @route   POST api/users/unfollow/:id
// @desc    Unfollow user
// @access  Private
router.post('/unfollow/:id', auth, unfollowUser);

module.exports = router;
