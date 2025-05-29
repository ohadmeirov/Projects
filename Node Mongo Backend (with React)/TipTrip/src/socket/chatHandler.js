const Chat = require('../models/Chat');
const User = require('../models/User');
const chatController = require('../controllers/chatController');

module.exports = (io) => {
    // Store online users
    const onlineUsers = new Map();

    // Handle user connection
    io.on('connection', (socket) => {
        console.log('User connected:', socket.id);

        // Handle user login
        socket.on('login', async (userId) => {
            onlineUsers.set(userId, socket.id);
            console.log('User logged in:', userId);

            // Notify others that user is online
            socket.broadcast.emit('userOnline', userId);
        });

        // Handle user logout
        socket.on('logout', (userId) => {
            onlineUsers.delete(userId);
            console.log('User logged out:', userId);

            // Notify others that user is offline
            socket.broadcast.emit('userOffline', userId);
        });

        // Handle joining chat room
        socket.on('joinChat', (chatId) => {
            socket.join(chatId);
            console.log('User joined chat:', chatId);
        });

        // Handle leaving chat room
        socket.on('leaveChat', (chatId) => {
            socket.leave(chatId);
            console.log('User left chat:', chatId);
        });

        // Handle new message
        socket.on('sendMessage', async (data) => {
            try {
                const { chatId, content, senderId } = data;

                // Save message to database
                const chat = await Chat.findById(chatId);
                if (!chat) {
                    return socket.emit('error', { message: 'Chat not found' });
                }

                const message = {
                    sender: senderId,
                    content,
                    timestamp: new Date()
                };

                chat.messages.push(message);
                chat.lastMessage = new Date();
                await chat.save();

                // Populate sender info
                await chat.populate('messages.sender', 'name profilePicture');

                // Get the last message
                const lastMessage = chat.messages[chat.messages.length - 1];

                // Emit message to all users in the chat
                io.to(chatId).emit('newMessage', {
                    chatId,
                    message: lastMessage
                });

                // Notify offline participants
                chat.participants.forEach(participantId => {
                    const participantSocketId = onlineUsers.get(participantId.toString());
                    if (!participantSocketId) {
                        // User is offline, store notification
                        // You might want to implement a notification system here
                        console.log('User is offline:', participantId);
                    }
                });

            } catch (error) {
                console.error('Error sending message:', error);
                socket.emit('error', { message: 'Error sending message' });
            }
        });

        // Handle chat message
        socket.on('chat message', async (data) => {
            console.log('Received chat message:', data);

            try {
                let chat = await Chat.findOne();
                if (!chat) {
                    chat = new Chat({ messages: [] });
                }

                const message = {
                    user: data.user,
                    content: data.content,
                    timestamp: new Date()
                };

                chat.messages.push(message);
                await chat.save();
                console.log('Saved message to DB');

                // Broadcast to all clients
                io.emit('chat message', message);
                console.log('Broadcasted message to all clients');
            } catch (error) {
                console.error('Error handling chat message:', error);
                socket.emit('error', { message: 'Failed to save message' });
            }
        });

        // Handle typing status
        socket.on('typing', (data) => {
            const { chatId, userId } = data;
            socket.to(chatId).emit('userTyping', { chatId, userId });
        });

        // Handle stop typing
        socket.on('stopTyping', (data) => {
            const { chatId, userId } = data;
            socket.to(chatId).emit('userStopTyping', { chatId, userId });
        });

        // Handle disconnection
        socket.on('disconnect', () => {
            console.log('User disconnected:', socket.id);

            // Find and remove user from online users
            for (const [userId, socketId] of onlineUsers.entries()) {
                if (socketId === socket.id) {
                    onlineUsers.delete(userId);
                    socket.broadcast.emit('userOffline', userId);
                    break;
                }
            }
        });
    });

    return {
        // Helper function to get socket ID of a user
        getUserSocket: (userId) => onlineUsers.get(userId),

        // Helper function to check if user is online
        isUserOnline: (userId) => onlineUsers.has(userId),

        // Helper function to emit to specific user
        emitToUser: (userId, event, data) => {
            const socketId = onlineUsers.get(userId);
            if (socketId) {
                io.to(socketId).emit(event, data);
            }
        }
    };
};
