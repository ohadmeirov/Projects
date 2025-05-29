const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const dotenv = require('dotenv');
const path = require('path');
const multer = require('multer');

// Load environment variables
dotenv.config();

// Initialize express app
const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// Connect to MongoDB
mongoose.connect(process.env.MONGODB_URI, {
    useNewUrlParser: true,
    useUnifiedTopology: true
})
.then(() => console.log('MongoDB connected'))
.catch(err => console.error('MongoDB connection error:', err));

// Set up multer for file uploads
const storage = multer.diskStorage({
    destination: function (req, file, cb) {
        cb(null, path.join(__dirname, '../uploads'));
    },
    filename: function (req, file, cb) {
        cb(null, Date.now() + '-' + file.originalname);
    }
});
const upload = multer({ storage: storage });

// Serve uploaded files statically
app.use('/uploads', express.static(path.join(__dirname, '../uploads')));

// Upload endpoint
app.post('/api/upload', upload.single('media'), (req, res) => {
    if (!req.file) {
        return res.status(400).json({ message: 'No file uploaded' });
    }
    // Return the relative URL to the uploaded file
    res.json({ url: `/uploads/${req.file.filename}` });
});

// Routes
app.use('/api/users', require('./routes/users'));
app.use('/api/posts', require('./routes/posts'));
app.use('/api/countries', require('./routes/countries'));
app.use('/api/stats', require('./routes/stats'));
const groupRoutes = require('./routes/groups');
app.use('/api/groups', groupRoutes);

// Error handling middleware
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({ message: 'Something went wrong!' });
});

// Add error handling for groups
app.use('/api/groups/*', (err, req, res, next) => {
    console.error('Groups API error:', err);
    res.status(500).json({ message: 'Error in groups API', error: err.message });
});

// Create HTTP server
const httpServer = require('http').createServer(app);

// Socket.IO setup
const io = require('socket.io')(httpServer, {
    cors: {
        origin: "http://localhost:3000",
        methods: ["GET", "POST"],
        credentials: true
    }
});

// Socket events
io.on('connection', (socket) => {
    console.log('Client connected:', socket.id);

    socket.on('chat message', async (data) => {
        try {
            const chat = await Chat.findOne() || await new Chat({ messages: [] }).save();
            const message = {
                user: data.user,
                content: data.content,
                timestamp: new Date()
            };
            chat.messages.push(message);
            await chat.save();
            io.emit('chat message', message);
        } catch (error) {
            console.error('Error handling message:', error);
        }
    });

    socket.on('disconnect', () => {
        console.log('Client disconnected:', socket.id);
    });
});

// Start server
const PORT = process.env.PORT || 5000;
httpServer.listen(PORT, () => console.log(`Server running on port ${PORT}`));
