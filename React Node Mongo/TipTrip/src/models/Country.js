const mongoose = require('mongoose');

const countrySchema = new mongoose.Schema({
    name: {
        type: String,
        required: true,
        unique: true,
        trim: true
    },
    code: {
        type: String,
        required: true,
        unique: true,
        trim: true,
        uppercase: true
    },
    flag: {
        type: String
    },
    description: {
        type: String,
        default: ''
    },
    images: [{
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
            required: true
        }
    },
    population: {
        type: Number
    },
    capital: {
        type: String,
        default: ''
    },
    languages: [{
        type: String
    }],
    currency: {
        type: String,
        default: ''
    },
    timezone: {
        type: String,
        default: 'UTC'
    },
    language: {
        type: String,
        required: true
    },
    averageRating: {
        type: Number,
        default: 0,
        min: 0,
        max: 5
    },
    totalRatings: {
        type: Number,
        default: 0
    },
    posts: [{
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Post'
    }]
}, {
    timestamps: true,
    toJSON: { virtuals: true },
    toObject: { virtuals: true }
});

// Create 2dsphere index for location
countrySchema.index({ location: '2dsphere' });

// Create text index for search
countrySchema.index({ name: 'text', description: 'text' });

// Add virtual for posts count
countrySchema.virtual('postsCount').get(function() {
    return this.posts ? this.posts.length : 0;
});

const Country = mongoose.model('Country', countrySchema);

module.exports = Country;
