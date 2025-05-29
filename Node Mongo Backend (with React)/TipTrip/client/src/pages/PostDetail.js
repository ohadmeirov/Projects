import React, { useEffect, useState, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Container,
  Paper,
  Typography,
  Box,
  Rating,
  Button,
  TextField,
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
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import PhotoCamera from '@mui/icons-material/PhotoCamera';
import Videocam from '@mui/icons-material/Videocam';
import { format } from 'date-fns';
import { AuthContext } from '../context/AuthContext';
import axios from 'axios';

const PostDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user: currentUser } = useContext(AuthContext);
  const [post, setPost] = useState(null);
  const [comment, setComment] = useState('');
  const [isLiked, setIsLiked] = useState(false);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [editForm, setEditForm] = useState({ title: '', content: '', rating: 0 });
  const [editMediaFile, setEditMediaFile] = useState(null);
  const [editPreviewUrl, setEditPreviewUrl] = useState(post?.mediaUrl || '');
  const [removeMedia, setRemoveMedia] = useState(false);
  const token = localStorage.getItem('token');

  useEffect(() => {
    const fetchPost = async () => {
      const token = localStorage.getItem('token');
      const res = await axios.get(`http://localhost:5000/api/posts/${id}`, {
        headers: { 'Authorization': `Bearer ${token}` }
      });
      setPost(res.data);
      setIsLiked(res.data.likes.includes(localStorage.getItem('userId')));
    };
    fetchPost();
  }, [id]);

  const handleLike = async () => {
    if (!token) {
      alert('Please login to like posts');
      navigate('/login');
      return;
    }

    try {
      const response = await fetch(`http://localhost:5000/api/posts/${id}/like`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.ok) {
        setIsLiked(!isLiked);
        setPost(prev => ({
          ...prev,
          likes: isLiked
            ? prev.likes.filter(id => id !== localStorage.getItem('userId'))
            : [...prev.likes, localStorage.getItem('userId')]
        }));
      }
    } catch (error) {
      console.error('Error liking post:', error);
    }
  };

  const handleCommentLike = async (commentId) => {
    if (!token) {
      alert('Please login to like comments');
      navigate('/login');
      return;
    }

    try {
      const response = await fetch(`http://localhost:5000/api/posts/${id}/comments/${commentId}/like`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.ok) {
        const data = await response.json();
        setPost(prev => ({
          ...prev,
          comments: prev.comments.map(c =>
            c._id === commentId ? data.comment : c
          )
        }));
      }
    } catch (error) {
      console.error('Error liking comment:', error);
    }
  };

  const handleComment = async (e) => {
    e.preventDefault();
    if (!token) {
      alert('Please login to comment');
      navigate('/login');
      return;
    }

    try {
      const response = await fetch(`http://localhost:5000/api/posts/${id}/comment`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ text: comment })
      });

      if (response.ok) {
        const data = await response.json();
        setPost(prev => ({
          ...prev,
          comments: [data.comment, ...prev.comments]
        }));
        setComment('');
      }
    } catch (error) {
      console.error('Error posting comment:', error);
    }
  };

  const handleEdit = () => {
    setEditForm({
      title: post.title,
      content: post.content,
      rating: post.rating
    });
    setEditDialogOpen(true);
  };

  const handleEditMediaChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setEditMediaFile(file);
      const reader = new FileReader();
      reader.onloadend = () => setEditPreviewUrl(reader.result);
      reader.readAsDataURL(file);
      setRemoveMedia(false);
    }
  };

  const handleRemoveEditMedia = () => {
    setEditMediaFile(null);
    setEditPreviewUrl('');
    setRemoveMedia(true);
  };

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    try {
      let mediaUrl = post.mediaUrl;
      if (editMediaFile) {
        const token = localStorage.getItem('token');
        const mediaFormData = new FormData();
        mediaFormData.append('media', editMediaFile);
        const uploadResponse = await axios.post('http://localhost:5000/api/upload', mediaFormData, {
          headers: {
            'Content-Type': 'multipart/form-data',
            'Authorization': `Bearer ${token}`
          }
        });
        mediaUrl = uploadResponse.data.url;
      }
      if (removeMedia) {
        mediaUrl = '';
      }
      const response = await fetch(`http://localhost:5000/api/posts/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({ ...editForm, mediaUrl })
      });
      if (response.ok) {
        const data = await response.json();
        setPost(data);
        setEditDialogOpen(false);
      }
    } catch (error) {
      console.error('Error updating post:', error);
    }
  };

  const handleDelete = async () => {
    try {
      const response = await fetch(`http://localhost:5000/api/posts/${id}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.ok) {
        navigate('/');
      }
    } catch (error) {
      console.error('Error deleting post:', error);
    }
  };

  if (!post) {
    return <Typography>Loading...</Typography>;
  }

  const isAuthor = currentUser && post.author._id === currentUser._id;

  return (
    <Container maxWidth="md">
      <Box sx={{ mt: 4, mb: 4 }}>
        <Paper elevation={3} sx={{ p: 4 }}>
          {post.mediaUrl && (
            <Box sx={{ width: '100%', maxHeight: 350, overflow: 'hidden', mb: 3 }}>
              {post.mediaUrl.match(/\.(mp4|webm|ogg)$/i) ? (
                <video src={post.mediaUrl} controls style={{ width: '100%', borderRadius: 8 }} />
              ) : (
                <img src={post.mediaUrl} alt={post.title} style={{ width: '100%', borderRadius: 8 }} />
              )}
            </Box>
          )}
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 2 }}>
            <Box>
              <Typography variant="h4" component="h1" gutterBottom>
                {post.title}
              </Typography>
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                <Avatar src={post.author.avatar} sx={{ width: 24, height: 24, mr: 1 }}>
                  {post.author.username?.[0]}
                </Avatar>
                <Typography variant="body2" color="text.secondary">
                  Posted by {post.author.username}
                </Typography>
              </Box>
            </Box>
            {isAuthor && (
              <Box>
                <IconButton onClick={handleEdit} size="small">
                  <EditIcon />
                </IconButton>
                <IconButton onClick={() => setDeleteDialogOpen(true)} size="small" color="error">
                  <DeleteIcon />
                </IconButton>
              </Box>
            )}
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
            <Rating value={post.rating} readOnly />
            <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
              ({post.rating})
            </Typography>
          </Box>
          <Typography variant="body1" paragraph>
            {post.content}
          </Typography>
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
            <IconButton onClick={handleLike} color="primary">
              {isLiked ? <FavoriteIcon /> : <FavoriteBorderIcon />}
            </IconButton>
            <Typography variant="body2" color="text.secondary">
              {post.likes.length} likes
            </Typography>
          </Box>
          <Divider sx={{ my: 3 }} />
          <Typography variant="h6" gutterBottom>
            Comments
          </Typography>
          <List>
            {post.comments.map((comment) => (
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
                        <Typography component="span" variant="subtitle2">
                          {comment.user?.username}
                        </Typography>
                        <Typography variant="caption" color="text.secondary" sx={{ ml: 1 }}>
                          {format(new Date(comment.createdAt), 'MMM dd, yyyy HH:mm')}
                        </Typography>
                      </Box>
                    }
                    secondary={
                      <>
                        <Typography component="span" variant="body2" color="text.primary">
                          {comment.text}
                        </Typography>
                        <Box sx={{ display: 'flex', alignItems: 'center', mt: 1 }}>
                          <IconButton
                            size="small"
                            onClick={() => handleCommentLike(comment._id)}
                            color={comment.likes.includes(localStorage.getItem('userId')) ? 'primary' : 'default'}
                          >
                            {comment.likes.includes(localStorage.getItem('userId')) ?
                              <FavoriteIcon fontSize="small" /> :
                              <FavoriteBorderIcon fontSize="small" />
                            }
                          </IconButton>
                          <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                            {comment.likes.length} likes
                          </Typography>
                        </Box>
                      </>
                    }
                  />
                </ListItem>
                <Divider variant="inset" component="li" />
              </React.Fragment>
            ))}
          </List>
          <Box component="form" onSubmit={handleComment} sx={{ mt: 3 }}>
            <TextField
              fullWidth
              label="Add a comment"
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              multiline
              rows={2}
              required
            />
            <Button
              type="submit"
              variant="contained"
              color="primary"
              sx={{ mt: 2 }}
            >
              Post Comment
            </Button>
          </Box>
        </Paper>
      </Box>

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
          <Button onClick={handleDelete} color="error">Delete</Button>
        </DialogActions>
      </Dialog>

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
              onChange={e => setEditForm({ ...editForm, title: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Content"
              value={editForm.content}
              onChange={e => setEditForm({ ...editForm, content: e.target.value })}
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
            {/* Media upload & preview */}
            <Box sx={{ mb: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
              <input
                accept="image/*"
                style={{ display: 'none' }}
                id="edit-image-upload"
                type="file"
                onChange={handleEditMediaChange}
                disabled={!!editMediaFile || !!editPreviewUrl}
              />
              <label htmlFor="edit-image-upload">
                <Button
                  variant="outlined"
                  component="span"
                  startIcon={<PhotoCamera />}
                  disabled={!!editMediaFile || (!!editPreviewUrl && !removeMedia)}
                >
                  Upload Image
                </Button>
              </label>
              <input
                accept="video/*"
                style={{ display: 'none' }}
                id="edit-video-upload"
                type="file"
                onChange={handleEditMediaChange}
                disabled={!!editMediaFile || !!editPreviewUrl}
              />
              <label htmlFor="edit-video-upload">
                <Button
                  variant="outlined"
                  component="span"
                  startIcon={<Videocam />}
                  disabled={!!editMediaFile || (!!editPreviewUrl && !removeMedia)}
                >
                  Upload Video
                </Button>
              </label>
              {(editMediaFile || editPreviewUrl) && (
                <Button
                  variant="outlined"
                  color="error"
                  startIcon={<DeleteIcon />}
                  onClick={handleRemoveEditMedia}
                >
                  Remove
                </Button>
              )}
            </Box>
            {(editPreviewUrl && !removeMedia) && (
              <Box sx={{ mt: 2 }}>
                {editMediaFile
                  ? (
                    editMediaFile.type.startsWith('video')
                      ? <video src={editPreviewUrl} controls style={{ maxWidth: '100%', maxHeight: 200, borderRadius: 8 }} />
                      : <img src={editPreviewUrl} alt="Preview" style={{ maxWidth: '100%', maxHeight: 200, borderRadius: 8 }} />
                  )
                  : (
                    post.mediaUrl && post.mediaUrl.match(/\.(mp4|webm|ogg)$/i)
                      ? <video src={post.mediaUrl} controls style={{ maxWidth: '100%', maxHeight: 200, borderRadius: 8 }} />
                      : <img src={post.mediaUrl} alt="Preview" style={{ maxWidth: '100%', maxHeight: 200, borderRadius: 8 }} />
                  )
                }
              </Box>
            )}
            <Button type="submit" variant="contained" color="primary" sx={{ mt: 2 }}>
              Save Changes
            </Button>
          </Box>
        </DialogContent>
      </Dialog>
    </Container>
  );
};

export default PostDetail;
