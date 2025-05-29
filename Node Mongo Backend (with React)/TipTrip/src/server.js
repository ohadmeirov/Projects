const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const http = require('http');
const socketIo = require('socket.io');
const path = require('path');
require('dotenv').config();
const seedCountries = require('./seed/countries');

// Import routes
const userRoutes = require('./routes/users');
const countryRoutes = require('./routes/countries');
const postRoutes = require('./routes/posts');
const chatRoutes = require('./routes/chats');
const uploadRoutes = require('./routes/upload');
const statsRoutes = require('./routes/stats');
const groupRoutes = require('./routes/groups');
const chatHandler = require('./socket/chatHandler');
const groupChat = require('./socket/groupChat');

const app = express();
const server = http.createServer(app);

// Socket.IO configuration
const io = socketIo(server, {
    cors: {
        origin: "http://localhost:3000",
        methods: ["GET", "POST"],
        credentials: true
    }
});

// Import Chat model
const Chat = require('./models/Chat');

// Socket.IO handler
io.on('connection', async (socket) => {
    console.log('New client connected:', socket.id);
    const username = socket.handshake.query.username;

    if (!username) {
        console.error('No username provided');
        socket.disconnect();
        return;
    }

    socket.on('chat message', async (data, callback) => {
        try {
            console.log('Received message from', username, ':', data);

            if (!data?.content) {
                console.error('Invalid message data');
                callback?.({ status: 'error', message: 'Invalid message data' });
                return;
            }

            const message = {
                user: username,
                content: data.content,
                timestamp: new Date()
            };

            // Save to database
            const chat = await Chat.findOne() || await new Chat({ messages: [] }).save();
            chat.messages.push(message);
            await chat.save();

            console.log('Message saved, broadcasting');
            io.emit('chat message', message);
            callback?.({ status: 'ok' });

        } catch (error) {
            console.error('Error handling message:', error);
            callback?.({ status: 'error', message: error.message });
        }
    });

    socket.on('disconnect', () => {
        console.log('Client disconnected:', socket.id);
    });
});

app.use((req, res, next) => {
    console.log(`${req.method} ${req.url}`); // Debug logging
    next();
});

// Middleware
app.use(cors({
    origin: process.env.CLIENT_URL || "http://localhost:3000",
    credentials: true
}));
app.use(express.json());

// Debug logging
app.use((req, res, next) => {
    console.log(`${req.method} ${req.url}`, {
        headers: req.headers,
        body: req.body
    });
    next();
});

// Serve static files from the uploads directory
app.use('/uploads', express.static('uploads'));

// Routes
app.use('/api/users', userRoutes);
app.use('/api/countries', countryRoutes);
app.use('/api/posts', postRoutes);
app.use('/api/chats', chatRoutes);  // Make sure this line exists
app.use('/api/upload', uploadRoutes);
app.use('/api/stats', statsRoutes);
app.use('/api/groups', groupRoutes);

// Add debug middleware
app.use((req, res, next) => {
    console.log('Request:', {
        method: req.method,
        url: req.url,
        body: req.body
    });
    next();
});

// Debug route
app.get('/api/test', (req, res) => {
    res.json({ message: 'Server is working!' });
});

// Error handler for debugging
app.use((err, req, res, next) => {
    console.error('Server error:', err);
    res.status(500).json({
        message: 'Server error',
        error: err.message
    });
});

// Connect to MongoDB
mongoose.connect(process.env.MONGODB_URI)
    .then(async () => {
        console.log('MongoDB Connected');
        // Seed countries
        await seedCountries();
    })
    .catch(err => {
        console.error('MongoDB connection error:', err);
        process.exit(1);
    });

// Catch-all handler for API routes that don't exist
app.use('/api/*', (req, res) => {
    console.log('API route not found:', req.path);
    res.status(404).json({ message: `API route ${req.path} not found` });
});

// Error handling middleware
app.use((err, req, res, next) => {
    console.error('Server error:', err.stack);
    res.status(500).json({ message: 'Something went wrong!' });
});

const PORT = process.env.PORT || 5000;
server.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
    console.log('Available routes:');
    console.log('  GET  /api/test');
    console.log('  GET  /api/groups');
    console.log('  POST /api/groups');
    console.log('  Socket.IO enabled on same port');
});
