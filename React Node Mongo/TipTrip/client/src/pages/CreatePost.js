import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    Box,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Rating,
    Grid,
    Alert,
    ListItemText,
    ListItemIcon,
    Avatar
} from '@mui/material';
import { PhotoCamera, Videocam, Delete } from '@mui/icons-material';
import axios from 'axios';

const CreatePost = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        title: '',
        content: '',
        country: '',
        rating: 5,
        location: {
            coordinates: [0, 0]
        }
    });
    const [countries, setCountries] = useState([]);
    const [mediaFile, setMediaFile] = useState(null);
    const [previewUrl, setPreviewUrl] = useState(null);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchCountries();
    }, []);

    const fetchCountries = async () => {
        try {
            console.log('Fetching countries...');
            const token = localStorage.getItem('token');
            const response = await axios.get('http://localhost:5000/api/countries', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            console.log('Countries response:', response.data);
            const sortedCountries = response.data.sort((a, b) => a.name.localeCompare(b.name));
            console.log('Sorted countries:', sortedCountries);
            setCountries(sortedCountries);
        } catch (error) {
            console.error('Error fetching countries:', error);
            setError('Failed to load countries');
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        console.log('Select changed:', name, value);
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleRatingChange = (event, newValue) => {
        setFormData(prev => ({
            ...prev,
            rating: newValue
        }));
    };

    const handleMediaChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            setMediaFile(file);
            const reader = new FileReader();
            reader.onloadend = () => {
                setPreviewUrl(reader.result);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleRemoveMedia = () => {
        setMediaFile(null);
        setPreviewUrl(null);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (!formData.country) {
            setError('Please select a country');
            return;
        }

        try {
            const token = localStorage.getItem('token');
            let mediaUrl = null;
            if (mediaFile) {
                const mediaFormData = new FormData();
                mediaFormData.append('media', mediaFile);
                console.log('Uploading media:', mediaFile);
                const uploadResponse = await axios.post('http://localhost:5000/api/upload', mediaFormData, {
                    headers: {
                        'Content-Type': 'multipart/form-data',
                        'Authorization': `Bearer ${token}`
                    }
                });
                console.log('Upload response:', uploadResponse.data);
                mediaUrl = uploadResponse.data.url;
                if (!mediaUrl) {
                    setError('Upload failed: No URL returned');
                    return;
                }
            }

            const postData = {
                ...formData,
                mediaUrl
            };
            console.log('Creating post:', postData);

            const response = await axios.post('http://localhost:5000/api/posts', postData, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });
            console.log('Post created:', response.data);

            navigate(`/post/${response.data._id}`);
        } catch (error) {
            console.error('Error creating post:', error, error.response?.data);
            setError(error.response?.data?.message || 'Failed to create post');
        }
    };

    console.log('Current countries state:', countries);
    console.log('Current form data:', formData);

    return (
        <Container maxWidth="md" sx={{ py: 4 }}>
            <Paper elevation={3} sx={{ p: 4 }}>
                <Typography variant="h4" component="h1" gutterBottom>
                    Create New Post
                </Typography>

                {error && (
                    <Alert severity="error" sx={{ mb: 2 }}>
                        {error}
                    </Alert>
                )}

                <form onSubmit={handleSubmit}>
                    <Grid container spacing={3}>
                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                label="Title"
                                name="title"
                                value={formData.title}
                                onChange={handleChange}
                                required
                            />
                        </Grid>

                        <Grid item xs={12}>
                            <TextField
                                fullWidth
                                label="Content"
                                name="content"
                                value={formData.content}
                                onChange={handleChange}
                                multiline
                                rows={4}
                                required
                            />
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <FormControl fullWidth required error={!formData.country}>
                                <InputLabel>Country</InputLabel>
                                <Select
                                    name="country"
                                    value={formData.country}
                                    onChange={handleChange}
                                    label="Country"
                                    MenuProps={{
                                        PaperProps: {
                                            style: {
                                                maxHeight: 300
                                            }
                                        }
                                    }}
                                >
                                    {countries && countries.length > 0 ? (
                                        countries.map((country) => (
                                            <MenuItem key={country._id} value={country._id}>
                                                <ListItemIcon>
                                                    <Avatar
                                                        src={`https://flagcdn.com/w40/${country.code.toLowerCase()}.png`}
                                                        alt={country.name}
                                                        sx={{ width: 24, height: 24 }}
                                                    />
                                                </ListItemIcon>
                                                <ListItemText primary={country.name} />
                                            </MenuItem>
                                        ))
                                    ) : (
                                        <MenuItem disabled>Loading countries...</MenuItem>
                                    )}
                                </Select>
                            </FormControl>
                        </Grid>

                        <Grid item xs={12} md={6}>
                            <Box>
                                <Typography component="legend">Rating</Typography>
                                <Rating
                                    name="rating"
                                    value={formData.rating > 5 ? 5 : formData.rating}
                                    onChange={handleRatingChange}
                                    max={5}
                                />
                            </Box>
                        </Grid>

                        <Grid item xs={12}>
                            <Box sx={{ mb: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
                                {/* Upload Image */}
                                <input
                                    accept="image/*"
                                    style={{ display: 'none' }}
                                    id="image-upload"
                                    type="file"
                                    onChange={handleMediaChange}
                                />
                                <label htmlFor="image-upload">
                                    <Button
                                        variant="outlined"
                                        component="span"
                                        startIcon={<PhotoCamera />}
                                        disabled={!!mediaFile}
                                    >
                                        Upload Image
                                    </Button>
                                </label>
                                {/* Upload Video */}
                                <input
                                    accept="video/*"
                                    style={{ display: 'none' }}
                                    id="video-upload"
                                    type="file"
                                    onChange={handleMediaChange}
                                />
                                <label htmlFor="video-upload">
                                    <Button
                                        variant="outlined"
                                        component="span"
                                        startIcon={<Videocam />}
                                        disabled={!!mediaFile}
                                    >
                                        Upload Video
                                    </Button>
                                </label>
                                {/* Remove Media */}
                                {mediaFile && (
                                    <Button
                                        variant="outlined"
                                        color="error"
                                        startIcon={<Delete />}
                                        onClick={handleRemoveMedia}
                                    >
                                        Remove
                                    </Button>
                                )}
                            </Box>
                            {previewUrl && (
                                <Box sx={{ mt: 2 }}>
                                    {mediaFile && mediaFile.type.startsWith('video') ? (
                                        <video src={previewUrl} controls style={{ maxWidth: '100%', maxHeight: 200, borderRadius: 8 }} />
                                    ) : (
                                        <img src={previewUrl} alt="Preview" style={{ maxWidth: '100%', maxHeight: 200, borderRadius: 8 }} />
                                    )}
                                </Box>
                            )}
                        </Grid>

                        <Grid item xs={12}>
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                size="large"
                            >
                                Create Post
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            </Paper>
        </Container>
    );
};

export default CreatePost;
