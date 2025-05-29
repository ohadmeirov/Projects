const Group = require('../models/Group');

exports.createGroup = async (req, res) => {
    try {
        console.log('Creating group with data:', { body: req.body, user: req.user }); // DEBUG

        const { name } = req.body;
        if (!name) {
            return res.status(400).json({ message: 'Group name is required' });
        }

        // ודא שיש משתמש מחובר
        if (!req.user || !req.user._id) {
            return res.status(401).json({ message: 'User not authenticated' });
        }

        const group = new Group({
            name: name.trim(),
            admin: req.user._id,
            members: [req.user._id],
            messages: []
        });

        await group.save();

        // פופולציה של המשתמשים
        await group.populate([
            { path: 'admin', select: 'username' },
            { path: 'members', select: 'username' }
        ]);

        console.log('Group created successfully:', group); // DEBUG
        res.status(201).json(group);
    } catch (error) {
        console.error('Error in createGroup:', error); // DEBUG
        res.status(500).json({
            message: 'Error creating group',
            error: error.message
        });
    }
};

exports.getGroups = async (req, res) => {
    try {
        console.log('Fetching groups for user:', req.user); // DEBUG

        if (!req.user) {
            return res.status(401).json({ message: 'User not authenticated' });
        }

        const groups = await Group.find()
            .populate('admin', 'username')
            .populate('members', 'username')
            .sort({ createdAt: -1 }); // Sort by newest first

        console.log('Groups fetched:', groups.length); // DEBUG
        res.json(groups);
    } catch (error) {
        console.error('Error fetching groups:', error);
        res.status(500).json({ message: 'Error fetching groups: ' + error.message });
    }
};

exports.joinGroup = async (req, res) => {
    try {
        console.log('User joining group:', req.params.id, 'User:', req.user); // DEBUG

        if (!req.user) {
            return res.status(401).json({ message: 'User not authenticated' });
        }

        const group = await Group.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: 'Group not found' });
        }

        // Check if user is already a member
        if (!group.members.includes(req.user._id)) {
            group.members.push(req.user._id);
            await group.save();
        }

        // Return populated group
        const populatedGroup = await Group.findById(group._id)
            .populate('admin', 'username')
            .populate('members', 'username');

        res.json(populatedGroup);
    } catch (error) {
        console.error('Error joining group:', error);
        res.status(500).json({ message: 'Error joining group: ' + error.message });
    }
};

exports.getGroupMessages = async (req, res) => {
    try {
        console.log('Fetching messages for group:', req.params.id); // DEBUG

        if (!req.user) {
            return res.status(401).json({ message: 'User not authenticated' });
        }

        const group = await Group.findById(req.params.id);
        if (!group) {
            return res.status(404).json({ message: 'Group not found' });
        }

        // Check if user is a member of the group
        if (!group.members.includes(req.user._id)) {
            return res.status(403).json({ message: 'You are not a member of this group' });
        }

        // Sort messages by creation date
        const messages = group.messages || [];
        messages.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));

        res.json(messages);
    } catch (error) {
        console.error('Error fetching group messages:', error);
        res.status(500).json({ message: 'Error fetching messages: ' + error.message });
    }
};
