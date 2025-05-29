const Post = require('../models/Post');
const User = require('../models/User');
const mongoose = require('mongoose');

// פוסטים חדשים בכל חודש
exports.postsPerMonth = async (req, res) => {
    const data = await Post.aggregate([
        {
            $group: {
                _id: { $dateToString: { format: "%Y-%m", date: "$createdAt" } },
                count: { $sum: 1 }
            }
        },
        { $sort: { _id: 1 } }
    ]);
    res.json(data.map(d => ({ month: d._id, count: d.count })));
};

// שינוי הפונקציה להצגת משתמשים חדשים לפי חודש
exports.usersPerMonth = async (req, res) => {
    const data = await User.aggregate([
        {
            $group: {
                _id: { $dateToString: { format: "%Y-%m", date: "$createdAt" } },
                count: { $sum: 1 }
            }
        },
        { $sort: { _id: 1 } }
    ]);
    res.json(data.map(d => ({ month: d._id, count: d.count })));
};

// ממוצע פוסטים לקבוצה בכל חודש
exports.avgPostsPerGroupPerMonth = async (req, res) => {
    const data = await Post.aggregate([
        {
            $match: { group: { $exists: true, $ne: null } }
        },
        {
            $group: {
                _id: {
                    month: { $dateToString: { format: "%Y-%m", date: "$createdAt" } },
                    group: "$group"
                },
                count: { $sum: 1 }
            }
        },
        {
            $group: {
                _id: "$_id.month",
                avg: { $avg: "$count" }
            }
        },
        { $sort: { _id: 1 } }
    ]);
    res.json(data.map(d => ({ month: d._id, avg: d.avg })));
};
