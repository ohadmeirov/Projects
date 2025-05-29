const mongoose = require('mongoose');

const messageSchema = new mongoose.Schema({
    user: {
        type: String,
        required: true
    },
    content: {
        type: String,
        required: true
    },
    timestamp: {
        type: Date,
        default: Date.now
    }
});

const chatSchema = new mongoose.Schema({
    messages: [messageSchema]
}, {
    timestamps: true,
    collection: 'chats'  // explicitly set collection name
});

module.exports = mongoose.model('Chat', chatSchema);
