const { Chat, GeneralChat } = require('../models/Chat');
const User = require('../models/User');
const { validationResult } = require('express-validator');

// Create new chat
exports.createChat = async (req, res) => {
    try {
        const { participants, isGroup, groupName } = req.body;

        // Validate participants
        if (!isGroup && participants.length !== 2) {
            return res.status(400).json({ error: 'Direct chat must have exactly 2 participants' });
        }

        // Check if direct chat already exists
        if (!isGroup) {
            const existingChat = await Chat.findOne({
                isGroup: false,
                participants: { $all: participants }
            });

            if (existingChat) {
                return res.json(existingChat);
            }
        }

        const chat = new Chat({
            participants,
            isGroup,
            groupName,
            groupAdmin: isGroup ? req.user.id : null
        });

        await chat.save();
        res.status(201).json(chat);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

// Get user's chats
exports.getUserChats = async (req, res) => {
    try {
        const chats = await Chat.find({ participants: req.user.id })
            .populate('participants', 'name profilePicture')
            .populate('groupAdmin', 'name')
            .sort({ lastMessage: -1 });

        res.json(chats);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

// Get chat by ID
exports.getChatById = async (req, res) => {
    try {
        const chat = await Chat.findById(req.params.id)
            .populate('participants', 'name profilePicture')
            .populate('messages.sender', 'name profilePicture')
            .populate('groupAdmin', 'name');

        if (!chat) {
            return res.status(404).json({ error: 'Chat not found' });
        }

        // Check if user is participant
        if (!chat.participants.some(p => p._id.toString() === req.user.id)) {
            return res.status(403).json({ error: 'Not authorized' });
        }

        res.json(chat);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

// Add message to chat
exports.addMessage = async (req, res) => {
    try {
        const { content } = req.body;
        console.log('Adding new message:', content);

        let chat = await Chat.findOne();
        if (!chat) {
            chat = new Chat({ messages: [] });
        }

        chat.messages.push({
            user: req.user.username,
            content,
            timestamp: new Date()
        });

        await chat.save();
        console.log('Message saved successfully');

        res.json(chat.messages[chat.messages.length - 1]);
    } catch (error) {
        console.error('Error in addMessage:', error);
        res.status(500).json({ message: 'Error saving message' });
    }
};

// Add participant to group chat
exports.addParticipant = async (req, res) => {
    try {
        const { userId } = req.body;
        const chat = await Chat.findById(req.params.id);

        if (!chat) {
            return res.status(404).json({ error: 'Chat not found' });
        }

        // Check if user is group admin
        if (chat.groupAdmin.toString() !== req.user.id) {
            return res.status(403).json({ error: 'Only group admin can add participants' });
        }

        // Check if user is already participant
        if (chat.participants.includes(userId)) {
            return res.status(400).json({ error: 'User is already a participant' });
        }

        chat.participants.push(userId);
        await chat.save();

        res.json(chat);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

// Remove participant from group chat
exports.removeParticipant = async (req, res) => {
    try {
        const { userId } = req.body;
        const chat = await Chat.findById(req.params.id);

        if (!chat) {
            return res.status(404).json({ error: 'Chat not found' });
        }

        // Check if user is group admin
        if (chat.groupAdmin.toString() !== req.user.id) {
            return res.status(403).json({ error: 'Only group admin can remove participants' });
        }

        // Check if trying to remove admin
        if (userId === chat.groupAdmin.toString()) {
            return res.status(400).json({ error: 'Cannot remove group admin' });
        }

        chat.participants = chat.participants.filter(p => p.toString() !== userId);
        await chat.save();

        res.json(chat);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Server error' });
    }
};

exports.getMessages = async (req, res) => {
    try {
        console.log('Fetching chat messages');
        let chat = await Chat.findOne();

        if (!chat) {
            console.log('No chat found, creating new one');
            chat = await Chat.create({ messages: [] });
        }

        console.log(`Found ${chat.messages.length} messages`);
        res.json(chat.messages);
    } catch (error) {
        console.error('Error in getMessages:', error);
        res.status(500).json({ message: 'Error fetching messages' });
    }
};
