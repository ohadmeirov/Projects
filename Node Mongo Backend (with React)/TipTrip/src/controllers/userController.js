const User = require('../models/User');
const Post = require('../models/Post');
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const config = require('../config/config');
const { validationResult } = require('express-validator');

// @desc    Register user
// @route   POST /api/users/register
// @access  Public
exports.register = async (req, res) => {
    try {
        const { username, email, password } = req.body;

        // Check if user exists
        let user = await User.findOne({ email });
        if (user) {
            return res.status(400).json({ message: 'User already exists' });
        }

        // Create user
        user = new User({
            username,
            email,
            password
        });

        // Hash password
        const salt = await bcrypt.genSalt(10);
        user.password = await bcrypt.hash(password, salt);

        await user.save();

        // Create token
        const token = jwt.sign(
            { id: user._id },
            process.env.JWT_SECRET,
            { expiresIn: '30d' }
        );

        res.status(201).json({
            token,
            user: {
                id: user._id,
                username: user.username,
                email: user.email,
                avatar: user.avatar
            }
        });
    } catch (error) {
        console.error('Error registering user:', error);
        res.status(500).json({ message: 'Error registering user' });
    }
};

// @desc    Login user
// @route   POST /api/users/login
// @access  Public
exports.login = async (req, res) => {
    try {
        const { email, password } = req.body;

        // Check if user exists
        const user = await User.findOne({ email });
        if (!user) {
            return res.status(400).json({ message: 'Invalid credentials' });
        }

        // Check password
        const isMatch = await bcrypt.compare(password, user.password);
        if (!isMatch) {
            return res.status(400).json({ message: 'Invalid credentials' });
        }

        // Create token
        const token = jwt.sign(
            { id: user._id },
            process.env.JWT_SECRET,
            { expiresIn: '30d' }
        );

        res.json({
            token,
            user: {
                id: user._id,
                username: user.username,
                email: user.email,
                avatar: user.avatar
            }
        });
    } catch (error) {
        console.error('Error logging in:', error);
        res.status(500).json({ message: 'Error logging in' });
    }
};

// @desc    Get current user
// @route   GET /api/users/me
// @access  Private
exports.getCurrentUser = async (req, res) => {
    try {
        const user = await User.findById(req.user.id).select('-password');
        res.json(user);
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
};

// @desc    Get user by ID
// @route   GET /api/users/:id
// @access  Private
exports.getUserById = async (req, res) => {
    try {
        const user = await User.findById(req.params.id)
            .select('-password')
            .populate('followers', 'username avatar')
            .populate('following', 'username avatar');

        if (!user) {
            return res.status(404).json({ message: 'User not found' });
        }

        res.json(user);
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
};

// @desc    Get user posts
// @route   GET /api/users/posts
// @access  Private
exports.getUserPosts = async (req, res) => {
    try {
        const posts = await Post.find({ author: req.user.id })
            .populate({
                path: 'country',
                select: 'name code flag',
                model: 'Country'
            })
            .sort({ createdAt: -1 });

        res.json(posts);
    } catch (error) {
        console.error('Error fetching user posts:', error);
        res.status(500).json({ message: 'Error fetching user posts' });
    }
};

// @desc    Get user profile
// @route   GET /api/users/profile
// @access  Private
exports.getProfile = async (req, res) => {
    try {
        const user = await User.findById(req.user.id).select('-password');
        if (!user) {
            return res.status(404).json({ message: 'User not found' });
        }
        res.json(user);
    } catch (error) {
        console.error('Error fetching profile:', error);
        res.status(500).json({ message: 'Error fetching profile' });
    }
};

// @desc    Update user profile
// @route   PUT /api/users/profile
// @access  Private
exports.updateProfile = async (req, res) => {
    try {
        const user = await User.findById(req.user.id);
        if (!user) {
            return res.status(404).json({ message: 'User not found' });
        }
        // Update username if provided
        if (req.body.username && req.body.username !== user.username) {
            user.username = req.body.username;
        }
        // Update email if provided
        if (req.body.email && req.body.email !== user.email) {
            user.email = req.body.email;
        }
        // Update avatar if provided
        if (req.body.avatar && req.body.avatar !== user.avatar) {
            user.avatar = req.body.avatar;
        }

        await user.save();
        res.json(user);
    } catch (error) {
        res.status(500).json({ message: 'Error updating profile' });
    }
};

// @desc    Follow user
// @route   POST /api/users/follow/:id
// @access  Private
exports.followUser = async (req, res) => {
    try {
        const userToFollow = await User.findById(req.params.id);
        const currentUser = await User.findById(req.user.id);

        if (!userToFollow) {
            return res.status(404).json({ message: 'User not found' });
        }

        // Check if already following
        if (currentUser.following.includes(req.params.id)) {
            return res.status(400).json({ message: 'Already following this user' });
        }

        // Add to following
        currentUser.following.push(req.params.id);
        await currentUser.save();

        // Add to followers
        userToFollow.followers.push(req.user.id);
        await userToFollow.save();

        res.json({ message: 'User followed successfully' });
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
};

// @desc    Unfollow user
// @route   POST /api/users/unfollow/:id
// @access  Private
exports.unfollowUser = async (req, res) => {
    try {
        const userToUnfollow = await User.findById(req.params.id);
        const currentUser = await User.findById(req.user.id);

        if (!userToUnfollow) {
            return res.status(404).json({ message: 'User not found' });
        }

        // Check if following
        if (!currentUser.following.includes(req.params.id)) {
            return res.status(400).json({ message: 'Not following this user' });
        }

        // Remove from following
        currentUser.following = currentUser.following.filter(
            id => id.toString() !== req.params.id
        );
        await currentUser.save();

        // Remove from followers
        userToUnfollow.followers = userToUnfollow.followers.filter(
            id => id.toString() !== req.user.id
        );
        await userToUnfollow.save();

        res.json({ message: 'User unfollowed successfully' });
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
};

// Get all users (admin only)
exports.getAllUsers = async (req, res) => {
    try {
        const users = await User.find().select('-password');
        res.json(users);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

// Delete user
exports.deleteUser = async (req, res) => {
    try {
        await User.findByIdAndDelete(req.user.id);
        res.json({ message: 'User deleted successfully' });
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

// Add/Remove favorite country
exports.toggleFavoriteCountry = async (req, res) => {
    try {
        const { countryId } = req.body;
        const user = await User.findById(req.user.id);

        if (!user) {
            return res.status(404).json({ message: 'User not found' });
        }

        const index = user.favoriteCountries.indexOf(countryId);
        if (index === -1) {
            user.favoriteCountries.push(countryId);
        } else {
            user.favoriteCountries.splice(index, 1);
        }

        await user.save();
        res.json(user.favoriteCountries);
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server Error');
    }
};
