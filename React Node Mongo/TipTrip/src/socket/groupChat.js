const Group = require('../models/GroupChat');

module.exports = (io) => {
    io.on('connection', (socket) => {
        console.log('Client connected:', socket.id);

        socket.on('join group', async (groupId) => {
            socket.join(groupId);
            console.log(`User ${socket.id} joined group ${groupId}`);
        });

        socket.on('leave group', (groupId) => {
            socket.leave(groupId);
            console.log(`User ${socket.id} left group ${groupId}`);
        });

        socket.on('group message', async ({ groupId, content, userId }) => {
            try {
                const group = await Group.findById(groupId);
                if (!group) {
                    socket.emit('error', { message: 'Group not found' });
                    return;
                }

                if (!group.members.includes(userId)) {
                    socket.emit('error', { message: 'Not a member of this group' });
                    return;
                }

                const message = {
                    sender: userId,
                    content,
                    timestamp: new Date()
                };

                group.messages.push(message);
                await group.save();

                io.to(groupId).emit('group message', {
                    ...message,
                    sender: { _id: userId }
                });
            } catch (error) {
                console.error('Error handling group message:', error);
            }
        });

        socket.on('disconnect', () => {
            console.log('Client disconnected:', socket.id);
        });
    });
};
