const jwt = require('jsonwebtoken');
const config = require('../config/config');
const User = require('../models/User');

const auth = async (req, res, next) => {
    try {
        const token = req.header('Authorization')?.replace('Bearer ', '');
        if (!token) {
            return res.status(401).json({ message: 'No token, authorization denied' });
        }

        const decoded = jwt.verify(token, process.env.JWT_SECRET);
        // בדיקת ולידציה לdecoded
        if (!decoded || !decoded.id) {
            return res.status(401).json({ message: 'Invalid token structure' });
        }

        const user = await User.findById(decoded.id).select('-password');
        if (!user) {
            return res.status(401).json({ message: 'User not found' });
        }

        // שמירת המשתמש ב-req
        req.user = user;
        next();
    } catch (error) {
        console.error('Auth middleware error:', error);
        res.status(401).json({ message: 'Token is not valid' });
    }
};

const adminAuth = async (req, res, next) => {
    try {
        await auth(req, res, () => {
            if (req.user.role !== 'admin') {
                return res.status(403).json({ error: 'Access denied. Admin only.' });
            }
            next();
        });
    } catch (error) {
        res.status(401).json({ error: 'Please authenticate.' });
    }
};

module.exports = {
    auth,
    adminAuth
};
