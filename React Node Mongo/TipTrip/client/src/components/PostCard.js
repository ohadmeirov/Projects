import React from 'react';
import { Button } from '@mui/material';
import axios from 'axios';

const PostCard = ({ post, onAddFriend }) => {
    const handleAddFriend = async (friendId) => {
        try {
            const response = await axios.post('/add-friend', { friendId });
            if (response.data.success) {
                onAddFriend(friendId);
            }
        } catch (error) {
            console.error('Error adding friend:', error);
        }
    };

    return (
        <div>
            {/* ...post content... */}
            <Button onClick={() => handleAddFriend(post.author._id)}>
                Add Friend
            </Button>
        </div>
    );
};

export default PostCard;
