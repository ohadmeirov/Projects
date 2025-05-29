const Post = require('../models/Post');
const Country = require('../models/Country');
const { validationResult } = require('express-validator');
const User = require('../models/User');

// @desc    Create a post
// @route   POST /api/posts
// @access  Private
exports.createPost = async (req, res) => {
    try {
        const { title, content, country, rating, mediaUrl, location } = req.body;

        // Validate country exists
        const countryExists = await Country.findById(country);
        if (!countryExists) {
            return res.status(400).json({ message: 'Country not found' });
        }

        const post = new Post({
            title,
            content,
            author: req.user.id,
            country,
            rating,
            mediaUrl,
            location: location || { type: 'Point', coordinates: [0, 0] }
        });

        await post.save();

        // Add post to country's posts array
        countryExists.posts.push(post._id);
        await countryExists.save();

        await post.populate('author', 'username avatar');
        await post.populate('country', 'name');

        res.status(201).json(post);
    } catch (error) {
        console.error('Error creating post:', error);
        res.status(500).json({ message: 'Error creating post' });
    }
};

// @desc    Get all posts
// @route   GET /api/posts
// @access  Public
exports.getPosts = async (req, res) => {
    try {
        const posts = await Post.find()
            .populate({
                path: 'author',
                select: 'username avatar',
                model: 'User'
            })
            .populate({
                path: 'country',
                select: 'name code flag',
                model: 'Country'
            })
            .sort({ createdAt: -1 });

        if (!posts) {
            return res.status(404).json({ message: 'No posts found' });
        }

        res.json(posts);
    } catch (error) {
        console.error('Error fetching posts:', error);
        res.status(500).json({ message: 'Error fetching posts' });
    }
};

// @desc    Get post by ID
// @route   GET /api/posts/:id
// @access  Public
exports.getPost = async (req, res) => {
    try {
        const post = await Post.findById(req.params.id)
            .populate({
                path: 'author',
                select: 'username avatar',
                model: 'User'
            })
            .populate({
                path: 'country',
                select: 'name code flag',
                model: 'Country'
            })
            .populate({
                path: 'comments.user',
                select: 'username avatar',
                model: 'User'
            });

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        res.json(post);
    } catch (error) {
        console.error('Error fetching post:', error);
        res.status(500).json({ message: 'Error fetching post' });
    }
};

// @desc    Update post
// @route   PUT /api/posts/:id
// @access  Private
exports.updatePost = async (req, res) => {
    try {
        const { title, content, country, rating, mediaUrl, location } = req.body;
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        // Check if user owns the post
        if (post.author.toString() !== req.user.id) {
            return res.status(401).json({ message: 'Not authorized' });
        }

        // If country is being updated, validate it exists
        if (country && country !== post.country.toString()) {
            const countryExists = await Country.findById(country);
            if (!countryExists) {
                return res.status(400).json({ message: 'Country not found' });
            }

            // Remove post from old country's posts array
            const oldCountry = await Country.findById(post.country);
            if (oldCountry) {
                oldCountry.posts = oldCountry.posts.filter(p => p.toString() !== post._id.toString());
                await oldCountry.save();
            }

            // Add post to new country's posts array
            countryExists.posts.push(post._id);
            await countryExists.save();
        }

        post.title = title || post.title;
        post.content = content || post.content;
        post.country = country || post.country;
        post.rating = rating || post.rating;
        post.mediaUrl = mediaUrl || post.mediaUrl;
        post.location = location || post.location;

        await post.save();
        await post.populate('author', 'username avatar');
        await post.populate('country', 'name');

        res.json(post);
    } catch (error) {
        console.error('Error updating post:', error);
        res.status(500).json({ message: 'Error updating post' });
    }
};

// @desc    Delete post
// @route   DELETE /api/posts/:id
// @access  Private
exports.deletePost = async (req, res) => {
    try {
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        // Check if user owns the post
        if (post.author.toString() !== req.user.id) {
            return res.status(401).json({ message: 'Not authorized' });
        }

        // Remove post from country's posts array
        const country = await Country.findById(post.country);
        if (country) {
            country.posts = country.posts.filter(p => p.toString() !== post._id.toString());
            await country.save();
        }

        await Post.findByIdAndDelete(req.params.id);
        res.json({ message: 'Post removed' });
    } catch (error) {
        console.error('Error deleting post:', error);
        res.status(500).json({ message: 'Error deleting post' });
    }
};

// @desc    Toggle like on post
// @route   POST /api/posts/:id/like
// @access  Private
exports.toggleLike = async (req, res) => {
    try {
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        const likeIndex = post.likes.indexOf(req.user.id);
        if (likeIndex === -1) {
            post.likes.push(req.user.id);
        } else {
            post.likes.splice(likeIndex, 1);
        }

        await post.save();
        res.json({
            likes: post.likes,
            likesCount: post.likesCount
        });
    } catch (error) {
        console.error('Error toggling like:', error);
        res.status(500).json({ message: 'Error toggling like' });
    }
};

// @desc    Add comment to post
// @route   POST /api/posts/:id/comment
// @access  Private
exports.addComment = async (req, res) => {
    try {
        const { text } = req.body;
        const post = await Post.findById(req.params.id);

        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        if (!text || text.trim() === '') {
            return res.status(400).json({ message: 'Comment text is required' });
        }

        const comment = {
            user: req.user.id,
            text: text.trim(),
            likes: []
        };

        post.comments.unshift(comment);
        await post.save();

        // Populate the new comment with user details
        await post.populate({
            path: 'comments.user',
            select: 'username avatar',
            model: 'User'
        });

        // Return the new comment
        const newComment = post.comments[0];
        res.json({
            comment: newComment,
            message: 'Comment added successfully'
        });
    } catch (error) {
        console.error('Error adding comment:', error);
        res.status(500).json({ message: 'Error adding comment' });
    }
};

// @desc    Toggle like on comment
// @route   POST /api/posts/:postId/comments/:commentId/like
// @access  Private
exports.toggleCommentLike = async (req, res) => {
    try {
        const post = await Post.findById(req.params.postId);
        if (!post) {
            return res.status(404).json({ message: 'Post not found' });
        }

        const comment = post.comments.id(req.params.commentId);
        if (!comment) {
            return res.status(404).json({ message: 'Comment not found' });
        }

        const likeIndex = comment.likes.indexOf(req.user.id);
        if (likeIndex === -1) {
            comment.likes.push(req.user.id);
        } else {
            comment.likes.splice(likeIndex, 1);
        }

        await post.save();

        // Populate the comment with user details
        await post.populate({
            path: 'comments.user',
            select: 'username avatar',
            model: 'User'
        });

        // Return the updated comment
        const updatedComment = post.comments.id(req.params.commentId);
        res.json({
            comment: updatedComment,
            message: likeIndex === -1 ? 'Comment liked' : 'Comment unliked'
        });
    } catch (error) {
        console.error('Error toggling comment like:', error);
        res.status(500).json({ message: 'Error toggling comment like' });
    }
};

// @desc    Search posts
// @route   GET /api/posts/search
// @access  Public
exports.searchPosts = async (req, res) => {
    try {
        const { query, country, minRating, maxRating } = req.query;
        const searchQuery = {};

        if (query) {
            searchQuery.$or = [
                { title: { $regex: query, $options: 'i' } },
                { content: { $regex: query, $options: 'i' } }
            ];
        }

        if (country) {
            searchQuery.country = country;
        }

        if (minRating || maxRating) {
            searchQuery.rating = {};
            if (minRating) searchQuery.rating.$gte = Number(minRating);
            if (maxRating) searchQuery.rating.$lte = Number(maxRating);
        }

        const posts = await Post.find(searchQuery)
            .populate({
                path: 'author',
                select: 'username avatar',
                model: 'User'
            })
            .populate({
                path: 'country',
                select: 'name code flag',
                model: 'Country'
            })
            .sort({ createdAt: -1 });

        res.json(posts);
    } catch (error) {
        console.error('Error searching posts:', error);
        res.status(500).json({ message: 'Error searching posts' });
    }
};
