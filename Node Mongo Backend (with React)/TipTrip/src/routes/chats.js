const express = require('express');
const router = express.Router();
const { auth } = require('../middleware/auth');
const Chat = require('../models/Chat');

// Get all messages
router.get('/messages', auth, async (req, res) => {
    try {
        console.log('Getting chat messages');
        let chat = await Chat.findOne();

        if (!chat) {
            console.log('Creating new chat');
            chat = await new Chat({ messages: [] }).save();
        }

        console.log('Returning messages:', chat.messages.length);
        res.json(chat.messages);
    } catch (error) {
        console.error('Error fetching messages:', error);
        res.status(500).json({ message: 'Error fetching messages' });
    }
});

router.post('/messages', auth, async (req, res) => {
    try {
        const { content } = req.body;
        let chat = await Chat.findOne() || await Chat.create({ messages: [] });

        const message = {
            user: req.user.username,
            content,
            timestamp: new Date()
        };

        chat.messages.push(message);
        await chat.save();

        res.status(201).json(message);
    } catch (error) {
        console.error('Error saving message:', error);
        res.status(500).json({ message: 'Error saving message' });
    }
});

module.exports = router;
