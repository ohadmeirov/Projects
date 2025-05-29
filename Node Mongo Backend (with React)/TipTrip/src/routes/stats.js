const express = require('express');
const router = express.Router();
const statsController = require('../controllers/statsController');

router.get('/posts-per-month', statsController.postsPerMonth);
router.get('/users-per-month', statsController.usersPerMonth);  // שינוי הנתיב

module.exports = router;
