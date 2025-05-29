const express = require('express');
const router = express.Router();
const { auth } = require('../middleware/auth');
const GroupChat = require('../models/GroupChat');

// Get all groups
router.get('/', auth, async (req, res) => {
    console.log('Getting all groups');
    try {
        const groups = await GroupChat.find()
            .populate('owner', 'username')
            .populate('members', 'username')
            .sort({ createdAt: -1 });
        console.log('Found groups:', groups.length);
        res.json(groups);
    } catch (error) {
        console.error('Error fetching groups:', error);
        res.status(500).json({ message: error.message });
    }
});

// Create new group
router.post('/', auth, async (req, res) => {
    console.log('Creating new group:', req.body);
    try {
        const { name } = req.body;
        if (!name || typeof name !== 'string' || !name.trim()) {
            return res.status(400).json({ message: "Group name is required" });
        }

        

        const group = new GroupChat({
            name: name.trim(),
            owner: req.user._id,
            members: [req.user._id]
        });

        await group.save();
        await group.populate('owner', 'username');
        await group.populate('members', 'username');

        console.log('Group created:', group);
        res.status(201).json(group);
    } catch (error) {
        console.error('Error creating group:', error);
        res.status(500).json({ message: error.message });
    }
});

// Join group
router.post('/:id/join', auth, async (req, res) => {
    try {
        const group = await GroupChat.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: "Group not found" });
        }

        // Check if user is already a member
        if (group.members.includes(req.user._id)) {
            return res.status(400).json({ message: "User is already a member of this group" });
        }

        group.members.push(req.user._id);
        await group.save();
        await group.populate('members', 'username');

        res.json(group);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// Get group messages
router.get('/:id/messages', auth, async (req, res) => {
    try {
        const group = await GroupChat.findById(req.params.id)
            .populate('messages.sender', 'username');

        if (!group) {
            return res.status(404).json({ message: "Group not found" });
        }

        if (!group.members.includes(req.user._id)) {
            return res.status(403).json({ message: "Not a member of this group" });
        }

        res.json(group.messages);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// Delete group
router.delete('/:id', auth, async (req, res) => {
    try {
        const group = await GroupChat.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: "Group not found" });
        }

        if (group.owner.toString() !== req.user._id.toString()) {
            return res.status(403).json({ message: "Only group owner can delete group" });
        }

        // Delete the group permanently
        await GroupChat.deleteOne({ _id: req.params.id });

        res.json({ message: "Group deleted successfully" });
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// Leave group
router.post('/:id/leave', auth, async (req, res) => {
    try {
        const group = await GroupChat.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: "Group not found" });
        }

        group.members = group.members.filter(id => id.toString() !== req.user._id.toString());
        await group.save();
        res.json({ message: "Left group successfully" });
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// Edit group name (only owner)
router.put('/:id', auth, async (req, res) => {
    try {
        const group = await GroupChat.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: "Group not found" });
        }
        if (group.owner.toString() !== req.user._id.toString()) {
            return res.status(403).json({ message: "Only group owner can edit group" });
        }
        group.name = req.body.name;
        await group.save();
        await group.populate('owner', 'username');
        await group.populate('members', 'username');
        res.json(group);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

// Add message to group chat (persist message)
router.post('/:id/messages', auth, async (req, res) => {
    try {
        const group = await GroupChat.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: "Group not found" });
        }
        if (!group.members.includes(req.user._id)) {
            return res.status(403).json({ message: "Not a member of this group" });
        }
        const message = {
            sender: req.user._id,
            content: req.body.content,
            timestamp: new Date()
        };
        group.messages.push(message);
        await group.save();
        await group.populate('messages.sender', 'username');
        res.status(201).json(group.messages[group.messages.length - 1]);
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
});

module.exports = router;
