const express = require('express');
const router = express.Router();
const { check } = require('express-validator');
const { auth } = require('../middleware/auth');
const {
    createPost,
    getAllPosts,
    getPostById,
    updatePost,
    deletePost,
    toggleLike,
    addComment,
    searchPosts,
    getPosts,
    getPost
} = require('../controllers/postController');
const { postValidation } = require('../middleware/validation');
const { validationResult } = require('express-validator');
const Post = require('../models/Post');
const User = require('../models/User');

// Validation middleware
const commentValidation = [
    check('content', 'Comment content is required').not().isEmpty()
];

// Routes
router.post('/', auth, postValidation, createPost);
router.get('/', getPosts);
router.get('/search/:query', searchPosts);
router.get('/:id', getPost);
router.put('/:id', auth, postValidation, updatePost);
router.delete('/:id', auth, deletePost);
router.post('/:id/like', auth, toggleLike);
router.post('/:id/comment', auth, commentValidation, addComment);

// @route   GET api/posts
// @desc    Get all posts with filtering and sorting
// @access  Public
router.get('/', async (req, res) => {
    try {
        const { country, search, sort } = req.query;
        let query = {};

        // Filter by country if specified
        if (country) {
            query.country = country;
        }

        // Search in title and content if search term provided
        if (search) {
            query.$or = [
                { title: { $regex: search, $options: 'i' } },
                { content: { $regex: search, $options: 'i' } }
            ];
        }

        // Build sort object
        let sortObj = {};
        switch (sort) {
            case 'oldest':
                sortObj = { createdAt: 1 };
                break;
            case 'rating':
                sortObj = { rating: -1 };
                break;
            default: // newest
                sortObj = { createdAt: -1 };
        }

        const posts = await Post.find(query)
            .sort(sortObj)
            .populate('user', 'name')
            .populate('country', 'name code')
            .populate({
                path: 'comments',
                populate: {
                    path: 'user',
                    select: 'name'
                }
            });

        res.json(posts);
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server Error');
    }
});

// @route   POST api/posts
// @desc    Create a post
// @access  Private
router.post('/', auth, postValidation, async (req, res) => {
    try {
        const newPost = new Post({
            ...req.body,
            user: req.user.id
        });

        const post = await newPost.save();
        await post.populate('user', 'name');
        await post.populate('country', 'name code');

        res.json(post);
    } catch (err) {
        console.error(err.message);
        res.status(500).send('Server Error');
    }
});

// @route   GET api/posts/:id
// @desc    Get post by ID
// @access  Public
router.get('/:id', async (req, res) => {
    try {
        const post = await Post.findById(req.params.id)
            .populate('user', 'name')
            .populate('country', 'name code')
            .populate({
                path: 'comments',
                populate: {
                    path: 'user',
                    select: 'name'
                }
            });

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        res.json(post);
    } catch (err) {
        console.error(err.message);
        if (err.kind === 'ObjectId') {
            return res.status(404).json({ message: 'Post not found' });
        }
        res.status(500).send('Server Error');
    }
});

// @route   PUT api/posts/:id
// @desc    Update a post
// @access  Private
router.put('/:id', auth, postValidation, async (req, res) => {
    try {
        let post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        // Check user
        if (post.user.toString() !== req.user.id) {
            return res.status(401).json({ message: 'User not authorized' });
        }

        post = await Post.findByIdAndUpdate(
            req.params.id,
            { $set: req.body },
            { new: true }
        )
        .populate('user', 'name')
        .populate('country', 'name code');

        res.json(post);
    } catch (err) {
        console.error(err.message);
        if (err.kind === 'ObjectId') {
            return res.status(404).json({ message: 'Post not found' });
        }
        res.status(500).send('Server Error');
    }
});

// @route   DELETE api/posts/:id
// @desc    Delete a post
// @access  Private
router.delete('/:id', auth, async (req, res) => {
    try {
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        // Check user
        if (post.user.toString() !== req.user.id) {
            return res.status(401).json({ message: 'User not authorized' });
        }

        await post.remove();
        res.json({ message: 'Post removed' });
    } catch (err) {
        console.error(err.message);
        if (err.kind === 'ObjectId') {
            return res.status(404).json({ message: 'Post not found' });
        }
        res.status(500).send('Server Error');
    }
});

// @route   POST api/posts/:id/like
// @desc    Like/Unlike a post
// @access  Private
router.post('/:id/like', auth, async (req, res) => {
    try {
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        // Check if the post has already been liked by this user
        if (post.likes.includes(req.user.id)) {
            // Unlike
            post.likes = post.likes.filter(like => like.toString() !== req.user.id);
        } else {
            // Like
            post.likes.push(req.user.id);
        }

        await post.save();
        res.json(post.likes);
    } catch (err) {
        console.error(err.message);
        if (err.kind === 'ObjectId') {
            return res.status(404).json({ message: 'Post not found' });
        }
        res.status(500).send('Server Error');
    }
});

// @route   POST api/posts/:id/comment
// @desc    Comment on a post
// @access  Private
router.post('/:id/comment', auth, [
    check('text', 'Text is required').not().isEmpty()
], async (req, res) => {
    const errors = validationResult(req);
    if (!errors.isEmpty()) {
        return res.status(400).json({ errors: errors.array() });
    }

    try {
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        const newComment = {
            text: req.body.text,
            user: req.user.id
        };

        post.comments.unshift(newComment);
        await post.save();

        await post.populate({
            path: 'comments',
            populate: {
                path: 'user',
                select: 'name'
            }
        });

        res.json(post.comments);
    } catch (err) {
        console.error(err.message);
        if (err.kind === 'ObjectId') {
            return res.status(404).json({ message: 'Post not found' });
        }
        res.status(500).send('Server Error');
    }
});

module.exports = router;
