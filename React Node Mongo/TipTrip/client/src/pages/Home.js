import React, { useState, useEffect } from 'react';
import {
    Container,
    Grid,
    Card,
    CardContent,
    CardMedia,
    Typography,
    Box,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    TextField,
    InputAdornment,
    Rating,
    List,
    ListItem,
    ListItemText,
    ListItemAvatar,
    Avatar,
    Divider,
    Button,
    IconButton,
    Paper,
    Chip
} from '@mui/material';
import {
    Search as SearchIcon,
    LocationOn,
    FilterList,
    Clear,
    Star
} from '@mui/icons-material';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

const Home = () => {
    const [posts, setPosts] = useState([]);
    const [allPosts, setAllPosts] = useState([]); // Store all posts for client-side filtering
    const [countries, setCountries] = useState([]);
    const [selectedCountry, setSelectedCountry] = useState('');
    const [searchQuery, setSearchQuery] = useState('');
    const [sortBy, setSortBy] = useState('newest');
    const [showFilters, setShowFilters] = useState(false);
    const [error, setError] = useState('');
    const [userLocation, setUserLocation] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        fetchCountries();
        fetchPosts('', '', 'newest'); // Load all posts by default
        getUserLocation();
    }, []);

    // Get user's geolocation for distance-based sorting
    const getUserLocation = () => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    setUserLocation({
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    });
                },
                (error) => {
                    console.error('Error getting location:', error);
                }
            );
        }
    };


    // Fetch posts from API with filters and sorting
    const fetchPosts = async (country = '', query = '', sort = 'newest') => {
        try {
            const token = localStorage.getItem('token');
            let url = 'http://localhost:5000/api/posts?';
            if (country) url += `country=${country}&`;
            if (sort) url += `sort=${sort}&`;
            const response = await axios.get(url, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            let sortedPosts = response.data;
            // Apply sorting
            switch (sortBy) {
                case 'rating':
                    sortedPosts.sort((a, b) => a.rating - b.rating);
                    break;
                case 'oldest':
                    sortedPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
                    break;
                default: // newest
                    sortedPosts.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
            }

            setAllPosts(sortedPosts); // Save all posts for filtering
            setPosts(sortedPosts);    // Show all by default
        } catch (error) {
            console.error('Error fetching posts:', error);
            setError('Failed to load posts');
        }
    };

    // Fetch countries from API
    const fetchCountries = async () => {
        try {
            const response = await axios.get('http://localhost:5000/api/countries');
            const sortedCountries = response.data.sort((a, b) => a.name.localeCompare(b.name));
            setCountries(sortedCountries);
        } catch (error) {
            console.error('Error fetching countries:', error);
        }
    };

    // Filter posts by selected country and search query
    const filterPosts = (country = selectedCountry, query = searchQuery) => {
        let filtered = allPosts;

        if (country) {
            filtered = filtered.filter(post => post.country?._id === country);
        }

        if (query.trim()) {
            const q = query.trim().toLowerCase();
            filtered = filtered.filter(post =>
                (post.title && post.title.toLowerCase().includes(q)) ||
                (post.content && post.content.toLowerCase().includes(q)) ||
                (post.country?.name && post.country.name.toLowerCase().includes(q))
            );
        }

        setPosts(filtered);
    };

    // Handle search button click or form submit
    const handleSearch = (e) => {
        if (e) e.preventDefault();
        filterPosts();
    };

    // Handle Enter key in search box
    const handleKeyPress = (e) => {
        if (e.key === 'Enter') {
            handleSearch(e);
        }
    };

    // Navigate to post details page
    const handlePostClick = (postId) => {
        navigate(`/post/${postId}`);
    };

    // Handle country selection from country list
    const handleCountryClick = (countryId) => {
        setSelectedCountry(countryId);
        setSearchQuery('');
        fetchPosts(countryId, '', sortBy);
    };

    // Clear all filters and reload posts
    const handleClearFilters = () => {
        setSelectedCountry('');
        setSearchQuery('');
        setSortBy('newest');
        fetchPosts('', '', 'newest');
    };

    // Handle sort selection change
    const handleSortChange = (e) => {
        setSortBy(e.target.value);
        fetchPosts(selectedCountry, '', e.target.value);
    };

    // When changing country, filter posts
    const handleCountryChange = (e) => {
        setSelectedCountry(e.target.value);
        filterPosts(e.target.value, searchQuery);
    };

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            {/* Hero Section */}
            <Paper
                elevation={3}
                sx={{
                    p: 6,
                    mb: 6,
                    background: 'linear-gradient(45deg, #2196F3 30%, #21CBF3 90%)',
                    color: 'white',
                    borderRadius: 2
                }}
            >
                <Typography variant="h2" component="h1" gutterBottom>
                    Welcome to TipTrip
                </Typography>
                <Typography variant="h5" paragraph>
                    Your Ultimate Travel Companion
                </Typography>
                <Typography variant="body1" paragraph>
                    TipTrip is a social network designed specifically for travelers. Share your experiences, discover new destinations, and connect with fellow travelers from around the world. Whether you're planning your next adventure or looking for inspiration, TipTrip helps you find the perfect destination based on real experiences and recommendations.
                </Typography>
                <Box sx={{ mt: 4 }}>
                    <Button
                        variant="outlined"
                        color="inherit"
                        size="large"
                        component={Link}
                        to="/create-post"
                    >
                        Share Your Experience
                    </Button>
                </Box>
            </Paper>

            {/* Search Section */}
            <Paper elevation={3} sx={{ p: 3, mb: 4 }}>
                <Box component="form" onSubmit={handleSearch}>
                    <Grid container spacing={2} alignItems="center">
                        <Grid item xs={12} md={4}>
                            <TextField
                                fullWidth
                                label="Search posts"
                                value={searchQuery}
                                onChange={(e) => setSearchQuery(e.target.value)}
                                onKeyDown={handleKeyPress}
                                InputProps={{
                                    startAdornment: (
                                        <InputAdornment position="start">
                                            <SearchIcon />
                                        </InputAdornment>
                                    ),
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <Button
                                                variant="contained"
                                                color="primary"
                                                size="small"
                                                onClick={handleSearch}
                                                sx={{ minWidth: 0, px: 1 }}
                                            >
                                                Search
                                            </Button>
                                        </InputAdornment>
                                    )
                                }}
                            />
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <FormControl fullWidth>
                                <InputLabel>Country</InputLabel>
                                <Select
                                    value={selectedCountry}
                                    onChange={handleCountryChange}
                                    label="Country"
                                >
                                    <MenuItem value="">All Countries</MenuItem>
                                    {countries.map((country) => (
                                        <MenuItem key={country._id} value={country._id}>
                                            {country.name}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12} md={3}>
                            <FormControl fullWidth>
                                <InputLabel>Sort By</InputLabel>
                                <Select
                                    value={sortBy}
                                    onChange={handleSortChange}
                                    label="Sort By"
                                >
                                    <MenuItem value="newest">Newest</MenuItem>
                                    <MenuItem value="oldest">Oldest</MenuItem>
                                    <MenuItem value="rating">Highest Rating</MenuItem>
                                </Select>
                            </FormControl>
                        </Grid>
                        <Grid item xs={12} md={2}>
                            <Button
                                fullWidth
                                variant="outlined"
                                onClick={handleClearFilters}
                                startIcon={<Clear />}
                            >
                                Clear
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Paper>

            <Grid container spacing={4}>
                {/* Countries List */}
                <Grid item xs={12} md={3}>
                    <Card elevation={3} sx={{ height: '100%' }}>
                        <CardContent>
                            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                                <Typography variant="h6">
                                    Countries
                                </Typography>
                            </Box>
                            <List sx={{ maxHeight: '600px', overflow: 'auto' }}>
                                <ListItem
                                    button
                                    selected={selectedCountry === ''}
                                    onClick={() => handleCountryClick('')}
                                >
                                    <ListItemText primary="All Countries" />
                                </ListItem>
                                <Divider />
                                {countries.map((country) => (
                                    <React.Fragment key={country._id}>
                                        <ListItem
                                            button
                                            selected={selectedCountry === country._id}
                                            onClick={() => handleCountryClick(country._id)}
                                        >
                                            <ListItemAvatar>
                                                <Avatar
                                                    src={`https://flagcdn.com/w40/${country.code.toLowerCase()}.png`}
                                                    alt={country.name}
                                                    sx={{ width: 32, height: 32 }}
                                                />
                                            </ListItemAvatar>
                                            <ListItemText
                                                primary={country.name}
                                                secondary={`${country.posts?.length || 0} posts`}
                                            />
                                        </ListItem>
                                        <Divider />
                                    </React.Fragment>
                                ))}
                            </List>
                        </CardContent>
                    </Card>
                </Grid>

                {/* Main Content */}
                <Grid item xs={12} md={9}>
                    {/* Posts Grid */}
                    <Grid container spacing={4}>
                        {posts.map((post) => (
                            <Grid item key={post._id} xs={12} sm={6} md={4}>
                                <Card
                                    sx={{
                                        height: '100%',
                                        display: 'flex',
                                        flexDirection: 'column',
                                        cursor: 'pointer',
                                        '&:hover': {
                                            transform: 'translateY(-4px)',
                                            boxShadow: 4,
                                            transition: 'all 0.3s ease-in-out'
                                        }
                                    }}
                                    onClick={() => handlePostClick(post._id)}
                                >
                                    <CardMedia
                                        component={post.mediaUrl && post.mediaUrl.match(/\.(mp4|webm|ogg)$/i) ? "video" : "img"}
                                        height="200"
                                        image={post.mediaUrl && !post.mediaUrl.match(/\.(mp4|webm|ogg)$/i) ? post.mediaUrl : undefined}
                                        src={post.mediaUrl && post.mediaUrl.match(/\.(mp4|webm|ogg)$/i) ? post.mediaUrl : undefined}
                                        controls={post.mediaUrl && post.mediaUrl.match(/\.(mp4|webm|ogg)$/i) ? true : undefined}
                                        alt={post.title}
                                        style={{ objectFit: 'cover', background: '#eee' }}
                                    />
                                    <CardContent sx={{ flexGrow: 1 }}>
                                        <Typography gutterBottom variant="h5" component="h2">
                                            {post.title}
                                        </Typography>
                                        <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                            <LocationOn sx={{ mr: 1, color: 'text.secondary' }} />
                                            <Typography variant="body2" color="text.secondary">
                                                {post.country?.name}
                                            </Typography>
                                        </Box>
                                        <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                                            {post.content.substring(0, 100)}...
                                        </Typography>
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Rating value={post.rating} readOnly size="small" />
                                            <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                                                ({post.rating})
                                            </Typography>
                                        </Box>
                                    </CardContent>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                </Grid>
            </Grid>

            {error && (
                <Typography color="error" sx={{ mt: 2 }}>
                    {error}
                </Typography>
            )}
        </Container>
    );
};

export default Home;
