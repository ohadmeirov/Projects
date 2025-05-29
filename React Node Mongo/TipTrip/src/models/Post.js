const mongoose = require('mongoose');

const commentSchema = new mongoose.Schema({
    user: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'User',
        required: true
    },
    text: {
        type: String,
        required: true
    },
    likes: [{
        type: mongoose.Schema.Types.ObjectId,
        ref: 'User'
    }],
    createdAt: {
        type: Date,
        default: Date.now
    }
}, {
    toJSON: { virtuals: true },
    toObject: { virtuals: true }
});

// Add virtual for comment likes count
commentSchema.virtual('likesCount').get(function() {
    return this.likes ? this.likes.length : 0;
});

const postSchema = new mongoose.Schema({
    title: {
        type: String,
        required: true,
        trim: true
    },
    content: {
        type: String,
        required: true
    },
    author: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'User',
        required: true
    },
    country: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Country',
        required: true
    },
    rating: {
        type: Number,
        required: true,
        min: 1,
        max: 5
    },
    mediaUrl: {
        type: String
    },
    images: [{
        type: String
    }],
    videos: [{
        type: String
    }],
    location: {
        type: {
            type: String,
            enum: ['Point'],
            default: 'Point'
        },
        coordinates: {
            type: [Number],
            default: [0, 0]
        }
    },
    likes: [{
        type: mongoose.Schema.Types.ObjectId,
        ref: 'User'
    }],
    comments: [commentSchema],
    tags: [{
        type: String,
        trim: true
    }],
    isPublic: {
        type: Boolean,
        default: true
    },
    media: {
        type: String
    }
}, {
    timestamps: true,
    toJSON: { virtuals: true },
    toObject: { virtuals: true }
});

// Add text index for search
postSchema.index({ title: 'text', content: 'text' });

// Create a 2dsphere index for location
postSchema.index({ location: '2dsphere' });

// Add virtual for likes count
postSchema.virtual('likesCount').get(function() {
    return this.likes ? this.likes.length : 0;
});

// Method to calculate average rating
postSchema.methods.calculateAverageRating = async function() {
    try {
        const country = await this.model('Country').findById(this.country);
        if (!country) return;

        const posts = await this.model('Post').find({ country: this.country });
        if (!posts.length) return;

        const totalRating = posts.reduce((sum, post) => sum + (post.rating || 0), 0);
        const averageRating = totalRating / posts.length;

        // Only update if we have a valid number
        if (!isNaN(averageRating) && isFinite(averageRating)) {
            country.averageRating = Number(averageRating.toFixed(1));
            country.totalRatings = posts.length;
            await country.save();
        }
    } catch (error) {
        console.error('Error calculating average rating:', error);
    }
};

// Update country rating when post rating changes
postSchema.pre('save', async function(next) {
    if (this.isModified('rating')) {
        await this.calculateAverageRating();
    }
    next();
});

module.exports = mongoose.model('Post', postSchema);
