require('dotenv').config();

module.exports = {
    mongoURI: process.env.MONGO_URI,
    jwtSecret: process.env.JWT_SECRET,
    port: process.env.PORT || 5000,
    jwtExpiration: '24h',
    clientURL: process.env.CLIENT_URL || 'http://localhost:3000'
};
