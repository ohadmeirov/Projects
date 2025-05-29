import React, { useState, useEffect } from 'react';
import {
    Container,
    Paper,
    Typography,
    Box,
    Grid,
    TextField,
    Button,
    Rating,
    List,
    ListItem,
    ListItemText,
    ListItemAvatar,
    Avatar,
    Divider,
    IconButton,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions
} from '@mui/material';
import {
    Send as SendIcon,
    Edit as EditIcon,
    Delete as DeleteIcon,
    LocationOn
} from '@mui/icons-material';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';

const Post = () => {
    const [post, setPost] = useState(null);
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState('');
    const [isCurrentUser, setIsCurrentUser] = useState(false);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [editForm, setEditForm] = useState({
        title: '',
        content: '',
        rating: 0
    });
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetchPost();
        fetchComments();
    }, [id]);

    const fetchPost = async () => {
        try {
            const token = localStorage.getItem('token');
            const [postResponse, currentUserResponse] = await Promise.all([
                axios.get(`http://localhost:5000/api/posts/${id}`, {
                    headers: { 'Authorization': `Bearer ${token}` }
                }),
                axios.get('http://localhost:5000/api/users/me', {
                    headers: { 'Authorization': `Bearer ${token}` }
                })
            ]);

            setPost(postResponse.data);
            setIsCurrentUser(currentUserResponse.data._id === postResponse.data.author._id);
        } catch (error) {
            console.error('Error fetching post:', error);
        }
    };

    const fetchComments = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`http://localhost:5000/api/posts/${id}/comments`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            setComments(response.data);
        } catch (error) {
            console.error('Error fetching comments:', error);
        }
    };

    const handleCommentSubmit = async (e) => {
        e.preventDefault();
        if (!newComment.trim()) return;

        try {
            const token = localStorage.getItem('token');
            const response = await axios.post(
                `http://localhost:5000/api/posts/${id}/comments`,
                { content: newComment },
                {
                    headers: { 'Authorization': `Bearer ${token}` }
                }
            );
            setComments([...comments, response.data]);
            setNewComment('');
        } catch (error) {
            console.error('Error posting comment:', error);
        }
    };

    const handleDeleteClick = () => {
        setDeleteDialogOpen(true);
    };

    const handleEditClick = () => {
        setEditForm({
            title: post.title,
            content: post.content,
            rating: post.rating
        });
        setEditDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`http://localhost:5000/api/posts/${id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            navigate('/');
        } catch (error) {
            console.error('Error deleting post:', error);
        }
    };

    const handleEditSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem('token');
            const response = await axios.put(
                `http://localhost:5000/api/posts/${id}`,
                editForm,
                {
                    headers: { 'Authorization': `Bearer ${token}` }
                }
            );
            setPost(response.data);
            setEditDialogOpen(false);
        } catch (error) {
            console.error('Error updating post:', error);
        }
    };

    if (!post) {
        return (
            <Container maxWidth="lg" sx={{ py: 4 }}>
                <Typography variant="h4" align="center">
                    Loading...
                </Typography>
            </Container>
        );
    }

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            <Paper elevation={3} sx={{ p: 4, mb: 4 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 3 }}>
                    <Box>
                        <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                            <Avatar src={post.author.avatar} sx={{ width: 40, height: 40, mr: 2 }}>
                                {post.author.username?.[0]}
                            </Avatar>
                            <Box>
                                <Typography variant="subtitle1">
                                    {post.author.username}
                                </Typography>
                                <Typography variant="caption" color="text.secondary">
                                    {new Date(post.createdAt).toLocaleDateString()}
                                </Typography>
                            </Box>
                        </Box>
                        <Typography variant="h4" gutterBottom>
                            {post.title}
                        </Typography>
                        <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                            <LocationOn color="action" sx={{ mr: 1 }} />
                            <Typography variant="body1" color="text.secondary">
                                {post.country?.name}
                            </Typography>
                        </Box>
                    </Box>
                    {isCurrentUser && (
                        <Box>
                            <IconButton onClick={handleEditClick} color="primary">
                                <EditIcon />
                            </IconButton>
                            <IconButton onClick={handleDeleteClick} color="error">
                                <DeleteIcon />
                            </IconButton>
                        </Box>
                    )}
                </Box>

                {post.media && (
                    <Box sx={{ mb: 3 }}>
                        <img
                            src={post.media}
                            alt={post.title}
                            style={{
                                width: '100%',
                                maxHeight: 400,
                                objectFit: 'cover',
                                borderRadius: 8
                            }}
                        />
                    </Box>
                )}

                <Typography variant="body1" paragraph>
                    {post.content}
                </Typography>

                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <Rating value={post.rating} readOnly />
                    <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                        ({post.rating})
                    </Typography>
                </Box>
            </Paper>

            <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="h5" gutterBottom>
                    Comments
                </Typography>
                <Box component="form" onSubmit={handleCommentSubmit} sx={{ mb: 4 }}>
                    <Grid container spacing={2}>
                        <Grid item xs>
                            <TextField
                                fullWidth
                                placeholder="Write a comment..."
                                value={newComment}
                                onChange={(e) => setNewComment(e.target.value)}
                                multiline
                                rows={2}
                            />
                        </Grid>
                        <Grid item>
                            <Button
                                type="submit"
                                variant="contained"
                                endIcon={<SendIcon />}
                                sx={{ height: '100%' }}
                            >
                                Post
                            </Button>
                        </Grid>
                    </Grid>
                </Box>

                <List>
                    {comments.map((comment) => (
                        <React.Fragment key={comment._id}>
                            <ListItem alignItems="flex-start">
                                <ListItemAvatar>
                                    <Avatar src={comment.user?.avatar}>
                                        {comment.user?.username?.[0]}
                                    </Avatar>
                                </ListItemAvatar>
                                <ListItemText
                                    primary={
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography variant="subtitle2" component="span">
                                                {comment.user?.username}
                                            </Typography>
                                            <Typography
                                                variant="caption"
                                                color="text.secondary"
                                                sx={{ ml: 1 }}
                                            >
                                                {new Date(comment.createdAt).toLocaleDateString()}
                                            </Typography>
                                        </Box>
                                    }
                                    secondary={comment.text}
                                />
                            </ListItem>
                            <Divider variant="inset" component="li" />
                        </React.Fragment>
                    ))}
                </List>
            </Paper>

            {/* Delete Confirmation Dialog */}
            <Dialog
                open={deleteDialogOpen}
                onClose={() => setDeleteDialogOpen(false)}
            >
                <DialogTitle>Delete Post</DialogTitle>
                <DialogContent>
                    <Typography>
                        Are you sure you want to delete this post? This action cannot be undone.
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setDeleteDialogOpen(false)}>Cancel</Button>
                    <Button onClick={handleDeleteConfirm} color="error">
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Edit Post Dialog */}
            <Dialog
                open={editDialogOpen}
                onClose={() => setEditDialogOpen(false)}
                maxWidth="md"
                fullWidth
            >
                <DialogTitle>Edit Post</DialogTitle>
                <DialogContent>
                    <Box component="form" onSubmit={handleEditSubmit} sx={{ mt: 2 }}>
                        <TextField
                            fullWidth
                            label="Title"
                            value={editForm.title}
                            onChange={(e) => setEditForm({ ...editForm, title: e.target.value })}
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="Content"
                            value={editForm.content}
                            onChange={(e) => setEditForm({ ...editForm, content: e.target.value })}
                            margin="normal"
                            required
                            multiline
                            rows={4}
                        />
                        <Box sx={{ mt: 2 }}>
                            <Typography component="legend">Rating</Typography>
                            <Rating
                                value={editForm.rating}
                                onChange={(event, newValue) => {
                                    setEditForm({ ...editForm, rating: newValue });
                                }}
                            />
                        </Box>
                    </Box>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setEditDialogOpen(false)}>Cancel</Button>
                    <Button onClick={handleEditSubmit} color="primary">
                        Save Changes
                    </Button>
                </DialogActions>
            </Dialog>
        </Container>
    );
};

export default Post;
