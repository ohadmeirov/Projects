import React, { useState, useEffect, useContext } from 'react';
import {
    Container,
    Grid,
    Card,
    CardContent,
    Typography,
    Box,
    Avatar,
    Button,
    Divider,
    List,
    ListItem,
    ListItemText,
    ListItemAvatar,
    IconButton,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Rating,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Paper
} from '@mui/material';
import {
    Edit as EditIcon,
    Delete as DeleteIcon,
    LocationOn,
    Star
} from '@mui/icons-material';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import GroupIcon from '@mui/icons-material/Group';

const Profile = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { user: authUser, loading: authLoading } = useContext(AuthContext);
    const [user, setUser] = useState(null);
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isCurrentUser, setIsCurrentUser] = useState(false);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [postToDelete, setPostToDelete] = useState(null);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [postToEdit, setPostToEdit] = useState(null);
    const [editForm, setEditForm] = useState({
        title: '',
        content: '',
        rating: 0,
        country: ''
    });
    const [countries, setCountries] = useState([]);
    const [editNameDialogOpen, setEditNameDialogOpen] = useState(false);
    const [newUsername, setNewUsername] = useState('');
    const [createdGroups, setCreatedGroups] = useState([]);
    const [memberGroups, setMemberGroups] = useState([]);

    useEffect(() => {
        let isMounted = true;
        if (authLoading) return;
        if (!authUser || !authUser._id) {
            navigate('/login');
            return;
        }
        const token = localStorage.getItem('token');
        const userId = authUser._id;
        const targetId = id;

        const fetchData = async () => {
            try {
                // Fetch user
                const userRes = await axios.get(`http://localhost:5000/api/users/${targetId}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!isMounted) return;
                setUser(userRes.data);
                setIsCurrentUser(targetId === userId);

                // Fetch only posts created by this user
                const postsRes = await axios.get(`http://localhost:5000/api/users/${targetId}/posts`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!isMounted) return;
                setPosts(postsRes.data);

                // Fetch groups
                const groupsRes = await axios.get('http://localhost:5000/api/groups', {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!isMounted) return;
                setCreatedGroups(groupsRes.data.filter(g => (g.owner?._id || g.owner) === targetId));
                setMemberGroups(groupsRes.data.filter(
                    g =>
                        g.members.some(m => (m._id || m) === targetId) &&
                        (g.owner?._id || g.owner) !== targetId
                ));

                setLoading(false); // <-- set loading false only after all fetches succeeded
            } catch (err) {
                if (!isMounted) return;
                // Only set user to null if the user fetch failed with 404
                if (err.response?.status === 404 && err.config?.url?.includes(`/api/users/${targetId}`)) {
                    setUser(null);
                }
                setLoading(false);
            }
        };

        fetchData();
        fetchCountries();

        return () => { isMounted = false; };
    }, [id, navigate, authUser, authLoading]);

    const fetchCountries = async () => {
        try {
            console.log('Fetching countries');
            const response = await axios.get('http://localhost:5000/api/countries');
            setCountries(response.data);
            console.log('Countries loaded:', response.data);
        } catch (error) {
            console.error('Error fetching countries:', error);
        }
    };

    const handleDeleteClick = (post) => {
        console.log('Open delete dialog for post', post);
        setPostToDelete(post);
        setDeleteDialogOpen(true);
    };

    const handleEditClick = (post) => {
        console.log('Open edit dialog for post', post);
        setPostToEdit(post);
        setEditForm({
            title: post.title,
            content: post.content,
            rating: post.rating,
            country: post.country._id
        });
        setEditDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        try {
            const token = localStorage.getItem('token');
            console.log('Deleting post', postToDelete._id);
            await axios.delete(`http://localhost:5000/api/posts/${postToDelete._id}`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            setPosts(posts.filter(post => post._id !== postToDelete._id));
            setDeleteDialogOpen(false);
            setPostToDelete(null);
            console.log('Post deleted');
        } catch (error) {
            console.error('Error deleting post:', error);
        }
    };

    const handleEditSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem('token');
            console.log('Saving post', postToEdit._id, editForm);
            const response = await axios.put(
                `http://localhost:5000/api/posts/${postToEdit._id}`,
                editForm,
                {
                    headers: { 'Authorization': `Bearer ${token}` }
                }
            );
            setPosts(posts.map(post =>
                post._id === postToEdit._id ? response.data : post
            ));
            setEditDialogOpen(false);
            setPostToEdit(null);
            console.log('Post updated', response.data);
        } catch (error) {
            console.error('Error updating post:', error);
        }
    };

    // Update username handler
    const handleEditName = () => {
        setNewUsername(user.username);
        setEditNameDialogOpen(true);
    };

    const handleEditNameSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem('token');
            const response = await axios.put(
                'http://localhost:5000/api/users/profile',
                { username: newUsername },
                { headers: { 'Authorization': `Bearer ${token}` } }
            );
            setUser(prev => ({ ...prev, username: response.data.username }));
            setEditNameDialogOpen(false);
        } catch (error) {
            console.error('Error updating username:', error);
        }
    };

    if (loading) return <div>Loading...</div>;
    if (!user) return <div>User not found</div>;

    return (
        <Container maxWidth="md" sx={{ py: 4 }}>
            <Card sx={{ mb: 4, p: 2, display: 'flex', alignItems: 'center' }}>
                <Avatar src={user.avatar} sx={{ width: 80, height: 80, mr: 2 }} />
                <div>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                        <Typography variant="h4">{user.username}</Typography>
                        {isCurrentUser && (
                            <IconButton size="small" onClick={handleEditName}>
                                <EditIcon fontSize="small" />
                            </IconButton>
                        )}
                    </Box>
                    <Typography variant="body2" color="text.secondary">{user.email}</Typography>
                </div>
            </Card>
            <Grid container spacing={2}>
                <Grid item xs={12} md={4}>
                    <Typography variant="h5" sx={{ mb: 2 }}>Posts</Typography>
                    {posts.length === 0 ? (
                        <div style={{textAlign:'center'}}>
                            <Typography>You haven't posted yet - POST NOW!</Typography>
                            <Button variant="contained" color="primary" sx={{ mt: 2 }} onClick={() => {console.log('Navigate to create-post');navigate('/create-post')}}>Create Post</Button>
                        </div>
                    ) : (
                        <Grid container spacing={2}>
                            {posts.map(post => (
                                <Grid item xs={12} md={6} key={post._id}>
                                    <Card sx={{ mb: 2, cursor: 'pointer' }} onClick={() => {console.log('Navigate to post', post._id);navigate(`/post/${post._id}`)}}>
                                        <CardContent>
                                            <Typography variant="h6">{post.title}</Typography>
                                            <Typography variant="body2" color="text.secondary">{post.content.substring(0, 100)}...</Typography>
                                            <Typography variant="caption">{new Date(post.createdAt).toLocaleDateString()}</Typography>
                                        </CardContent>
                                    </Card>
                                </Grid>
                            ))}
                        </Grid>
                    )}
                </Grid>
                <Grid item xs={12} md={4}>
                    <Typography variant="h5" sx={{ mb: 2, display: 'flex', alignItems: 'center', gap: 1 }}>
                        <GroupIcon fontSize="medium" /> Groups Created
                    </Typography>
                    {createdGroups.length === 0 ? (
                        <Typography>No groups created yet.</Typography>
                    ) : (
                        <List>
                            {createdGroups.map(group => (
                                <ListItem key={group._id} sx={{ cursor: 'pointer' }}>
                                    <ListItemText
                                        primary={group.name}
                                        secondary={`Members: ${group.members.length}`}
                                    />
                                </ListItem>
                            ))}
                        </List>
                    )}
                </Grid>
                <Grid item xs={12} md={4}>
                    <Typography variant="h5" sx={{ mb: 2, display: 'flex', alignItems: 'center', gap: 1 }}>
                        <GroupIcon fontSize="medium" /> Groups Joined
                    </Typography>
                    {memberGroups.length === 0 ? (
                        <Typography>No groups joined yet.</Typography>
                    ) : (
                        <List>
                            {memberGroups.map(group => (
                                <ListItem key={group._id} sx={{ cursor: 'pointer' }}>
                                    <ListItemText
                                        primary={group.name}
                                        secondary={`Members: ${group.members.length}`}
                                    />
                                </ListItem>
                            ))}
                        </List>
                    )}
                </Grid>
            </Grid>

            {/* Delete Confirmation Dialog */}
            <Dialog
                open={deleteDialogOpen}
                onClose={() => {console.log('Close delete dialog');setDeleteDialogOpen(false);}}
            >
                <DialogTitle>Delete Post</DialogTitle>
                <DialogContent>
                    <Typography>
                        Are you sure you want to delete this post? This action cannot be undone.
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => {console.log('Cancel delete');setDeleteDialogOpen(false);}}>Cancel</Button>
                    <Button onClick={handleDeleteConfirm} color="error">
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Edit Post Dialog */}
            <Dialog
                open={editDialogOpen}
                onClose={() => {console.log('Close edit dialog');setEditDialogOpen(false);}}
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
                        <FormControl fullWidth margin="normal">
                            <InputLabel>Country</InputLabel>
                            <Select
                                value={editForm.country}
                                onChange={(e) => setEditForm({ ...editForm, country: e.target.value })}
                                label="Country"
                                required
                            >
                                {countries.map((country) => (
                                    <MenuItem key={country._id} value={country._id}>
                                        {country.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
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

            {/* Edit Username Dialog */}
            <Dialog open={editNameDialogOpen} onClose={() => setEditNameDialogOpen(false)}>
                <DialogTitle>Edit Username</DialogTitle>
                <form onSubmit={handleEditNameSubmit}>
                    <DialogContent>
                        <TextField
                            fullWidth
                            label="Username"
                            value={newUsername}
                            onChange={e => setNewUsername(e.target.value)}
                            required
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setEditNameDialogOpen(false)}>Cancel</Button>
                        <Button type="submit" variant="contained" color="primary">Save</Button>
                    </DialogActions>
                </form>
            </Dialog>
        </Container>
    );
};

export default Profile;
