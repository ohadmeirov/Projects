const express = require('express');
const router = express.Router();
const {
    createPost,
    getPosts,
    getPost,
    updatePost,
    deletePost,
    toggleLike,
    addComment,
    toggleCommentLike,
    searchPosts
} = require('../controllers/postController');
const auth = require('../middleware/auth');

// @route   GET /api/posts/search
// @desc    Search posts
// @access  Public
router.get('/search', searchPosts);

// @route   POST /api/posts
// @desc    Create a post
// @access  Private
router.post('/', auth, createPost);

// @route   GET /api/posts
// @desc    Get all posts
// @access  Public
router.get('/', getPosts);

// @route   GET /api/posts/:id
// @desc    Get post by ID
// @access  Public
router.get('/:id', getPost);

// @route   PUT /api/posts/:id
// @desc    Update post
// @access  Private
router.put('/:id', auth, updatePost);

// @route   DELETE /api/posts/:id
// @desc    Delete post
// @access  Private
router.delete('/:id', auth, deletePost);

// @route   POST /api/posts/:id/like
// @desc    Toggle like on post
// @access  Private
router.post('/:id/like', auth, toggleLike);

// @route   POST /api/posts/:id/comment
// @desc    Add comment to post
// @access  Private
router.post('/:id/comment', auth, addComment);

// @route   POST /api/posts/:postId/comments/:commentId/like
// @desc    Toggle like on comment
// @access  Private
router.post('/:postId/comments/:commentId/like', auth, toggleCommentLike);

module.exports = router;
